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

namespace Reports.Reports_fi
{
    public partial class EmployeeLocationCRView_fi : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        public EmployeeLocationCRView_fi(DataSet data, string selWorkingUnit, string selEmployee, string selPassType, string selLocation, string selUser)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeeLocationCRView_fi).Assembly);
            this.Text = rm.GetString("EmployeeLocationReport", culture);

            Reports.Reports_fi.EmployeeLocationCR_fi cr = new Reports.Reports_fi.EmployeeLocationCR_fi();
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