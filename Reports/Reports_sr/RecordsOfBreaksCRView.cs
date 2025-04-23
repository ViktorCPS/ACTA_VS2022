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
    public partial class RecordsOfBreaksCRView : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public RecordsOfBreaksCRView(DataSet data, DateTime from, DateTime to, string selWorkingUnit,
            string selEmployee, string selGate)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeePassesCRView).Assembly);
            this.Text = rm.GetString("employeePassesReports", culture);

            Reports.Reports_sr.RecordsOfBreaksCR_sr cr = new Reports.Reports_sr.RecordsOfBreaksCR_sr();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
            cr.DataDefinition.FormulaFields["selPassType"].Text = "\"" + selGate + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }

        private void EmployeePassesCRView_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.Zoom(1);
        }
    }
}
