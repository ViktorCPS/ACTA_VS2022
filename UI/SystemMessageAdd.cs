using System;
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
    public partial class SystemMessageAdd : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        SystemMessageTO msgTO = new SystemMessageTO();

        string wuString = "";

        public SystemMessageAdd(SystemMessageTO msgTO)
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(SystemMessageAdd).Assembly);

                logInUser = NotificationController.GetLogInUser();

                this.msgTO = msgTO;

                setLanguage();

                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setLanguage()
        {
            try
            {
                if (this.msgTO.MessageID != -1)
                    this.Text = rm.GetString("SystemMessageUpd", culture);
                else
                    this.Text = rm.GetString("SystemMessageAdd", culture);

                //label's text                
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblCompany.Text = rm.GetString("lblCompany", culture);
                this.lblRole.Text = rm.GetString("lblRole", culture);
                
                //button's text
                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
                                
                //group box text
                this.gbDateInterval.Text = rm.GetString("gbDateInterval", culture);
                this.gbMessage.Text = rm.GetString("gbMessage", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemMessageAdd.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void SystemMessageAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

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

                if (msgTO.MessageID != -1)
                {
                    dtpFrom.Value = msgTO.StartTime;
                    dtpTo.Value = msgTO.EndTime;
                    
                    cbCompany.SelectedValue = msgTO.WorkingUnitID;
                    cbRole.SelectedValue = msgTO.ApplUserCategoryID;

                    tbMessageSR.Text = msgTO.MessageSR.Trim();
                    tbMessageEN.Text = msgTO.MessageEN.Trim();

                    btnSave.Visible = false;
                    btnUpdate.Visible = true;
                }
                else
                {
                    btnSave.Visible = true;
                    btnUpdate.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemMessageAdd.SystemMessageAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateRoles()
        {
            try
            {
                List<ApplUserCategoryTO> roleList = new ApplUserCategory().Search(null);
                
                cbRole.DataSource = roleList;
                cbRole.DisplayMember = "Name";
                cbRole.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SystemMessageAdd.populateRoles(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void populateCompany()
        {
            try
            {
                List<WorkingUnitTO> companyList = new WorkingUnit().getRootWorkingUnitsList(wuString);
                
                cbCompany.DataSource = companyList;
                cbCompany.DisplayMember = "Name";
                cbCompany.ValueMember = "WorkingUnitID";
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemMessageAdd.populateCompany(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (validate())
                {
                    SystemMessage msg = new SystemMessage();
                    msg.MessageTO = msgTO;
                    if (msg.Save(true) > 0)
                    {
                        MessageBox.Show(rm.GetString("messageSaved", culture));
                        this.Close();
                    }
                    else
                        MessageBox.Show(rm.GetString("messageNotSaved", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemMessageAdd.btnSave_Click(): " + ex.Message + "\n");
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

                if (validate())
                {
                    SystemMessage msg = new SystemMessage();
                    msg.MessageTO = msgTO;
                    if (msg.Update(true))
                    {
                        MessageBox.Show(rm.GetString("messageUpdated", culture));
                        this.Close();
                    }
                    else
                        MessageBox.Show(rm.GetString("messageNotUpdated", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemMessageAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private bool validate()
        {
            try
            {                
                msgTO.WorkingUnitID = (int)cbCompany.SelectedValue;
                msgTO.ApplUserCategoryID = (int)cbRole.SelectedValue;

                // messages must be entered
                if (tbMessageSR.Text.Trim() == "")
                {
                    MessageBox.Show(rm.GetString("noMessages", culture));
                    tbMessageSR.Focus();
                    return false;
                }

                if (tbMessageEN.Text.Trim() == "")
                {
                    MessageBox.Show(rm.GetString("noMessages", culture));
                    tbMessageEN.Focus();
                    return false;
                }
                
                DateTime startTime = dtpFrom.Value.AddSeconds(-dtpFrom.Value.Second);
                DateTime endTime = dtpTo.Value.AddSeconds(-dtpFrom.Value.Second);

                if (startTime >= endTime)
                {
                    MessageBox.Show(rm.GetString("invalidPeriod", culture));
                    dtpFrom.Focus();
                    return false;
                }

                if (startTime < DateTime.Now.AddMinutes(-10) && endTime < DateTime.Now.AddMinutes(-10))
                {
                    MessageBox.Show(rm.GetString("pastMessage", culture));
                    dtpFrom.Focus();
                    return false;
                }

                // check overlaping with existing messages
                List<SystemMessageTO> msgList = new SystemMessage().Search(startTime, endTime, msgTO.WorkingUnitID.ToString().Trim(), msgTO.ApplUserCategoryID);

                bool overlaping = false;

                foreach (SystemMessageTO msg in msgList)
                {
                    if (msg.MessageID == msgTO.MessageID)
                        continue;

                    if ((startTime <= msg.StartTime && endTime > msg.StartTime)
                             || (startTime > msg.StartTime && startTime < msg.EndTime))
                    {
                        overlaping = true;
                        break;
                    }
                }

                if (overlaping)
                {
                    MessageBox.Show(rm.GetString("overlapingMessages", culture));
                    dtpFrom.Focus();
                    return false;
                }

                msgTO.StartTime = startTime;
                msgTO.EndTime = endTime;

                msgTO.MessageSR = tbMessageSR.Text.Trim();
                msgTO.MessageEN = tbMessageEN.Text.Trim();

                return true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemMessageAdd.validate(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
