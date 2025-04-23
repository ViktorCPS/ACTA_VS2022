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
    public partial class SystemMessages : Form
    {
        const int CompanyIndex = 0;
        const int RoleIndex = 1;
        const int StartIndex = 2;
        const int EndIndex = 3;
        const int MsgSRIndex = 4;
        const int MsgENIndex = 5;
        
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

        List<SystemMessageTO> currentMsgList = new List<SystemMessageTO>();

        Dictionary<int, WorkingUnitTO> companyDict = new Dictionary<int, WorkingUnitTO>();
        Dictionary<int, ApplUserCategoryTO> roleDict = new Dictionary<int, ApplUserCategoryTO>();

        private string wuString = "";
        private string companyString = "";

        public SystemMessages()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(SystemMessages).Assembly);

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

        private class ArrayListSort : IComparer<SystemMessageTO>
        {
            private int compOrder;
            private int compField;
            public ArrayListSort(int sortOrder, int sortField)
            {
                compOrder = sortOrder;
                compField = sortField;
            }

            public int Compare(SystemMessageTO x, SystemMessageTO y)
            {
                SystemMessageTO msg1 = null;
                SystemMessageTO msg2 = null;

                if (compOrder == Constants.sortAsc)
                {
                    msg1 = x;
                    msg2 = y;
                }
                else
                {
                    msg1 = y;
                    msg2 = x;
                }

                switch (compField)
                {
                    case SystemMessages.CompanyIndex:
                        return msg1.Company.CompareTo(msg2.Company);
                    case SystemMessages.RoleIndex:
                        return msg1.Role.CompareTo(msg2.Role);
                    case SystemMessages.StartIndex:
                        return msg1.StartTime.CompareTo(msg2.StartTime);
                    case SystemMessages.EndIndex:
                        return msg1.EndTime.CompareTo(msg2.EndTime);
                    case SystemMessages.MsgSRIndex:
                        return msg1.MessageSR.CompareTo(msg2.MessageSR);
                    case SystemMessages.MsgENIndex:
                        return msg1.MessageEN.CompareTo(msg2.MessageEN);                    
                    default:
                        return msg1.StartTime.CompareTo(msg2.StartTime);
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
                this.Text = rm.GetString("SystemMessages", culture);

                //label's text                
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblCompany.Text = rm.GetString("lblCompany", culture);
                this.lblRole.Text = rm.GetString("lblRole", culture);

                //button's text
                this.btnClose.Text = rm.GetString("btnClose", culture);
                this.btnSearch.Text = rm.GetString("btnSearch", culture);
                this.btnAdd.Text = rm.GetString("btnAdd", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
                this.btnDelete.Text = rm.GetString("btnDelete", culture);

                //group box text
                this.gbDateInterval.Text = rm.GetString("gbDateInterval", culture);

                // list view
                lvMessages.BeginUpdate();
                lvMessages.Columns.Add(rm.GetString("hdrCompany", culture), lvMessages.Width / 6 - 4, HorizontalAlignment.Left);
                lvMessages.Columns.Add(rm.GetString("hdrRole", culture), lvMessages.Width / 6 - 4, HorizontalAlignment.Left);
                lvMessages.Columns.Add(rm.GetString("hdrStart", culture), lvMessages.Width / 6 - 4, HorizontalAlignment.Left);
                lvMessages.Columns.Add(rm.GetString("hdrEnd", culture), lvMessages.Width / 6 - 4, HorizontalAlignment.Left);
                lvMessages.Columns.Add(rm.GetString("hdrMessageSR", culture), lvMessages.Width / 6 - 4, HorizontalAlignment.Left);
                lvMessages.Columns.Add(rm.GetString("hdrMessageEN", culture), lvMessages.Width / 6 - 4, HorizontalAlignment.Left);
                lvMessages.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemMessages.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void SystemMessages_Load(object sender, EventArgs e)
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

                List<WorkingUnitTO> wUnits = new List<WorkingUnitTO>();
                if (logInUser != null)                
                    wUnits = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.EmployeesPurpose);                

                foreach (WorkingUnitTO wUnit in wUnits)
                {
                    wuString += wUnit.WorkingUnitID.ToString().Trim() + ",";                    
                }

                if (wuString.Length > 0)
                    wuString = wuString.Substring(0, wuString.Length - 1);

                populateCompany();
                populateRoles();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemMessages.SystemMessages_Load(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SystemMessages.setVisibility(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateRoles()
        {
            try
            {
                List<ApplUserCategoryTO> roleList = new ApplUserCategory().Search(null);

                foreach (ApplUserCategoryTO category in roleList)
                {
                    if (!roleDict.ContainsKey(category.CategoryID))
                        roleDict.Add(category.CategoryID, category);
                }

                ApplUserCategoryTO emptyRole = new ApplUserCategoryTO();
                emptyRole.Name = rm.GetString("all", culture);

                roleList.Insert(0, emptyRole);

                cbRole.DataSource = roleList;
                cbRole.DisplayMember = "Name";
                cbRole.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessages.populateRoles(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateCompany()
        {
            try
            {
                List<WorkingUnitTO> companyList = new WorkingUnit().getRootWorkingUnitsList(wuString);

                foreach (WorkingUnitTO wu in companyList)
                {
                    if (!companyDict.ContainsKey(wu.WorkingUnitID))
                        companyDict.Add(wu.WorkingUnitID, wu);

                    companyString += wu.WorkingUnitID.ToString().Trim() + ",";
                }

                if (companyString.Length > 0)
                    companyString = companyString.Substring(0, companyString.Length - 1);

                companyList.Insert(0, new WorkingUnitTO(-1, -1, "", rm.GetString("all", culture), "", -1));

                cbCompany.DataSource = companyList;
                cbCompany.DisplayMember = "Name";
                cbCompany.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemMessages.populateCompany(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateMessagesView()
        {
            try
            {
                lvMessages.BeginUpdate();
                lvMessages.Items.Clear();

                foreach (SystemMessageTO msgTO in currentMsgList)
                {
                    ListViewItem item = new ListViewItem();
                    if (companyDict.ContainsKey(msgTO.WorkingUnitID))
                        msgTO.Company = companyDict[msgTO.WorkingUnitID].Name.Trim();
                    item.Text = msgTO.Company.Trim();
                    if (roleDict.ContainsKey(msgTO.ApplUserCategoryID))
                        msgTO.Role = roleDict[msgTO.ApplUserCategoryID].Name.Trim();
                    item.SubItems.Add(msgTO.Role.Trim());
                    string timeFormat = Constants.dateFormat + " " + Constants.timeFormat;
                    item.SubItems.Add(msgTO.StartTime.ToString(timeFormat));
                    item.SubItems.Add(msgTO.EndTime.ToString(timeFormat));                    
                    item.SubItems.Add(msgTO.MessageSR.Trim());
                    item.SubItems.Add(msgTO.MessageEN.Trim());                    
                    item.Tag = msgTO;
                    lvMessages.Items.Add(item);
                }

                lvMessages.EndUpdate();
                lvMessages.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessages.populateMessagesView(): " + ex.Message + "\n");
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

                string company = companyString;
                if (cbCompany.SelectedIndex > 0)
                    company = cbCompany.SelectedValue.ToString().Trim();

                int role = -1;
                if (cbRole.SelectedIndex > 0 && !int.TryParse(cbRole.SelectedValue.ToString().Trim(), out role))
                    role = -1;

                currentMsgList = new SystemMessage().Search(dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddMinutes(-1), company, role);

                populateMessagesView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessages.btnSearch_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvMessages_ColumnClick(object sender, ColumnClickEventArgs e)
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

                currentMsgList.Sort(new ArrayListSort(sortOrder, sortField));
                populateMessagesView();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessages.lvMessages_ColumnClick(): " + ex.Message + "\n");
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
                if (lvMessages.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noMessagesSelected", culture));
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                bool deleted = true;
                SystemMessage msg = new SystemMessage();
                foreach (ListViewItem item in lvMessages.SelectedItems)
                {
                    if (!msg.Delete(((SystemMessageTO)item.Tag).MessageID, true))
                        deleted = false;
                }

                if (deleted)
                    MessageBox.Show(rm.GetString("messagesDeleted", culture));
                else
                    MessageBox.Show(rm.GetString("messagesNotDeleted", culture));

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessages.btnDelete_Click(): " + ex.Message + "\n");
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
                SystemMessageAdd addForm = new SystemMessageAdd(new SystemMessageTO());
                addForm.ShowDialog();

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessages.btnAdd_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvMessages.SelectedItems.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noMessagesSelected", culture));
                    return;
                }

                SystemMessageAdd addForm = new SystemMessageAdd((SystemMessageTO)lvMessages.SelectedItems[0].Tag);
                addForm.ShowDialog();

                btnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessages.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
