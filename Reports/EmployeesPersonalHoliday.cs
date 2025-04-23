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
    public partial class EmployeesPersonalHoliday : Form
    {

        public SqlConnection cn = new SqlConnection("Server=assurance;Database=ACTA_hyatt;User Id=actamgr;Password=actamgr2005;");
        public SqlDataReader dr;
        public String s;
        public SqlDataAdapter da1;
        public SqlDataAdapter da2;
        public DataSet ds = new DataSet();

        public EmployeesPersonalHoliday()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            CREmployeesPersonalHoliday cro = new CREmployeesPersonalHoliday();
            cro.SetDatabaseLogon("actamgr", "actamgr2005");
            crystalReportViewer1.ReportSource = cro;
            crystalReportViewer1.Refresh();
        }
    }
}
