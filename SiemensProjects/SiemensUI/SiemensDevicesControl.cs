using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Resources;
using System.Globalization;
using Common;
using UI;
using TransferObjects;
using System.Collections;

namespace SiemensUI
{
    public partial class SiemensDevicesControl : UserControl
    {
        private PointTO localPoint = new PointTO();

        public PointTO LocalPoint
        {
            get { return localPoint; }
            set { localPoint = value; }
        }

        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;

        public SiemensDevicesControl()
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            setLanguage();
        }

        public SiemensDevicesControl(PointTO point)
        {
            InitializeComponent();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Passes).Assembly);
            setLanguage();

            this.localPoint = point;
        }


        private void setLanguage()
        {
            try
            {
                ////set gruop box's text
                //gbDevice.Text = rm.GetString("gbDevice", culture);
                //gbDirection.Text = rm.GetString("gbDirection", culture);
                //gbPassType.Text = rm.GetString("gbPassType", culture);
                //gbPointName.Text = rm.GetString("gbPointName", culture);

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensDevicesControl.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void SiemensDevicesControl_Load(object sender, EventArgs e)
        {
            try
            {
              
                if (localPoint.Gate != -1)
                {
                    chbReadPasses.Checked = (localPoint.ReadPasses == Constants.SiemensPointPassReading);
                    chTimeAttCounter.Checked = (localPoint.TimeAttCounter == Constants.SiemensPointCounter);
                    tbName.Text = localPoint.PointName;
                    rbIn.Checked = (localPoint.Direction==Constants.SiemensDirectionIn);
                    rbOut.Checked = (localPoint.Direction==Constants.SiemensDirectionOut);
                    nudBrezaGate.Value = localPoint.Gate;
                }
                else
                {
                    rbIn.Checked = true;         
                    tbName.Text = localPoint.PointName;
                    localPoint.Gate = 0;
                    localPoint.Direction = Constants.SiemensDirectionIn;
                    localPoint.TimeAttCounter = Constants.SiemensPointNOTCounter;
                    nudBrezaGate.Value = 0;

                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensDevicesControl.SiemensDevicesControl_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

      

        private void chTimeAttCounter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chTimeAttCounter.Checked)
                {
                    localPoint.TimeAttCounter = Constants.SiemensPointCounter;
                    chbReadPasses.Checked = true;
                }
                else
                {
                    localPoint.TimeAttCounter = Constants.SiemensPointNOTCounter;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensDevicesControl.chTimeAttCounter_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void rbIn_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbIn.Checked)
                {
                    localPoint.Direction = Constants.SiemensDirectionIn;
                }
                else
                {
                    localPoint.Direction = Constants.SiemensDirectionOut;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensDevicesControl. rbIn_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

      
        private void chbReadPasses_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbReadPasses.Checked)
                {
                    localPoint.ReadPasses = Constants.SiemensPointPassReading;
                }
                else
                {
                    localPoint.ReadPasses = Constants.SiemensPointNOTPassReading;
                    chTimeAttCounter.Checked = false;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " SiemensDevicesControl. chbReadPasses_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void tbBrezaGate_TextChanged(object sender, EventArgs e)
        {

        }

        private void nudBrezaGate_ValueChanged(object sender, EventArgs e)
        {
            localPoint.Gate = (int)nudBrezaGate.Value;
        }
    }
}
