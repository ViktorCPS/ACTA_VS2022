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

using Util;
using Common;
using TransferObjects;

namespace UI
{
    public partial class SystemClosingEvents : Form
    {
        const int TypeIndex = 0;
        const int StartIndex = 1;
        const int EndIndex = 2;
        const int MsgSRIndex = 3;
        const int MsgENIndex = 4;
        const int DPStateIndex = 5;
        const int DPStateModTimeIndex = 6;

        private int sortOrder;
        private int sortField;
        
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        List<ApplRoleTO> currentRoles;
        Hashtable menuItemsPermissions;
        string menuItemID;

        bool readPermission = false;
        bool addPermission = false;
        bool updatePermission = false;
        bool deletePermission = false;

        List<SystemClosingEventTO> currentEventList = new List<SystemClosingEventTO>();
        
        public SystemClosingEvents()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(SystemClosingEvents).Assembly);

                logInUser = NotificationController.GetLogInUser();

                setLanguage();
                
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Inner Class for sorting Array List of Closing Events

        /*
		 *  Class used for sorting Array List of Closing Events
		*/

        private class ArrayListSort : IComparer<SystemClosingEventTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(SystemClosingEventTO x, SystemClosingEventTO y)
            {
                SystemClosingEventTO e1 = null;
                SystemClosingEventTO e2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    e1 = x;
                    e2 = y;
                }
                else
                {
                    e1 = y;
                    e2 = x;
                }

