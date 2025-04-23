using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Resources;
using System.Globalization;
using EmailNotificationManagement;
using Common;
using UI;

namespace ACTAEmailNotification
{
    public partial class NotificationForm : Form
    {
        //// Debug
        //DebugLog log;

        // Language settings
        private ResourceManager rm;
        private CultureInfo culture;
			
        public NotificationForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("UI.Resource", typeof(Employees).Assembly);
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            // Start automatic Data processing
            NotificationManager procManager = NotificationManager.GetInstance();

            if (!procManager.IsProcessing)
            {
                //if (procManager.chekPrerequests())
                //{
                    procManager.StartNotification();
                    this.lblMessage.Text = "";
                    this.btnStartStop.Text = rm.GetString("btnStop", culture);
                    //this.lblStateVal.Text = rm.GetString("activated", culture);
                //}
                //else
                //{
                //    DialogResult procreq = MessageBox.Show(rm.GetString("SettingsFailed", culture), "", MessageBoxButtons.OK);
                //    if (procreq == DialogResult.OK)
                //    {
                //        return;
                //    }
                //}

            }
            else
            {
                lblThreadState.Text = "Thread: processing is being stopped";
                if (procManager.StopNotification())
                {
                    MessageBox.Show("Processing stopped");
                    this.btnStartStop.Text = rm.GetString("btnStart", culture);
                    //this.lblStateVal.Text = rm.GetString("stopped", culture);
                    this.lblMessage.Text = "";
                }
                else
                {
                    MessageBox.Show(rm.GetString("LogProcCantStop", culture));
                }
            }
        }

       
    }
}