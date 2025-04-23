using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Resources;
using System.Globalization;
using Common;

namespace Reports.Reports_en
{
    public partial class EmployeePassesCRView_en : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public EmployeePassesCRView_en(DataSet data, DateTime from, DateTime to, string selWorkingUnit,
            string selEmployee, string selPassType, string selDirection, string selLocation,
            string fromTime, string toTime, string firstIn, string lastOut, string advanced)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeePassesCRView_en).Assembly);
            this.Text = rm.GetString("employeePassesReports", culture);

            Reports.Reports_en.EmployeePassesCR_en cr = new Reports.Reports_en.EmployeePassesCR_en();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
            cr.DataDefinition.FormulaFields["selPassType"].Text = "\"" + selPassType + "\"";
            cr.DataDefinition.FormulaFields["selDirection"].Text = "\"" + selDirection + "\"";
            cr.DataDefinition.FormulaFields["selLocation"].Text = "\"" + selLocation + "\"";
            cr.DataDefinition.FormulaFields["fromTime"].Text = "\"" + fromTime + "\"";
            cr.DataDefinition.FormulaFields["toTime"].Text = "\"" + toTime + "\"";
            cr.DataDefinition.FormulaFields["firstIn"].Text = "\"" + firstIn + "\"";
            cr.DataDefinition.FormulaFields["lastOut"].Text = "\"" + lastOut + "\"";
            cr.DataDefinition.FormulaFields["advanced"].Text = "\"" + advanced + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }

        private void EmployeePassesCRView_en_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.Zoom(1);
        }
    }
}