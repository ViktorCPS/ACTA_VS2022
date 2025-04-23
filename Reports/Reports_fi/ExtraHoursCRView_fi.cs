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

namespace Reports.Reports_fi
{
    public partial class ExtraHoursCRView_fi : Form
    {
         CultureInfo culture;
        ResourceManager rm;

        public ExtraHoursCRView_fi(DataSet data, string selWorkingUnit,
            string selEmplName, string timeName, string reportName,DateTime from, DateTime to)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ExtraHoursCRView_fi).Assembly);
            this.Text = reportName;

            Reports.Reports_fi.ExtraHoursCR_fi cr = new Reports.Reports_fi.ExtraHoursCR_fi();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmplName + "\"";
            cr.DataDefinition.FormulaFields["timeName"].Text = "\"" + timeName + "\"";
            cr.DataDefinition.FormulaFields["reportName"].Text = "\"" + reportName + "\"";
                       
            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}