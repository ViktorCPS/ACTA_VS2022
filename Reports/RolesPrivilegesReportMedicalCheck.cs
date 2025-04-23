using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reports
{
    public partial class RolesPrivilegesReportMedicalCheck : Form
    {
        public RolesPrivilegesReportMedicalCheck()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            CRMedicalCheckCRolesPrivileges cro = new CRMedicalCheckCRolesPrivileges();
            cro.SetDatabaseLogon("actamgr", "actamgr2005");
            crystalReportViewer1.ReportSource = cro;
            crystalReportViewer1.Refresh();
        }

        private void CRMedicalCheckCRolesPrivileges1_InitReport(object sender, EventArgs e)
        {

        }
    }
}
