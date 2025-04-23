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
    public partial class SummaryEmployeeLocationCRView_en : Form
    {
        CultureInfo culture;
        ResourceManager rm;
        public SummaryEmployeeLocationCRView_en(DataSet data, string selUser,int report)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(SummaryEmployeeLocationCRView_en).Assembly);
            this.Text = rm.GetString("EmployeeLocationReport", culture);

            if (report == 1)
            {
                Reports.Reports_en.SummaryEmployeeWULocCR_en cr = new Reports.Reports_en.SummaryEmployeeWULocCR_en();
                cr.DataDefinition.FormulaFields["selUser"].Text = "\"" + selUser + "\"";

                cr.SetDataSource(data);
                this.crystalReportViewer1.ReportSource = cr;
            }
            else
            {
                Reports.Reports_en.SummaryEmployeeLocWUCR_en cr = new Reports.Reports_en.SummaryEmployeeLocWUCR_en();
                cr.DataDefinition.FormulaFields["selUser"].Text = "\"" + selUser + "\"";

                cr.SetDataSource(data);
                this.crystalReportViewer1.ReportSource = cr;
            }

        }
    }
}