using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common;
using System.Globalization;
using System.Resources;

namespace Reports.ZIN.ZIN_sr
{
    public partial class VisitsVDCRView : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public VisitsVDCRView(DataSet data, string VisitorName, string DocNum, string JMBG, string State, string Company, string Ban)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ZINEmployeePassesCRView).Assembly);
            this.Text = rm.GetString("ZINVisits", culture);

            Reports.ZIN.ZIN_sr.VisitorsVDCR_sr cr = new Reports.ZIN.ZIN_sr.VisitorsVDCR_sr();
            cr.DataDefinition.FormulaFields["selVisitorName"].Text = "\"" + VisitorName + "\"";
            cr.DataDefinition.FormulaFields["selDocNum"].Text = "\"" + DocNum + "\"";
            cr.DataDefinition.FormulaFields["selJMBG"].Text = "\"" + JMBG + "\"";
            cr.DataDefinition.FormulaFields["selState"].Text = "\"" + State + "\"";
            cr.DataDefinition.FormulaFields["selCompany"].Text = "\"" + Company + "\"";
            cr.DataDefinition.FormulaFields["selBan"].Text = "\"" + Ban + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}