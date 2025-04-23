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
    public partial class EmployeeLocationCRView_en : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        public EmployeeLocationCRView_en(DataSet data,string selWorkingUnit, string selEmployee, string selPassType, string selLocation, string selUser)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeeLocationCRView_en).Assembly);
            this.Text = rm.GetString("EmployeeLocationReport", culture);

            Reports.Reports_en.EmployeeLocationCR_en cr = new Reports.Reports_en.EmployeeLocationCR_en();
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
            cr.DataDefinition.FormulaFields["selPassType"].Text = "\"" + selPassType + "\"";
            cr.DataDefinition.FormulaFields["selLocation"].Text = "\"" + selLocation + "\"";
            cr.DataDefinition.FormulaFields["selUser"].Text = "\"" + selUser + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}