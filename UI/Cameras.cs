using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using Util;
using TransferObjects;

namespace UI
{
    /// <summary>
    /// Summary description for Cameras.
    /// </summary>
    public partial class Cameras : Form
    {
        Camera currentCamera = null;
        CamerasXReaders cxr = null;

        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;

        bool readPermission = false;
        bool addPermission = false;
        bool updatePermission = false;
        bool deletePermission = false;

        private ListViewItemComparer _comp;

        // List View indexes
        const int IDIndex = 0;
        const int AddressIndex = 1;
        const int DescriptionIndex = 2;
        const int TypeIndex = 3;

        public Cameras()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentCamera = new Camera();
                cxr = new CamerasXReaders();
                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("UI.Resource", typeof(Cameras).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.Cameras(): " + ex.Message + "\n");
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
                    case Cameras.IDIndex:
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
                    case Cameras.AddressIndex:
                    case Cameras.DescriptionIndex:
                    case Cameras.TypeIndex:
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
                this.Text = rm.GetString("camerasForm", culture);

                // button's text
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
                btnMaps.Text = rm.GetString("btnMaps", culture);
                
                // list view
                lvCameras.BeginUpdate();
                lvCameras.Columns.Add(rm.GetString("hdrCameraID", culture), (lvCameras.Width / 4) - 54, HorizontalAlignment.Left);
                lvCameras.Columns.Add(rm.GetString("hdrDescripton", culture), (lvCameras.Width / 4) + 50, HorizontalAlignment.Left);
                lvCameras.Columns.Add(rm.GetString("hdrAddress", culture), lvCameras.Width / 4, HorizontalAlignment.Left);
                lvCameras.Columns.Add(rm.GetString("hdrType", culture), lvCameras.Width / 4, HorizontalAlignment.Left);
                lvCameras.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate List View with Access Groups found
        /// </summary>
        /// <param name="accessGroupsList"></param>
        private void populateListView()
        {
            try
            {
                ArrayList cameraList = currentCamera.Search(-1, "", "", "");

                lvCameras.BeginUpdate();
                lvCameras.Items.Clear();

                if (cameraList.Count > 0)
                {
                    foreach (Camera camera in cameraList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = camera.CameraID.ToString();
                        item.SubItems.Add(camera.Description.Trim());
                        item.SubItems.Add(camera.ConnAddress.Trim());
                        item.SubItems.Add(camera.Type.Trim());

                        lvCameras.Items.Add(item);
                    }
                }

                lvCameras.EndUpdate();
                lvCameras.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void Cameras_Load(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
            
                // Initialize comparer object
                _comp = new ListViewItemComparer(lvCameras);
                lvCameras.ListViewItemSorter = _comp;
                lvCameras.Sorting = SortOrder.Ascending;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();

                populateListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.Cameras_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " Cameras.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Open Add Form
                CameraAdd addForm = new CameraAdd();
                addForm.ShowDialog(this);

                populateListView();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.btnAdd_Click(): " + ex.Message + "\n");
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
                this.Cursor = Cursors.WaitCursor;
                if (this.lvCameras.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selOneCamera", culture));
                }
                else
                {
                    int cameraID = Int32.Parse(lvCameras.SelectedItems[0].SubItems[0].Text);
                    string description = lvCameras.SelectedItems[0].SubItems[1].Text;
                    string address = lvCameras.SelectedItems[0].SubItems[2].Text;
                    string type = lvCameras.SelectedItems[0].SubItems[3].Text;

                    // Open Update Form
                    CameraAdd addForm = new CameraAdd(cameraID, description, address, type);
                    addForm.ShowDialog(this);

                    populateListView();
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lvCameras.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelCameraDel", culture));
                }
                else
                {
                    DialogResult result = MessageBox.Show(rm.GetString("deleteCameras", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        bool isDeleted = true;
                        foreach (ListViewItem item in lvCameras.SelectedItems)
                        {
                            int cameraID = Int32.Parse(item.SubItems[0].Text);
                            bool xref = true;

                            ArrayList xrefList = cxr.Search(cameraID, -1, "");

                            bool trans = currentCamera.BeginTransaction();
                            if (trans)
                            {
                                if (xrefList.Count > 0)
                                {
                                    cxr.SetTransaction(currentCamera.GetTransaction());
                                    xref = cxr.Delete(cameraID, -1, false);
                                }
                                if (xref)
                                {
                                    xref = currentCamera.Delete(cameraID, false);
                                }
                                isDeleted = xref && isDeleted;

                                if (xref)
                                    currentCamera.CommitTransaction();
                                else
                                    currentCamera.RollbackTransaction();
                            } //if (trans)
                            else
                            {
                                isDeleted = false;
                            }
                        }

                        if (isDeleted)
                        {
                            MessageBox.Show(rm.GetString("cameraDel", culture));
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("noCameraDel", culture));
                        }

                        populateListView();
                        this.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.btnDelete_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " Cameras.lvAccessGroups_ColumnClick(): " + ex.Message + "\n");
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
                int permission;

                foreach (ApplRoleTO role in currentRoles)
                {
                    permission = (((int[])menuItemsPermissions[menuItemID])[role.ApplRoleID]);
                    readPermission = readPermission || (((permission / 8) % 2) == 0 ? false : true);
                    addPermission = addPermission || (((permission / 4) % 2) == 0 ? false : true);
                    updatePermission = updatePermission || (((permission / 2) % 2) == 0 ? false : true);
                    deletePermission = deletePermission || ((permission % 2) == 0 ? false : true);
                }

                btnAdd.Enabled = addPermission;
                btnUpdate.Enabled = updatePermission;
                btnDelete.Enabled = deletePermission;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.setVisibility(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void Cameras_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Cameras.Cameras_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnMaps_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                MapObjects mapObjects = new MapObjects(Constants.cameraObjectType);
                mapObjects.ShowDialog();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.btnMaps_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}