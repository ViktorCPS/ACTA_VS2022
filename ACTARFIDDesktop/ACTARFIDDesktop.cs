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

namespace ACTARFIDDesktop
{
    public partial class ACTARFIDDesktop : Form
    {
        private Thread reader;
        bool stopReading;
        DebugLog log;
        CultureInfo culture;
        ResourceManager rm;

        int desktopReaderPort;
        private ulong oldTagID = 0;

        private bool closeApplicationFlag = false;

        public ACTARFIDDesktop()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // If there aren't any Description attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Description attribute, return its value
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // If there aren't any Product attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Product attribute, return its value
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // If there aren't any Company attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Company attribute, return its value
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

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
                reader = new Thread(new ThreadStart(ReadTagWriteText));
                reader.Name = "ReadTagWriteText";
                reader.Start();
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ": Thread Started \n");
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.StartThread(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.StopThread(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void ReadTagWriteText()
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
                                if (tagID != oldTagID)
                                {
                                    System.Media.SystemSounds.Beep.Play();
                                    oldTagID = tagID;
                                    SendKeys.SendWait(tagID.ToString().Trim() + "\r");
                                }
                            }
                            else
                            {
                                oldTagID = tagID;
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.GetDesktopReaderProductInfoPort(): " + ex.Message + "\n");
                return "";
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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.GetTagSerialNumber(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void TagReader_Load(object sender, EventArgs e)
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
                    rm = new ResourceManager("ACTARFIDDesktop.Resource", typeof(ACTARFIDDesktop).Assembly);
                    this.Hide();

                    desktopReaderPort = 0;
                    StartThread();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.TagReader_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //DialogResult result = MessageBox.Show(rm.GetString("ACTARFIDDesktopClosed", culture), "", MessageBoxButtons.YesNo);
                DialogResult result = MessageBox.Show("Do you want to close the application?","Info", MessageBoxButtons.YesNo);

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
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.notifyIcon_MouseDoubleClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
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

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.okButton_Click(): " + ex.Message + "\n");
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

        private void ACTARFIDDesktop_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StopThread();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " TagReader.TagReader_FormClosed(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }
    }
}