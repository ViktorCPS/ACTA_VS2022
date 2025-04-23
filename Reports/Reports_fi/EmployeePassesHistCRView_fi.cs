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
     
    public partial class EmployeePassesHistCRView_fi : Form
    {

        CultureInfo culture;
        ResourceManager rm;
        public EmployeePassesHistCRView_fi(DataSet data, string from, string to, string selWorkingUnit,
            string selEmployee, string selPassType, string selDirection, string selLocation, string selOperator, string fromModified, string toModified)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeePassesHistCRView_fi).Assembly);
            this.Text = rm.GetString("employeePassesHistoryReports", culture);

            Reports.Reports_fi.EmployeePassesHistCR_fi cr = new Reports.Reports_fi.EmployeePassesHistCR_fi();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
            cr.DataDefinition.FormulaFields["selPassType"].Text = "\"" + selPassType + "\"";
            cr.DataDefinition.FormulaFields["selDirection"].Text = "\"" + selDirection + "\"";
            cr.DataDefinition.FormulaFields["selLocation"].Text = "\"" + selLocation + "\"";
            cr.DataDefinition.FormulaFields["selOperator"].Text = "\"" + selOperator + "\"";
            cr.DataDefinition.FormulaFields["fromModified"].Text = "\"" + fromModified + "\"";
            cr.DataDefinition.FormulaFields["toModified"].Text = "\"" + toModified + "\"";


            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}