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

using TransferObjects;
using Util;
using Common;

namespace Reports
{
    public partial class VisitorReportsDetails : Form
    {
        IOPairTO currentIOPair = null;

        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        private ListViewItemComparer _comp;

        // List View indexes
        const int LocationIndex = 0;
        const int EntryIndex = 1;
        const int ExitIndex = 2;

        int visitID = -1;
        string lastName = "";
        string firstName = "";

        public VisitorReportsDetails(int visitID, string lastName, string firstName)
        {
            try
            {
                InitializeComponent();

                this.visitID = visitID;
                this.lastName = lastName;
                this.firstName = firstName;

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentIOPair = new IOPairTO();
                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("Reports.ReportResource", typeof(VisitorReportsDetails).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReportsDetails.VisitorReportsDetails(): " + ex.Message + "\n");
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
                    case VisitorReportsDetails.LocationIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text.Trim(), sub2.Text.Trim());
                        }
                    case VisitorReportsDetails.EntryIndex:
                    case VisitorReportsDetails.ExitIndex:
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
                this.Text = rm.GetString("visitDetailsForm", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);

                // label's text				
                lblVisitor.Text = rm.GetString("lblVisitor", culture);

                // list view
                lvIOPairs.BeginUpdate();
                lvIOPairs.Columns.Add(rm.GetString("hdrLocation", culture), lvIOPairs.Width / 2, HorizontalAlignment.Left);
                lvIOPairs.Columns.Add(rm.GetString("hdrEnter", culture), (lvIOPairs.Width / 4) - 2, HorizontalAlignment.Left);
                lvIOPairs.Columns.Add(rm.GetString("hdrExit", culture), (lvIOPairs.Width / 4) - 2, HorizontalAlignment.Left);
                lvIOPairs.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReportsDetails.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with earned hours found
        /// </summary>
        /// <param name="accessGroupsList"></param>
        private void populateIOPairsListView()
        {
            try
            {
                lvIOPairs.BeginUpdate();
                lvIOPairs.Items.Clear();

                List<IOPairTO> ioPairsList = new IOPair().SearchForVisit(visitID);

                if (ioPairsList.Count > 0)
                {
                    foreach (IOPairTO ioPair in ioPairsList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = ioPair.LocationName.ToString();
                        item.SubItems.Add(ioPair.StartTime.ToString("dd.MM.yyyy   HH:mm").Trim());

                        if (ioPair.EndTime != new DateTime())
                            item.SubItems.Add(ioPair.EndTime.ToString("dd.MM.yyyy   HH:mm").Trim());
                        else
                            item.SubItems.Add("");

                        item.Tag = ioPair.IOPairID.ToString();

                        lvIOPairs.Items.Add(item);
                    }
                }

                lvIOPairs.EndUpdate();
                lvIOPairs.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReportsDetails.populateIOPairsListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void VisitorReportsDetails_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
            
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvIOPairs);
                lvIOPairs.ListViewItemSorter = _comp;
                lvIOPairs.Sorting = SortOrder.Ascending;

                lblVisitorText.Text = lastName + " " + firstName;

                populateIOPairsListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReportsDetails.VisitorReportsDetails_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReportsDetails.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvIOPairs_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvIOPairs.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvIOPairs.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvIOPairs.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvIOPairs.Sorting = SortOrder.Ascending;
                }
                lvIOPairs.Sort();
                lvIOPairs.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VisitorReportsDetails.lvIOPairs_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void VisitorReportsDetails_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " VisitorReportsDetails.VisitorReportsDetails_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}