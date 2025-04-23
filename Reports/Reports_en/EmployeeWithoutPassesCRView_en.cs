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
    public partial class EmployeeWithoutPassesCRView_en : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public EmployeeWithoutPassesCRView_en(DataSet data, DateTime from, DateTime to, string selWorkingUnit,
            string selEmployee, string selPassType, string selDirection, string selLocation,
            string fromTime, string toTime)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeeWithoutPassesCRView_en).Assembly);
            this.Text = rm.GetString("employeeWithoutPassesReports", culture);

            Reports.Reports_en.EmployeeWithoutPassesCR_en cr = new Reports.Reports_en.EmployeeWithoutPassesCR_en();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
            cr.DataDefinition.FormulaFields["selPassType"].Text = "\"" + selPassType + "\"";
            cr.DataDefinition.FormulaFields["selDirection"].Text = "\"" + selDirection + "\"";
            cr.DataDefinition.FormulaFields["selLocation"].Text = "\"" + selLocation + "\"";
            cr.DataDefinition.FormulaFields["fromTime"].Text = "\"" + fromTime + "\"";
            cr.DataDefinition.FormulaFields["toTime"].Text = "\"" + toTime + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}