using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Collections;

using Util;
using Common;
using TransferObjects;

namespace Reports
{
    public partial class VisitorReports : Form
    {
        Visit currentVisit = null;

        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        private ListViewItemComparer1 _comp1;
        private ListViewItemComparer2 _comp2;

        // List View indexes
        const int TagIndex1 = 0;
        const int FirstNameIndex1 = 1;
        const int LastNameIndex1 = 2;
        const int JMBGIndex1 = 3;
        const int DescrIndex1 = 4;
        const int DateStartIndex1 = 5;
        const int WUIndex1 = 6;
        const int PersonIndex1 = 7;
        const int DateEndIndex1 = 8;

        const int TagIndex2 = 0;
        const int FirstNameIndex2 = 1;
        const int LastNameIndex2 = 2;
        const int JMBGIndex2 = 3;
        const int DescrIndex2 = 4;
        const int DateStartIndex2 = 5;
        const int DateEndIndex2 = 6;
        const int WUIndex2 = 7;
        const int PersonIndex2 = 8;

        private List<WorkingUnitTO> workingunits = new List<WorkingUnitTO>();
        private string wuStringVisitor = "";
        private string wuString = "";
        private List<EmployeeTO> visitedEmployees = new List<EmployeeTO>();
        private string[] statuses = { Constants.statusActive, Constants.statusBlocked };

        ArrayList currentVisitorsList;

        #region Inner Class for sorting Array List of Employees

        /*
		 *  Class used for sorting Array List of Employees
		*/

        private class ArrayListSort : IComparer
        {
            ResourceManager rm;
            private CultureInfo culture;

            private int compOrder;
            private int compField;

            public ArrayListSort(int sortOrder, int sortField)
            {
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(VisitorReports).Assembly);

                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(object x, object y)
            {
                Visit vis1 = null;
                Visit vis2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    vis1 = (Visit)x;
                    vis2 = (Visit)y;
                }
                else
                {
                    vis1 = (Visit)y;
                    vis2 = (Visit)x;
                }

                switch (compField)
                {
                    case VisitorReports.TagIndex2:
                        {
                            string tag1 = vis1.EmployeeLastName + " " + vis1.EmployeeFirstName;
                            string tag2 = vis2.EmployeeLastName + " " + vis2.EmployeeFirstName;

                            return tag1.CompareTo(tag2);
                        }
                    case VisitorReports.FirstNameIndex2:
                        return vis1.FirstName.CompareTo(vis2.FirstName);
                    case VisitorReports.LastNameIndex2:
                        return vis1.LastName.CompareTo(vis2.LastName);
                    case VisitorReports.JMBGIndex2:
                        return vis1.VisitorJMBG.CompareTo(vis2.VisitorJMBG);
                    case VisitorReports.DescrIndex2:
                        {
                            string vis1Descr = vis1.VisitDescr.Trim();
                            if (vis1Descr == Constants.visitorPrivate)
                            {
                                vis1Descr = rm.GetString("privatePurpose", culture);
                            }
                            else if (vis1Descr == Constants.visitorOfficial)
                            {
                                vis1Descr = rm.GetString("officialPurpose", culture);
                            }
                            else if (vis1Descr == Constants.visitorOther)
                            {
                                vis1Descr = rm.GetString("otherPurpose", culture);
                            }
                            string vis2Descr = vis2.VisitDescr.Trim();
                            if (vis2Descr == Constants.visitorPrivate)
                            {
                                vis2Descr = rm.GetString("privatePurpose", culture);
                            }
                            else if (vis2Descr == Constants.visitorOfficial)
                            {
                                vis2Descr = rm.GetString("officialPurpose", culture);
                            }
                            else if (vis2Descr == Constants.visitorOther)
                            {
                                vis2Descr = rm.GetString("otherPurpose", culture);
                            }
                            return vis1Descr.CompareTo(vis2Descr);
                        }
                    case VisitorReports.DateStartIndex2:
                        return vis1.DateStart.CompareTo(vis2.DateStart);
                    case VisitorReports.DateEndIndex2:
                        return vis1.DateEnd.CompareTo(vis2.DateEnd);
                    case VisitorReports.WUIndex2:
                        return vis1.WUName.CompareTo(vis2.WUName);
                    case VisitorReports.PersonIndex2:
                        {
                            string per1 = vis1.VisitedLastName + " " + vis1.VisitedFirstName;
                            string per2 = vis2.VisitedLastName + " " + vis2.VisitedFirstName;

                            return per1.CompareTo(per2);
                        }
                    default:
                        return vis1.FirstName.CompareTo(vis2.FirstName);
                }
            }
        }