                switch (compField)
                {
                    case SystemClosingEvents.TypeIndex:
                        return e1.Type.CompareTo(e2.Type);
                    case SystemClosingEvents.StartIndex:
                        return e1.StartTime.CompareTo(e2.StartTime);
                    case SystemClosingEvents.EndIndex:
                        return e1.EndTime.CompareTo(e2.EndTime);
                    case SystemClosingEvents.MsgSRIndex:
                        return e1.MessageSR.CompareTo(e2.MessageSR);
                    case SystemClosingEvents.MsgENIndex:
                        return e1.MessageEN.CompareTo(e2.MessageEN);
                    case SystemClosingEvents.DPStateIndex:
                        return e1.DPEngineState.CompareTo(e2.DPEngineState);
                    case SystemClosingEvents.DPStateModTimeIndex:
                        return e1.DPEngineStateModifiedTime.CompareTo(e2.DPEngineStateModifiedTime);                    
                    default:
                        return e1.StartTime.CompareTo(e2.StartTime);
                }
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setLanguage()
        {
            try
            {
                this.Text = rm.GetString("SystemClosingEvents", culture);

                //label's text                
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnSearch.Text = rm.GetString("btnSearch", culture);
                this.btnAdd.Text = rm.GetString("btnAdd", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
                this.btnDelete.Text = rm.GetString("btnDelete", culture);

                //group box text
                this.gbDateInterval.Text = rm.GetString("gbDateInterval", culture);
                                
                // list view
                lvClosingEvents.BeginUpdate();
                lvClosingEvents.Columns.Add(rm.GetString("hdrType", culture), lvClosingEvents.Width / 7 - 3, HorizontalAlignment.Left);
                lvClosingEvents.Columns.Add(rm.GetString("hdrStart", culture), lvClosingEvents.Width / 7 - 3, HorizontalAlignment.Left);
                lvClosingEvents.Columns.Add(rm.GetString("hdrEnd", culture), lvClosingEvents.Width / 7 - 3, HorizontalAlignment.Left);
                lvClosingEvents.Columns.Add(rm.GetString("hdrMessageSR", culture), lvClosingEvents.Width / 7 - 3, HorizontalAlignment.Left);
                lvClosingEvents.Columns.Add(rm.GetString("hdrMessageEN", culture), lvClosingEvents.Width / 7 - 3, HorizontalAlignment.Left);
                lvClosingEvents.Columns.Add(rm.GetString("hdrDPEngineState", culture), lvClosingEvents.Width / 7 - 3, HorizontalAlignment.Left);
                lvClosingEvents.Columns.Add(rm.GetString("hdrDPEngineStateModTime", culture), lvClosingEvents.Width / 7 - 3, HorizontalAlignment.Left);
                lvClosingEvents.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEvents.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void SystemClosingEvents_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                sortOrder = Constants.sortAsc;
                sortField = StartIndex;

                menuItemsPermissions = NotificationController.GetMenuItemsPermissions();
                currentRoles = NotificationController.GetCurrentRoles();
                menuItemID = NotificationController.GetCurrentMenuItemID();

                setVisibility();                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEvents.SystemClosingEvents_Load(): " + ex.Message + "\n");
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

                btnSearch.Enabled = readPermission;
                btnAdd.Enabled = addPermission;
                btnUpdate.Enabled = updatePermission;
                btnDelete.Enabled = deletePermission;                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvents.setVisibility(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateClosingEvents()
        {
            try
            {
                lvClosingEvents.BeginUpdate();
                lvClosingEvents.Items.Clear();

                foreach (SystemClosingEventTO eventTO in currentEventList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = eventTO.Type;
                    string timeFormat = Constants.dateFormat + " " + Constants.timeFormat;
                    if (eventTO.Type.Trim().ToUpper().Equals(Constants.closingEventTypeRegularPeriodical))
                        timeFormat = Constants.timeFormat;
                    item.SubItems.Add(eventTO.StartTime.ToString(timeFormat));
                    if (eventTO.EndTime != new DateTime())
                        item.SubItems.Add(eventTO.EndTime.ToString(timeFormat));
                    else
                        item.SubItems.Add("N/A");
                    item.SubItems.Add(eventTO.MessageSR.Trim());
                    item.SubItems.Add(eventTO.MessageEN.Trim());
                    if (eventTO.DPEngineState.Trim() != "")
                        item.SubItems.Add(eventTO.DPEngineState.Trim());
                    else
                        item.SubItems.Add("N/A");
                    if (eventTO.DPEngineStateModifiedTime != new DateTime())
                        item.SubItems.Add(eventTO.DPEngineStateModifiedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat));
                    else
                        item.SubItems.Add("N/A");
                    item.Tag = eventTO;
                    lvClosingEvents.Items.Add(item);
                }

                lvClosingEvents.EndUpdate();
                lvClosingEvents.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvents.populateClosingEvents(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    MessageBox.Show(rm.GetString("invalidPeriod", culture));
                    return;
                }

                currentEventList = new SystemClosingEvent().Search(dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddMinutes(-1));

                populateClosingEvents();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvents.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvClosingEvents_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int prevOrder = sortOrder;

                if (e.Column == sortField)
                {
                    if (prevOrder == Constants.sortAsc)
                    {
                        sortOrder = Constants.sortDesc;
                    }
                    else
                    {
                        sortOrder = Constants.sortAsc;
                    }
                }
                else
                {
                    // New Sort Order
                    sortOrder = Constants.sortAsc;
                }

                sortField = e.Column;

                currentEventList.Sort(new ArrayListSort(sortOrder, sortField));                
                populateClosingEvents();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvents.lvClosingEvents_ColumnClick(): " + ex.Message + "\n");
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
                if (lvClosingEvents.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noClosingEventsSelected", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                bool deleted = true;
                SystemClosingEvent closingEvent = new SystemClosingEvent();
                foreach (ListViewItem item in lvClosingEvents.SelectedItems)
                {
                    if (!closingEvent.Delete(((SystemClosingEventTO)item.Tag).EventID, true))
                        deleted = false;
                }

                if (deleted)
                    MessageBox.Show(rm.GetString("closingEventsDeleted", culture));
                else
                    MessageBox.Show(rm.GetString("closingEventsNotDeleted", culture));

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvents.btnDelete_Click(): " + ex.Message + "\n");
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
                SystemClosingEventAdd addForm = new SystemClosingEventAdd(new SystemClosingEventTO());
                addForm.ShowDialog();

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvents.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvClosingEvents.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noClosingEventsSelected", culture));
                    return;
                }

                SystemClosingEventAdd addForm = new SystemClosingEventAdd((SystemClosingEventTO)lvClosingEvents.SelectedItems[0].Tag);
                addForm.ShowDialog();

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemClosingEvents.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
