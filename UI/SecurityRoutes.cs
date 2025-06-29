using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.IO;

using TransferObjects;
using Common;
using Util;

namespace UI
{
    public partial class SecurityRoutes : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        const int NameIndex = 0;
        const int FromIndex = 1;
        const int ToIndex = 2;

        const int EmplIndex = 0;
        const int DateIndex = 1;
        const int RouteIndex = 2;
        const int StatusIndex = 3;

        private ListViewItemComparer _comp;
        private ListViewItemComparer _comp2;
        private ListViewItemComparer _comp3;
        private ListViewItemComparer _comp4;
        private ListViewItemComparer _comp5;
        private ListViewItemComparer _comp6;
        private ListViewItemSchComparer _comp1;
        private ListViewItemLogComparer _comp7;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;
        string menuItemSchID;
        string menuItemRepID;
        string menuItemPointsID;
        string menuItemTerminalsID;
        string menuItemLogID;
        string menuItemLogReadID;
        string menuItemSchEmployeeID;
        string menuItemSchRoutsID;

        bool readRepPermission = false;

        bool addPermission = false;        
        bool deletePermission = false;

        bool readSchPermission = false;
        bool addSchPermission = false;
        bool updateSchPermission = false;
        bool deleteSchPermission = false;

        bool readPointPermission = false;
        bool addPointPermission = false;
        bool updatePointPermission = false;
        bool deletePointPermission = false;

        bool readTerminalPermission = false;
        bool addTerminalPermission = false;
        bool updateTerminalPermission = false;
        bool deleteTerminalPermission = false;

        bool readLog = false;
        bool readLogImport = false;
        bool readEmployee = false;
        bool readMaintenance = false;

        ArrayList routesSchedules;
        List<ListViewItem> originEmployees;
        List<ListViewItem> addedEmployees;
        List<ListViewItem> removedEmployees;

        // report data
        string selWorkingUnit = "*";
        string selEmployee = "*";
        string selRoute = "*";

        private string routeType;

        List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();

        Filter filter;

        XKWrapper.XKWrapper xkw;
        Int16 nDevices;
        StringBuilder sb1 = new StringBuilder("                                                                ", 256);
        StringBuilder sb2 = new StringBuilder("                  ", 256);
        bool Connected;

        SecurityRoutesReader DefaultReader = new SecurityRoutesReader();
        SecurityRoutesEmployee DefaultEmployee = new SecurityRoutesEmployee();
        ArrayList currentLogList = new ArrayList();

