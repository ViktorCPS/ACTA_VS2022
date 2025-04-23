using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Globalization;

using Common;
using Util;
using TransferObjects;

namespace UI
{
    public partial class AccessControlStatus : Form
    {
        private ReaderTO currentReader = null;
        private AccessControlFile currentAccessControlFile = null;

        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        private ListViewItemComparer _comp;

        // List View indexes
        const int ReaderIndex = 0;
        const int TypeIndex = 1;
        const int LastTimeIssuedIndex = 2;
        const int IssuedByIndex = 3;
        const int LastUploadTimeIndex = 4;
        const int PendingUploadIndex = 5;

        public AccessControlStatus()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentAccessControlFile = new AccessControlFile();
                currentReader = new ReaderTO();
                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("UI.Resource", typeof(AccessControlStatus).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlStatus.AccessControlStatus(): " + ex.Message + "\n");
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
                    case AccessControlStatus.ReaderIndex:
                    case AccessControlStatus.TypeIndex:
                    case AccessControlStatus.IssuedByIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case AccessControlStatus.LastTimeIssuedIndex:
                    case AccessControlStatus.LastUploadTimeIndex:
                        {
                            DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                            DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy   HH:mm",null);
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy   HH:mm",null);
                            }

                            return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
                        }
                    case AccessControlStatus.PendingUploadIndex:
                        {
                            int firstID = -1;
                            int secondID = -1;

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                firstID = Util.Misc.transformStringTimeToMin(sub1.Text.Trim());
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                secondID = Util.Misc.transformStringTimeToMin(sub2.Text.Trim());
                            }

                            return CaseInsensitiveComparer.Default.Compare(firstID, secondID);
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
                this.Text = rm.GetString("accessControlStatusForm", culture);

                // button's text
                btnRefresh.Text = rm.GetString("btnRefresh", culture);
                btnClose.Text = rm.GetString("btnClose", culture);

                // label's text
                lblLegend.Text = rm.GetString("lblLegend", culture);
                lblGreen.Text = rm.GetString("lblGreen", culture);
                lblOrange.Text = rm.GetString("lblOrange", culture);
                lblYellow.Text = rm.GetString("lblYellow", culture);
                lblRed.Text = rm.GetString("lblRed", culture);

                // list view
                lvReaders.BeginUpdate();
                lvReaders.Columns.Add(rm.GetString("hdrReader", culture), (lvReaders.Width / 6) + 10, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrType", culture), (lvReaders.Width / 6) - 30, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrLastTimeIssued", culture), (lvReaders.Width / 6) + 10, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrIssuedBy", culture), (lvReaders.Width / 6) - 10, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrLastUploadTime", culture), (lvReaders.Width / 6) + 20, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrPendingUpload", culture), (lvReaders.Width / 6) - 2, HorizontalAlignment.Left);
                lvReaders.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlStatus.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateListView()
        {
            try
            {
                CultureInfo ci = CultureInfo.InvariantCulture;
                Reader r = new Reader();
                r.RdrTO = currentReader;
                List<ReaderTO> readersList = r.SearchOnAntenna0();
                ArrayList controlFilesList = currentAccessControlFile.SearchLastIssuedACFiles();
                ArrayList uploadTimeList = currentAccessControlFile.SearchLastUploadTime();

                lvReaders.BeginUpdate();
                lvReaders.Items.Clear();

                if (readersList.Count > 0)
                {
                    foreach (ReaderTO reader in readersList)
                    {
                        string type = "";
                        string typeDisplay = "";
                        for (int counter = 0; counter < 2; counter++)
                        {
                            if (counter == 0)
                            {
                                type = Constants.ACFilesTypeCards;
                                typeDisplay = "Tags";
                            }
                            else if (counter == 1)
                            {
                                type = Constants.ACFilesTypeTAProfile;
                                typeDisplay = "Time access profiles";
                            }

                            ListViewItem item = new ListViewItem();
                            item.Text = reader.Description;
                            item.SubItems.Add(typeDisplay);
                            int delay = 0;
                            bool issued = false;
                            string status = "";
                            DateTime uploadStartTime = new DateTime(0);
                            foreach (AccessControlFile acf in controlFilesList)
                            {
                                if ((acf.ReaderID == reader.ReaderID)
                                    && (acf.Type == type))
                                {
                                    item.SubItems.Add(acf.CreatedTime.ToString("dd.MM.yyyy   HH:mm").Trim());
                                    item.SubItems.Add(acf.CreatedBy);
                                    delay = acf.Delayed;
                                    status = acf.Status;
                                    uploadStartTime = acf.UploadStartTime;
                                    issued = true;
                                    break;
                                }
                            }
                            if (!issued)
                            {
                                item.SubItems.Add("");
                                item.SubItems.Add("");
                            }

                            issued = false;
                            foreach (AccessControlFile acf in uploadTimeList)
                            {
                                if ((acf.ReaderID == reader.ReaderID)
                                    && (acf.Type == type))
                                {
                                    item.SubItems.Add(acf.UploadEndTime.ToString("dd.MM.yyyy   HH:mm").Trim());
                                    issued = true;
                                    break;
                                }
                            }
                            if (!issued)
                            {
                                item.SubItems.Add("");
                            }

                            if (delay == 1)
                            {
                                item.SubItems.Add(reader.DownloadStartTime.ToString("HH:mm", ci));
                            }
                            else
                            {
                                item.SubItems.Add("");
                            }

                            item.Tag = reader.ReaderID;

                            if ((status == Constants.ACFilesStatusUsed) || (status == Constants.ACFilesStatusOverwritten))
                                item.BackColor = Color.LightGreen;

                            if ((status == Constants.ACFilesStatusInProgress)
                                || ((status == Constants.ACFilesStatusUnused) && (delay == 0)))
                                item.BackColor = Color.Orange;

                            if ((status == Constants.ACFilesStatusUnused) && (delay == 1))
                            {
                                if (uploadStartTime == (new DateTime(0)))
                                    item.BackColor = Color.Yellow;
                                else
                                    item.BackColor = Color.OrangeRed;
                            }

                            lvReaders.Items.Add(item);
                        } //for (int counter = 0; counter < 2; counter++)
                    } //foreach (Reader reader in readersList)
                }

                lvReaders.EndUpdate();
                lvReaders.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlStatus.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void AccessControlStatus_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer object
                _comp = new ListViewItemComparer(lvReaders);
                lvReaders.ListViewItemSorter = _comp;
                lvReaders.Sorting = SortOrder.Ascending;

                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlStatus.AccessControlStatus_Load(): " + ex.Message + "\n");
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
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlStatus.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlStatus.btnRefresh_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvReaders_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvReaders.Sorting;

                if (e.Column == _comp.SortColumn)
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
                    _comp.SortColumn = e.Column;
                    lvReaders.Sorting = SortOrder.Ascending;
                }
                lvReaders.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " AccessControlStatus.lvReaders_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void AccessControlStatus_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " AccessControlStatus.AccessControlStatus_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}