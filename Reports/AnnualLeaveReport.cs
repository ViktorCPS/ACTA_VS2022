using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reports
{
    public partial class AnnualLeaveReport : Form
    {

      
        public AnnualLeaveReport()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            AnnualLeaveReports anu = new AnnualLeaveReports();
            anu.SetDatabaseLogon("actamgr", "actamgr2005");
            crystalReportViewer1.ReportSource = anu;
            crystalReportViewer1.Refresh();
        }

        private void AnualLeave1_InitReport(object sender, EventArgs e)
        {

        }

       
    }
}
