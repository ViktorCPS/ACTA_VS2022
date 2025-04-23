using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACTASurveillanceManagement;
using Common;

namespace ACTASurveillance
{
    public partial class Form1 : Form
    {
        SurveillanceManager manager;
        bool started = false;
        public Form1()
        {
            InitializeComponent();
            NotificationController.SetApplicationName("ACTASurveillance");
            manager = new SurveillanceManager();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            if (started)
            {
                manager.Stop();
                Stop.Text = "Start";
                started = false;
            }
            else
            {
                manager.Start();
                Stop.Text = "Stop";
                started = true;
            }
        }
    }
}
