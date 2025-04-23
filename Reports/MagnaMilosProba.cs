using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reports.Magna
{
    public partial class MagnaMilosProba : Form
    {
        public MagnaMilosProba()
        {
            InitializeComponent();
        }

        private void MagnaMilosProba_Load(object sender, EventArgs e)
        {
            CRMagnaMilos crmm = new CRMagnaMilos();
            crmm.SetDatabaseLogon("actamgr", "actamgr2005");
            crystalReportViewer1.ReportSource = crmm;
            crystalReportViewer1.Refresh();
        }
    }
}
