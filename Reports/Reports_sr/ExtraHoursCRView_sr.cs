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
    public partial class ExtraHoursCRView_sr : Form
    {
         CultureInfo culture;
        ResourceManager rm;

        public ExtraHoursCRView_sr(DataSet data, string selWorkingUnit,
            string selEmplName, string timeName, string reportName,DateTime from, DateTime to)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(IOPairsCRView).Assembly);
            this.Text = reportName;

            Reports.Reports_sr.ExtraHoursCR_sr cr = new Reports.Reports_sr.ExtraHoursCR_sr();
            if (!from.Equals(new DateTime()) && !to.Equals(new DateTime()))
            {
                cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            }
            else
            {
                cr.DataDefinition.FormulaFields["from"].Text = "";
                cr.DataDefinition.FormulaFields["to"].Text = "";
            }
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmplName + "\"";
            cr.DataDefinition.FormulaFields["timeName"].Text = "\"" + timeName + "\"";
            cr.DataDefinition.FormulaFields["reportName"].Text = "\"" + reportName + "\"";
                       
            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}