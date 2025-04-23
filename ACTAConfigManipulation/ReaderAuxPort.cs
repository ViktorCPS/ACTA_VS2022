using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;

using Util;
using Common;

namespace ACTAConfigManipulation
{
    public partial class ReaderAuxPort : Panel

    {
        DebugLog log;

        // Controller instance
        public NotificationController Controller;

        // Observer client instance
        public NotificationObserverClient observerClient;

        private int _posX;
        private int _posY;
        private string _auxPort;
        private int _gateID;
        private string _readerDesc;

        public int PosX
        {
            get { return _posX; }
            set { _posX = value; }
        }

        public int PosY
        {
            get { return _posY; }
            set { _posY = value; }
        }

        public int GateID
        {
            get { return _gateID; }
            set { _gateID = value; }
        }

        public string AuxPort
        {
            get { return _auxPort; }
            set { _auxPort = value; }
        }

        public string ReaderDesc
        {
            get { return _readerDesc; }
            set { _readerDesc = value; }
        }

        public ReaderAuxPort(int x, int y, int readerID, string readerDesc, int gateID)
        {
            try
            {
                InitializeComponent();
                IntitObserverClient();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                this.PosX = x;
                this.PosY = y;
                this.GateID = gateID;
                this.Location = new Point(this.PosX, this.PosY);
                this.BorderStyle = BorderStyle.FixedSingle;
                this.Size = new Size(Constants.readerAuxPortWidth, Constants.readerAuxPortHeight);

                this.lblReader.Location = new Point(2, 1);
                this.lblReader.Text = readerDesc;
                this.lblReader.Tag = readerID;
                this.ReaderDesc = readerDesc;

                this.cbAuxPort.Location = new Point(126, 1);

                string[] serialPorts = SerialPort.GetPortNames();

                ArrayList serialPortsList = new ArrayList();

                foreach (string port in serialPorts)
                {
                    serialPortsList.Add(port);
                }

                serialPortsList.Sort();

                this.cbAuxPort.DataSource = serialPortsList;

                if (serialPorts.Length > 0)
                {
                    this.AuxPort = serialPorts[0].Trim().Substring(3);
                }
                else
                {
                    this.AuxPort = "";
                }

                this.Controls.Clear();
                this.Controls.Add(this.lblReader);
                this.Controls.Add(this.cbAuxPort);

                this.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ReaderAuxPort(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void IntitObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());
        }

        private void lblReader_MouseHover(object sender, EventArgs e)
        {
            try
            {
                this.toolTipReader.Show(this.ReaderDesc, this, Constants.readerAuxPortWidth / 2, Constants.readerAuxPortHeight + 5, Constants.toolTipDuration);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbAuxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.AuxPort = this.cbAuxPort.Text.Trim().Substring(3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
