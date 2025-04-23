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

namespace Reports.PIO.PIO_sr
{
    public partial class PIOWorkListsAnalyticCRView_sr : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public PIOWorkListsAnalyticCRView_sr(DataSet data, DateTime from, DateTime to,
            string analyticRad, string analyticTotalPla, string analyticTotalNepla)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(PIOWorkListsCRView_sr).Assembly);
            this.Text = rm.GetString("pioAnalyticWorkListsReports", culture);

            Reports.PIO.PIO_sr.PIOWorkListsAnalyticCR_sr cr = new Reports.PIO.PIO_sr.PIOWorkListsAnalyticCR_sr();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";

            cr.DataDefinition.FormulaFields["analyticRad"].Text = "\"" + analyticRad + "\"";
            cr.DataDefinition.FormulaFields["analyticTotalPla"].Text = "\"" + analyticTotalPla + "\"";
            cr.DataDefinition.FormulaFields["analyticTotalNepla"].Text = "\"" + analyticTotalNepla + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}