        public SecurityRoutes(string routeType)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(SecurityRoutes).Assembly);

                setLanguage();

                logInUser = NotificationController.GetLogInUser();
                routesSchedules = new ArrayList();
                originEmployees = new List<ListViewItem>();
                addedEmployees = new List<ListViewItem>();
                removedEmployees = new List<ListViewItem>();

                DateTime now = DateTime.Now.Date;
                dtpFromDate.Value = new DateTime(now.Year, now.Month, 1);
                dtpToDate.Value = now;

                rbAnalytic.Checked = true;

                this.routeType = routeType;

                if (routeType.Equals(Constants.routeTag))
                {
                    tabControl.TabPages.Remove(tabPoints);
                    tabControl.TabPages.Remove(tabTerminals);
                    tabControl.TabPages.Remove(tabEmployees);
                    tabControl.TabPages.Remove(tabImportLog);
                    tabControl.TabPages.Remove(tabLogPreview);
                }
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
                    case SecurityRoutes.NameIndex:
                        {
                            if (_listView.Name.Equals("lvRoutes") || _listView.Name.Equals("lvDetails"))
                            {
                                return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                            }
                            else
                            {
                                int id1 = 0;
                                int id2 = 0;

                                if (!sub1.Text.Trim().Equals(""))
                                {
                                    id1 = Int32.Parse(sub1.Text.Trim());
                                }

                                if (!sub2.Text.Trim().Equals(""))
                                {
                                    id2 = Int32.Parse(sub2.Text.Trim());
                                }

                                return CaseInsensitiveComparer.Default.Compare(id1, id2);
                            }
                        }
                    case SecurityRoutes.FromIndex:
                        {
                            if (_listView.Name.Equals("lvRoutes"))
                            {
                                DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                                DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                                if (!sub1.Text.Trim().Equals(""))
                                {
                                    dt1 = DateTime.ParseExact(sub1.Text.Trim(), "HH:mm", null);
                                }

                                if (!sub2.Text.Trim().Equals(""))
                                {
                                    dt2 = DateTime.ParseExact(sub2.Text.Trim(), "HH:mm", null);
                                }

                                return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
                            }
                            else
                            {
                                return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                            }
                        }
                    case SecurityRoutes.ToIndex:
                        if (_listView.Name.Equals("lvRoutes") )
                        {
                            DateTime dt3 = new DateTime(1, 1, 1, 0, 0, 0);
                            DateTime dt4 = new DateTime(1, 1, 1, 0, 0, 0);

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                dt3 = DateTime.ParseExact(sub1.Text.Trim(), "HH:mm", null);
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                dt4 = DateTime.ParseExact(sub2.Text.Trim(), "HH:mm", null);
                            }

                            return CaseInsensitiveComparer.Default.Compare(dt3, dt4);
                        }
                        else if (_listView.Name.Equals("lvDetails"))
                        {
                            DateTime dt3 = new DateTime(1, 1, 1, 0, 0, 0);
                            DateTime dt4 = new DateTime(1, 1, 1, 0, 0, 0);

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                dt3 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy HH:mm", null);
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                dt4 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy HH:mm", null);
                            }

                            return CaseInsensitiveComparer.Default.Compare(dt3, dt4);
                        }
                        else
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");

                }
            }
        }

        #endregion

        #region Inner Class for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemSchComparer : IComparer
        {
            private ListView _listView;

            public ListViewItemSchComparer(ListView lv)
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
                    case SecurityRoutes.EmplIndex:
                    case SecurityRoutes.RouteIndex:
                    case SecurityRoutes.StatusIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case SecurityRoutes.DateIndex:
                        {
                            IFormatProvider culture = new CultureInfo("fr-FR", true);
                            DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                            DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                dt1 = DateTime.Parse(sub1.Text.Trim(), culture, DateTimeStyles.NoCurrentDateDefault);
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                dt2 = DateTime.Parse(sub2.Text.Trim(), culture, DateTimeStyles.NoCurrentDateDefault);
                            }

                            return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");

                }
            }
        }

        #endregion

        #region Inner Class for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemLogComparer : IComparer
        {
            private ListView _listView;

            public ListViewItemLogComparer(ListView lv)
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
                    case 0:
                    case 1:
                    case 2:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case 3:
                        {
                            DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                            DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy HH:mm", null);
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy HH:mm", null);
                            }

                            return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");

                }
            }
        }

        #endregion

        private void setLanguage()
        {
            try
            {
                // form text
                this.Text = rm.GetString("SecurityRoutesForm", culture);

                // tab page's text
                this.tabScheduledRoutes.Text = rm.GetString("tabScheduledRoute", culture);
                this.tabMaintenance.Text = rm.GetString("tabMaintenance", culture);
                this.tabPoints.Text = rm.GetString("tabPoints", culture);
                this.tabTerminals.Text = rm.GetString("tabTerminals", culture);
                this.tabEmployees.Text = rm.GetString("tabEmployees", culture);
                this.tabImportLog.Text = rm.GetString("tabImportLog", culture);
                this.tabLogPreview.Text = rm.GetString("tabLogs", culture);

                //check box's
                this.chbHierarhicly.Text = rm.GetString("chbHierarhicly", culture);
                this.chbHierarhiclyLog.Text = rm.GetString("chbHierarhicly", culture);

                // group box's text
                this.gbRouteSearch.Text = rm.GetString("gbRouteSearch", culture);
                this.gbSearch.Text = rm.GetString("gbRouteSchSearch", culture);
                this.gbRouteSch.Text = rm.GetString("gbRouteSch", culture);
                this.gbDetails.Text = rm.GetString("gbDetails", culture);
                this.gbReport.Text = rm.GetString("gbReport", culture);
                this.gbPointSearch.Text = rm.GetString("gbPointSearch", culture);
                this.gbPoints.Text = rm.GetString("gbPoints", culture);
                this.gbReaders.Text = rm.GetString("gbReaders", culture);
                this.gbReaderSearch.Text = rm.GetString("gbReaderSearch", culture);
                this.gbEmplNotRoute.Text = rm.GetString("gbEmplNotRoute", culture);
                this.gbEmplRoutes.Text = rm.GetString("gbEmplRoutes", culture);
                this.gbDate.Text = rm.GetString("gbDate", culture);
                this.gbTime.Text = rm.GetString("gbTime", culture);
                this.gbSearchLog.Text = rm.GetString("gbSearch", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);

                // label's text
                this.lblRoute.Text = rm.GetString("lblRoute", culture);
                this.lblWU.Text = rm.GetString("lblWU", culture);
                this.lblEmpl.Text = rm.GetString("lblEmployee", culture);
                this.lblRouteName.Text = rm.GetString("lblRoute", culture);
                this.lblFromDate.Text = rm.GetString("lblFrom", culture);
                this.lblToDate.Text = rm.GetString("lblTo", culture);
                this.lblRepType.Text = rm.GetString("lblRepType", culture);
                this.lblPointID.Text = rm.GetString("lblPointID", culture);
                this.lblPointName.Text = rm.GetString("lblName", culture);
                this.lblReaderID.Text = rm.GetString("lblReaderID", culture);
                this.lblReaderName.Text = rm.GetString("lblName", culture);
                this.lblReader.Text = rm.GetString("lblReader", culture);
                this.lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblFromTimeLog.Text = rm.GetString("lblFrom", culture);
                this.lblPointLog.Text = rm.GetString("lblPoint", culture);
                this.lblToTimeLog.Text = rm.GetString("lblTo", culture);
                this.lblWULog.Text = rm.GetString("lblWU", culture);
                this.lblDateFromLog.Text = rm.GetString("lblFrom", culture);
                this.lblDateToLog.Text = rm.GetString("lblTo", culture);
                this.lblEmployeeLog.Text = rm.GetString("lblEmployee", culture);

                // button's text
                this.btnAdd.Text = rm.GetString("btnAdd", culture);
                this.btnDelete.Text = rm.GetString("btnDelete", culture);
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnSearch.Text = rm.GetString("btnSearch", culture);
                btnSearchLog.Text = rm.GetString("btnSearch", culture);
                this.btnAssign.Text = rm.GetString("btnAssignRouteSch", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
                this.btnDeleteSch.Text = rm.GetString("btnDelete", culture);
                //this.btnListView.Text = rm.GetString("btnListView", culture);
                this.btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
                this.btnImport.Text = rm.GetString("btnImport", culture);
                this.btnPointAdd.Text = rm.GetString("btnAdd", culture);
                this.btnPointSearch.Text = rm.GetString("btnSearch", culture);
                this.btnPointUpd.Text = rm.GetString("btnUpdate", culture);
                this.btnPointDelete.Text = rm.GetString("btnDelete", culture);
                this.btnReaderAdd.Text = rm.GetString("btnAdd", culture);
                this.btnReaderSearch.Text = rm.GetString("btnSearch", culture);
                this.btnReaderUpd.Text = rm.GetString("btnUpdate", culture);
                this.btnReaderDelete.Text = rm.GetString("btnDelete", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnSetTime.Text = rm.GetString("btnSetTime", culture);

                // radio button's text
                this.rbAnalytic.Text = rm.GetString("rbAnalytic", culture);
                this.rbSyntetic.Text = rm.GetString("rbSyntetic", culture);

                // list views
                lvRoutes.BeginUpdate();
                lvRoutes.Columns.Add(rm.GetString("hdrGate", culture), (lvRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvRoutes.Columns.Add(rm.GetString("hdrFrom", culture), (lvRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvRoutes.Columns.Add(rm.GetString("hdrTo", culture), (lvRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvRoutes.EndUpdate();

                lvSchedule.BeginUpdate();
                lvSchedule.Columns.Add(rm.GetString("hdrEmployee", culture), (lvSchedule.Width - 4) / 4 - 5, HorizontalAlignment.Left);
                lvSchedule.Columns.Add(rm.GetString("hdrDate", culture), (lvSchedule.Width - 4) / 4 - 5, HorizontalAlignment.Left);
                lvSchedule.Columns.Add(rm.GetString("hdrRoute", culture), (lvSchedule.Width - 4) / 4 - 5, HorizontalAlignment.Left);
                lvSchedule.Columns.Add(rm.GetString("hdrStatus", culture), (lvSchedule.Width - 4) / 4 - 5, HorizontalAlignment.Left);
                lvSchedule.EndUpdate();

                lvDetails.BeginUpdate();
                lvDetails.Columns.Add(rm.GetString("hdrGate", culture), (lvDetails.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvDetails.Columns.Add(rm.GetString("hdrRouteSch", culture), (lvDetails.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvDetails.Columns.Add(rm.GetString("hdrRealizationTime", culture), (lvDetails.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvDetails.EndUpdate();

                lvPoints.BeginUpdate();
                lvPoints.Columns.Add(rm.GetString("hdrPointID", culture), (lvPoints.Width - 3) / 3 - 5, HorizontalAlignment.Right);
                lvPoints.Columns.Add(rm.GetString("hdrName", culture), (lvPoints.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvPoints.Columns.Add(rm.GetString("hdrDescription", culture), (lvPoints.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvPoints.EndUpdate();

                lvReaders.BeginUpdate();
                lvReaders.Columns.Add(rm.GetString("hdrReaderID", culture), (lvReaders.Width - 3) / 3 - 5, HorizontalAlignment.Right);
                lvReaders.Columns.Add(rm.GetString("hdrName", culture), (lvReaders.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrDescription", culture), (lvReaders.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvReaders.EndUpdate();

                lvEmplNotRoutes.BeginUpdate();
                lvEmplNotRoutes.Columns.Add(rm.GetString("hdrEmplID", culture), (lvEmplNotRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Right);
                lvEmplNotRoutes.Columns.Add(rm.GetString("hdrEmployee", culture), (lvEmplNotRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvEmplNotRoutes.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmplNotRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvEmplNotRoutes.EndUpdate();

                lvEmplRoutes.BeginUpdate();
                lvEmplRoutes.Columns.Add(rm.GetString("hdrEmplID", culture), (lvEmplRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Right);
                lvEmplRoutes.Columns.Add(rm.GetString("hdrEmployee", culture), (lvEmplRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvEmplRoutes.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvEmplRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvEmplRoutes.EndUpdate();

                lvLogs.BeginUpdate();
                lvLogs.Columns.Add(rm.GetString("hdrEmployee", culture), (lvPoints.Width ) / 4 , HorizontalAlignment.Left);
                lvLogs.Columns.Add(rm.GetString("hdrTagID", culture), (lvPoints.Width ) / 4 , HorizontalAlignment.Left);
                lvLogs.Columns.Add(rm.GetString("hdrPointName", culture), (lvPoints.Width ) / 4 , HorizontalAlignment.Left);
                lvLogs.Columns.Add(rm.GetString("hdrTime", culture), (lvPoints.Width ) /4 , HorizontalAlignment.Left);
                lvLogs.EndUpdate();

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void SecurityRoutes_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvRoutes);
                lvRoutes.ListViewItemSorter = _comp;
                _comp.SortColumn = FromIndex;
                lvRoutes.Sorting = SortOrder.Ascending;

                // Initialize comparer object
                _comp1 = new ListViewItemSchComparer(lvSchedule);
                lvSchedule.ListViewItemSorter = _comp1;
                _comp1.SortColumn = EmplIndex;
                lvSchedule.Sorting = SortOrder.Ascending;

                // Initialize comparer object
                _comp2 = new ListViewItemComparer(lvDetails);
                lvDetails.ListViewItemSorter = _comp2;
                _comp2.SortColumn = FromIndex;
                lvDetails.Sorting = SortOrder.Ascending;

                // Initialize comparer object
                _comp3 = new ListViewItemComparer(lvPoints);
                lvPoints.ListViewItemSorter = _comp3;
                _comp3.SortColumn = FromIndex;
                lvPoints.Sorting = SortOrder.Ascending;

                // Initialize comparer object
                _comp4 = new ListViewItemComparer(lvReaders);
                lvReaders.ListViewItemSorter = _comp4;
                _comp4.SortColumn = FromIndex;
                lvReaders.Sorting = SortOrder.Ascending;

                // Initialize comparer object
                _comp5 = new ListViewItemComparer(lvEmplNotRoutes);
                lvEmplNotRoutes.ListViewItemSorter = _comp5;
                _comp5.SortColumn = FromIndex;
                lvEmplNotRoutes.Sorting = SortOrder.Ascending;

                // Initialize comparer object
                _comp6 = new ListViewItemComparer(lvEmplRoutes);
                lvEmplRoutes.ListViewItemSorter = _comp6;
                _comp6.SortColumn = FromIndex;
                lvEmplRoutes.Sorting = SortOrder.Ascending;

                // Initialize comparer object
                _comp7 = new ListViewItemLogComparer(lvLogs);
                lvLogs.ListViewItemSorter = _comp7;
                _comp7.SortColumn = FromIndex;
                lvLogs.Sorting = SortOrder.Ascending;

                populateRoutes(cbRoute);
                populateRoutes(cbRouteName);

                if (routeType.Equals(Constants.routeTag))
                {
                    populateWorkingUnits(cbWU);
                    populateEmployeeCombo(-1);
                    populateWorkingUnits(cbWULog);
                    populateEmployeeCombo(-1);
                }
                else
                {
                    lblWU.Visible = cbWU.Visible = btnWUTree.Visible = chbHierarhicly.Visible = false;
                    populateRouteEmployeeCombo(cbEmpl);

                    populatePointNameCombo(cbPointName,false);
                    populatePointNameCombo(cbPointLog,true);
                    populateReaderNameCombo(cbReaderName);

                    ArrayList employeesRoutes = new SecurityRoutesEmployee().Search();
                    populateEmplRoutesListView(employeesRoutes);
                    string wuUnits = "";
                    List<WorkingUnitTO> wuList = new WorkingUnit().Search();
                    foreach (WorkingUnitTO wu in wuList)
                    {
                        wuUnits += wu.WorkingUnitID + ",";
                    }
                    if (wuUnits.Length > 0)
                    {
                        wuUnits = wuUnits.Substring(0, wuUnits.Length - 1);
                    }
                    List<EmployeeTO> employees = new Employee().SearchByWU(wuUnits);
                    List<EmployeeTO> employeesNotRoutes = new List<EmployeeTO>();
                    foreach (EmployeeTO empl in employees)
                    {
                        bool emplRoute = false;
                        foreach (SecurityRoutesEmployee routeEmpl in employeesRoutes)
                        {
                            if (empl.EmployeeID == routeEmpl.EmployeeID)
                            {
                                emplRoute = true;
                                break;
                            }
                        }

                        if (!emplRoute)
                        {
                            employeesNotRoutes.Add(empl);
                        }
                    }
                    populateEmplNotRoutesListView(employeesNotRoutes);

                    populateReaderNameCombo(cbTerminal);
                    populateRouteEmployeeCombo(cbEmployee);
                    populateRouteEmployeeCombo(cbEmployeeLog);
                }

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                
                menuItemID = NotificationController.GetCurrentMenuItemID();
                int index = menuItemID.LastIndexOf('_');
                menuItemSchID = menuItemID.Substring(0, index + 1) + rm.GetString("hdrRouteSch", culture);
                menuItemRepID = rm.GetString("menuReports", culture) + "_"
                    + rm.GetString("menuStandardRep", culture) + "_";
                menuItemRepID += routeType.Equals(Constants.routeTag) ? rm.GetString("menuRoutesTag", culture) : rm.GetString("menuRoutesTerminal", culture);
                menuItemPointsID = menuItemID.Substring(0, index + 1) + rm.GetString("tabPoints", culture);
                menuItemTerminalsID = menuItemID.Substring(0, index + 1) + rm.GetString("tabTerminals", culture);
                menuItemLogID = menuItemID.Substring(0, index + 1) + rm.GetString("tabLogs", culture);
                menuItemLogReadID = menuItemID.Substring(0, index + 1) + rm.GetString("tabImportLog", culture);
                menuItemSchEmployeeID = menuItemID.Substring(0, index + 1) + rm.GetString("tabSchEmployees", culture);
                menuItemSchRoutsID = menuItemID.Substring(0, index + 1) + rm.GetString("tabMaintenance", culture);

                filter = new Filter();
                //filter.SerachButton = btnSearch;
                filter.TabID = this.tabControl.SelectedTab.Text;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));

                setVisibility();

                this.Cursor = Cursors.Arrow;

               
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.SecurityRoutes_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        #region Button's click events

        private void btnPointSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int pointID = -1;
                string pointName = "";

                if (!tbPointID.Text.Trim().Equals(""))
                {
                    if (!Int32.TryParse(tbPointID.Text.Trim(), out pointID))
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("pointsNotFound", culture));
                        return;
                    }
                }

                if (cbPointName.SelectedIndex > 0)
                {
                    pointName = cbPointName.Text.Trim();
                }

                ArrayList points = new SecurityRoutesPoint().Search(pointID, pointName, "", "");

                if (points.Count > 0)
                {
                    populatePointListView(points);
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("pointsNotFound", culture));
                }

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnPointSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvSchedule.Items.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("security_routes_schedule");
                    DataTable tableDtl = new DataTable("security_routes_sch_dtl");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("route_sch_id", typeof(int));
                    tableCR.Columns.Add("employee", typeof(System.String));
                    tableCR.Columns.Add("date", typeof(System.String));
                    tableCR.Columns.Add("route", typeof(System.String));
                    tableCR.Columns.Add("status", typeof(System.String));
                    tableCR.Columns.Add("imageID", typeof(byte));

                    tableDtl.Columns.Add("route_sch_id", typeof(int));
                    tableDtl.Columns.Add("gate", typeof(System.String));
                    tableDtl.Columns.Add("time_scheduled", typeof(System.String));
                    tableDtl.Columns.Add("time_realized", typeof(System.String));

                    tableI.Columns.Add("imageID", typeof(byte));
                    tableI.Columns.Add("image", typeof(System.Byte[]));

                    //add logo image just once
                    DataRow rowI = tableI.NewRow();
                    rowI["image"] = Constants.LogoForReport;
                    rowI["imageID"] = 1;
                    tableI.Rows.Add(rowI);
                    tableI.AcceptChanges();

                    dataSetCR.Tables.Add(tableCR);
                    dataSetCR.Tables.Add(tableDtl);
                    dataSetCR.Tables.Add(tableI);

                    int schCounter = 0;
                    foreach (ListViewItem schItem in lvSchedule.Items)
                    {
                        DataRow row = tableCR.NewRow();

                        SecurityRouteSchedule sch = (SecurityRouteSchedule)schItem.Tag;

                        row["route_sch_id"] = schCounter;
                        row["employee"] = schItem.Text.Trim();
                        row["date"] = schItem.SubItems[1].Text.Trim();
                        row["route"] = schItem.SubItems[2].Text.Trim(); ;
                        row["status"] = schItem.SubItems[3].Text.Trim();
                        row["imageID"] = 1;

                        tableCR.Rows.Add(row);
                        tableCR.AcceptChanges();

                        if (rbAnalytic.Checked)
                        {
                            ArrayList routeDtl = new ArrayList();
                            List<PassTO> passes = new List<PassTO>();
                            ArrayList routes = new ArrayList();

                            if (routeType.Equals(Constants.routeTag))
                            {
                                routeDtl = new SecurityRouteHdr().SearchDetailsTag(sch.SecurityRouteID);
                                Pass pass = new Pass();
                                pass.PssTO.EmployeeID = sch.EmployeeID;
                                passes = pass.SearchInterval(sch.Date, sch.Date, "", new DateTime(1, 1, 1, 0, 0, 0), new DateTime(1, 1, 1, 23, 59, 0));
                            }
                            else
                            {
                                routeDtl = new SecurityRouteHdr().SearchDetailsTerminal(sch.SecurityRouteID);
                                routes = new SecurityRoutesLog().SearchInterval(sch.EmployeeID, -1, "", sch.Date, sch.Date, "");
                            }

                            foreach (SecurityRouteDtl dtl in routeDtl)
                            {
                                List<PassTO> dtlPasses = new List<PassTO>();
                                ArrayList dtlRoutes = new ArrayList();

                                if (routeType.Equals(Constants.routeTag))
                                {
                                    dtlPasses = findDtlPasses(dtl, passes);
                                }
                                else
                                {
                                    dtlRoutes = findDtlLogs(dtl, routes);
                                }

                                if (dtlPasses.Count == 0 && dtlRoutes.Count == 0)
                                {
                                    DataRow rowDtl = tableDtl.NewRow();
                                    rowDtl["route_sch_id"] = schCounter;
                                    rowDtl["gate"] = dtl.GateName.Trim();
                                    rowDtl["time_scheduled"] = dtl.TimeFrom.ToString("HH:mm") + "-" + dtl.TimeTo.ToString("HH:mm");
                                    rowDtl["time_realized"] = "";

                                    tableDtl.Rows.Add(rowDtl);
                                    tableDtl.AcceptChanges();
                                }
                                else
                                {
                                    if (routeType.Equals(Constants.routeTag))
                                    {
                                        foreach (PassTO pass in dtlPasses)
                                        {
                                            DataRow rowDtlPass = tableDtl.NewRow();
                                            rowDtlPass["route_sch_id"] = schCounter;
                                            rowDtlPass["gate"] = dtl.GateName.Trim();
                                            rowDtlPass["time_scheduled"] = dtl.TimeFrom.ToString("HH:mm") + "-" + dtl.TimeTo.ToString("HH:mm");
                                            rowDtlPass["time_realized"] = pass.EventTime.ToString("dd.MM.yyyy HH:mm");

                                            tableDtl.Rows.Add(rowDtlPass);
                                            tableDtl.AcceptChanges();
                                        }
                                    }
                                    else
                                    {
                                        foreach (SecurityRoutesLog log in dtlRoutes)
                                        {
                                            DataRow rowDtlPass = tableDtl.NewRow();
                                            rowDtlPass["route_sch_id"] = schCounter;
                                            rowDtlPass["gate"] = dtl.GateName.Trim();
                                            rowDtlPass["time_scheduled"] = dtl.TimeFrom.ToString("HH:mm") + "-" + dtl.TimeTo.ToString("HH:mm");
                                            rowDtlPass["time_realized"] = log.EventTime.ToString("dd.MM.yyyy HH:mm");

                                            tableDtl.Rows.Add(rowDtlPass);
                                            tableDtl.AcceptChanges();
                                        }
                                    }
                                }
                            }
                        }

                        schCounter++;
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }

                    if (rbAnalytic.Checked)
                    {
                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.SecurityRoutesSchAnalyticCRView_sr view = new Reports.Reports_sr.SecurityRoutesSchAnalyticCRView_sr(dataSetCR,
                                dtpFromDate.Value.Date, dtpToDate.Value.Date, selWorkingUnit, selEmployee, selRoute);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.SecurityRoutesSchAnalyticCRView_en view = new Reports.Reports_en.SecurityRoutesSchAnalyticCRView_en(dataSetCR,
                                dtpFromDate.Value.Date, dtpToDate.Value.Date, selWorkingUnit, selEmployee, selRoute);
                            view.ShowDialog(this);
                        }
                    }
                    else if (rbSyntetic.Checked)
                    {
                        if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                        {
                            Reports.Reports_sr.SecurityRoutesSchSynteticCRView_sr view = new Reports.Reports_sr.SecurityRoutesSchSynteticCRView_sr(dataSetCR,
                                dtpFromDate.Value.Date, dtpToDate.Value.Date, selWorkingUnit, selEmployee, selRoute);
                            view.ShowDialog(this);
                        }
                        else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                        {
                            Reports.Reports_en.SecurityRoutesSchSynteticCRView_en view = new Reports.Reports_en.SecurityRoutesSchSynteticCRView_en(dataSetCR,
                                dtpFromDate.Value.Date, dtpToDate.Value.Date, selWorkingUnit, selEmployee, selRoute);
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
                log.writeLog(DateTime.Now + " SecurityRoutes.clearSchTab(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvSchedule.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selectRouteSchUpd", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                SecurityRouteSchedule sch = (SecurityRouteSchedule)lvSchedule.SelectedItems[0].Tag;
                SecurityRoutesScheduleAdd schAdd = new SecurityRoutesScheduleAdd(sch.EmployeeID, sch.Date, routeType);
                schAdd.ShowDialog(this);
                clearSchTab();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SecurityRoutesAdd secRoutesAdd = new SecurityRoutesAdd(this.routeType);
                secRoutesAdd.ShowDialog(this);
                populateRoutes(cbRoute);
                populateRoutes(cbRouteName);
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbRoute.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("selectRouteToDelete", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                bool isDeleted = new SecurityRouteHdr().Delete((int)cbRoute.SelectedValue);

                if (isDeleted)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("routeDeleted", culture));
                    populateRoutes(cbRoute);
                    populateRoutes(cbRouteName);
                    cbRoute.SelectedIndex = 0;
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("routeNotDeleted", culture));
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnWUTree_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Data.DataSet dsWorkingUnits = new WorkingUnit().getWorkingUnits("");
                WorkingUnitsTreeView workingUnitsTreeView = new WorkingUnitsTreeView(dsWorkingUnits);
                workingUnitsTreeView.ShowDialog();
                if (!workingUnitsTreeView.selectedWorkingUnit.Equals(""))
                {
                    cbWU.SelectedIndex = cbWU.FindStringExact(workingUnitsTreeView.selectedWorkingUnit);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnWUTreeView_Click(): " + ex.Message + "\n");
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
                routesSchedules = new SecurityRouteSchedule().Search((int)cbEmpl.SelectedValue,
                    (int)cbRouteName.SelectedValue, dtpFromDate.Value.Date, dtpToDate.Value.Date);

                if (routesSchedules.Count > 0)
                {
                    // set report parameters
                    if (cbWU.SelectedIndex >= 0)
                        selWorkingUnit = cbWU.Text;
                    if (routeType.Equals(Constants.routeTerminal))
                        selWorkingUnit = "";
                    if (cbEmpl.SelectedIndex >= 0)
                        selEmployee = cbEmpl.Text;
                    if (cbRouteName.SelectedIndex >= 0)
                        selRoute = cbRouteName.Text;

                    populateSchListView((int)cbEmpl.SelectedValue,
                        (int)cbRouteName.SelectedValue, dtpFromDate.Value.Date, dtpToDate.Value.Date);
                    lvDetails.Items.Clear();
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("routeSchedulesNotFound", culture));
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteSch_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvSchedule.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selectRouteSchToDelete", culture));
                    return;
                }

                DialogResult result = MessageBox.Show(rm.GetString("deleteRouteSchedules", culture), "", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    bool isDeleted = true;
                    int deletedCount = 0;

                    foreach (ListViewItem item in lvSchedule.SelectedItems)
                    {
                        if (item.SubItems[3].Text.Trim().ToUpper().Equals(Constants.statusScheduled))
                        {
                            isDeleted = new SecurityRouteSchedule().Delete((SecurityRouteSchedule)item.Tag) && isDeleted;
                            deletedCount++;
                        }
                    }

                    if (isDeleted)
                    {
                        if (deletedCount > 0)
                        {
                            MessageBox.Show(rm.GetString("routeSchDeleted", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noScheduledRouteSch", culture));
                        }

                        clearSchTab();
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("routeSchNotDeleted", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnDeleteSch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SecurityRoutesScheduleAdd schAdd = new SecurityRoutesScheduleAdd(-1, DateTime.Today, routeType);
                schAdd.ShowDialog(this);
                clearSchTab();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnAssign_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPointDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvPoints.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selectPointToDelete", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                bool isDeleted = true;
                SecurityRoutesPoint point = new SecurityRoutesPoint();

                foreach (ListViewItem item in lvPoints.SelectedItems)
                {
                    isDeleted = point.Delete(((SecurityRoutesPoint)item.Tag).ControlPointID) && isDeleted;

                    if (!isDeleted)
                        break;
                }

                if (isDeleted)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("pointDeleted", culture));

                    clearPointForm();

                    btnPointSearch.PerformClick();
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("pointNotDeleted", culture));
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnPointDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReaderDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvReaders.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selectReaderToDelete", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                bool isDeleted = true;
                SecurityRoutesReader reader = new SecurityRoutesReader();

                foreach (ListViewItem item in lvReaders.SelectedItems)
                {
                    isDeleted = reader.Delete(((SecurityRoutesReader)item.Tag).ReaderID) && isDeleted;

                    if (!isDeleted)
                        break;
                }

                if (isDeleted)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("readerDeleted", culture));

                    clearReaderForm();
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("readerNotDeleted", culture));
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnReaderDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPointAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SecurityRoutesPointsAdd pointAdd = new SecurityRoutesPointsAdd(new SecurityRoutesPoint(),this.routeType);
                pointAdd.ShowDialog();

                clearPointForm();
                btnPointSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnPointAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPointUpd_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvPoints.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noPointsSelectedUpd", culture));
                    return;
                }

                SecurityRoutesPointsAdd pointAdd = new SecurityRoutesPointsAdd((SecurityRoutesPoint)lvPoints.SelectedItems[0].Tag,this.routeType);
                pointAdd.ShowDialog();

                clearPointForm();

                btnPointSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnPointUpd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReaderSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int readerID = -1;
                string readerName = "";

                if (!tbReaderID.Text.Trim().Equals(""))
                {
                    if (!Int32.TryParse(tbReaderID.Text.Trim(), out readerID))
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noReadersFound", culture));
                        return;
                    }
                }

                if (cbReaderName.SelectedIndex > 0)
                {
                    readerName = cbReaderName.Text.Trim();
                }

                ArrayList readers = new SecurityRoutesReader().Search(readerID, readerName, "");

                if (readers.Count > 0)
                {
                    populateReaderListView(readers);
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noReadersFound", culture));
                }

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnReaderSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReaderAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SecurityRoutesReadersAdd readerAdd = new SecurityRoutesReadersAdd(new SecurityRoutesReader());
                readerAdd.ShowDialog();

                clearReaderForm();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnReaderAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReaderUpd_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvReaders.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noReaderSelectedUpd", culture));
                    return;
                }

                SecurityRoutesReadersAdd readerAdd = new SecurityRoutesReadersAdd((SecurityRoutesReader)lvReaders.SelectedItems[0].Tag);
                readerAdd.ShowDialog();

                clearReaderForm();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnReaderUpd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddEmpl_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem empl in lvEmplNotRoutes.SelectedItems)
                {
                    lvEmplNotRoutes.Items.Remove(empl);
                    lvEmplRoutes.Items.Add(empl);

                    if (contain(originEmployees, empl) < 0 && contain(addedEmployees, empl) < 0)
                    {
                        addedEmployees.Add(empl);
                        int index = contain(removedEmployees, empl);
                        if (index >= 0)
                            removedEmployees.RemoveAt(index);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnAddEmpl_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRemoveEmpl_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem empl in lvEmplRoutes.SelectedItems)
                {
                    if (!containLogEmpl(empl.Text.Trim()))
                    {
                        lvEmplRoutes.Items.Remove(empl);
                        lvEmplNotRoutes.Items.Add(empl);

                        if (contain(originEmployees, empl) >= 0 && contain(removedEmployees, empl) < 0)
                        {
                            removedEmployees.Add(empl);

                            int index = contain(addedEmployees, empl);
                            if (index >= 0)
                                addedEmployees.RemoveAt(index);
                        }
                    }
                    else
                    {
                        MessageBox.Show(empl.Text.Trim() + " " + empl.SubItems[1].Text.Trim() + ": " + rm.GetString("emplContainsLog", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnRemoveEmpl_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SecurityRoutesEmployee secRouteEmpl = new SecurityRoutesEmployee();
                bool saved = true;

                foreach (ListViewItem removedEmpl in removedEmployees)
                {
                    saved = secRouteEmpl.Delete(removedEmpl.Text.Trim()) && saved;

                    int index = contain(originEmployees, removedEmpl);
                    if (index >= 0)
                        originEmployees.RemoveAt(index);
                }

                foreach (ListViewItem addedEmpl in addedEmployees)
                {
                    saved = secRouteEmpl.Save(addedEmpl.Text.Trim()) > 0 ? true : false && saved;

                    int index = contain(originEmployees, addedEmpl);
                    if (index < 0)
                        originEmployees.Add(addedEmpl);
                }

                removedEmployees.Clear();
                addedEmployees.Clear();

                if (saved)
                {
                    MessageBox.Show(rm.GetString("emplSaved", culture));
                }
                else
                {
                    MessageBox.Show(rm.GetString("emplNotSaved", culture));
                }

                populateRouteEmployeeCombo(cbEmpl);
                populateRouteEmployeeCombo(cbEmployee);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Populate combo boxes

        private void populatePointNameCombo(ComboBox cb, bool tagID)
        {
            try
            {
                ArrayList points = new SecurityRoutesPoint().Search(-1, "", "", "");
                SecurityRoutesPoint point = new SecurityRoutesPoint();
                point.Name = rm.GetString("all", culture);
                points.Insert(0, point);

                cb.DataSource = points;
                cb.DisplayMember = "Name";
                if(! tagID)
                cb.ValueMember = "ControlPointID";
                if(tagID)
                cb.ValueMember = "TagID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populatePointNameCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateReaderNameCombo(ComboBox cb)
        {
            try
            {
                ArrayList readers = new SecurityRoutesReader().Search(-1, "", "");
                //if (this.routeType.Equals(Constants.routeTerminal))
                //{
                //    if (readers.Count > 0)
                //    {
                //        DefaultReader = (SecurityRoutesReader)readers[0];
                //        readers = new ArrayList();
                //        readers.Add(DefaultReader);
                //    }
                //}
                if (!cb.Name.Equals(cbTerminal.Name))
                {
                    SecurityRoutesReader reader = new SecurityRoutesReader();
                    reader.Name = rm.GetString("all", culture);
                    readers.Insert(0, reader);
                }
                cb.DataSource = readers;
                cb.DisplayMember = "Name";
                cb.ValueMember = "ReaderID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateReaderNameCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateRoutes(ComboBox cb)
        {
            try
            {
                ArrayList routes = new SecurityRouteHdr().Search("", "");
                SecurityRouteHdr route = new SecurityRouteHdr();
                route.Name = rm.GetString("all", culture);
                routes.Insert(0, route);

                cb.DataSource = routes;
                cb.DisplayMember = "Name";
                cb.ValueMember = "SecurityRouteID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateRoutes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateWorkingUnits(ComboBox cb)
        {
            try
            {
                List<WorkingUnitTO> wuArray = new WorkingUnit().Search();
                
                wUnits = new WorkingUnit().Search();

                wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cb.DataSource = wuArray;
                cb.DisplayMember = "Name";
                cb.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateWorkingUnits(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
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
                    emplArray = new Employee().Search();
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
                log.writeLog(DateTime.Now + " SecurityRoutes.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateEmployeeLogCombo(int wuID)
        {
            try
            {
                List<EmployeeTO> emplArray = new List<EmployeeTO>();
                string workUnitID = wuID.ToString();
                if (wuID == -1)
                {
                    emplArray = new Employee().Search();
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

                cbEmployeeLog.DataSource = emplArray;
                cbEmployeeLog.DisplayMember = "LastName";
                cbEmployeeLog.ValueMember = "EmployeeID";
                cbEmployeeLog.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void populateRouteEmployeeCombo(ComboBox cb)
        {
            try
            {
                ArrayList routeEmplArray = new ArrayList();

                routeEmplArray = new SecurityRoutesEmployee().SearchByWU("");

                //if (routeType.Equals(Constants.routeTerminal))
                //{
                //    if (routeEmplArray.Count > 0)
                //    {
                //        DefaultEmployee = (SecurityRoutesEmployee)routeEmplArray[0];
                //        routeEmplArray = new ArrayList();
                //        routeEmplArray.Add(DefaultEmployee);
                //    }
                //}
                if (!cb.Name.Equals(cbEmployee.Name))
                {
                    SecurityRoutesEmployee routeEmpl = new SecurityRoutesEmployee();
                    routeEmpl.EmployeeName = rm.GetString("all", culture);
                    routeEmplArray.Insert(0, routeEmpl);
                }
                
                cb.DataSource = routeEmplArray;
                cb.DisplayMember = "EmployeeName";
                cb.ValueMember = "EmployeeID";
                cb.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateRouteEmployeeCombo(): " + ex.Message + "\n");
                throw ex;
            }
        }

        #endregion

        #region Combo box's selected index changed events

        private void cbRoute_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbRoute.SelectedIndex <= 0)
                {
                    lvRoutes.Items.Clear();
                }
                else
                {
                    ArrayList routesDtl = new ArrayList();
                    if (routeType.Equals(Constants.routeTag))
                    {
                        routesDtl = new SecurityRouteHdr().SearchDetailsTag((int)cbRoute.SelectedValue);
                    }
                    else
                    {
                        routesDtl = new SecurityRouteHdr().SearchDetailsTerminal((int)cbRoute.SelectedValue);
                    }

                    populateListView(routesDtl);
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.cbRoute_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.cbWU_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }           
        }

        #endregion

        #region Populate list views

        public void populateListView(ArrayList routesDtl)
        {
            try
            {
                lvRoutes.BeginUpdate();
                lvRoutes.Items.Clear();

                if (routesDtl.Count > 0)
                {
                    foreach (SecurityRouteDtl dtl in routesDtl)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = dtl.GateName.Trim();
                        item.SubItems.Add(dtl.TimeFrom.ToString("HH:mm"));
                        item.SubItems.Add(dtl.TimeTo.ToString("HH:mm"));
                        item.ToolTipText = "Segment ID: " + dtl.SegmentID.ToString().Trim();

                        lvRoutes.Items.Add(item);
                    }
                }

                lvRoutes.EndUpdate();
                lvRoutes.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateListViewLog()
        {
            try
            {
                lvLogs.BeginUpdate();
                lvLogs.Items.Clear();

                if (currentLogList.Count > 0)
                {
                    foreach (TransferObjects.SecurityRoutesLogTO log in currentLogList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = log.EmployeeName.Trim();
                        item.SubItems.Add(log.TagID);
                        item.SubItems.Add(log.PointName);
                        item.SubItems.Add(log.EventTime.ToString("dd.MM.yyyy HH:mm"));
                       
                        lvLogs.Items.Add(item);
                    }
                }

                lvLogs.EndUpdate();
                lvLogs.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateListViewLog(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateSchListView(int emplID, int routeID, DateTime from, DateTime to)
        {
            try
            {
                lvSchedule.BeginUpdate();
                lvSchedule.Items.Clear();

                if (routesSchedules.Count > 0)
                {
                    Hashtable routeDetails = new Hashtable();
                    Hashtable passesTable = new Hashtable();

                    ArrayList routeDtl = new ArrayList();

                    if (routeType.Equals(Constants.routeTag))
                    {
                        routeDtl = new SecurityRouteHdr().SearchDetailsTag((int)cbRouteName.SelectedValue);
                    }
                    else
                    {
                        routeDtl = new SecurityRouteHdr().SearchDetailsTerminal((int)cbRouteName.SelectedValue);
                    }

                    foreach (SecurityRouteDtl dtl in routeDtl)
                    {
                        if (!routeDetails.ContainsKey(dtl.SecurityRouteID))
                        {
                            routeDetails.Add(dtl.SecurityRouteID, new ArrayList());
                        }

                        ((ArrayList)routeDetails[dtl.SecurityRouteID]).Add(dtl);
                    }

                    if (routeType.Equals(Constants.routeTag))
                    {
                        setStatusTag(routeDetails);
                    }
                    else
                    {
                        setStatusTerminal(routeDetails);
                    }

                    foreach (SecurityRouteSchedule sch in routesSchedules)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = sch.EmployeeName.Trim();
                        item.SubItems.Add(sch.Date.ToString("dd.MM.yyyy"));
                        item.SubItems.Add(sch.RouteName.Trim());
                        item.SubItems.Add(sch.Status);
                        if (sch.Status.Equals(Constants.statusNotCompleted))
                        {
                            item.BackColor = Color.Red;
                        }
                        if (sch.Status.Equals(Constants.statusPartially))
                            item.BackColor = Color.Yellow;
                        if (sch.Status.Equals(Constants.statusCompleted))
                        {
                            item.BackColor = Color.FromArgb(50,Color.Green);
                        }
                        item.Tag = sch;

                        lvSchedule.Items.Add(item);
                    }
                }

                lvSchedule.EndUpdate();
                lvSchedule.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateSchListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateDtlListView(ArrayList routesDtl, List<PassTO> passes, ArrayList routes)
        {
            try
            {
                lvDetails.BeginUpdate();
                lvDetails.Items.Clear();

                if (routesDtl.Count > 0)
                {
                    foreach (SecurityRouteDtl dtl in routesDtl)
                    {
                        List<PassTO> dtlPasses = new List<PassTO>();
                        ArrayList dtlRoutes = new ArrayList();

                        if (routeType.Equals(Constants.routeTag))
                        {
                            dtlPasses = findDtlPasses(dtl, passes);
                        }
                        else
                        {
                            dtlRoutes = findDtlLogs(dtl, routes);
                        }

                        if (dtlPasses.Count == 0 && dtlRoutes.Count == 0)
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = dtl.GateName.Trim();
                            item.SubItems.Add(dtl.TimeFrom.ToString("HH:mm") + " - " + dtl.TimeTo.ToString("HH:mm"));
                            item.SubItems.Add("");
                            item.BackColor = Color.Red;

                            lvDetails.Items.Add(item);
                        }
                        else
                        {
                            if (routeType.Equals(Constants.routeTag))
                            {
                                foreach (PassTO pass in dtlPasses)
                                {
                                    ListViewItem item = new ListViewItem();
                                    item.Text = dtl.GateName.Trim();
                                    item.SubItems.Add(dtl.TimeFrom.ToString("HH:mm") + "-" + dtl.TimeTo.ToString("HH:mm"));
                                    item.SubItems.Add(pass.EventTime.ToString("dd.MM.yyyy HH:mm"));

                                    lvDetails.Items.Add(item);
                                }
                            }
                            else
                            {
                                foreach (SecurityRoutesLog log in dtlRoutes)
                                {
                                    ListViewItem item = new ListViewItem();
                                    item.Text = dtl.GateName.Trim();
                                    item.SubItems.Add(dtl.TimeFrom.ToString("HH:mm") + "-" + dtl.TimeTo.ToString("HH:mm"));
                                    item.SubItems.Add(log.EventTime.ToString("dd.MM.yyyy HH:mm"));

                                    lvDetails.Items.Add(item);
                                }
                            }
                        }
                    }
                }

                lvDetails.EndUpdate();
                lvDetails.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateDtlListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populatePointListView(ArrayList points)
        {
            try
            {
                lvPoints.BeginUpdate();
                lvPoints.Items.Clear();

                if (points.Count > 0)
                {
                    foreach (SecurityRoutesPoint point in points)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = point.ControlPointID.ToString().Trim();
                        item.SubItems.Add(point.Name.Trim());
                        item.SubItems.Add(point.Description.Trim());
                        item.Tag = point;
                        item.ToolTipText = "Tag ID: " + point.TagID.ToString().Trim();

                        lvPoints.Items.Add(item);
                    }
                }

                lvPoints.EndUpdate();
                lvPoints.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populatePointListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateReaderListView(ArrayList readers)
        {
            try
            {
                lvReaders.BeginUpdate();
                lvReaders.Items.Clear();

                if (readers.Count > 0)
                {
                    foreach (SecurityRoutesReader reader in readers)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = reader.ReaderID.ToString().Trim();
                        item.SubItems.Add(reader.Name.Trim());
                        item.SubItems.Add(reader.Description.Trim());
                        item.Tag = reader;

                        lvReaders.Items.Add(item);
                    }
                }

                lvReaders.EndUpdate();
                lvReaders.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateReaderListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateEmplNotRoutesListView(List<EmployeeTO> employees)
        {
            try
            {
                lvEmplNotRoutes.BeginUpdate();
                lvEmplNotRoutes.Items.Clear();

                if (employees.Count > 0)
                {
                    foreach (EmployeeTO empl in employees)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = empl.EmployeeID.ToString().Trim();
                        item.SubItems.Add(empl.LastName.Trim() + " " + empl.FirstName.Trim());
                        item.SubItems.Add(empl.WorkingUnitName.Trim());

                        lvEmplNotRoutes.Items.Add(item);
                    }
                }

                lvEmplNotRoutes.EndUpdate();
                lvEmplNotRoutes.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateEmplNotRoutesListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void populateEmplRoutesListView(ArrayList employees)
        {
            try
            {
                lvEmplRoutes.BeginUpdate();
                lvEmplRoutes.Items.Clear();

                if (employees.Count > 0)
                {
                    foreach (SecurityRoutesEmployee empl in employees)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = empl.EmployeeID.ToString().Trim();
                        item.SubItems.Add(empl.EmployeeName.Trim());
                        item.SubItems.Add(empl.WUName.Trim());

                        lvEmplRoutes.Items.Add(item);
                        originEmployees.Add(item);
                    }
                }

                lvEmplRoutes.EndUpdate();
                lvEmplRoutes.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.populateEmplRoutesListView(): " + ex.Message + "\n");
                throw ex;
            }
        }

        #endregion

        #region List view's column click events

        private void lvPoints_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvPoints.Sorting;

                if (e.Column == _comp3.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvPoints.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvPoints.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp3.SortColumn = e.Column;
                    lvPoints.Sorting = SortOrder.Ascending;
                }

                lvPoints.Sort();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.lvPoints_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvReaders_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvReaders.Sorting;

                if (e.Column == _comp4.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvReaders.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvReaders.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp4.SortColumn = e.Column;
                    lvReaders.Sorting = SortOrder.Ascending;
                }

                lvReaders.Sort();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.lvReaders_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvDetails_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvDetails.Sorting;

                if (e.Column == _comp2.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvDetails.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvDetails.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp2.SortColumn = e.Column;
                    lvDetails.Sorting = SortOrder.Ascending;
                }

                lvDetails.Sort();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.lvDetails_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvSchedule_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                SortOrder prevOrder = lvSchedule.Sorting;

                if (e.Column == _comp1.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvSchedule.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvSchedule.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp1.SortColumn = e.Column;
                    lvSchedule.Sorting = SortOrder.Ascending;
                }

                lvSchedule.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.lvSchedule_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvRoutes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvRoutes.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvRoutes.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvRoutes.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvRoutes.Sorting = SortOrder.Ascending;
                }

                lvRoutes.Sort();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.lvRoutes_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvEmplNotRoutes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvEmplNotRoutes.Sorting;

                if (e.Column == _comp5.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvEmplNotRoutes.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvEmplNotRoutes.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp5.SortColumn = e.Column;
                    lvEmplNotRoutes.Sorting = SortOrder.Ascending;
                }

                lvEmplNotRoutes.Sort();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.lvEmplNotRoutes_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvEmplRoutes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvEmplRoutes.Sorting;

                if (e.Column == _comp6.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvEmplRoutes.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvEmplRoutes.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp6.SortColumn = e.Column;
                    lvEmplRoutes.Sorting = SortOrder.Ascending;
                }

                lvEmplRoutes.Sort();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.lvEmplRoutes_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void lvSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvSchedule.SelectedItems.Count > 0)
                {
                    SecurityRouteSchedule sch = (SecurityRouteSchedule)lvSchedule.SelectedItems[0].Tag;

                    ArrayList routeDtl = new ArrayList();
                    List<PassTO> passes = new List<PassTO>();
                    ArrayList routes = new ArrayList();

                    if (routeType.Equals(Constants.routeTag))
                    {
                        routeDtl = new SecurityRouteHdr().SearchDetailsTag(sch.SecurityRouteID);

                        Pass pass = new Pass();
                        pass.PssTO.EmployeeID = sch.EmployeeID;
                        passes = pass.SearchInterval(sch.Date, sch.Date, "", new DateTime(1, 1, 1, 0, 0, 0), new DateTime(1, 1, 1, 23, 59, 0));
                    }
                    else
                    {
                        routeDtl = new SecurityRouteHdr().SearchDetailsTerminal(sch.SecurityRouteID);

                        routes = new SecurityRoutesLog().SearchInterval(sch.EmployeeID, -1, "", sch.Date, sch.Date, "");
                    }

                    populateDtlListView(routeDtl, passes, routes);
                }

                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.lvSchedule_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private List<PassTO> findDtlPasses(SecurityRouteDtl dtl, List<PassTO> passes)
        {
            List<PassTO> dtlPasses = new List<PassTO>();

            try
            {
                foreach (PassTO pass in passes)
                {
                    if (pass.GateID == dtl.GateID && pass.EventTime.TimeOfDay >= dtl.TimeFrom.TimeOfDay
                        && pass.EventTime.TimeOfDay <= dtl.TimeTo.TimeOfDay)
                    {
                        dtlPasses.Add(pass);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.findDtlPasses(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }

            return dtlPasses;
        }

        private void setVisibility()
        {
            try
            {
                int permission;
                int permissionSch;
                int permissionRep;

                int permissionPoints;
                int permissionTerminals;
                int permissionLogImport;
                int permissionLog;
                int permissionEmployee;
                int permissionMaintenance;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);
                    permissionSch = (((int[])menuItemsPermissions[menuItemSchID])[role.ApplRoleID]);
                    permissionRep = (((int[])menuItemsPermissions[menuItemRepID])[role.ApplRoleID]);
                    permissionPoints = (((int[])menuItemsPermissions[menuItemPointsID])[role.ApplRoleID]);
                    permissionTerminals = (((int[])menuItemsPermissions[menuItemTerminalsID])[role.ApplRoleID]);
                    permissionLog = (((int[])menuItemsPermissions[menuItemLogID])[role.ApplRoleID]);
                    permissionLogImport = (((int[])menuItemsPermissions[menuItemLogReadID])[role.ApplRoleID]);
                    permissionEmployee = (((int[])menuItemsPermissions[menuItemSchEmployeeID])[role.ApplRoleID]);
                    permissionMaintenance = (((int[])menuItemsPermissions[menuItemSchRoutsID])[role.ApplRoleID]);
                    
                    
                    readRepPermission = readRepPermission || (((permissionRep / 8) % 2) == 0 ? false : true);
                    addPermission = addPermission || (((permission / 4) % 2) == 0 ? false : true);
                    deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);
                    readSchPermission = readSchPermission || (((permissionSch / 8) % 2) == 0 ? false : true);
                    addSchPermission = addSchPermission || (((permissionSch / 4) % 2) == 0 ? false : true);
                    updateSchPermission = updateSchPermission || (((permissionSch / 2) % 2) == 0 ? false : true);
                    deleteSchPermission = deleteSchPermission || ((permissionSch % 2) == 0 ? false : true);

                    readPointPermission = readPointPermission || (((permissionPoints / 8) % 2) == 0 ? false : true);
                    addPointPermission = addPointPermission || (((permissionPoints / 4) % 2) == 0 ? false : true);
                    updatePointPermission = updatePointPermission || (((permissionPoints / 2) % 2) == 0 ? false : true);
                    deletePointPermission = deletePointPermission || ((permissionPoints % 2) == 0 ? false : true);

                    readTerminalPermission = readTerminalPermission || (((permissionTerminals / 8) % 2) == 0 ? false : true);
                    addTerminalPermission = addTerminalPermission || (((permissionTerminals / 4) % 2) == 0 ? false : true);
                    updateTerminalPermission = updateTerminalPermission || (((permissionTerminals / 2) % 2) == 0 ? false : true);
                    deleteTerminalPermission = deleteTerminalPermission || ((permissionTerminals % 2) == 0 ? false : true);

                    readLog = readLog || (((permissionLog / 8) % 2) == 0 ? false : true);
                    readLogImport = readLogImport || (((permissionLogImport / 8) % 2) == 0 ? false : true);
                    readMaintenance = readMaintenance || (((permissionMaintenance / 8) % 2) == 0 ? false : true);
                    readEmployee = readEmployee || (((permissionEmployee / 8) % 2) == 0 ? false : true);
                }

                if (!readSchPermission)
                    tabControl.TabPages.Remove(tabScheduledRoutes);
                if (!readPointPermission)
                    tabControl.TabPages.Remove(tabPoints);
                if (!readTerminalPermission)
                    tabControl.TabPages.Remove(tabTerminals);
                if (!readEmployee)
                    tabControl.TabPages.Remove(tabEmployees);
                if (!readMaintenance)
                    tabControl.TabPages.Remove(tabMaintenance);
                if (!readLog)
                    tabControl.TabPages.Remove(tabLogPreview);
                if (!readLogImport)
                    tabControl.TabPages.Remove(tabImportLog);
                btnAdd.Enabled = addPermission;
                btnDelete.Enabled = deletePermission;

                btnSearch.Enabled = readSchPermission;
                btnAssign.Enabled = addSchPermission;
                btnUpdate.Enabled = updateSchPermission;
                btnDeleteSch.Enabled = deleteSchPermission;

                btnGenerate.Enabled = readRepPermission;

                if (routeType.Equals(Constants.routeTerminal))
                {
                    btnPointAdd.Enabled = addPointPermission;
                    btnPointUpd.Enabled = updatePointPermission;
                    btnPointDelete.Enabled = deletePointPermission;

                    btnReaderAdd.Enabled = addTerminalPermission;
                    btnReaderUpd.Enabled = updateTerminalPermission;
                    btnReaderDelete.Enabled = deleteTerminalPermission;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Securityroutes.setVisibility(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void clearSchTab()
        {
            try
            {
                if (routeType.Equals(Constants.routeTag))
                    cbWU.SelectedIndex = 0;
                cbRouteName.SelectedIndex = 0;
                DateTime now = DateTime.Now.Date;
                dtpFromDate.Value = new DateTime(now.Year, now.Month, 1);
                dtpToDate.Value = now;

                lvSchedule.Items.Clear();
                lvDetails.Items.Clear();

                routesSchedules.Clear();

                rbAnalytic.Checked = true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Securityroutes.clearSchTab(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private ArrayList findDtlLogs(SecurityRouteDtl dtl, ArrayList passes)
        {
            ArrayList dtlPasses = new ArrayList();

            try
            {
                foreach (SecurityRoutesLog log in passes)
                {
                    if (log.PointID == dtl.GateID && log.EventTime.TimeOfDay >= dtl.TimeFrom.TimeOfDay
                        && log.EventTime.TimeOfDay <= dtl.TimeTo.TimeOfDay)
                    {
                        dtlPasses.Add(log);
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.findDtlLogs(): " + ex.Message + "\n");
                throw ex;
            }

            return dtlPasses;
        }

        private void setStatusTag(Hashtable routeDetails)
        {
            ArrayList dtlPasses = new ArrayList();
            Dictionary<int, List<PassTO>> passesTable = new Dictionary<int,List<PassTO>>();

            try
            {
                Pass pass1 = new Pass();
                pass1.PssTO.EmployeeID = (int)cbEmpl.SelectedValue;
                List<PassTO> passes = pass1.SearchInterval(dtpFromDate.Value.Date, dtpToDate.Value.Date, "", new DateTime(1, 1, 1, 0, 0, 0), new DateTime(1, 1, 1, 23, 59, 0));

                foreach (PassTO pass in passes)
                {
                    if (!passesTable.ContainsKey(pass.EmployeeID))
                    {
                        passesTable.Add(pass.EmployeeID, new List<PassTO>());
                    }

                    passesTable[pass.EmployeeID].Add(pass);
                }

                foreach (SecurityRouteSchedule sch in routesSchedules)
                {
                    int counter = 0;

                    if (routeDetails.ContainsKey(sch.SecurityRouteID))
                    {
                        foreach (SecurityRouteDtl detail in (ArrayList)routeDetails[sch.SecurityRouteID])
                        {
                            if (passesTable.ContainsKey(sch.EmployeeID))
                            {
                                foreach (PassTO pass in passesTable[sch.EmployeeID])
                                {
                                    if (pass.GateID == detail.GateID &&
                                        pass.EventTime.Date == sch.Date.Date
                                        && pass.EventTime.TimeOfDay >= detail.TimeFrom.TimeOfDay
                                        && pass.EventTime.TimeOfDay <= detail.TimeTo.TimeOfDay)
                                    {
                                        counter++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (sch.Date > DateTime.Now)
                    {
                        sch.Status = Constants.statusScheduled;
                    }
                    else if (counter == 0)
                    {
                        sch.Status = Constants.statusNotCompleted;
                    }
                    else if (counter == ((ArrayList)routeDetails[sch.SecurityRouteID]).Count)
                    {
                        sch.Status = Constants.statusCompleted;
                    }
                    else if (counter < ((ArrayList)routeDetails[sch.SecurityRouteID]).Count)
                    {
                        sch.Status = Constants.statusPartially;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.setStatusTag(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void setStatusTerminal(Hashtable routeDetails)
        {
            ArrayList dtlLogs = new ArrayList();
            Hashtable logsTable = new Hashtable();

            try
            {
                ArrayList logs = new SecurityRoutesLog().SearchInterval((int)cbEmpl.SelectedValue, -1, "",
                    dtpFromDate.Value.Date, dtpToDate.Value.Date, "");

                foreach (SecurityRoutesLog log in logs)
                {
                    if (!logsTable.ContainsKey(log.EmployeeID))
                    {
                        logsTable.Add(log.EmployeeID, new ArrayList());
                    }

                    ((ArrayList)logsTable[log.EmployeeID]).Add(log);
                }

                foreach (SecurityRouteSchedule sch in routesSchedules)
                {
                    int counter = 0;

                    if (routeDetails.ContainsKey(sch.SecurityRouteID))
                    {
                        foreach (SecurityRouteDtl detail in (ArrayList)routeDetails[sch.SecurityRouteID])
                        {
                            if (logsTable.ContainsKey(sch.EmployeeID))
                            {
                                foreach (SecurityRoutesLog log in ((ArrayList)logsTable[sch.EmployeeID]))
                                {
                                    if (log.PointID == detail.GateID &&
                                        log.EventTime.Date == sch.Date.Date
                                        && log.EventTime.TimeOfDay >= detail.TimeFrom.TimeOfDay
                                        && log.EventTime.TimeOfDay <= detail.TimeTo.TimeOfDay)
                                    {
                                        counter++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (sch.Date > DateTime.Now)
                    {
                        sch.Status = Constants.statusScheduled;
                    }
                    else if (counter == 0)
                    {
                        sch.Status = Constants.statusNotCompleted;
                    }
                    else if (counter == ((ArrayList)routeDetails[sch.SecurityRouteID]).Count)
                    {
                        sch.Status = Constants.statusCompleted;
                    }
                    else if (counter < ((ArrayList)routeDetails[sch.SecurityRouteID]).Count)
                    {
                        sch.Status = Constants.statusPartially;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.setStatusTerminal(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void clearPointForm()
        {
            try
            {
                tbPointID.Text = "";
                populatePointNameCombo(cbPointName,false);
                cbPointName.SelectedIndex = 0;
                lvPoints.Items.Clear();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.clearPointForm(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void clearReaderForm()
        {
            try
            {
                tbReaderID.Text = "";
                populateReaderNameCombo(cbReaderName);
                populateReaderNameCombo(cbTerminal);
                cbReaderName.SelectedIndex = 0;
                lvReaders.Items.Clear();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.clearReaderForm(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private int contain(List<ListViewItem> list, ListViewItem item)
        {
            try
            {
                int index = -1;
                bool found = false;

                foreach (ListViewItem listItem in list)
                {
                    index++;

                    if (listItem.Text.Trim().Equals(item.Text))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    index = -1;

                return index;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.contain(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private bool containLogEmpl(string employeeID)
        {
            bool contains = false;
            try
            {
                contains = new SecurityRoutesLog().SearchEmplCount(employeeID) > 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.containLogEmpl(): " + ex.Message + "\n");
                throw ex;
            }

            return contains;
        }

        private void SecurityRoutes_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " SecurityRoutes.SecurityRoutes_KeyUp(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SecurityRoutes.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (cbFilter.SelectedIndex == 0)
                {
                    this.btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                }
                else
                {
                    this.btnSaveCriteria.Text = rm.GetString("UpdateFilter", culture);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + "SecurityRoutes.cbFilter_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter, this.tabControl.SelectedTab);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (tabControl.SelectedIndex == 0)
                    filter.SerachButton = btnSearch;
                if (tabControl.SelectedIndex == 2)
                    filter.SerachButton = btnPointSearch;
                if (tabControl.SelectedIndex == 3)
                    filter.SerachButton = btnReaderSearch;
                if (tabControl.SelectedIndex == 5)
                {
                    pbImport.Value = 0;
                    pbImport.Maximum = 0;
                    lblProgressStatus.Text = "";
                    if (cbTerminal.Items.Count > 1)
                        cbTerminal.SelectedIndex = 1;
                    if (cbEmployee.Items.Count > 1)
                        cbEmployee.SelectedIndex = 1;

                }
                filter.TabID = this.tabControl.SelectedTab.Text;
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnSaveCriteria_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                int XPocketPointer = -1;

                //if (DefaultReader.ReaderID != -1)
                //    cbTerminal.SelectedValue = DefaultReader.ReaderID;
                //else
                //{
                //    this.Cursor = Cursors.Arrow;
                //    MessageBox.Show(rm.GetString("noReaders", culture));
                //    return;
                //}
                //if (DefaultEmployee.EmployeeID != -1)
                //    cbEmployee.SelectedValue = DefaultEmployee.EmployeeID;
                //else
                //{
                //    this.Cursor = Cursors.Arrow;
                //    MessageBox.Show(rm.GetString("noEmployees", culture));
                //    return;
                //}
                bool init = InitializeXPocket();
                if (init)
                {
                    Int16 r;
                    String s = "";
                   
                    int num = NumberOfNewRecord();                   

                    if (num > 0)
                    {
                        this.pbImport.Maximum = num;
                       
                        bool succeeded = true;
                       
                        while (num > 0)
                        {
                            SecurityRoutesLog SRlog = new SecurityRoutesLog();
                            SRlog.EmployeeID = (int)cbEmployee.SelectedValue;
                            SRlog.ReaderID = (int)cbTerminal.SelectedValue;
                            bool trans = SRlog.BeginTransaction();
                            if (trans)
                            {
                                try
                                {                                  
                                                                        
                                    r = xkw.ReqNextTransact2XK(1, 1, sb1);
                                    
                                    if (r == 0)
                                    {
                                        XPocketPointer = XKWrapper.XKWrapper.Hex4_to_Int(sb1.ToString().Substring(0, 4));
                                        s = sb1.ToString();
                                        SRlog.TagID = s.Substring(6, 10);

                                        int year = int.Parse(DateTime.Now.Year.ToString().Substring(0,2)+s.Substring(16, 2));
                                        int month = int.Parse(s.Substring(18, 2));
                                        int day = int.Parse(s.Substring(20, 2));
                                        int hour = int.Parse(s.Substring(22, 2));
                                        int minute = int.Parse(s.Substring(24, 2));
                                        //if datetime is bad just skip log record and continue reading
                                        DateTime dt = new DateTime();
                                        try
                                        {
                                            dt = new DateTime(year, month, day, hour, minute, 0);
                                            string date = s.Substring(20, 2) + "." + s.Substring(18, 2) + "." + s.Substring(16, 2) + " " + s.Substring(22, 2) + ":" + s.Substring(24, 2) + ":00";
                                            log.writeBenchmarkLog(DateTime.Now + " btnImport_Click() DEVICE date = " + date);
                                        }
                                        catch(Exception ex)
                                        {
                                            log.writeLog(DateTime.Now + " SecurityRoutes.btnImport_Click(): " + ex.Message + "; TagID = " + SRlog.TagID + "; EmployeeID = " + SRlog.EmployeeID);
                                            r = xkw.ReqNextPointer2XK(1, 1, s);   // move the downloadable record pointer to the next.
                                            num--;
                                            succeeded = false;
                                            SRlog.RollbackTransaction();
                                            continue;
                                        }

                                        int i = SRlog.Save(SRlog.ReaderID, SRlog.TagID, SRlog.EmployeeID, dt, false);
                                        if (i <= 0)
                                        {
                                            succeeded = false;
                                            SRlog.RollbackTransaction();
                                        }
                                        else
                                        {
                                            r = xkw.ReqNextPointer2XK(1, 1, s);   // move the downloadable record pointer to the next.
                                            if (r == 0)
                                            {
                                                SRlog.CommitTransaction();
                                            }
                                            else
                                            {
                                                SRlog.RollbackTransaction();
                                                succeeded = false;
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        this.Cursor = Cursors.Arrow;
                                        MessageBox.Show(rm.GetString("errorReadingDevice", culture));
                                        SRlog.RollbackTransaction();                                        
                                        return;
                                    }
                                    num--;
                                    if(this.pbImport.Value <this.pbImport.Maximum)
                                    this.pbImport.Value++;
                                    this.lblProgressStatus.Text = this.pbImport.Value + "/" + this.pbImport.Maximum;
                                    this.lblProgressStatus.Refresh();
                                }                                    
                                catch (Exception ex)
                                {
                                    log.writeLog(DateTime.Now + " SecurityRoutes.btnImport_Click(): " + ex.Message + "\n");
                                    MessageBox.Show(ex.Message);
                                    SRlog.RollbackTransaction();
                                }                                
                                                          
                               
                            }
                            else
                            {
                                this.Cursor = Cursors.Arrow;
                                MessageBox.Show(rm.GetString("errorTrans", culture));
                                return;
                            }
                        }
                        SinchDateTimeOnDevice();
                        if (XPocketPointer >= Constants.XPocketDelPoint)
                            DeleteTransactionsfromDevice(); 
                        if (!succeeded)
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("someReadingNotInserted", culture));
                        }
                        else
                        {
                            this.Cursor = Cursors.Arrow;
                            MessageBox.Show(rm.GetString("LogImportSucceeded", culture));                            
                        }

                    }
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("noTransactionsToRead", culture));
                        bool suc = SinchDateTimeOnDevice();
                        if(suc)
                            MessageBox.Show(rm.GetString("timeSinchSucceeded", culture));
                        return;
                    }


                   
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.btnImport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void DeleteTransactionsfromDevice()
        {
            try
            {
                Int16 r;
                
                r = xkw.DeleteTransactBufferXK(1, 1);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.DeleteTransactionsfromDevice(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private bool SinchDateTimeOnDevice()
        {
            bool sinch = false;
            try 
            {
                Int32 r = xkw.ReqDateTimeXK(1, 1, sb1);
                DateTime dt = new DateTime();
                if (r == 0)
                {
                    string s = sb1.ToString();
                    try
                    {
                        try
                        {
                            dt = new DateTime(int.Parse(s.Substring(0, 4)), int.Parse(s.Substring(4, 2)), int.Parse(s.Substring(6, 2))
                                , Int16.Parse(s.Substring(8, 2)), int.Parse(s.Substring(10, 2)), int.Parse(s.Substring(12, 2)));
                        }
                        catch { }
                        StringBuilder sbDateTime = new StringBuilder();
                        sbDateTime.Append(DateTime.Now.ToString("yyyyMMddHHmmss"));
                        //sbDateTime.Append("16000000080100");
                        r = xkw.SetDateTimeXK(1, 1,sbDateTime);

                        if (r == 0)
                            sinch = true;
                    }
                    catch
                    {
                        sinch = false;
                    }
                }
                else
                    sinch = false;
                if (sinch)
                {
                    log.writeLog(DateTime.Now + " SecurityRoutes.SinchDateTimeOnDevice():Sinch succeded, Prev DEVICE Datetime: " + dt);

                }
                else
                    MessageBox.Show(rm.GetString("sichDateTimeFaild", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.SinchDateTimeOnDevice(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return sinch;
        }

        private bool sendPointer(int pointer)
        {
             bool saved = false;
             try
             {
                 if (File.Exists(Constants.XKPointerFilePath))
                     File.Delete(Constants.XKPointerFilePath);
                 // Write the string to a file.
                 FileStream fs = new System.IO.FileStream(Constants.XKPointerFilePath, FileMode.OpenOrCreate);
                 //System.IO.StreamWriter file = new System.IO.StreamWriter(fs,Encoding.Unicode);
                 System.IO.StreamWriter file = new System.IO.StreamWriter(fs, Encoding.Unicode);
                 file.WriteLine(pointer);
                 file.Close();
                 fs.Close();
                 saved = true;
             }
             catch (Exception ex)
             {
                 log.writeLog(DateTime.Now + " SecurityRoutes.getPointer(): " + ex.Message + "\n");
                 MessageBox.Show(ex.Message);
             }
             return saved;
        }

        private int getPointer()
        {
            int pointer = -1;
            try
            {
                if (File.Exists(Constants.XKPointerFilePath))
                {
                    System.IO.FileStream fs = new FileStream(Constants.XKPointerFilePath, FileMode.Open);
                    StreamReader reader = new StreamReader(fs);
                    string point = reader.ReadLine();
                    bool isNum = int.TryParse(point, out pointer);
                    reader.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.getPointer(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return pointer;
        }

        private int NumberOfNewRecord()
        {
            int recordNum = 0;
            try
            {
                Int16 r;

                r = xkw.ReqTransactNum2XK(1, 1, sb1);
                if (r == 0)
                {
                    r = XKWrapper.XKWrapper.Hex4_to_Int(sb1.ToString());
                    recordNum = r;
                }
                else
                {
                    recordNum = r;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.InitializeXPocket(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return recordNum;
        }


        private bool InitializeXPocket()
        {
            bool init = false;
            try
            {
                //initialize wrapper
                xkw = new XKWrapper.XKWrapper();
                //initialize XKdll
                xkw.Init();
                //find devices, this will work for one at the time
                nDevices = xkw.FindDevices();
                //if device is found
                if (nDevices > 0)
                {
                    if (nDevices == 1)
                    {
                        bool ret = false;
                        Int16 n;
                        for (n = 0; n < nDevices; n++)  //Get the first one oly
                        {
                            ret = xkw.GetName(n, sb1, sb2);
                            if (ret)
                            {
                                //open USB connecton with XPocket
                                if (xkw.SetChannelXK(1, ",,0.0.0.0,,,,1,3") == 0)
                                {
                                    string s1 = sb2.ToString();
                                    s1 = s1.Substring(s1.IndexOf(' ') + 1);
                                    Int16 num = xkw.SetUsbParameters(1, s1);
                                    if (num == 0)
                                    {
                                        num = xkw.OpenChannelXK(1);
                                        if (num == 0)
                                        {
                                            Connected = true;
                                            s1 = XKWrapper.XKWrapper.ToHexString("00000000");
                                            if (xkw.SendMdcSerialXK(1, 1, s1) != 0)
                                            {
                                                xkw.CloseChannelXK(1);
                                                Connected = false;
                                            }
                                        }
                                    }

                                }
                            }
                            // The connected condition is when the "program user" knows the XPocket password.
                            if (!Connected)
                            {
                                MessageBox.Show(rm.GetString("InitConnectionFaild", culture));
                                init = false;
                            }
                            else
                            {
                                init = true;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("MoreThenOneXPocket", culture));
                        init = false;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("NoXPocketFound", culture));
                    init = false;
                }


            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.InitializeXPocket(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return init;
        }

        private void cbWULog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool check = false;
                if (!chbHierarhicly.Checked)
                {
                    foreach (WorkingUnitTO wu in wUnits)
                    {
                        if (cbWULog.SelectedIndex != 0)
                        {
                            if (wu.WorkingUnitID == (int)cbWULog.SelectedValue && wu.EmplNumber == 0 && wu.ChildWUNumber > 0)
                            {
                                chbHierarhiclyLog.Checked = true;
                                check = true;
                            }
                        }
                    }
                }
                if (cbWULog.SelectedValue is int && !check)
                {
                    populateEmployeeLogCombo((int)cbWULog.SelectedValue);
                }
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.cbWULog_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }           
        }

        private void chbHierarhiclyLog_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (cbWULog.SelectedValue is int)
                {
                    if ((int)cbWULog.SelectedValue >= 0)
                        populateEmployeeLogCombo((int)cbWULog.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSearchLog_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                SecurityRoutesLog srl = new SecurityRoutesLog();
                int EmployeeID = (int)cbEmployeeLog.SelectedValue;
                string PointID = (string) cbPointLog.SelectedValue;
                //int workingUnitID = (int)cbWULog.SelectedValue;
                string wuStr = "";
                //if (workingUnitID >= 0)
                //{
                //    if (chbHierarhiclyLog.Checked)
                //    {
                //        ArrayList wuList = new ArrayList();
                //        WorkingUnit workUnit = new WorkingUnit();
                //        foreach (WorkingUnit workingUnit in wUnits)
                //        {
                //            if (workingUnit.WorkingUnitID == (int)this.cbWULog.SelectedValue)
                //            {
                //                wuList.Add(workingUnit);
                //                workUnit = workingUnit;
                //            }
                //        }
                //        if (workUnit.ChildWUNumber > 0)
                //            wuList = workUnit.FindAllChildren(wuList);
                //        wuStr = "";
                //        foreach (WorkingUnit wunit in wuList)
                //        {
                //            wuStr += wunit.WorkingUnitID.ToString().Trim() + ",";
                //        }

                //        if (wuStr.Length > 0)
                //        {
                //            wuStr = wuStr.Substring(0, wuStr.Length - 1);
                //        }
                //    }
                //}
                DateTime fromTime  = new DateTime();
                DateTime toTime  = new DateTime();
                if(dtpFromTimeLog.Value.Hour != 0 || dtpFromTimeLog.Value.Minute != 0)
                fromTime = dtpFromTimeLog.Value;
                if(dtpToTimeLog.Value.Hour != 23 || dtpToTimeLog.Value.Minute != 59)
                toTime = dtpToTimeLog.Value;

            currentLogList = srl.Search(EmployeeID, PointID, wuStr, dtpFromDateLog.Value.Date, dtpToDateLog.Value.Date, fromTime, toTime);

                populateListViewLog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutes.chbHierarhicly_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void gbSearchLog_Enter(object sender, EventArgs e)
        {

        }

        private void lvLogs_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvLogs.Sorting;

                if (e.Column == _comp7.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvLogs.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvLogs.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp7.SortColumn = e.Column;
                    lvLogs.Sorting = SortOrder.Ascending;
                }

                lvLogs.Sort();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.lvLogs_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            try
            { 
                bool init = InitializeXPocket();
                if (init)
                {
                    //Int16 r;
                    //String s = "";

                    this.Cursor = Cursors.Arrow;
                    bool suc = SinchDateTimeOnDevice();
                    if (suc)
                        MessageBox.Show(rm.GetString("timeSinchSucceeded", culture));
                    else
                    {
                        MessageBox.Show(rm.GetString("timeSinchSucceededFaild", culture));
                    }
                }
 
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutes.btnSetTime_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
           
    }
}