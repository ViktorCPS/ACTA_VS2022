using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using TransferObjects;
using Common;
using Util;

namespace UI
{
    public partial class SecurityRoutesReadersAdd : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        ApplUserTO logInUser;

        SecurityRoutesReader reader;

        XKWrapper.XKWrapper xkw;
        Int16 nDevices;
        StringBuilder sb1 = new StringBuilder("                                                                ", 256);
        StringBuilder sb2 = new StringBuilder("                  ", 256);
        bool Connected;


        public SecurityRoutesReadersAdd(SecurityRoutesReader reader)
        {
            try
            {
                InitializeComponent();
                this.CenterToScreen();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                this.reader = reader;

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(SecurityRoutesReadersAdd).Assembly);

                setLanguage();

                logInUser = NotificationController.GetLogInUser();

                // Update form
                if (this.reader.ReaderID >= 0)
                {
                    tbID.Enabled = false;
                    tbID.Text = reader.ReaderID.ToString().Trim();
                    tbName.Text = reader.Name.Trim();
                    tbDesc.Text = reader.Description.Trim();
                    btnSave.Visible = false;
                }
                // Add form
                else
                {
                    lblID.Visible = false;
                    tbID.Visible = false;
                    btnUpdate.Visible = false;
                }
                bool init = InitializeXPocket();
                btnReadTagXPocket.Enabled = init;
               
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
                // form text
                if (this.reader.ReaderID < 0)
                {
                    this.Text = rm.GetString("addReader", culture);
                }
                else
                {
                    this.Text = rm.GetString("updateReader", culture);
                }

                // label's text
                this.lblID.Text = rm.GetString("lblID", culture);
                this.lblName.Text = rm.GetString("lblName", culture);
                this.lblDesc.Text = rm.GetString("lblDescription", culture);

                // button's text
                this.btnCancel.Text = rm.GetString("btnCancel", culture);
                this.btnSave.Text = rm.GetString("btnSave", culture);
                this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesReadersAdd.setLanguage(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " SecurityRoutesReadersAdd.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

                // validate
                int rowsAffected = new SecurityRoutesReader().Save(tbName.Text.Trim(), tbDesc.Text.Trim());

                if (rowsAffected > 0)
                {
                    DialogResult result = MessageBox.Show(rm.GetString("secRouteReaderSaved", culture), "", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        tbID.Text = tbName.Text = tbDesc.Text = "";
                        tbName.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("readerNotSaved", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesReadersAdd.btnSave_Click(): " + ex.Message + "\n");
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

                bool isUpdated = new SecurityRoutesReader().Update(this.reader.ReaderID, tbName.Text.Trim(), tbDesc.Text.Trim());

                if (isUpdated)
                {
                    MessageBox.Show(rm.GetString("readerUpdated", culture));

                    this.Close();
                }
                else
                {
                    MessageBox.Show(rm.GetString("readerNotUpdated", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesReadersAdd.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void SecurityRoutesReadersAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " SecurityRoutesReadersAdd.SecurityRoutesReadersAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnReadTagXPocket_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Int16 r;
                String s = "";
                bool b = false;
                if (b)
                {
                    r = xkw.ReqMdcCodeXK(1, 1, sb2);

                    if (r == 0)
                    {
                        s = sb2.ToString();
                        tbDesc.Text = XKWrapper.XKWrapper.ToAsciiString(s);
                        //r = xkw.ReqMdcCodeXK(1, 1, s);   // move the downloadable record pointer to the next.
                    }
                    else
                    {
                        this.Cursor = Cursors.Arrow;
                        MessageBox.Show(rm.GetString("readingNumFaild", culture));
                        return;
                    }
                }
                else
                {
                    //s = "1111";
                    //sb1.Remove(0, sb1.Length);
                    //s = "000003501510111000000000000000000001234567890000000000000000000000";
                    //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    //byte[] bytes = new byte[64];
                    //bytes[0] = null;
                    //bytes[1] = null;
                    //bytes[2] = 0;
                    //bytes[3] = 5;
                    //bytes[4] = 45;
                    //bytes[5] = 37;
                    //bytes[6] = 1;
                    //bytes[7] = 0;
                    //bytes[8] = 1;
                    //bytes[9] = 1;
                    //bytes[10] = 0;
                    //for(inti = 11; int<

                    //int i = s.Length;
                    //sb1.Append(s);
                    //r = xkw.WriteMdcParamsXK(1, 1,bytes);
                    ////tbDesc.Text = sb1.ToString(2,4);
                    //sb1.Remove(0, sb2.Length);
                    //r = xkw.ReadMdcParamsXK(1, 1, bytes);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SecurityRoutesReadersAdd.btnReadTagXPocket_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
           
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
                                            num = xkw.SendMdcSerialXK(1, 1, s1);
                                            //num = xkw.ResetDeviceXK(1, 1);
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
    }
}