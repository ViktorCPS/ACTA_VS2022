using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reports
{
    public partial class RolesPrivilegesReportCANTEEN : Form
    {
        public RolesPrivilegesReportCANTEEN()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            CRCANTEENRolesPrivileges cro = new CRCANTEENRolesPrivileges();
            cro.SetDatabaseLogon("actamgr", "actamgr2005");
            crystalReportViewer1.ReportSource = cro;
            crystalReportViewer1.Refresh();
        }
    }
}
