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

using Common;
using Util;
using TransferObjects;

namespace UI
{
    public partial class SecurityRoutesAdd : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        const int NameIndex = 0;
        const int DescriptionFromIndex = 1;
        const int ToIndex = 2;

        private ListViewItemComparer _comp1;
        private ListViewItemComparer _comp2;

        SecurityRouteHdr secRouteHdr;
        SecurityRouteDtl secRouteDtl;

        private string routeType;

        public SecurityRoutesAdd(string routeType)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(SecurityRoutesAdd).Assembly);

                setLanguage();

                logInUser = NotificationController.GetLogInUser();
                secRouteHdr = new SecurityRouteHdr();
                secRouteDtl = new SecurityRouteDtl();

                numNextVisit.Value = Constants.defaultNextVisit;
                dtpFrom.Value = new DateTime(1900, 1, 1, 0, 0, 0);
                dtpTo.Value = new DateTime(1900, 1, 1, 0, 0, 0);

                this.routeType = routeType;
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
                    case SecurityRoutesAdd.NameIndex:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    case SecurityRoutesAdd.DescriptionFromIndex:
                        {
                            if (_listView.Name.Equals("lvGates"))
                            {
                                return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                            }
                            else
                            {
                                DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                                DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                                if (!sub1.Text.Trim().Equals(""))
                                {
                                    dt1 = DateTime.ParseExact(sub1.Text.Trim(), "HH:mm", null);
                                }

                                if (!sub2.Text.Trim().Equals(""))
                                {
                                    dt2 = DateTime.ParseExact(sub2.Text.Trim(), "HH:mm",null);
                                }

                                return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
                            }
                        }
                    case SecurityRoutesAdd.ToIndex:
                        {
                            DateTime dt1 = new DateTime(1, 1, 1, 0, 0, 0);
                            DateTime dt2 = new DateTime(1, 1, 1, 0, 0, 0);

                            if (!sub1.Text.Trim().Equals(""))
                            {
                                dt1 = DateTime.ParseExact(sub1.Text.Trim(), "HH:mm", null);
                            }

                            if (!sub2.Text.Trim().Equals(""))
                            {
                                dt2 = DateTime.ParseExact(sub2.Text.Trim(),"HH:mm", null);
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
                this.Text = rm.GetString("SecurityRoutesAddForm", culture);

                // group box's text
                this.gbAddRoute.Text = rm.GetString("gbAddRoute", culture);
                this.gbGate.Text = rm.GetString("gbGates", culture);
                this.gbTimeInterval.Text = rm.GetString("gbRouteTimeInterval", culture);
                this.gbRoutes.Text = rm.GetString("gbRoutes", culture);

                // label's text
                this.lblDesc.Text = rm.GetString("lblDesc", culture);
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblMin.Text = rm.GetString("lblMin", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
                this.lblNextVisit.Text = rm.GetString("lblNextVisit", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);

                // button's text
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnRemove.Text = rm.GetString("btnRemoveSel", culture);

                // list views
                lvRoutes.BeginUpdate();
                lvRoutes.Columns.Add(rm.GetString("hdrGate", culture), (lvRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvRoutes.Columns.Add(rm.GetString("hdrFrom", culture), (lvRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvRoutes.Columns.Add(rm.GetString("hdrTo", culture), (lvRoutes.Width - 3) / 3 - 5, HorizontalAlignment.Left);
                lvRoutes.EndUpdate();

                lvGates.BeginUpdate();
                lvGates.Columns.Add(rm.GetString("hdrName", culture), (lvRoutes.Width - 2) / 2 - 15, HorizontalAlignment.Left);
                lvGates.Columns.Add(rm.GetString("hdrDescription", culture), (lvRoutes.Width - 2) / 2 - 15, HorizontalAlignment.Left);
                lvGates.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void SecurityRoutesAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer object
                _comp1 = new ListViewItemComparer(lvGates);
                lvGates.ListViewItemSorter = _comp1;
                lvGates.Sorting = SortOrder.Ascending;

                // Initialize comparer object
                _comp2 = new ListViewItemComparer(lvRoutes);
                lvRoutes.ListViewItemSorter = _comp2;
                _comp2.SortColumn = DescriptionFromIndex;
                lvRoutes.Sorting = SortOrder.Ascending;

                if (routeType.Equals(Constants.routeTag))
                {
                    populateGates();
                }
                else
                {
                    populatePoints();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.SecurityRoutesAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
              this.Cursor = Cursors.Arrow;
            }
        }

        private void populateGates()
        {
            try
            {
                List<GateTO> gates = new Gate().Search();

                lvGates.BeginUpdate();
                lvGates.Items.Clear();

                if (gates.Count > 0)
                {
                    foreach (GateTO gate in gates)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = gate.Name.ToString().Trim();
                        item.SubItems.Add(gate.Description.ToString().Trim());
                        item.Tag = gate;
                        item.ToolTipText = "Gate ID: " + gate.GateID.ToString().Trim();

                        lvGates.Items.Add(item);
                    }
                }

                lvGates.EndUpdate();
                lvGates.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.populateGates(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populatePoints()
        {
            try
            {
                ArrayList points = new SecurityRoutesPoint().Search(-1, "", "", "");

                lvGates.BeginUpdate();
                lvGates.Items.Clear();

                if (points.Count > 0)
                {
                    foreach (SecurityRoutesPoint point in points)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = point.Name.ToString().Trim();
                        item.SubItems.Add(point.Description.ToString().Trim());
                        item.Tag = point;
                        item.ToolTipText = "Point ID: " + point.ControlPointID.ToString().Trim();

                        lvGates.Items.Add(item);
                    }
                }

                lvGates.EndUpdate();
                lvGates.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.populatePoints(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvGates.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("selectGates", culture));
                    return;
                }

                if (dtpFrom.Value.TimeOfDay > dtpTo.Value.TimeOfDay)
                {
                    MessageBox.Show(rm.GetString("fromDateGreaterToDate", culture));
                    return;
                }

                lvRoutes.BeginUpdate();
                ListViewItem item = new ListViewItem();

                if (routeType.Equals(Constants.routeTag))
                {
                    GateTO gate = (GateTO)lvGates.SelectedItems[0].Tag;

                    item.Text = gate.Name;
                    item.Tag = gate;
                }
                else
                {
                    SecurityRoutesPoint point = (SecurityRoutesPoint)lvGates.SelectedItems[0].Tag;

                    item.Text = point.Name;
                    item.Tag = point;
                }

                item.SubItems.Add(dtpFrom.Value.ToString("HH:mm"));
                item.SubItems.Add(dtpTo.Value.ToString("HH:mm"));

                lvRoutes.Items.Add(item);
                lvRoutes.EndUpdate();

                dtpFrom.Value = dtpFrom.Value.AddMinutes((int)numNextVisit.Value);
                dtpTo.Value = dtpTo.Value.AddMinutes((int)numNextVisit.Value);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (ListViewItem item in lvRoutes.SelectedItems)
                {
                    lvRoutes.Items.Remove(item);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.btnRemove_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvGates_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvGates.Sorting;

                if (e.Column == _comp1.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvGates.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvGates.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp1.SortColumn = e.Column;
                    lvGates.Sorting = SortOrder.Ascending;
                }

                lvGates.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.lvGates_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvRoutes_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                SortOrder prevOrder = lvRoutes.Sorting;

                if (e.Column == _comp2.SortColumn)
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
                    _comp2.SortColumn = e.Column;
                    lvRoutes.Sorting = SortOrder.Ascending;
                }

                lvRoutes.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.lvRoutes_ColumnClick(): " + ex.Message + "\n");
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

                if (tbName.Text.Trim().Equals(""))
                {
                    MessageBox.Show(rm.GetString("enterRouteName", culture));
                    return;
                }

                secRouteHdr.Name = tbName.Text.Trim();
                secRouteHdr.Description = tbDesc.Text.Trim();
                secRouteHdr.RouteType = this.routeType.Trim();

                int segmentID = 0;
                foreach (ListViewItem item in lvRoutes.Items)
                {
                    secRouteDtl = new SecurityRouteDtl();
                    if (routeType.Equals(Constants.routeTag))
                    {
                        secRouteDtl.GateID = ((GateTO)item.Tag).GateID;
                    }
                    else
                    {
                        secRouteDtl.GateID = ((SecurityRoutesPoint)item.Tag).ControlPointID;
                    }
                    secRouteDtl.GateName = item.Text.Trim();
                    secRouteDtl.SegmentID = segmentID;
                    secRouteDtl.TimeFrom = DateTime.Parse(item.SubItems[1].Text.Trim());
                    secRouteDtl.TimeTo = DateTime.Parse(item.SubItems[2].Text.Trim());

                    secRouteHdr.Segments.Add(segmentID, secRouteDtl);

                    segmentID++;
                }

                int rowsAffected = secRouteHdr.Save();

                if (rowsAffected > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("routesInserted", culture), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        if (routeType.Equals(Constants.routeTag))
                        {
                            populateGates();
                        }
                        else
                        {
                            populatePoints();
                        }
                        lvRoutes.Items.Clear();
                        numNextVisit.Value = Constants.defaultNextVisit;
                        dtpFrom.Value = new DateTime(1900, 1, 1, 0, 0, 0);
                        dtpTo.Value = new DateTime(1900, 1, 1, 0, 0, 0);
                        tbName.Text = tbDesc.Text = "";
                        secRouteHdr = new SecurityRouteHdr();
                        secRouteDtl = new SecurityRouteDtl();
                        tbName.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("routesNotInserted", culture));
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.WaitCursor;
            }
        }

        private void SecurityRoutesAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " SecurityRoutesAdd.SecurityRoutesAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}