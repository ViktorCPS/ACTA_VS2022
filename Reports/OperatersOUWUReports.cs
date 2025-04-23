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
    public partial class OperatersOUWUReports : Form
    {
      
        public SqlDataReader dr;
        public String s;
        public SqlDataAdapter da1;
        public SqlDataAdapter da2;
        public DataSet ds = new DataSet();


        public OperatersOUWUReports()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load_1(object sender, EventArgs e)
        {
            
            CROperatersOUWU cro = new CROperatersOUWU();
            cro.SetDatabaseLogon("actamgr", "actamgr2005");
            crystalReportViewer1.ReportSource = cro;
            crystalReportViewer1.Refresh();
        }
    }
}
