using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.Reflection;
using System.Globalization;
using System.Resources;
using System.Runtime.InteropServices;

using ReaderInterface;
using Util;
using Common;
using TransferObjects;
using System.Net;
using System.Net.Sockets;

namespace ACTARFIDLogin
{
    public partial class ACTARFIDLogin : Form
    {
        private Thread reader;
        bool stopReading;
        DebugLog log;
        CultureInfo culture;
        ResourceManager rm;
        ApplUserLoginRFIDTO currentUser;

        int desktopReaderPort;
        private ulong oldTagID = 0;

        private bool closeApplicationFlag = false;

        public ACTARFIDLogin()
        {
            try
            {
                InitializeComponent();
                currentUser = new ApplUserLoginRFIDTO();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void closeApplication()
        {
            closeApplicationFlag = true;
        }
        public delegate void closeApplicationDelegate();


        public void StartThread()
        {
            try
            {
                stopReading = false;
                reader = new Thread(new ThreadStart(ReadTag));
                reader.Name = "ReadTag";
                reader.Start();
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Thread Started \n");
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTARFIDLogin.StartThread(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public void StopThread()
        {
            try
            {
                this.stopReading = true;
                if (reader != null)
                {
                    reader.Join(); // Wait for the worker's thread to finish.
                }

                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Thread Stopped \n");
                Application.Exit();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTARFIDLogin.StopThread(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void ReadTag()
        {
            try
            {
                while (true)
                {
                    if (!stopReading)
                    {
                        // find terminal
                        string terminalID = GetDesktopReaderProductInfoPort();
                        if (terminalID.Trim().Length == 0)
                        {
                            //MessageBox.Show(rm.GetString("noTerminalFound", culture));
                            MessageBox.Show("Desktop RFID device not found.\n\n     Application will be closed!", "Info");
                            this.Invoke(new closeApplicationDelegate(closeApplication));
                            stopReading = true;
                        }
                        else
                        {
                            // detect RFID card
                            ulong tagID = GetTagSerialNumber();

                            if (tagID > 0)
                            {
                                System.Media.SystemSounds.Beep.Play();
                                string thisHost = LocalIPAddress();
                                currentUser.Host = thisHost;
                                currentUser.TagID = tagID.ToString();
                                ApplUserLoginRFIDTO findUserLogin = new ApplUserLoginRFID().SearchLoginRFID(thisHost);
                                ApplUserLoginRFID userLoginRFID = new ApplUserLoginRFID();
                                userLoginRFID.RfidTO = currentUser;
                                if (findUserLogin.Host.Equals("") && findUserLogin.Host != null)
                                {

                                    int inserted = userLoginRFID.Save(true);
                                    if (inserted == 1)
                                    {
                                        //MessageBox.Show("Inserted");
                                    }
                                    else
                                    {
                                        //MessageBox.Show("Not inserted");
                                    }

                                }
                                else if (findUserLogin.Host.Equals(currentUser.Host))
                                {
                                    bool trans =  userLoginRFID.BeginTransaction();
                                    bool isUpdated = trans;
                                    if (trans)
                                    {
                                        try
                                        {
                                            isUpdated = isUpdated && (userLoginRFID.Delete(thisHost, new DateTime(), false));
                                            if (isUpdated)
                                            {
                                                ApplUserLoginRFID userLoginRFID2 = new ApplUserLoginRFID();
                                                userLoginRFID2.RfidTO = currentUser;
                                                userLoginRFID2.SetTransaction(userLoginRFID.GetTransaction());
                                                isUpdated = isUpdated && userLoginRFID2.Save(false) == 1;
                                                if (isUpdated)
                                                {
                                                    userLoginRFID.CommitTransaction();
                                                }
                                                else
                                                {
                                                    userLoginRFID.RollbackTransaction();
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            isUpdated = false;
                                            if (userLoginRFID.GetTransaction() != null)
                                                userLoginRFID.RollbackTransaction();
                                            log.writeLog(DateTime.Now + " ApplUsers.btnUpdate_Click(): " + ex.Message + "\n");
                                            MessageBox.Show(ex.Message);
                                        }
                                    }
                                    if (isUpdated)
                                    {
                                        //MessageBox.Show(rm.GetString("userUpdated", culture));
                                        //this.Close();
                                    }

                                    //bool updated = userLoginRFID.Update(true);
                                    //if (updated)
                                    //{
                                    //    //MessageBox.Show("Updated");
                                    //}
                                    //else
                                    //{
                                    //    //MessageBox.Show("Not updated");
                                    //}


                                    

                                }
                            }

                            Thread.Sleep(500);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.ReadTagWriteText(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }


        private void ACTARFIDLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StopThread();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTARFIDLogin.ACTARFIDLogin_FormClosing(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void controlTimer_Tick(object sender, EventArgs e)
        {
            if (closeApplicationFlag)
            {
                this.Close();
                Application.ExitThread();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AboutForm aform = new AboutForm();
                aform.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.aboutToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //DialogResult result = MessageBox.Show(rm.GetString("ACTARFIDDesktopClosed", culture), "", MessageBoxButtons.YesNo);
                DialogResult result = MessageBox.Show("Do you want to close the application?", "Info", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.exitToolStripMenuItem_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTARFIDLogin.okButton_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.Hide();
                }
                else if (FormWindowState.Minimized == this.WindowState && this.Visible == true)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                else
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTARFIDLogin.notifyIcon_MouseDoubleClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void ACTARFIDLogin_Load(object sender, EventArgs e)
        {
            try
            {
                NotificationController.SetApplicationName(this.Name);
                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                string terminalID = GetDesktopReaderProductInfoPort();
                if (terminalID.Trim().Length == 0)
                {
                    MessageBox.Show("Desktop RFID device not found.\n\n     Application will be closed!", "Info");
                    this.Close();
                }
                else
                {
                    culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                    rm = new ResourceManager("ACTARFIDLogin.Resource", typeof(ACTARFIDLogin).Assembly);
                    this.Hide();

                    desktopReaderPort = 0;
                    StartThread();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTARFIDLogin.ACTARFIDLogin_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        public ulong GetTagSerialNumber()
        {
            try
            {
                ReaderFactory.TechnologyType = "MIFARE";
                IReaderInterface ri = ReaderFactory.GetReader;
                if (desktopReaderPort == 0)
                {
                    desktopReaderPort = ri.FindDesktopReader();
                    Thread.Sleep(200);
                }
                //Thread.Sleep(200);
                ulong tagID = UInt64.Parse(ri.GetTagID(desktopReaderPort));
                return tagID;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTARFIDLogin.GetTagSerialNumber(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public string GetDesktopReaderProductInfoPort()
        {
            try
            {
                ReaderFactory.TechnologyType = "MIFARE";
                IReaderInterface ri = ReaderFactory.GetReader;
                string desktopReaderProductInfo = "";
                if (desktopReaderPort == 0)
                {
                    desktopReaderPort = ri.FindDesktopReader();
                    Thread.Sleep(200);
                }
                //Thread.Sleep(200);

                bool desktopReaderPortExists = false;
                string[] serialPorts = SerialPort.GetPortNames();
                foreach (string serialPortName in serialPorts)
                {
                    int serialPortNumber = Int32.Parse(serialPortName.Substring(3));
                    if (serialPortNumber == desktopReaderPort)
                    {
                        desktopReaderPortExists = true;
                        break;
                    }
                }
                if (!desktopReaderPortExists) return String.Empty;

                desktopReaderProductInfo = ri.GetProductInfo(desktopReaderPort);
                return desktopReaderProductInfo;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " ACTARFIDLogin.GetDesktopReaderProductInfoPort(): " + ex.Message + "\n");
                return "";
            }
        }

    }
}
