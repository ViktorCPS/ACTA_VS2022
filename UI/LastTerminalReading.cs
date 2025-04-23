using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;

using TransferObjects;
using Common;
using Util;

namespace UI
{
    public partial class LastTerminalReading : Form
    {
        ResourceManager rm;
        CultureInfo culture;
        DebugLog log;
        
        ApplUserTO logInUser;
                
        public LastTerminalReading()
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                
                logInUser = NotificationController.GetLogInUser();
                               
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(LastTerminalReading).Assembly);
                setLanguage();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LastTerminalReading.LastTerminalReading(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
        
        private void setLanguage()
        {
            try
            {
                // Form name
                this.Text = rm.GetString("LastTerminalReading", culture);

                // button's text                
                btnClose.Text = rm.GetString("btnClose", culture);
                btnRefresh.Text = rm.GetString("btnRefresh", culture);

                // label's text                
                lblReaders.Text = rm.GetString("lblReaders", culture);
                lblProcessed.Text = rm.GetString("lblProcessed", culture);

                // list view
                lvReaders.BeginUpdate();
                lvReaders.Columns.Add(rm.GetString("hdrReaderID", culture), (lvReaders.Width) / 7 - 70, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrDescripton", culture), (lvReaders.Width) / 7 + 100, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrPing", culture), (lvReaders.Width) / 7 - 40, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrLastIn", culture), (lvReaders.Width) / 7 - 20, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrLastOut", culture), (lvReaders.Width) / 7 - 20, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrLastTags", culture), (lvReaders.Width) / 7 - 10, HorizontalAlignment.Left);
                lvReaders.Columns.Add(rm.GetString("hdrLastAccessProfiles", culture), (lvReaders.Width) / 7 + 45, HorizontalAlignment.Left);
                lvReaders.EndUpdate();

                lvLogs.BeginUpdate();
                lvLogs.Columns.Add(rm.GetString("lblTerminalLogs", culture), lvLogs.Width - 25, HorizontalAlignment.Left);
                lvLogs.EndUpdate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LastTerminalReading.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void populateListView()
        {
            try
            {
                Reader r = new Reader();
                r.RdrTO.Status = Constants.readerStatusEnabled.Trim();
                List<ReaderTO> readerList = r.Search();                
                ArrayList uploadTimeList = new AccessControlFile().SearchLastUploadTime();

                lvReaders.BeginUpdate();
                lvReaders.Items.Clear();

                foreach (ReaderTO reader in readerList)
                {
                    ListViewItem item = new ListViewItem();
                    item.UseItemStyleForSubItems = false;
                    item.Text = reader.ReaderID.ToString();
                    item.SubItems.Add(reader.Description);

                   acPing ping = new acPing();

                    // Try Ping if IP
                    if (((reader.ConnectionType.Equals(Constants.ConnTypeIP))
                         && (ping.Completed(reader.ConnectionAddress, 2)))
                         || (reader.ConnectionType.Equals(Constants.ConnTypeSerial)))
                    {
                        item.SubItems.Add(rm.GetString("ok", culture));
                        item.SubItems[2].BackColor = Color.LightGreen;
                    }
                    else
                    {
                        item.SubItems.Add(rm.GetString("notok", culture));
                        item.SubItems[2].BackColor = Color.Red;
                    }

                    // get last IN and OUT
                    DateTime lastIN = new Reader().SearchLastLogUsed(reader.ReaderID, Constants.DirectionIn.Trim().ToUpper());
                    DateTime lastOUT = new Reader().SearchLastLogUsed(reader.ReaderID, Constants.DirectionOut.Trim().ToUpper());
                    
                    if (lastIN.Equals(new DateTime()))
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(lastIN.ToString("dd.MM.yyyy HH:mm"));
                    if (lastIN < DateTime.Now.Date)
                        item.SubItems[3].BackColor = Color.Red;

                    if (lastOUT.Equals(new DateTime()))
                        item.SubItems.Add("");
                    else
                        item.SubItems.Add(lastOUT.ToString("dd.MM.yyyy HH:mm"));
                    if (lastOUT < DateTime.Now.Date)
                        item.SubItems[4].BackColor = Color.Red;

                    string type = "";                   
                    for (int counter = 0; counter < 2; counter++)
                    {
                        DateTime lastUploadTime = new DateTime();
                        if (counter == 0)
                        {
                            type = Constants.ACFilesTypeCards;                            
                        }
                        else if (counter == 1)
                        {
                            type = Constants.ACFilesTypeTAProfile;                            
                        }                                                

                        foreach (AccessControlFile acf in uploadTimeList)
                        {
                            if ((acf.ReaderID == reader.ReaderID)
                                && (acf.Type == type))
                            {
                                lastUploadTime = acf.UploadEndTime;
                                break;
                            }
                        }

                        if (lastUploadTime.Equals(new DateTime()))
                            item.SubItems.Add("");
                        else
                            item.SubItems.Add(lastUploadTime.ToString("dd.MM.yyyy HH:mm"));
                    }

                    item.Tag = reader.ReaderID;
                    lvReaders.Items.Add(item);
                }

                lvReaders.EndUpdate();
                lvReaders.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LastTerminalReading.populateListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void LastTerminalReading_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                btnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LastTerminalReading.LastTerminalReading_Load(): " + ex.Message + "\n");
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
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LastTerminalReading.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void lvReaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (lvReaders.SelectedItems.Count > 0)
                {
                    List<LogTO> logs = new Log().SearchLogsForReader((int) lvReaders.SelectedItems[0].Tag);

                    lvLogs.BeginUpdate();
                    lvLogs.Items.Clear();
                    foreach (LogTO log in logs)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = log.TagID.ToString().Trim().PadRight(20, ' ') + log.Antenna.ToString().Trim().PadRight(20, ' ')
                            + log.EventHappened.ToString().Trim().PadRight(20, ' ') + log.ActionCommited.ToString().Trim().PadRight(20, ' ')
                            + log.EventTime.ToString("dd.MM.yyyy HH:mm").Trim().PadRight(25, ' ') + log.PassGenUsed.ToString().Trim().PadRight(20, ' ');
                        item.ToolTipText = log.LogID.ToString().Trim();
                        lvLogs.Items.Add(item);
                    }
                    lvLogs.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LastTerminalReading.lvReaders_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                MessageForm msgForm = new MessageForm(rm.GetString("msgReadersConnTrying", culture));
                msgForm.Show();

                lvLogs.BeginUpdate();
                lvLogs.Items.Clear();
                lvLogs.EndUpdate();

                populateListView();

                msgForm.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " LastTerminalReading.btnRefresh_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}