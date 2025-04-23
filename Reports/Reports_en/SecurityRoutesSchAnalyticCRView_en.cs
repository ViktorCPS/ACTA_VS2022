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
    public partial class SecurityRoutesSchAnalyticCRView_en : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public SecurityRoutesSchAnalyticCRView_en(DataSet data, DateTime from, DateTime to, string selWorkingUnit,
            string selEmployee, string selRoute)
        {
            try
            {
                InitializeComponent();

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(SecurityRoutesSchAnalyticCRView_en).Assembly);
                this.Text = rm.GetString("routesSchReports", culture);

                Reports.Reports_en.SecurityRoutesSchAnalyticCR_en cr = new Reports.Reports_en.SecurityRoutesSchAnalyticCR_en();
                cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
                cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
                cr.DataDefinition.FormulaFields["selRoute"].Text = "\"" + selRoute + "\"";

                cr.SetDataSource(data);
                this.crystalReportViewer1.ReportSource = cr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SecurityRoutesSchAnalyticCRView_en_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.Zoom(1);
        }
    }
}