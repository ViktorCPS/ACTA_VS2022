using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using Common;
using System.Resources;
using System.Globalization;
using System.Threading;

using TransferObjects;
using Util;

namespace Reports
{
    /// <summary>
    /// Summary description for Video_surveillance.
    /// </summary>
    public partial class Video_surveillance : Form
    {
        Camera currentCamera = null;

        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        private ListViewItemComparer _comp;

        // List View indexes
        const int GateIndex = 0;
        const int IDIndex = 1;
        const int DescriptionIndex = 2;

        private int cameraID;
        Hashtable allCameras = new Hashtable();

        public Video_surveillance()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentCamera = new Camera();
                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("Reports.ReportResource", typeof(Video_surveillance).Assembly);
                setLanguage();

                cameraID = -1;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Video_surveillance.Video_surveillance(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public Video_surveillance(int camera)
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentCamera = new Camera();
                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("Reports.ReportResource", typeof(Video_surveillance).Assembly);
                setLanguage();
                cameraID = camera;

                lvCameras.Enabled = false;
                cbGate.Enabled = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Video_surveillance.Video_surveillance(): " + ex.Message + "\n");
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
                    case Video_surveillance.IDIndex:
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
                    case Video_surveillance.GateIndex:
                    case Video_surveillance.DescriptionIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
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
                this.Text = rm.GetString("videoSurveillanceForm", culture);

                // group box text
                gbGatesAndCameras.Text = rm.GetString("gbGatesAndCameras", culture);
                gbSurveillance.Text = rm.GetString("gbSurveillance", culture);

                // button's text
                btnClose.Text = rm.GetString("btnClose", culture);

                // label's text
                lblGate.Text = rm.GetString("lblGate", culture);
                lblCamerasOnGate.Text = rm.GetString("lblCamerasOnGate", culture);
                lblLegend.Text = rm.GetString("lblLegend", culture);
                lblCamera.Text = rm.GetString("lblCamera", culture);

                // list view
                lvCameras.BeginUpdate();
                lvCameras.Columns.Add(rm.GetString("hdrGate", culture), 2 * (lvCameras.Width / 5) - 2, HorizontalAlignment.Left);
                lvCameras.Columns.Add(rm.GetString("hdrCameraID", culture), (lvCameras.Width / 5) , HorizontalAlignment.Left);
                lvCameras.Columns.Add(rm.GetString("hdrDescripton", culture), 2 * (lvCameras.Width / 5) , HorizontalAlignment.Left);
                lvCameras.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Video_surveillance.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateGateCb()
        {
            try
            {                
                List<GateTO> gates = new Gate().Search();

                gates.Insert(0, new GateTO(-1, rm.GetString("all", culture), "", new DateTime(0), -1, -1));

                cbGate.DataSource = gates;
                cbGate.DisplayMember = "Name";
                cbGate.ValueMember = "GateID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Video_surveillance.populateGateCb(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateListView()
        {
            try
            {
               
                int gateID = -1;
                if (cbGate.SelectedValue is int)
                {
                    gateID = (int)cbGate.SelectedValue;
                }

                ArrayList cameraList = currentCamera.SearchOnGate(gateID);

                lvCameras.BeginUpdate();
                lvCameras.Items.Clear();

                if (cameraList.Count > 0)
                {
                    
                    foreach (Camera camera in cameraList)
                    {                       
                        ListViewItem item = new ListViewItem();
                        item.Text = camera.Type.Trim(); //gate Description is in Type field
                        item.SubItems.Add(camera.CameraID.ToString());
                        item.SubItems.Add(camera.Description.Trim());

                        item.Tag = camera.ConnAddress.Trim();
                        lvCameras.Items.Add(item);
                    }
                }

                lvCameras.EndUpdate();
                lvCameras.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Video_surveillance.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void Video_surveillance_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvCameras);
                lvCameras.ListViewItemSorter = _comp;
                lvCameras.Sorting = SortOrder.Ascending;

                populateGateCb();

                if (cameraID != -1)
                {
                    foreach (ListViewItem item in lvCameras.Items)
                    {
                        if (int.Parse(item.SubItems[1].Text.ToString()) == cameraID)
                        {
                            item.Selected = true;
                            lvCameras_MouseDoubleClick(this, new MouseEventArgs(MouseButtons.Left, 2, 0, 0, 0));
                        }
                    }
                }
                allCameras = new Hashtable();
                ArrayList cameras = currentCamera.Search(-1, "", "", "");
                foreach (Camera camera in cameras)
                {
                    if (!allCameras.ContainsKey(camera.ConnAddress))
                        allCameras.Add(camera.ConnAddress, camera);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Video_surveillance.Video_surveillance_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " Video_surveillance.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void lvCameras_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvCameras.Sorting;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvCameras.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvCameras.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvCameras.Sorting = SortOrder.Ascending;
                }
                lvCameras.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Video_surveillance.lvCameras_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {

                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbGate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbGate.SelectedIndex >= 0)
                {
                    populateListView();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Video_surveillance.cbGate_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvCameras_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lvCameras.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selOneCamera", culture));
                }
                else
                {
                    string address = lvCameras.SelectedItems[0].Tag.ToString();
                    if (!address.Equals(""))
                    {
                        Camera cam= new Camera();
                        if(allCameras.ContainsKey(address))
                           cam = (Camera)allCameras[address];
                       if (cam.Type.Trim().ToUpper().Equals(Constants.cameraTypeSANYO))
                       {
                           MessageBox.Show(rm.GetString("unableToPreview", culture));
                           return;
                       }

                       acPing ping = new acPing();
                       if (ping.Completed(address, 2))
                       {
                           lblCamera.Text = rm.GetString("lblCamera", culture) + " " + lvCameras.SelectedItems[0].SubItems[2].Text;
                           webBrowser1.Navigate(@"http://" + address);
                       }
                       else
                       {
                           MessageBox.Show(rm.GetString("noConnToCamera", culture));
                       }
                    }
                    else
                    {
                        webBrowser1.Navigate(@"about:blank");
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Video_surveillance.lvCameras_MouseDoubleClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void Video_surveillance_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Video_surveillance.Video_surveillance_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}