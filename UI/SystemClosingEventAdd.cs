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
    public partial class SystemClosingEventAdd : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        SystemClosingEventTO eventTO = new SystemClosingEventTO();

        public SystemClosingEventAdd(SystemClosingEventTO eventTO)
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

                this.eventTO = eventTO;

                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setLanguage()
        {
            try
            {
                if (eventTO.EventID == -1)
                    this.Text = rm.GetString("SystemClosingEventAdd", culture);
                else
                    this.Text = rm.GetString("SystemClosingEventUpd", culture);

                //label's text                
                this.lblFrom.Text = rm.GetString("lblFrom", culture);
                this.lblTo.Text = rm.GetString("lblTo", culture);
                this.lblType.Text = rm.GetString("lblType", culture);

                //button's text
                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
                
                // check box text
                this.chbNotDefined.Text = rm.GetString("chbNotDefined", culture);

                //group box text
                this.gbDateInterval.Text = rm.GetString("gbDateInterval", culture);
                this.gbMessage.Text = rm.GetString("gbMessage", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEventAdd.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void SystemClosingEventAdd_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                populateTypes();

                if (eventTO.EventID != -1)
                {
                    dtpFrom.Value = eventTO.StartTime;

                    if (eventTO.EndTime != new DateTime())
                        dtpTo.Value = eventTO.EndTime;
                    else
                        chbNotDefined.Checked = true;

                    cbType.Text = eventTO.Type.Trim();

                    tbMessageSR.Text = eventTO.MessageSR.Trim();
                    tbMessageEN.Text = eventTO.MessageEN.Trim();

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEventAdd.SystemClosingEventAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void populateTypes()
        {
            try
            {
                List<string> types = new List<string>();
                types.Add(Constants.closingEventTypeDemanded);
                types.Add(Constants.closingEventTypeRegularPeriodical);

                cbType.DataSource = types;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEventAdd.populateTypes(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string customFormat = "dd.MM.yyyy. HH:mm";

                if (cbType.Text != null && cbType.Text.Trim().ToUpper() == Constants.closingEventTypeRegularPeriodical.Trim().ToUpper())
                    customFormat = "HH:mm";

                dtpFrom.CustomFormat = customFormat;
                dtpTo.CustomFormat = customFormat;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEventAdd.cbType_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void chbNotDefined_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblTo.Enabled = dtpTo.Enabled = !chbNotDefined.Checked;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEventAdd.chbNotDefined_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (validate())
                {
                    SystemClosingEvent closingEvent = new SystemClosingEvent();
                    closingEvent.EventTO = eventTO;
                    if (closingEvent.Save(true) > 0)
                    {
                        MessageBox.Show(rm.GetString("closingEventSaved", culture));
                        this.Close();
                    }
                    else
                        MessageBox.Show(rm.GetString("closingEventNotSaved", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEventAdd.btnSave_Click(): " + ex.Message + "\n");
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
                    SystemClosingEvent closingEvent = new SystemClosingEvent();
                    closingEvent.EventTO = eventTO;
                    if (closingEvent.Update(true))
                    {
                        MessageBox.Show(rm.GetString("closingEventUpdated", culture));
                        this.Close();
                    }
                    else
                        MessageBox.Show(rm.GetString("closingEventNotUpdated", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEventAdd.btnUpdate_Click(): " + ex.Message + "\n");
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
                if (cbType.Text.Trim().ToUpper() != Constants.closingEventTypeDemanded.Trim().ToUpper()
                    && cbType.Text.Trim().ToUpper() != Constants.closingEventTypeRegularPeriodical.Trim().ToUpper())
                {
                    MessageBox.Show(rm.GetString("notRegularType", culture));
                    cbType.Focus();
                    return false;
                }

                eventTO.Type = cbType.Text.Trim().ToUpper();

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

                if (eventTO.Type == Constants.closingEventTypeRegularPeriodical && chbNotDefined.Checked)
                {
                    MessageBox.Show(rm.GetString("periodicalEventWithoutEnd", culture));
                    chbNotDefined.Focus();
                    return false;
                }

                DateTime startTime = new DateTime();
                DateTime endTime = new DateTime();

                if (eventTO.Type == Constants.closingEventTypeDemanded)
                {
                    startTime = dtpFrom.Value.AddSeconds(-dtpFrom.Value.Second);

                    if (!chbNotDefined.Checked)
                    {
                        endTime = dtpTo.Value.AddSeconds(-dtpTo.Value.Second);
                        
                        if (startTime >= endTime)
                        {
                            MessageBox.Show(rm.GetString("invalidPeriod", culture));
                            dtpFrom.Focus();
                            return false;
                        }

                        if (startTime < DateTime.Now.AddMinutes(-10) && endTime < DateTime.Now.AddMinutes(-10))
                        {
                            MessageBox.Show(rm.GetString("pastClosing", culture));
                            dtpFrom.Focus();
                            return false;
                        }
                    }
                }
                else if (eventTO.Type == Constants.closingEventTypeRegularPeriodical)
                {
                    startTime = Constants.dateTimeNullValue().AddHours(dtpFrom.Value.Hour).AddMinutes(dtpFrom.Value.Minute);
                    endTime = Constants.dateTimeNullValue().AddHours(dtpTo.Value.Hour).AddMinutes(dtpTo.Value.Minute);
                }

                // check overlaping with existing closing intervals
                List<SystemClosingEventTO> closingList = new SystemClosingEvent().Search(startTime, endTime);

                bool overlaping = false;

                foreach (SystemClosingEventTO evtTO in closingList)
                {
                    if (evtTO.EventID == eventTO.EventID)
                        continue;

                    if (eventTO.Type == Constants.closingEventTypeDemanded)
                    {
                        if (evtTO.Type != Constants.closingEventTypeDemanded)
                            continue;

                        if (endTime == new DateTime())
                        {
                            if (evtTO.EndTime == new DateTime())
                            {
                                overlaping = true;
                                break;
                            }

                            if (evtTO.EndTime > startTime)
                            {
                                overlaping = true;
                                break;
                            }
                        }
                        else
                        {
                            if (evtTO.EndTime == new DateTime() && endTime > evtTO.StartTime)
                            {
                                overlaping = true;
                                break;
                            }
                            else if ((startTime <= evtTO.StartTime && endTime > evtTO.StartTime)
                                || (startTime > evtTO.StartTime && startTime < evtTO.EndTime))
                            {
                                overlaping = true;
                                break;
                            }
                        }
                    }
                    else if (eventTO.Type == Constants.closingEventTypeRegularPeriodical)
                    {
                        if (evtTO.Type != Constants.closingEventTypeRegularPeriodical)
                            continue;

                        if (startTime.TimeOfDay > endTime.TimeOfDay)
                        {
                            if (evtTO.StartTime.TimeOfDay > evtTO.EndTime.TimeOfDay)
                            {
                                overlaping = true;
                                break;
                            }
                            else
                            {
                                if (evtTO.StartTime.TimeOfDay < endTime.TimeOfDay || evtTO.EndTime.TimeOfDay > startTime.TimeOfDay)
                                {
                                    overlaping = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (evtTO.StartTime.TimeOfDay > evtTO.EndTime.TimeOfDay
                                && (evtTO.EndTime.TimeOfDay > startTime.TimeOfDay || evtTO.StartTime.TimeOfDay < endTime.TimeOfDay))
                            {
                                overlaping = true;
                                break;
                            }
                            else if ((startTime.TimeOfDay <= evtTO.StartTime.TimeOfDay && endTime.TimeOfDay > evtTO.StartTime.TimeOfDay)
                                || (startTime.TimeOfDay > evtTO.StartTime.TimeOfDay && startTime.TimeOfDay < evtTO.EndTime.TimeOfDay))
                            {
                                overlaping = true;
                                break;
                            }
                        }
                    }
                }

                if (overlaping)
                {
                    MessageBox.Show(rm.GetString("overlapingClosingEvents", culture));
                    dtpFrom.Focus();
                    return false;
                }

                eventTO.StartTime = startTime;
                eventTO.EndTime = endTime;

                eventTO.MessageSR = tbMessageSR.Text.Trim();
                eventTO.MessageEN = tbMessageEN.Text.Trim();

                eventTO.DPEngineState = "";
                eventTO.DPEngineStateModifiedTime = new DateTime();

                return true;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEventAdd.validate(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void btnSelectMsgSR_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> msgList = new SystemClosingEvent().SearchMessages(Constants.Lang_sr);

                if (msgList.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noMessageSRDefined", culture));
                    return;
                }

                MessagesPanel form = new MessagesPanel(msgList);
                form.ShowDialog();

                if (form.Message.Trim() != "")
                    tbMessageSR.Text = form.Message.Trim();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEventAdd.btnSelectMsgSR_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSelectMsgEN_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> msgList = new SystemClosingEvent().SearchMessages(Constants.Lang_en);

                if (msgList.Count <= 0)
                {
                    MessageBox.Show(rm.GetString("noMessageENDefined", culture));
                    return;
                }

                MessagesPanel form = new MessagesPanel(msgList);
                form.ShowDialog();

                if (form.Message.Trim() != "")
                    tbMessageEN.Text = form.Message.Trim();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SystemClosingEventAdd.btnSelectMsgEN_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}
