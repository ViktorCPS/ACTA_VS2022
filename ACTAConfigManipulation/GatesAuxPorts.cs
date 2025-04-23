using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using Util;
using Common;
using TransferObjects;

namespace ACTAConfigManipulation
{
    public partial class GatesAuxPorts : Panel
    {
        DebugLog log;

        // Controller instance
        public NotificationController Controller;

        // Observer client instance
        public NotificationObserverClient observerClient;

        private GateTO _gate = new GateTO();

        private ArrayList _readerAuxPortsList = new ArrayList();
        
        private int _posX;
        private int _posY;
        private bool _selected;

        int startPosX = 5;
        //int startPosY = 21;

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

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public ArrayList ReaderAuxPortsList
        {
            get { return _readerAuxPortsList; }
            set { _readerAuxPortsList = value; }
        }

        public GateTO Gate
        {
            get { return _gate; }
            set 
            {
                this._gate.GateID = ((GateTO) value).GateID;
                this._gate.Name = ((GateTO)value).Name;
                this._gate.Description = ((GateTO)value).Description;
                this._gate.DownloadStartTime = ((GateTO)value).DownloadStartTime;
                this._gate.DownloadInterval = ((GateTO)value).DownloadInterval;
                this._gate.DownloadEraseCounter = ((GateTO)value).DownloadEraseCounter;
            }
        }

        public GatesAuxPorts(int x, int y, GateTO gate)
        {
            try
            {
                InitializeComponent();
                IntitObserverClient();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                this.PosX = x;
                this.PosY = y;
                this.Location = new Point(x, y);
                this.BorderStyle = BorderStyle.FixedSingle;
                this.Size = new Size(Constants.gateAuxPortWidth, Constants.gateAuxPortHeight);

                this.cbGateSel.Location = new Point(0, 0);
                this.cbGateSel.Checked = false;
                this.cbGateSel.Enabled = true;
                this.Selected = false;

                this.Gate = gate;

                this.lblID.Location = new Point(30, 0);
                this.lblID.Text = this.Gate.GateID.ToString().Trim();

                this.lblName.Location = new Point(75, 0);
                this.lblName.Text = this.Gate.Name.Trim();

                this.lblDesc.Location = new Point(175, 0);
                this.lblDesc.Text = this.Gate.Description.Trim();

                this.cbAuxPort.Location = new Point(275, 0);
                this.cbAuxPort.Checked = false;
                this.cbAuxPort.Enabled = false;

                this.Controls.Clear();
                this.Controls.Add(this.cbGateSel);
                this.Controls.Add(this.lblID);
                this.Controls.Add(this.lblName);
                this.Controls.Add(this.lblDesc);
                this.Controls.Add(this.cbAuxPort);

                this.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public GatesAuxPorts(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void IntitObserverClient()
        {
            Controller = NotificationController.GetInstance();
            observerClient = new NotificationObserverClient(this.ToString());
        }

        private void cbGateSel_CheckedChanged(object sender, EventArgs e)
        {
            if (cbGateSel.Checked)
            {
                this.cbAuxPort.Checked = false;
                this.cbAuxPort.Enabled = true;
                this.Selected = true;
            }
            else
            {
                this.cbAuxPort.Checked = false;
                this.cbAuxPort.Enabled = false;
                this.Selected = false;
            }
        }

        private void cbAuxPort_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAuxPort.Checked)
            {
                List<ReaderTO> readers = new Reader().Search(this.Gate.GateID.ToString().Trim());

                int x = startPosX + Constants.gateAuxPortWidth;
                foreach (ReaderTO reader in readers)
                {
                    ReaderAuxPort readerAuxPort = new ReaderAuxPort(x, this.PosY, reader.ReaderID, reader.Description, this.Gate.GateID);
                    ReaderAuxPortsList.Add(readerAuxPort);
                    x += Constants.readerAuxPortWidth;
                }

                Controller.AuxPortChanged(ReaderAuxPortsList);
            }
            else
            {
                ReaderAuxPortsList.Clear();
                Controller.AuxPortChanged(this.Gate.GateID);
            }
        }

        private void lblName_MouseHover(object sender, EventArgs e)
        {
            try
            {
                this.toolTip.Show(this.Gate.Name, this, Constants.gateAuxPortWidth / 2, Constants.gateAuxPortHeight + 5, Constants.toolTipDuration);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lblDesc_MouseHover(object sender, EventArgs e)
        {
            try
            {
                this.toolTip.Show(this.Gate.Description, this, Constants.gateAuxPortWidth / 2 + this.lblName.Width, Constants.gateAuxPortHeight + 5, Constants.toolTipDuration);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