        #endregion

        public VisitorReports()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentVisit = new Visit();
                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("Reports.ReportResource", typeof(VisitorReports).Assembly);
                setLanguage();

                currentVisitorsList = new ArrayList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Inner Class1 for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		 */
        private class ListViewItemComparer1 : IComparer
        {
            private ListView _listView;

            public ListViewItemComparer1(ListView lv)
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
                    case VisitorReports.TagIndex1:
                    case VisitorReports.FirstNameIndex1:
                    case VisitorReports.LastNameIndex1:
                    case VisitorReports.JMBGIndex1:
                    case VisitorReports.WUIndex1:
                    case VisitorReports.PersonIndex1:
                    case VisitorReports.DescrIndex1:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text.Trim(), sub2.Text.Trim());
                        }
                    case VisitorReports.DateStartIndex1:
                    case VisitorReports.DateEndIndex1:
                        {

                            DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                            DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
                            }

                            return CaseInsensitiveComparer.Default.Compare(dt1, dt2);

                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        #region Inner Class2 for sorting items in View List

        /*
		 *  Class used for sorting items in the List View 
		 */
        private class ListViewItemComparer2 : IComparer
        {
            private ListView _listView;

            public ListViewItemComparer2(ListView lv)
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
                    case VisitorReports.TagIndex2:
                    case VisitorReports.FirstNameIndex2:
                    case VisitorReports.LastNameIndex2:
                    case VisitorReports.JMBGIndex2:
                    case VisitorReports.WUIndex2:
                    case VisitorReports.PersonIndex2:
                    case VisitorReports.DescrIndex2:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text.Trim(), sub2.Text.Trim());
                        }
                    case VisitorReports.DateStartIndex2:
                    case VisitorReports.DateEndIndex2:
                        {

                            DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                            DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
                            }

                            return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
                        }
                    default:
                        throw new IndexOutOfRangeException("Unrecognized column name extension");
                }
            }
        }

        #endregion

        /// <summary>
        /// Set proper language and initialize List View
        /// </summary>
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("visitorsForm", culture);

                // TabPage text
                tabPageCurrentVisitors.Text = rm.GetString("tabPageCurrentVisitors", culture);
                tabPageVisitSearch.Text = rm.GetString("tabPageVisitSearch", culture);

                // group box text
                gbVisit.Text = rm.GetString("gbVisit", culture);

                // button's text
                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnDetails.Text = rm.GetString("btnDetails", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnScanDocument.Text = rm.GetString("btnScanDocument", culture);
                btnReport.Text = rm.GetString("btnReport", culture);

                // label's text				
                lblCardNum.Text = rm.GetString("lblCardNum", culture);
                lblWU.Text = rm.GetString("lblWorkingUnit", culture);
                lblVisitor.Text = rm.GetString("lblVisitor", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
                lblVisitDescr.Text = rm.GetString("lblVisitDescr", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);

                // list view
                lvVisitorsView.BeginUpdate();
                lvVisitorsView.Columns.Add(rm.GetString("hdrTag", culture), lvVisitorsView.Width / 8, HorizontalAlignment.Left);
                lvVisitorsView.Columns.Add(rm.GetString("hdrFirstName", culture), lvVisitorsView.Width / 8, HorizontalAlignment.Left);
                lvVisitorsView.Columns.Add(rm.GetString("hdrLastName", culture), lvVisitorsView.Width / 8, HorizontalAlignment.Left);
                lvVisitorsView.Columns.Add(rm.GetString("hdrJMBGID", culture), lvVisitorsView.Width / 8, HorizontalAlignment.Left);
                lvVisitorsView.Columns.Add(rm.GetString("hdrPurpose", culture), lvVisitorsView.Width / 8, HorizontalAlignment.Left);
                lvVisitorsView.Columns.Add(rm.GetString("hdrDateStart", culture), (lvVisitorsView.Width / 8) + 25, HorizontalAlignment.Left);
                lvVisitorsView.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvVisitorsView.Width / 8) + 100, HorizontalAlignment.Left);
                lvVisitorsView.Columns.Add(rm.GetString("hdrEmployee", culture), lvVisitorsView.Width / 8, HorizontalAlignment.Left);
                lvVisitorsView.EndUpdate();

                lvVisitorsViewSearch.BeginUpdate();
                lvVisitorsViewSearch.Columns.Add(rm.GetString("hdrTag", culture), lvVisitorsViewSearch.Width / 8, HorizontalAlignment.Left);
                lvVisitorsViewSearch.Columns.Add(rm.GetString("hdrFirstName", culture), lvVisitorsViewSearch.Width / 8, HorizontalAlignment.Left);
                lvVisitorsViewSearch.Columns.Add(rm.GetString("hdrLastName", culture), lvVisitorsViewSearch.Width / 8, HorizontalAlignment.Left);
                lvVisitorsViewSearch.Columns.Add(rm.GetString("hdrJMBGID", culture), lvVisitorsViewSearch.Width / 8, HorizontalAlignment.Left);
                lvVisitorsViewSearch.Columns.Add(rm.GetString("hdrPurpose", culture), lvVisitorsViewSearch.Width / 8, HorizontalAlignment.Left);
                lvVisitorsViewSearch.Columns.Add(rm.GetString("hdrDateStart", culture), (lvVisitorsViewSearch.Width / 8) + 25, HorizontalAlignment.Left);
                lvVisitorsViewSearch.Columns.Add(rm.GetString("hdrDateEnd", culture), (lvVisitorsViewSearch.Width / 8) + 25, HorizontalAlignment.Left);
                lvVisitorsViewSearch.Columns.Add(rm.GetString("hdrWorkingUnit", culture), (lvVisitorsViewSearch.Width / 8) + 100, HorizontalAlignment.Left);
                lvVisitorsViewSearch.Columns.Add(rm.GetString("hdrEmployee", culture), lvVisitorsViewSearch.Width / 8, HorizontalAlignment.Left);
                lvVisitorsViewSearch.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateCardNumCombo()
        {
            try
            {
                Employee empl = new Employee();
                List<EmployeeTO> emplArrayCombo = new List<EmployeeTO>();

                List<string> status = new List<string>();
                emplArrayCombo = empl.SearchVisitors(wuStringVisitor, status, "");

                foreach (EmployeeTO employee in emplArrayCombo)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl1 = new EmployeeTO();
                empl1.LastName = rm.GetString("all", culture);
                emplArrayCombo.Insert(0, empl1);

                cbCardNum.DataSource = emplArrayCombo;
                cbCardNum.DisplayMember = "LastName";
                cbCardNum.ValueMember = "EmployeeID";

                cbCardNum.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.populateCardNumCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateWorkingUnitCombo()
        {
            try
            {
                workingunits.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

                cbWorkingUnit.DataSource = workingunits;
                cbWorkingUnit.DisplayMember = "Name";
                cbWorkingUnit.ValueMember = "WorkingUnitID";

                cbWorkingUnit.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.populateWorkingUnitCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateEmployeeCombo(int wuID)
        {
            try
            {
                Employee empl = new Employee();
                List<EmployeeTO> emplArrayCombo = new List<EmployeeTO>();

                List<string> status = new List<string>();
                if (wuID < 0)
                {
                    emplArrayCombo = empl.SearchByWUWithStatuses(wuString, status);
                }
                else
                {
                    emplArrayCombo = empl.SearchWithStatuses(status, wuID.ToString().Trim());
                }

                foreach (EmployeeTO employee in emplArrayCombo)
                {
                    employee.LastName += " " + employee.FirstName;
                }

                EmployeeTO empl1 = new EmployeeTO();
                empl1.LastName = rm.GetString("all", culture);
                emplArrayCombo.Insert(0, empl1);

                cbPerson.DataSource = emplArrayCombo;
                cbPerson.DisplayMember = "LastName";
                cbPerson.ValueMember = "EmployeeID";

                cbPerson.SelectedIndex = 0;

                cbPerson.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.populateEmployeeCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateVisitorCombo()
        {
            try
            {
                ArrayList visitorCombo = currentVisit.SearchAll(wuString);

                foreach (Visit visit in visitorCombo)
                {
                    visit.LastName += " " + visit.FirstName;
                }

                Visit visitALL = new Visit();
                visitALL.LastName = rm.GetString("all", culture);
                visitorCombo.Insert(0, visitALL);

                cbVisitor.DataSource = visitorCombo;
                cbVisitor.DisplayMember = "LastName";
                cbVisitor.ValueMember = "VisitorID";

                cbVisitor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.populateVisitorCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateVisitDescrCombo()
        {
            try
            {
                ArrayList purposeList = new ArrayList();
                purposeList.Add(rm.GetString("all", culture));
                purposeList.Add(rm.GetString("privatePurpose", culture));
                purposeList.Add(rm.GetString("officialPurpose", culture));
                purposeList.Add(rm.GetString("otherPurpose", culture));

                cbVisitDescr.DataSource = purposeList;
                cbVisitDescr.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.populateVisitDescrCombo(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with earned hours found
        /// </summary>
        /// <param name="accessGroupsList"></param>
        private void populateVisitorsListView()
        {
            try
            {
                lvVisitorsView.BeginUpdate();
                lvVisitorsView.Items.Clear();

                ArrayList visitsList = currentVisit.GetCurrentVisitsDetail(wuString);

                if (visitsList.Count > 0)
                {
                    foreach (Visit visit in visitsList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = visit.EmployeeLastName.ToString() + " " + visit.EmployeeFirstName.ToString();
                        item.SubItems.Add(visit.FirstName.Trim());
                        item.SubItems.Add(visit.LastName.Trim());
                        if (visit.VisitorJMBG.Trim() != "")
                            item.SubItems.Add(visit.VisitorJMBG.Trim());
                        else
                            item.SubItems.Add(visit.VisitorID.Trim());

                        string visitDescr = visit.VisitDescr.Trim();
                        if (visitDescr == Constants.visitorPrivate)
                        {
                            visitDescr = rm.GetString("privatePurpose", culture);
                        }
                        else if (visitDescr == Constants.visitorOfficial)
                        {
                            visitDescr = rm.GetString("officialPurpose", culture);
                        }
                        else if (visitDescr == Constants.visitorOther)
                        {
                            visitDescr = rm.GetString("otherPurpose", culture);
                        }
                        item.SubItems.Add(visitDescr);

                        item.SubItems.Add(visit.DateStart.ToString("dd.MM.yyyy   HH:mm").Trim());
                        item.SubItems.Add(visit.WUName.Trim());
                        item.SubItems.Add(visit.VisitedLastName.ToString() + " " + visit.VisitedFirstName.ToString());

                        item.Tag = visit.Remarks.Trim();

                        lvVisitorsView.Items.Add(item);
                    }
                }

                lvVisitorsView.EndUpdate();
                lvVisitorsView.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.populateVisitorsListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with used hours found
        /// </summary>
        /// <param name="accessGroupsList"></param>
        private void populateVisitorsSearchListView(ArrayList visitsSearchList)
        {
            try
            {
                lvVisitorsViewSearch.BeginUpdate();
                lvVisitorsViewSearch.Items.Clear();

                if (visitsSearchList.Count > 0)
                {
                    foreach (Visit visit in visitsSearchList)
                    {
                        foreach (EmployeeTO employee in visitedEmployees)
                        {
                            if (employee.EmployeeID == visit.VisitedPerson)
                            {
                                visit.VisitedFirstName = employee.FirstName;
                                visit.VisitedLastName = employee.LastName;
                                break;
                            }
                        }

                        ListViewItem item = new ListViewItem();
                        item.Text = visit.EmployeeLastName.ToString() + " " + visit.EmployeeFirstName.ToString();
                        item.SubItems.Add(visit.FirstName.Trim());
                        item.SubItems.Add(visit.LastName.Trim());
                        if (visit.VisitorJMBG.Trim() != "")
                            item.SubItems.Add(visit.VisitorJMBG.Trim());
                        else
                            item.SubItems.Add(visit.VisitorID.Trim());

                        string visitDescr = visit.VisitDescr.Trim();
                        if (visitDescr == Constants.visitorPrivate)
                        {
                            visitDescr = rm.GetString("privatePurpose", culture);
                        }
                        else if (visitDescr == Constants.visitorOfficial)
                        {
                            visitDescr = rm.GetString("officialPurpose", culture);
                        }
                        else if (visitDescr == Constants.visitorOther)
                        {
                            visitDescr = rm.GetString("otherPurpose", culture);
                        }
                        item.SubItems.Add(visitDescr);

                        item.SubItems.Add(visit.DateStart.ToString("dd.MM.yyyy   HH:mm").Trim());
                        if (visit.DateEnd != new DateTime())
                            item.SubItems.Add(visit.DateEnd.ToString("dd.MM.yyyy   HH:mm").Trim());
                        else
                            item.SubItems.Add("");

                        item.SubItems.Add(visit.WUName.Trim());
                        item.SubItems.Add(visit.VisitedLastName.ToString() + " " + visit.VisitedFirstName.ToString());

                        Visit visitTag = new Visit();
                        visitTag.Remarks = visit.Remarks;
                        visitTag.VisitID = visit.VisitID;
                        visitTag.LastName = visit.LastName;
                        visitTag.FirstName = visit.FirstName;
                        item.Tag = visitTag;

                        lvVisitorsViewSearch.Items.Add(item);
                    }
                }

                lvVisitorsViewSearch.EndUpdate();
                lvVisitorsViewSearch.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.populateVisitorsSearchListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void clearListView()
        {
            lvVisitorsViewSearch.BeginUpdate();
            lvVisitorsViewSearch.Items.Clear();
            lvVisitorsViewSearch.EndUpdate();
            lvVisitorsViewSearch.Invalidate();
        }

        private void VisitorReports_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp1 = new ListViewItemComparer1(lvVisitorsView);
                _comp1.SortColumn = DateStartIndex1;
                lvVisitorsView.ListViewItemSorter = _comp1;
                lvVisitorsView.Sorting = SortOrder.Descending;

                _comp2 = new ListViewItemComparer2(lvVisitorsViewSearch);
                _comp2.SortColumn = DateStartIndex2;
                lvVisitorsViewSearch.ListViewItemSorter = _comp2;
                lvVisitorsViewSearch.Sorting = SortOrder.Descending;

                DateTime date = DateTime.Now.Date;
                dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
                dtpToDate.Value = date;

                List<WorkingUnitTO> wuActive = new List<WorkingUnitTO>();
                if (logInUser != null)
                {
                    wuActive = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                }

                WorkingUnit workingunit = new WorkingUnit();
                workingunit.WUTO.WorkingUnitID = Constants.basicVisitorCode;
                List<WorkingUnitTO> wUnitsVisitors = workingunit.Search();
                wUnitsVisitors = workingunit.FindAllChildren(wUnitsVisitors);

                foreach (WorkingUnitTO wu in wuActive)
                {
                    bool visitWU = false;
                    foreach (WorkingUnitTO wuV in wUnitsVisitors)
                    {
                        if (wuV.WorkingUnitID == wu.WorkingUnitID)
                        {
                            visitWU = true;
                            break;
                        }
                    }
                    if (!visitWU)
                    {
                        workingunits.Add(wu);
                        wuString += wu.WorkingUnitID.ToString().Trim() + ",";
                    }
                }
                if (wuString.Length > 0)
                {
                    wuString = wuString.Substring(0, wuString.Length - 1);
                }

                foreach (WorkingUnitTO wuV in wUnitsVisitors)
                {
                    wuStringVisitor += wuV.WorkingUnitID.ToString().Trim() + ",";
                }
                if (wuStringVisitor.Length > 0)
                {
                    wuStringVisitor = wuStringVisitor.Substring(0, wuStringVisitor.Length - 1);
                }

                visitedEmployees = new Employee().SearchByWU(wuString);

                string message = ValidateStrings();
                if (!message.Equals(""))
                {
                    MessageBox.Show(message);
                    return;
                }

                populateCardNumCombo();
                populateWorkingUnitCombo();
                populateVisitorCombo();
                populateVisitDescrCombo();

                populateVisitorsListView();

                if (Common.Misc.isADMINRole(logInUser.UserID))
                {
                    btnScanDocument.Enabled = true;
                }
                else
                {
                    btnScanDocument.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.VisitorReports_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private string ValidateStrings()
        {
            StringBuilder sb = new StringBuilder();

            if (wuStringVisitor == "")
            {
                sb.Append("\n" + rm.GetString("noVisitors", culture) + ", ");
            }
            if (wuString == "")
            {
                sb.Append("\n" + rm.GetString("noWUGranted", culture) + ", ");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 2, 2);
            }

            return sb.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //ArrayList visitsSearchList = new ArrayList();
                int count = 0;

                string message = ValidateStrings();
                if (message.Equals(""))
                {
                    string employeeID = "";
                    if (cbCardNum.SelectedIndex >= 0 && (int)cbCardNum.SelectedValue >= 0)
                    {
                        employeeID = cbCardNum.SelectedValue.ToString();
                    }

                    string visitedWorkingUnit = "";
                    if (cbWorkingUnit.SelectedIndex >= 0 && (int)cbWorkingUnit.SelectedValue >= 0)
                    {
                        visitedWorkingUnit = cbWorkingUnit.SelectedValue.ToString();
                    }

                    string visitedPerson = "";
                    if (cbPerson.SelectedIndex >= 0 && (int)cbPerson.SelectedValue >= 0)
                    {
                        visitedPerson = cbPerson.SelectedValue.ToString();
                    }

                    string visitorIdent = "";
                    if (cbVisitor.SelectedIndex >= 0 && (cbVisitor.SelectedValue.ToString() != ""))
                    {
                        visitorIdent = cbVisitor.SelectedValue.ToString();
                    }

                    string visitDescr = "";
                    if (cbVisitDescr.SelectedIndex >= 0 && (cbVisitDescr.SelectedValue.ToString() != rm.GetString("all", culture)))
                    {
                        visitDescr = cbVisitDescr.SelectedValue.ToString();
                        if (visitDescr == rm.GetString("privatePurpose", culture))
                        {
                            visitDescr = Constants.visitorPrivate;
                        }
                        else if (visitDescr == rm.GetString("officialPurpose", culture))
                        {
                            visitDescr = Constants.visitorOfficial;
                        }
                        else if (visitDescr == rm.GetString("otherPurpose", culture))
                        {
                            visitDescr = Constants.visitorOther;
                        }
                    }

                    count = currentVisit.SearchDateIntervalCount(employeeID, visitedWorkingUnit,
                                visitedPerson, visitorIdent, dtpFromDate.Value.Date, dtpToDate.Value.Date,
                                visitDescr, wuString);

                    if (count > Constants.maxRecords)
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("resultsGreaterThenAllowed", culture), "", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            //visitsSearchList = currentVisit.SearchDateInterval(employeeID, visitedWorkingUnit,
                            //    visitedPerson, visitorIdent, dtpFromDate.Value.Date, dtpToDate.Value.Date,
                            //    visitDescr, wuString);
                            currentVisitorsList = currentVisit.SearchDateInterval(employeeID, visitedWorkingUnit,
                                visitedPerson, visitorIdent, dtpFromDate.Value.Date, dtpToDate.Value.Date,
                                visitDescr, wuString);
                        }
                        else
                        {
                            currentVisitorsList.Clear();
                        }
                    }
                    else
                    {
                        if (count > 0)
                        {
                            //visitsSearchList = currentVisit.SearchDateInterval(employeeID, visitedWorkingUnit,
                            //   visitedPerson, visitorIdent, dtpFromDate.Value.Date, dtpToDate.Value.Date,
                            //    visitDescr, wuString);
                            currentVisitorsList = currentVisit.SearchDateInterval(employeeID, visitedWorkingUnit,
                                visitedPerson, visitorIdent, dtpFromDate.Value.Date, dtpToDate.Value.Date,
                                visitDescr, wuString);
                        }
                        else
                        {
                            currentVisitorsList.Clear();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(message);
                    return;
                }

                //if (visitsSearchList.Count > 0)
                if (currentVisitorsList.Count > 0)
                {
                    populateVisitorsSearchListView(currentVisitorsList);
                    currentVisitorsList.Sort(new ArrayListSort(Constants.sortDesc, DateStartIndex2));
                }
                else
                {
                    clearListView();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvVisitorsViewSearch.SelectedItems.Count == 1)
                {
                    //Tag containes Visit object with VisitID, First Name, Last Name and Remarks properties
                    Visit visitTag = (Visit)lvVisitorsViewSearch.SelectedItems[0].Tag;

                    Reports.VisitorReportsDetails visitorReportsDetails = new Reports.VisitorReportsDetails(visitTag.VisitID,
                        visitTag.LastName, visitTag.FirstName);
                    visitorReportsDetails.ShowDialog(this);
                }
                else
                    MessageBox.Show(rm.GetString("visitNotSelected", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.btnDetails_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbWorkingUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbWorkingUnit.SelectedValue is int)
                {
                    populateEmployeeCombo((int)cbWorkingUnit.SelectedValue);
                }
                else
                {
                    populateEmployeeCombo(-1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.cbWorkingUnit_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvVisitorsView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvVisitorsView.Sorting;

                if (e.Column == _comp1.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvVisitorsView.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvVisitorsView.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp1.SortColumn = e.Column;
                    lvVisitorsView.Sorting = SortOrder.Ascending;
                }
                lvVisitorsView.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.lvVisitorsView_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvVisitorsViewSearch_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvVisitorsViewSearch.Sorting;
                int sortOrder = Constants.sortAsc;

                if (e.Column == _comp2.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvVisitorsViewSearch.Sorting = SortOrder.Descending;
                        sortOrder = Constants.sortDesc;
                    }
                    else
                    {
                        lvVisitorsViewSearch.Sorting = SortOrder.Ascending;
                        sortOrder = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp2.SortColumn = e.Column;
                    lvVisitorsViewSearch.Sorting = SortOrder.Ascending;
                    sortOrder = Constants.sortAsc;
                }
                lvVisitorsViewSearch.Sort();
                currentVisitorsList.Sort(new ArrayListSort(sortOrder, e.Column));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.lvVisitorsViewSearch_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvVisitorsView_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvVisitorsView.SelectedItems.Count == 1)
                {
                    //Tag containes remarks
                    rtbRemarks.Text = lvVisitorsView.SelectedItems[0].Tag.ToString();
                }
                else
                    rtbRemarks.Text = "";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.lvVisitorsView_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvVisitorsViewSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvVisitorsViewSearch.SelectedItems.Count == 1)
                {
                    //Tag containes Visit object with EmployeeID and Remarks properties
                    Visit visitTag = (Visit)lvVisitorsViewSearch.SelectedItems[0].Tag;
                    rtbRemarksSearch.Text = visitTag.Remarks;
                }
                else
                    rtbRemarksSearch.Text = "";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.lvVisitorsViewSearch_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbVisitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.toolTip1.Hide(this.cbVisitor);
            this.toolTip1.Show(this.cbVisitor.SelectedValue.ToString(), this.cbVisitor, 0, this.cbVisitor.Height - 40, 5000);
        }

        private void cbVisitor_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.Show(this.cbVisitor.SelectedValue.ToString(), this.cbVisitor, 0, this.cbVisitor.Height - 40, 5000);
        }

        private void cbVisitor_MouseLeave(object sender, EventArgs e)
        {
            this.toolTip1.Hide(this.cbVisitor);
        }

        private void VisitorReports_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " VisitorReports.VisitorReports_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnScanDocument_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvVisitorsViewSearch.SelectedItems.Count > 0)
                {
                    //Tag containes Visit object with VisitID, First Name, Last Name and Remarks properties
                    ListViewItem item = lvVisitorsViewSearch.SelectedItems[0];
                    Visit visitTag = (Visit)item.Tag;

                    VisitorReportsScanDocument visitorReportsScanDoc = new VisitorReportsScanDocument(visitTag.VisitID,
                        item.Text, item.SubItems[1].Text, item.SubItems[2].Text, item.SubItems[3].Text,
                        item.SubItems[4].Text, item.SubItems[5].Text, item.SubItems[6].Text, item.SubItems[7].Text,
                        item.SubItems[8].Text);
                    visitorReportsScanDoc.ShowDialog(this);
                }
                else
                    MessageBox.Show(rm.GetString("visitNotSelected", culture));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.btnScanDocument_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvVisitorsViewSearch.Items.Count > 0)
                {
                    // Table Definition for Crystal Reports
                    DataSet dataSetCR = new DataSet();
                    DataTable tableCR = new DataTable("visits");
                    DataTable tableI = new DataTable("images");

                    tableCR.Columns.Add("employee_tag", typeof(System.String));
                    tableCR.Columns.Add("first_name", typeof(System.String));
                    tableCR.Columns.Add("last_name", typeof(System.String));
                    tableCR.Columns.Add("visitor_jmbg", typeof(System.String));
                    tableCR.Columns.Add("visit_desc", typeof(System.String));
                    tableCR.Columns.Add("date_start", typeof(System.String));
                    tableCR.Columns.Add("date_end", typeof(System.String));
                    tableCR.Columns.Add("wu_name", typeof(System.String));
                    tableCR.Columns.Add("visited_name", typeof(System.String));
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

                    foreach (ListViewItem item in lvVisitorsViewSearch.Items)
                    {
                        DataRow row = tableCR.NewRow();

                        row["employee_tag"] = item.Text.ToString().Trim();
                        row["first_name"] = item.SubItems[1].Text.ToString().Trim();
                        row["last_name"] = item.SubItems[2].Text.ToString().Trim();
                        row["visitor_jmbg"] = item.SubItems[3].Text.ToString().Trim();
                        row["visit_desc"] = item.SubItems[4].Text.ToString().Trim();
                        row["date_start"] = item.SubItems[5].Text.ToString().Trim();
                        row["date_end"] = item.SubItems[6].Text.ToString().Trim();
                        row["wu_name"] = item.SubItems[7].Text.ToString().Trim();
                        row["visited_name"] = item.SubItems[8].Text.ToString().Trim();

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

                    string selTag = "*";
                    string selWorkingUnit = "*";
                    string selEmployee = "*";
                    string selVisitor = "*";
                    string selVisitDescr = "*";

                    if (cbCardNum.SelectedIndex >= 0 && (int)cbCardNum.SelectedValue >= 0)
                        selTag = cbCardNum.Text;
                    if (cbWorkingUnit.SelectedIndex >= 0 && (int)cbWorkingUnit.SelectedValue >= 0)
                        selWorkingUnit = cbWorkingUnit.Text;
                    if (cbPerson.SelectedIndex >= 0 && (int)cbPerson.SelectedValue >= 0)
                        selEmployee = cbPerson.Text;
                    if (cbVisitor.SelectedIndex >= 0 && !((string)cbVisitor.SelectedValue).Equals(""))
                        selVisitor = cbVisitor.Text;
                    if (cbVisitDescr.SelectedIndex >= 0 && !((string)cbVisitDescr.SelectedValue).Equals("*"))
                        selVisitDescr = cbVisitDescr.Text;

                    if (NotificationController.GetLanguage().Equals(Constants.Lang_sr))
                    {
                        Reports.Reports_sr.VisitorsCRView view = new Reports.Reports_sr.VisitorsCRView(dataSetCR,
                             selTag, selWorkingUnit, selEmployee, selVisitor, selVisitDescr, dtpFromDate.Value, dtpToDate.Value);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_en))
                    {
                        Reports.Reports_en.VisitorsCRView_en view = new Reports.Reports_en.VisitorsCRView_en(dataSetCR,
                             selTag, selWorkingUnit, selEmployee, selVisitor, selVisitDescr, dtpFromDate.Value, dtpToDate.Value);
                        view.ShowDialog(this);
                    }
                    else if (NotificationController.GetLanguage().Equals(Constants.Lang_fi))
                    {
                        Reports.Reports_fi.VisitorsCRView_fi view = new Reports.Reports_fi.VisitorsCRView_fi(dataSetCR,
                             selTag, selWorkingUnit, selEmployee, selVisitor, selVisitDescr, dtpFromDate.Value, dtpToDate.Value);
                        view.ShowDialog(this);
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("noRecordsForDisplay", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReports.btnReport_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}