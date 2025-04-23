using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACTASyncManagement;
using ACTAWorkAnalysisReports;

namespace ACTASynchronization
{
    public partial class ACTASynchronization : Form
    {
        SyncManager manager;
        WorkAnalysis managerInstance;

        public ACTASynchronization()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            manager = new ACTASyncManagement.SyncManager();
          

            manager.Start();
            btnStop.Visible = true;
            timer1.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            manager.Stop();
            timer1.Enabled = false;
            btnStop.Visible = false;
            this.Cursor = Cursors.Arrow;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = manager.managerThreadStatus;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            managerInstance = WorkAnalysis.GetInstance();
            if (!managerInstance.IsProcessing)
            {
                managerInstance.StartReporting();
                managerInstance.IsProcessing = true;
            }
            else
            {
                managerInstance.StopReporting();
                managerInstance.IsProcessing = false;
            }
        }
    }
}
