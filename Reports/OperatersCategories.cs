using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Reports
{
    public partial class OperatersCategories : Form
    {
        

        public OperatersCategories()
        {
            InitializeComponent();
          //  this.da1 = new SqlDataAdapter("select user_id, name, description, status from appl_users", cn);
           
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            
            CROperatersCategories cro = new CROperatersCategories();
            cro.SetDatabaseLogon("actamgr", "actamgr2005");
            crystalReportViewer1.ReportSource = cro;
            crystalReportViewer1.Refresh();
            
        }
    }
}
