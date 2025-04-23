using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Reports
{
    public partial class OperatersRoles : Form
    {
       
        public SqlDataReader dr;
        public String s;
        public SqlDataAdapter da1;
        public SqlDataAdapter da2;
        public DataSet ds = new DataSet();


        public OperatersRoles()
        {
            InitializeComponent();
        }

        private void OperatersRoles_Load(object sender, EventArgs e)
        {
            

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
           
            CROperatersRoles cro = new CROperatersRoles();
            cro.SetDatabaseLogon("actamgr", "actamgr2005");
            crystalReportViewer1.ReportSource = cro;
            crystalReportViewer1.Refresh();
        }
    }
}
