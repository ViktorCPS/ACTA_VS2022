using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TransferObjects;
using ReaderRemoteManagement;
using Common;
using System.Configuration;

namespace ReaderRemoteControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<VTControl> ctrlsList = new List<VTControl>();
        List<ReaderRemoteControlManager> controlList = new List<ReaderRemoteControlManager>();
        private void Start_Click(object sender, EventArgs e)
        {

            //RFID_comm.RFID_communication rfidComm = new RFID_comm.RFID_communication();
            string str = "";
            // ACTAReaderRemoteControlProcessing.ReaderRemoteControlProcessor procc = new ACTAReaderRemoteControlProcessing.ReaderRemoteControlProcessor();
            //string terminals = rfidComm.GetTerminals(ref str);
            //List<ValidationTerminalTO> vtList = getTerminalsFromString(terminals);

            //List<ValidationTerminalTO> vtList = new ValidationTerminal().Search();
            // List<ValidationTerminalTO> vtList = getTerminalsFromString(terminals);
            string readerIds = "";
            string pointsIDs = "";
            if (ConfigurationManager.AppSettings["Readers"] != null)
            {
                try
                {
                    readerIds = (string)ConfigurationManager.AppSettings["Readers"];
                }
                catch { }
            }

            if (readerIds.Length == 0)
            {
                MessageBox.Show("No readers found in config file!");
                return;
            }

            if (ConfigurationManager.AppSettings["Points"] != null)
            {
                try
                {
                    pointsIDs = (string)ConfigurationManager.AppSettings["Points"];
                }
                catch { }
            }

            if (pointsIDs.Length == 0)
            {
                MessageBox.Show("No Points found in config file!");
                return;
            }
            string[] pointID = pointsIDs.Split(';');
            string p = "";
            Dictionary<string, string> dictionaryPoints = new Dictionary<string, string>();

            foreach (string po in pointID)
            {
                p += po.Remove(po.IndexOf(':')) + ",";

                dictionaryPoints.Add(po.Remove(po.IndexOf(':')), po.Substring(po.IndexOf(':') + 1));

            }
            if (p.Length > 0)
                p = p.Remove(p.LastIndexOf(','));

            List<OnlineMealsPointsTO> points = new OnlineMealsPoints().searchForIDs(p);

            List<ValidationTerminalTO> vtList = new List<ValidationTerminalTO>();

            Dictionary<string, ValidationTerminalTO> vtDict = new Dictionary<string, ValidationTerminalTO>();


            foreach (OnlineMealsPointsTO point in points)
            {
                if (point.PointID == 1 || point.PointID == 2)
                {
                    if (point.ReaderIPAddress.Equals("10.20.8.200"))
                    {
                        if (!vtDict.ContainsKey(point.ReaderIPAddress.Trim()))
                        {
                            ValidationTerminalTO terminal = new ValidationTerminalTO();
                            terminal.IpAddress = point.ReaderIPAddress;
                            terminal.Name = point.RestaurantName;
                            terminal.Status = "ENABLED";
                            terminal.Description = point.RestaurantName;
                            terminal.ValidationTerminalID = point.RestaurantID;
                            if (!terminal.Locations.ContainsKey(point.Reader_ant))
                                terminal.Locations.Add(point.Reader_ant, dictionaryPoints[point.PointID.ToString()]);

                            vtDict.Add(terminal.IpAddress, terminal);

                        }
                        else
                        {
                            vtDict[point.ReaderIPAddress].Locations.Add(point.Reader_ant, dictionaryPoints[point.PointID.ToString()]);
                        }
                    }

                }
            }

            //foreach (ReaderTO reader in readers)
            //{
            //    ValidationTerminalTO terminal = new ValidationTerminalTO();
            //    terminal.IpAddress = reader.ConnectionAddress;
            //    terminal.Name = reader.ConnectionAddress;
            //    terminal.Status = reader.Status;
            //    terminal.Description = reader.Description;
            //    terminal.ValidationTerminalID = reader.ReaderID;
            //    vtList.Add(terminal);
            //}

            int ticketTransactionTimeOut = 10000;
            int PassTimeOut = 2000;
            int blockPassInterval = 500;
            int pingReaderTime = 600000;
            int logDebugMessages = 1;
            int ScreenTimeOut = 0;
            int downloadInterval = 0;
            string downloadStartTime = "00:00";
            string numberOfMealsTime = "00:00";
            if (ConfigurationManager.AppSettings["TicketTransactionTimeOut"] != null)
            {
                try
                {
                    ticketTransactionTimeOut = int.Parse((string)ConfigurationManager.AppSettings["TicketTransactionTimeOut"]);
                }
                catch { }
            }

            if (ConfigurationManager.AppSettings["PassTimeOut"] != null)
            {
                try
                {
                    PassTimeOut = int.Parse((string)ConfigurationManager.AppSettings["PassTimeOut"]);
                }
                catch { }
            }
            if (ConfigurationManager.AppSettings["ScreenTimeOut"] != null)
            {
                try
                {
                    ScreenTimeOut = int.Parse((string)ConfigurationManager.AppSettings["ScreenTimeOut"]);
                }
                catch { }
            }
            if (ConfigurationManager.AppSettings["BlockPassInterval"] != null)
            {
                try
                {
                    blockPassInterval = int.Parse((string)ConfigurationManager.AppSettings["BlockPassInterval"]);
                }
                catch { }
            }
            if (ConfigurationManager.AppSettings["readerPingPeriod"] != null)
            {
                try
                {
                    pingReaderTime = int.Parse((string)ConfigurationManager.AppSettings["readerPingPeriod"]) * 60 * 1000;
                }
                catch { }
            }
            if (ConfigurationManager.AppSettings["DebugLevel"] != null)
            {
                try
                {
                    logDebugMessages = int.Parse((string)ConfigurationManager.AppSettings["DebugLevel"]);
                }
                catch { }
            }
            if (ConfigurationManager.AppSettings["DownloadInterval"] != null)
            {
                try
                {
                    downloadInterval = int.Parse((string)ConfigurationManager.AppSettings["DownloadInterval"]);
                }
                catch { }
            }
            if (ConfigurationManager.AppSettings["DownloadStartTime"] != null)
            {
                try
                {
                    downloadStartTime = (string)ConfigurationManager.AppSettings["DownloadStartTime"];
                }
                catch { }
            }
            if (ConfigurationManager.AppSettings["NumberOfMealsTime"] != null)
            {
                try
                {
                    numberOfMealsTime = (string)ConfigurationManager.AppSettings["NumberOfMealsTime"];
                }
                catch { }
            }
            int x = 5;
            int y = 5;
            foreach (KeyValuePair<string, ValidationTerminalTO> pair in vtDict)
            {
                //if (validationTerminal.Status == "ENABLED")
                //{
                ValidationTerminalTO validationTerminal = pair.Value;
                VTControl ctrl = new VTControl();
                ctrl.ValidationTerminal = validationTerminal;
                ctrl.Location = new Point(x, y);
                panelControls.Controls.Add(ctrl);
                ctrlsList.Add(ctrl);
                if ((x + ctrl.Width * 2 + 5) > panelControls.Width)
                {
                    y += ctrl.Height + 5;
                    x = 0;
                }
                else
                {
                    x += ctrl.Width + 5;
                }

                ReaderRemoteControlManager control;
                validationTerminal.BlockPassInterval = blockPassInterval;
                validationTerminal.LogDebugMessages = logDebugMessages;
                validationTerminal.Name = validationTerminal.IpAddress;
                validationTerminal.PassTimeOut = PassTimeOut;
                validationTerminal.PingPeriod = pingReaderTime;
                validationTerminal.TicketTransactionTimeOut = ticketTransactionTimeOut;
                validationTerminal.ScreenTimeOut = ScreenTimeOut;
                validationTerminal.DownloadInterval = downloadInterval;
                validationTerminal.DownloadStartTime = downloadStartTime;
                validationTerminal.NumberOfMealsTime = numberOfMealsTime;
                //validationTerminal.ValidationTerminalID = 2;
                //validationTerminal.CameraIP1 = "10.20.8.7";


                control = new ReaderRemoteControlManager(validationTerminal);
                control.Start();
                controlList.Add(control);
                //}
            }
            Stop.Visible = true;
            timer1.Enabled = true;
        }
        private List<ValidationTerminalTO> getTerminalsFromString(string terminals)
        {
            List<ValidationTerminalTO> vTerminals = new List<ValidationTerminalTO>();
            try
            {
                string[] splitTerminals = terminals.Split('|');
                foreach (string str in splitTerminals)
                {
                    string[] vtTOStrings = str.Split(',');
                    if (vtTOStrings.Length == 2)
                    {
                        ValidationTerminalTO vtTO = new ValidationTerminalTO();
                        int vtID = -1;
                        bool succ = int.TryParse(vtTOStrings[0], out vtID);
                        if (succ)
                        {
                            vtTO.IpAddress = vtTOStrings[1];
                            vtTO.ValidationTerminalID = vtID;
                            vTerminals.Add(vtTO);
                        }
                    }

                }
            }
            catch //(Exception ex)
            {
                // log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "ReaderRemoteControlService.getTerminalsFromString(), Exception: " + ex.Message);
            }
            return vTerminals;
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            foreach (ReaderRemoteControlManager control in controlList)
            {
                control.Stop();
            }
            foreach (VTControl control in ctrlsList)
            {
                control.Dispose();
            }
            timer1.Enabled = false;
            panelControls.Controls.Clear();
            Stop.Visible = false;
            this.Cursor = Cursors.Arrow;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < controlList.Count; i++)
            {
                ctrlsList[i].setStatus0(controlList[i].workStatus0);
                ctrlsList[i].setStatus1(controlList[i].workStatus1);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnUpdateLog_Click(object sender, EventArgs e)
        {
            //string readerIds = "";
            //string pointsIDs = "";
            //if (ConfigurationManager.AppSettings["Readers"] != null)
            //{
            //    try
            //    {
            //        readerIds = (string)ConfigurationManager.AppSettings["Readers"];
            //    }
            //    catch { }
            //}

            //if (readerIds.Length == 0)
            //{
            //    MessageBox.Show("No readers found in config file!");
            //    return;
            //}

            //if (ConfigurationManager.AppSettings["Points"] != null)
            //{
            //    try
            //    {
            //        pointsIDs = (string)ConfigurationManager.AppSettings["Points"];
            //    }
            //    catch { }
            //}

            //if (pointsIDs.Length == 0)
            //{
            //    MessageBox.Show("No Points found in config file!");
            //    return;
            //}
            //string[] pointID = pointsIDs.Split(';');
            //string p = "";
            //Dictionary<string, string> dictionaryPoints = new Dictionary<string, string>();

            //foreach (string po in pointID)
            //{
            //    p += po.Remove(po.IndexOf(':')) + ",";

            //    dictionaryPoints.Add(po.Remove(po.IndexOf(':')), po.Substring(po.IndexOf(':') + 1));

            //}
            //if (p.Length > 0)
            //    p = p.Remove(p.LastIndexOf(','));

            //List<OnlineMealsPointsTO> points = new OnlineMealsPoints().searchForIDs(p);

            //List<ValidationTerminalTO> vtList = new List<ValidationTerminalTO>();

            //Dictionary<string, ValidationTerminalTO> vtDict = new Dictionary<string, ValidationTerminalTO>();


            //foreach (OnlineMealsPointsTO point in points)
            //{
            //    //if (point.PointID == 1 || point.PointID == 2)
            //    //{
            //        if (!vtDict.ContainsKey(point.ReaderIPAddress.Trim()))
            //        {
            //            //if (point.ReaderIPAddress.Equals("10.20.8.200"))
            //            //{
            //                ValidationTerminalTO terminal = new ValidationTerminalTO();
            //                terminal.IpAddress = point.ReaderIPAddress;
            //                terminal.Name = point.RestaurantName;
            //                terminal.Status = "ENABLED";
            //                terminal.Description = point.RestaurantName;
            //                terminal.ValidationTerminalID = point.RestaurantID;
            //                if (!terminal.Locations.ContainsKey(point.Reader_ant))
            //                    terminal.Locations.Add(point.Reader_ant, dictionaryPoints[point.PointID.ToString()]);

            //                vtDict.Add(terminal.IpAddress, terminal);
            //            //}
            //        }
            //        else
            //        {
            //            vtDict[point.ReaderIPAddress].Locations.Add(point.Reader_ant, dictionaryPoints[point.PointID.ToString()]);
            //        }

            //    //}
            //}
           
            //foreach (KeyValuePair<string, ValidationTerminalTO> pair in vtDict)
            //{
                
            //        DownloadLog log = new DownloadLog(pair.Value, pair.Value.Locations);
            //        log.GetLog(true);
                  
            //}
            Form2 frm = new Form2();
            frm.ShowDialog();
        }

      
    }
}
