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
    public partial class SummaryEmployeeLocationCRView_sr : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        public SummaryEmployeeLocationCRView_sr(DataSet data, string selUser, int report)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(SummaryEmployeeLocationCRView_sr).Assembly);
            this.Text = rm.GetString("EmployeeLocationReport", culture);
            if (report == 1)
            {
                Reports.Reports_sr.SummaryEmployeeWULocCR_sr cr = new Reports.Reports_sr.SummaryEmployeeWULocCR_sr();
                cr.DataDefinition.FormulaFields["selUser"].Text = "\"" + selUser + "\"";

                cr.SetDataSource(data);
                this.crystalReportViewer1.ReportSource = cr;
            }
            else
            {
                Reports.Reports_sr.SummaryEmployeeLocWUCR_sr cr = new Reports.Reports_sr.SummaryEmployeeLocWUCR_sr();
                cr.DataDefinition.FormulaFields["selUser"].Text = "\"" + selUser + "\"";

                cr.SetDataSource(data);
                this.crystalReportViewer1.ReportSource = cr;
            }

            //cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            //cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
            //cr.DataDefinition.FormulaFields["selPassType"].Text = "\"" + selPassType + "\"";
            //cr.DataDefinition.FormulaFields["selLocation"].Text = "\"" + selLocation + "\"";

        }
    }
}