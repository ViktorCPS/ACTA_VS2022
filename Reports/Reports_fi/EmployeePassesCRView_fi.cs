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
    public partial class EmployeePassesCRView_fi : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public EmployeePassesCRView_fi(DataSet data, DateTime from, DateTime to, string selWorkingUnit,
            string selEmployee, string selPassType, string selDirection, string selLocation,
            string fromTime, string toTime, string firstIn, string lastOut, string advanced)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmployeePassesCRView_fi).Assembly);
            this.Text = rm.GetString("employeePassesReports", culture);

            Reports.Reports_fi.EmployeePassesCR_fi cr = new Reports.Reports_fi.EmployeePassesCR_fi();
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
    }
}