using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Collections;

using Common;
using Util;
using TransferObjects;
using ReaderInterface;

using XKWrapper;

namespace UI
{
    public partial class SecurityRoutesPointsAdd : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        SecurityRoutesPoint point;

        IReaderInterface readerInterface;

        static int desktopReaderCOMPort = 0;

        XKWrapper.XKWrapper xkw;
        Int16 nDevices;
        StringBuilder sb1 = new StringBuilder("                                                                ", 256);
        StringBuilder sb2 = new StringBuilder("                  ", 256);
        bool Connected;

        public SecurityRoutesPointsAdd(SecurityRoutesPoint point,string routeType)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                this.point = point;

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(SecurityRoutesPointsAdd).Assembly);

                setLanguage();

                ReaderFactory.TechnologyType = new Reader().GetDefaultTechnology();
                readerInterface = ReaderFactory.GetReader;

                logInUser = NotificationController.GetLogInUser();
                                
                // Update form
                if (this.point.ControlPointID >= 0)
                {
                    tbID.Enabled = false;
                    tbID.Text = point.ControlPointID.ToString().Trim();
                    tbName.Text = point.Name.Trim();
                    tbDesc.Text = point.Description.Trim();
                    tbTag.Text = point.TagID.ToString().Trim();
                    btnSave.Visible = false;
                }
                // Add form
                else
                {
                   int maxID = new SecurityRoutesPoint().GetMaxID();
                   if (maxID >= 0)
                   {
                       maxID++;
                       tbID.Text = maxID.ToString();
                   }
                   else
                       tbID.Text = "1";
                    btnUpdate.Visible = false;
                }
                if (routeType.Equals(Constants.routeTag))
                {
                    btnReadTag.Visible = true;
                    btnReadTagXPocket.Visible = false;
                }
                else
                {
                    btnReadTagXPocket.Visible = true;
                    btnReadTag.Visible = false;
                    bool init = InitializeXPocket();
                    btnReadTagXPocket.Enabled = init;
                    if (init)
                    {
                        int num = NumberOfNewRecord();
                        if (num < 0)
                        {
                            MessageBox.Show(rm.GetString("InitConnectionFaild", culture));
                            btnReadTagXPocket.Enabled = false;
                        }
                        else if (num > 0)
                        {
                            btnReadTagXPocket.Enabled = false;
                            MessageBox.Show(rm.GetString("DownloadLogsFirst", culture));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int NumberOfNewRecord()
        {
            int recordNum = 0;
            try
            {
                Int16 r;                
               
                r = xkw.ReqTransactNum2XK(1, 1, sb1);
                if (r == 0)
                {
                    r = XKWrapper.XKWrapper.Hex4_to_Int(sb1.ToString());
                    recordNum = r;
                }
                else
                {
                    recordNum = r;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPointsAdd.InitializeXPocket(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return recordNum;
        }

        private bool InitializeXPocket()
        {
            bool init = false;
            try
            {
                //initialize wrapper
                xkw = new XKWrapper.XKWrapper();
                //initialize XKdll
                xkw.Init();
                //find devices, this will work for one at the time
                nDevices = xkw.FindDevices();
                //if device is found
                if (nDevices > 0)
                {
                    if (nDevices == 1)
                    {
                        bool ret = false;
                        Int16 n;
                        for (n = 0; n < nDevices; n++)  //Get the first one oly
                        {
                            ret = xkw.GetName(n, sb1, sb2);
                            if (ret)
                            {
                                //open USB connecton with XPocket
                                if (xkw.SetChannelXK(1, ",,0.0.0.0,,,,1,3") == 0)
                                {
                                    string s1 = sb2.ToString();                                     
                                    s1 = s1.Substring(s1.IndexOf(' ') + 1);
                                    Int16 num = xkw.SetUsbParameters(1, s1);
                                    if (num == 0)
                                    {
                                        num = xkw.OpenChannelXK(1);
                                        if (num == 0)
                                        {
                                            Connected = true;
                                            s1 = XKWrapper.XKWrapper.ToHexString("00000000");
                                            if (xkw.SendMdcSerialXK(1, 1, s1) != 0)
                                            {
                                                xkw.CloseChannelXK(1);
                                                Connected = false;
                                            }
                                        }
                                    }
                                    
                                }
                            }
                            // The connected condition is when the "program user" knows the XPocket password.
                            if (!Connected)
                            {
                                MessageBox.Show(rm.GetString("InitConnectionFaild", culture));
                                init = false;
                            }
                            else
                            {
                                init = true;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("MoreThenOneXPocket", culture));
                        init = false;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("NoXPocketFound", culture));
                    init = false;
                }              
                                   
                
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPointsAdd.InitializeXPocket(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            return init;
        }

        private void setLanguage()
        {
            try
            {
                // form text
                if (this.point.ControlPointID < 0)
                {
                    this.Text = rm.GetString("SecurityRoutesPointAdd", culture);
                }
                else
                {
                    this.Text = rm.GetString("SecurityRoutesPointUpd", culture);
                }

                // label's text
                this.lblID.Text = rm.GetString("lblID", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
                this.lblDesc.Text = rm.GetString("lblDescription", culture);
                this.lblTag.Text = rm.GetString("lblTagID", culture);

                // button's text
                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
                this.btnReadTag.Text = rm.GetString("btnFromReader", culture);
                this.btnReadTagXPocket.Text = rm.GetString("btnFromReader", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPointsAdd.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SecurityRoutesPointsAdd.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReadTag_Click(object sender, EventArgs e)
        {
            try
            {

                uint tagID = 0;
                this.Cursor = Cursors.WaitCursor;
                if (desktopReaderCOMPort == 0) desktopReaderCOMPort = readerInterface.FindDesktopReader();
                this.Cursor = Cursors.Arrow;
                if (desktopReaderCOMPort == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noDesktopReader", culture));
                    return;
                }
                else
                {
                    tagID = UInt32.Parse(readerInterface.GetTagID(desktopReaderCOMPort));
                }

                if (tagID == 0)
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("noTagOnReader", culture));
                }
                else
                {
                    this.tbTag.Text = tagID.ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                log.writeLog(DateTime.Now + " SecurityRoutesPointsAdd.btnReadTag_Click(): " + ex.Message + "\n");
                MessageBox.Show(rm.GetString("noTagOnReader", culture));
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int pointID = -1;
                string tagID = "";

                // validate
                if (tbID.Text.Equals(""))
                {
                    MessageBox.Show(rm.GetString("pointIDEmpty", culture));
                    tbID.SelectAll();
                    tbID.Focus();
                    return;
                }
                if (!Int32.TryParse(tbID.Text.Trim(), out pointID) || pointID < 0)
                {
                    MessageBox.Show(rm.GetString("pointIDNotNum", culture));
                    tbID.SelectAll();
                    tbID.Focus();
                    return;
                }
                if (tbTag.Text.Equals(""))
                {
                    MessageBox.Show(rm.GetString("tagIDEmpty", culture));
                    tbTag.SelectAll();
                    tbTag.Focus();
                    return;
                }
                tagID = tbTag.Text.Trim();
                //if (!UInt32.TryParse(tbTag.Text.Trim(), out tagID) || tagID == 0)
                //{
                //    MessageBox.Show(rm.GetString("messageTagSave1", culture));
                //    tbTag.SelectAll();
                //    tbTag.Focus();
                //    return;
                //}
                SecurityRoutesPoint point = new SecurityRoutesPoint();
                ArrayList l = point.Search(int.Parse(tbID.Text.ToString()), "", "", "");
                if (l.Count > 0)
                {
                    MessageBox.Show(rm.GetString("existPointWithSameID", culture));
                    tbID.SelectAll();
                    tbID.Focus();
                    return;
                }

                l = point.Search(-1, "", "", tagID);
                if (l.Count > 0)
                {
                    MessageBox.Show(rm.GetString("existPointWithSameTagID", culture));
                    tbTag.SelectAll();
                    tbTag.Focus();
                    return;
                }


                int rowsAffected = new SecurityRoutesPoint().Save(pointID, tbName.Text.Trim(), tbDesc.Text.Trim(), tagID);

                if (rowsAffected > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("pointSaved", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        tbID.Text = tbName.Text = tbDesc.Text = tbTag.Text = "";
                        tbID.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("pointNotSaved", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPointsAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string tagID = "";

                // validate
                if (tbTag.Text.Equals(""))
                {
                    MessageBox.Show(rm.GetString("tagIDEmpty", culture));
                    return;
                }
                tagID = tbTag.Text.Trim();
                //if (!UInt32.TryParse(tbTag.Text.Trim(), out tagID) || tagID == 0)
                //{
                //    MessageBox.Show(rm.GetString("messageTagSave1", culture));
                //    return;
                //}

                bool isUpdated = new SecurityRoutesPoint().Update(this.point.ControlPointID, tbName.Text.Trim(), tbDesc.Text.Trim(), tagID);

                if (isUpdated)
                {
                    MessageBox.Show(rm.GetString("pointUpdated", culture));

                    this.Close();
                }
                else
                {
                    MessageBox.Show(rm.GetString("pointNotUpdated", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesPointsAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void SecurityRoutesPointsAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " Restauran.Restauran_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReadTagXPocket_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int num = NumberOfNewRecord();
                if (num != 1)
                {
                    this.Cursor = Cursors.Arrow;
                    if (num > 1)
                    {
                        MessageBox.Show(rm.GetString("DownloadLogsFirst", culture));
                    }
                    if (num == 0)
                    {
                        MessageBox.Show(rm.GetString("ReadTagFirst", culture));
                    }
                    return;
                }

                Int16 r;
                String s = "";
                r = xkw.ReqNextTransact2XK(1, 1, sb1);  
         
                if (r == 0)
                {                    
                    s = sb1.ToString();
                    tbTag.Text = s.Substring(6, 10);                   
                    r = xkw.ReqNextPointer2XK(1, 1, s);   // move the downloadable record pointer to the next.
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                    MessageBox.Show(rm.GetString("readingNumFaild", culture));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " Restauran.Restauran_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}