using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reports
{
    public partial class StartWorkingDateReport : Form
    {

      
        public StartWorkingDateReport()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            StartWorkingDateReports swd = new StartWorkingDateReports();
            swd.SetDatabaseLogon("actamgr", "actamgr2005");
            crystalReportViewer1.ReportSource = swd;
            crystalReportViewer1.Refresh();
        }

        private void StartWorkingDateReport_Load(object sender, EventArgs e)
        {

        }
       
    }
}
