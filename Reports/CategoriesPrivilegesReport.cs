using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.Shared;

namespace Reports
{
    public partial class CategoriesPrivilegesReport : Form
    {

       
        public SqlDataReader dr;
        public String s;
        public SqlDataAdapter da1;
        public SqlDataAdapter da2;
        public DataSet ds = new DataSet();

        public CategoriesPrivilegesReport()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            
           
            CRCategoriesPrivilegesReport cro = new CRCategoriesPrivilegesReport();
            cro.SetDatabaseLogon("actamgr", "actamgr2005");
            crystalReportViewer1.ReportSource = cro;
            crystalReportViewer1.Refresh();
            
            
        }
    }
}
