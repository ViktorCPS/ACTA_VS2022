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

namespace Reports.Reports_sr
{
    public partial class SecurityRoutesSchSynteticCRView_sr : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public SecurityRoutesSchSynteticCRView_sr(DataSet data, DateTime from, DateTime to, string selWorkingUnit,
            string selEmployee, string selRoute)
        {
            try
            {
                InitializeComponent();

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(SecurityRoutesSchSynteticCRView_sr).Assembly);
                this.Text = rm.GetString("routesSchReports", culture);

                Reports.Reports_sr.SecurityRoutesSchSynteticCR_sr cr = new Reports.Reports_sr.SecurityRoutesSchSynteticCR_sr();
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

        private void SecurityRoutesSchSynteticCRView_sr_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.Zoom(1);
        }
    }
}