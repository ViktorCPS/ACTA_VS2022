using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using Common;

namespace Reports.Reports_sr
{
    public partial class MachinesXEmployeesCRView : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public MachinesXEmployeesCRView(DataSet data, string selMachine)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeePassesCRView).Assembly);
            this.Text = rm.GetString("employeePassesReports", culture);

            Reports.Reports_sr.MachinesXEmployeesCR_sr cr = new Reports.Reports_sr.MachinesXEmployeesCR_sr();
            cr.DataDefinition.FormulaFields["selMachine"].Text = "\"" + selMachine + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }

        private void EmployeePassesCRView_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.Zoom(1);
        }
    }
}
