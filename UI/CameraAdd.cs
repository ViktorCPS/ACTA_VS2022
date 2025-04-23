using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Text;
using System.Resources;
using System.Globalization;

using Common;
using Util;
using TransferObjects;

namespace UI
{
    /// <summary>
    /// Summary description for CameraAdd.
    /// </summary>
    public partial class CameraAdd : Form
    {
        Camera currentCamera = null;
        CamerasXReaders cxr = null;

        ApplUserTO logInUser;
        ResourceManager rm;
        private CultureInfo culture;
        DebugLog log;

        private ListViewItemComparer _comp1;
        private ListViewItemComparer _comp2;

        // List View indexes
        const int IDIndex = 0;
        const int DescriptionIndex = 1;
        const int DirectionIndex = 2;

        int cameraID = -1;
        string description = "";
        string address = "";
        string type = "";

        public CameraAdd()
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

                rm = new ResourceManager("UI.Resource", typeof(CameraAdd).Assembly);
                setLanguage();

                tbCameraID.Text = (currentCamera.GetCameraNextID()).ToString();
                tbType.Text = "AXIS 207";

                btnUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.CameraAdd(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public CameraAdd(int cameraID, string description, string address, string type)
        {
            try
            {
                InitializeComponent();

                this.cameraID = cameraID;
                this.description = description;
                this.address = address;
                this.type = type;

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                currentCamera = new Camera();
                cxr = new CamerasXReaders();
                logInUser = NotificationController.GetLogInUser();

                this.CenterToScreen();
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                rm = new ResourceManager("UI.Resource", typeof(CameraAdd).Assembly);
                setLanguage();

                tbCameraID.Text = cameraID.ToString();
                tbDesc.Text = description;
                tbType.Text = type;

                string[] oct = address.Split('.');
                if (oct.Length == 4)
                {
                    tbIP0.Text = oct[0];
                    tbIP1.Text = oct[1];
                    tbIP2.Text = oct[2];
                    tbIP3.Text = oct[3];
                }

                tbCameraID.Enabled = false;

                btnSave.Visible = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.CameraAdd(): " + ex.Message + "\n");
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
                    case CameraAdd.IDIndex:
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
                    case CameraAdd.DescriptionIndex:
                    case CameraAdd.DirectionIndex:
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
                if (cameraID != -1)
                {
                    this.Text = rm.GetString("cameraUpdForm", culture);
                }
                else
                {
                    this.Text = rm.GetString("cameraAddForm", culture);
                }

                // group box text
                gbCameraReaders.Text = rm.GetString("gbCameraReaders", culture);
                gbCamera.Text = rm.GetString("gbCamera", culture);

                // button's text
                btnSave.Text = rm.GetString("btnSave", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnRemove.Text = rm.GetString("btnRemoveSel", culture);

                // label's text
                lblCameraID.Text = rm.GetString("lblID", culture);
                lblDesc.Text = rm.GetString("lblDescription", culture);
                lblIP.Text = rm.GetString("lblAddress", culture);
                lblType.Text = rm.GetString("lblType", culture);
                lblNotSelReaders.Text = rm.GetString("lblNotSelReaders", culture);
                lblSelReaders.Text = rm.GetString("lblSelReaders", culture);

                // list view
                lvNotSelReaders.BeginUpdate();
                lvNotSelReaders.Columns.Add(rm.GetString("hdrReaderID", culture), (lvNotSelReaders.Width / 4), HorizontalAlignment.Left);
                lvNotSelReaders.Columns.Add(rm.GetString("hdrDescripton", culture), ((3 * lvNotSelReaders.Width) / 4) - 5, HorizontalAlignment.Left);
                lvNotSelReaders.EndUpdate();

                lvDirection.BeginUpdate();
                lvDirection.Columns.Add(rm.GetString("hdrDirection", culture), (lvDirection.Width) - 5, HorizontalAlignment.Left);
                lvDirection.EndUpdate();

                lvSelReaders.BeginUpdate();
                lvSelReaders.Columns.Add(rm.GetString("hdrReaderID", culture), (lvSelReaders.Width / 6) + 20, HorizontalAlignment.Left);
                lvSelReaders.Columns.Add(rm.GetString("hdrDescripton", culture), ((4 * lvSelReaders.Width) / 6) - 44, HorizontalAlignment.Left);
                lvSelReaders.Columns.Add(rm.GetString("hdrDirection", culture), (lvSelReaders.Width / 6) + 20, HorizontalAlignment.Left);
                lvSelReaders.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateNotSelReadersListView(List<ReaderTO> allReaders, ArrayList selReaders)
        {
            try
            {
                lvNotSelReaders.BeginUpdate();
                lvNotSelReaders.Items.Clear();

                if (allReaders.Count > 0)
                {
                    foreach (ReaderTO reader in allReaders)
                    {
                        bool selected = false;
                        foreach (CamerasXReaders cameraReader in selReaders)
                        {
                            if (reader.ReaderID == cameraReader.ReaderID)
                            {
                                selected = true;
                                break;
                            }
                        }
                        if (!selected)
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = reader.ReaderID.ToString();
                            item.SubItems.Add(reader.Description);

                            lvNotSelReaders.Items.Add(item);
                        }
                    }
                }

                lvNotSelReaders.EndUpdate();
                lvNotSelReaders.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.populateNotSelReadersListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateSelReadersListView(List<ReaderTO> allReaders, ArrayList selReaders)
        {
            try
            {
                lvSelReaders.BeginUpdate();
                lvSelReaders.Items.Clear();

                if (selReaders.Count > 0)
                {
                    foreach (CamerasXReaders cameraReader in selReaders)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = cameraReader.ReaderID.ToString();

                        string descr = "";
                        foreach (ReaderTO reader in allReaders)
                        {
                            if (reader.ReaderID == cameraReader.ReaderID)
                            {
                                descr = reader.Description;
                                break;
                            }
                        }
                        item.SubItems.Add(descr);
                        item.SubItems.Add(cameraReader.DirectionCovered);

                        lvSelReaders.Items.Add(item);
                    }
                }

                lvSelReaders.EndUpdate();
                lvSelReaders.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.populateSelReadersListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateDirectionListView()
        {
            try
            {
                lvDirection.BeginUpdate();
                lvDirection.Items.Clear();

                ListViewItem item1 = new ListViewItem();
                item1.Text = Constants.DirectionIn;
                lvDirection.Items.Add(item1);

                ListViewItem item2 = new ListViewItem();
                item2.Text = Constants.DirectionOut;
                lvDirection.Items.Add(item2);

                ListViewItem item3 = new ListViewItem();
                item3.Text = Constants.DirectionInOut;
                lvDirection.Items.Add(item3);   

                lvDirection.EndUpdate();
                lvDirection.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Cameras.populateDirectionListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void CameraAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer object
                _comp1 = new ListViewItemComparer(lvNotSelReaders);
                lvNotSelReaders.ListViewItemSorter = _comp1;
                lvNotSelReaders.Sorting = SortOrder.Ascending;

                _comp2 = new ListViewItemComparer(lvSelReaders);
                lvSelReaders.ListViewItemSorter = _comp2;
                lvSelReaders.Sorting = SortOrder.Ascending;

                List<ReaderTO> allReaders = new Reader().Search();
                ArrayList selReaders = new ArrayList();
                if (this.cameraID != -1)
                {
                    selReaders = cxr.Search(cameraID, -1, "");
                }
                populateNotSelReadersListView(allReaders, selReaders);
                populateSelReadersListView(allReaders, selReaders);
                populateDirectionListView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.CameraAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally { 
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string message = validate();
                if (!message.Equals(""))
                    MessageBox.Show(message);
                else
                {
                    string address = tbIP0.Text.Trim() + "." +
                    tbIP1.Text.Trim() + "." +
                    tbIP2.Text.Trim() + "." +
                    tbIP3.Text.Trim();

                    bool trans = currentCamera.BeginTransaction();
                    if (trans)
                    {
                        int cameraID = Int32.Parse(tbCameraID.Text);
                        bool inserted = false;
                        int insert = currentCamera.Save(cameraID, address, tbDesc.Text,
                            tbType.Text, false);
                        if (insert > 0)
                        {
                            inserted = true;
                            cxr.SetTransaction(currentCamera.GetTransaction());
                            foreach (ListViewItem item in lvSelReaders.Items)
                            {
                                inserted = (cxr.Save(cameraID, Int32.Parse(item.SubItems[0].Text),
                                    item.SubItems[2].Text, false) > 0) && inserted;
                            }
                        }

                        if (inserted)
                        {
                            currentCamera.CommitTransaction();
                            MessageBox.Show(rm.GetString("cameraSaved", culture));
                            this.Close();
                        }
                        else
                        {
                            currentCamera.RollbackTransaction();
                            MessageBox.Show(rm.GetString("cameraNotSaved", culture));
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }
                } //validation
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.btnSave_Click(): " + ex.Message + "\n");
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

                string message = validate();
                if (!message.Equals(""))
                    MessageBox.Show(message);
                else
                {
                    string address = tbIP0.Text.Trim() + "." +
                    tbIP1.Text.Trim() + "." +
                    tbIP2.Text.Trim() + "." +
                    tbIP3.Text.Trim();

                    bool trans = currentCamera.BeginTransaction();
                    if (trans)
                    {
                        int cameraID = Int32.Parse(tbCameraID.Text);
                        bool updated = currentCamera.Update(cameraID, address, tbDesc.Text,
                            tbType.Text, false);
                        if (updated)
                        {
                            cxr.SetTransaction(currentCamera.GetTransaction());
                            updated = cxr.Delete(cameraID, -1, false);
                            if (updated)
                            {
                                foreach (ListViewItem item in lvSelReaders.Items)
                                {
                                    updated = (cxr.Save(cameraID, Int32.Parse(item.SubItems[0].Text),
                                        item.SubItems[2].Text, false) > 0) && updated;
                                }
                            }
                        }

                        if (updated)
                        {
                            currentCamera.CommitTransaction();
                            MessageBox.Show(rm.GetString("cameraUpdated", culture));
                            this.Close();
                        }
                        else
                        {
                            currentCamera.RollbackTransaction();
                            MessageBox.Show(rm.GetString("cameraNotUpdated", culture));
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("cannotStartTransaction", culture));
                        return;
                    }
                } //validation  
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private string validate()
        {
            StringBuilder message = new StringBuilder();

            if (tbCameraID.Text.Equals(""))
            {
                message.Append(rm.GetString("noCameraID", culture) + "\n");
            }
            else
            {
                try
                {
                    Int32.Parse(tbCameraID.Text);
                }
                catch
                {
                    message.Append(rm.GetString("cameraIDNumber", culture) + "\n");
                }
            }

            if (tbDesc.Text.Equals(""))
            {
                message.Append(rm.GetString("noCamDescr", culture) + "\n");
            }

            if (tbIP0.Text.Trim().Equals("") ||
                tbIP1.Text.Trim().Equals("") ||
                tbIP2.Text.Trim().Equals("") ||
                tbIP3.Text.Trim().Equals(""))
            {
                message.Append(rm.GetString("noCamAddress", culture) + "\n");
            }
            else
            {
                try
                {
                    Int32.Parse(tbIP0.Text);
                    Int32.Parse(tbIP1.Text);
                    Int32.Parse(tbIP2.Text);
                    Int32.Parse(tbIP3.Text);
                }
                catch
                {
                    message.Append(rm.GetString("camAddressNumber", culture) + "\n");
                }
            }

            if (tbType.Text.Equals(""))
            {
                message.Append(rm.GetString("noCamType", culture) + "\n");
            }

            if (lvSelReaders.Items.Count <= 0)
                message.Append(rm.GetString("noCamReaders", culture) + "\n");

            foreach (ListViewItem item in lvSelReaders.Items)
            {
                string direction = item.SubItems[2].Text;
                if (direction.Equals(""))
                {
                    message.Append(rm.GetString("noCamDirection", culture) + "\n");
                    break;
                }
            }

            return message.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.btnCancel_Click(): " + ex.Message + "\n");
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

                foreach (ListViewItem item in lvNotSelReaders.SelectedItems)
                {
                    lvNotSelReaders.Items.Remove(item);

                    ListViewItem item1 = new ListViewItem();
                    item1.Text = item.SubItems[0].Text;
                    item1.SubItems.Add(item.SubItems[1].Text);
                    item1.SubItems.Add("");

                    lvSelReaders.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.btnAdd_Click(): " + ex.Message + "\n");
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
                this.Cursor = Cursors.WaitCursor;

                foreach (ListViewItem item in lvSelReaders.SelectedItems)
                {
                    lvSelReaders.Items.Remove(item);

                    ListViewItem item1 = new ListViewItem();
                    item1.Text = item.SubItems[0].Text;
                    item1.SubItems.Add(item.SubItems[1].Text);

                    lvNotSelReaders.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.btnRemove_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnDirectionAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvDirection.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noSelDirection", culture));
                }
                else
                {
                    if (lvSelReaders.SelectedItems.Count <= 0)
                        MessageBox.Show(rm.GetString("noSelReader", culture));

                    foreach (ListViewItem item in lvSelReaders.SelectedItems)
                    {
                        item.SubItems[2].Text = lvDirection.SelectedItems[0].Text;
                        if (tbDesc.Text.Equals(""))
                            tbDesc.Text = item.SubItems[1].Text;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.btnDirectionAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvNotSelReaders_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvNotSelReaders.Sorting;

                if (e.Column == _comp1.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvNotSelReaders.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvNotSelReaders.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp1.SortColumn = e.Column;
                    lvNotSelReaders.Sorting = SortOrder.Ascending;
                }
                lvNotSelReaders.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.lvNotSelReaders_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvSelReaders_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvSelReaders.Sorting;

                if (e.Column == _comp2.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvSelReaders.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvSelReaders.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp2.SortColumn = e.Column;
                    lvSelReaders.Sorting = SortOrder.Ascending;
                }
                lvSelReaders.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " CameraAdd.lvSelReaders_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void CameraAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " CameraAdd.CameraAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}