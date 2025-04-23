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

namespace Reports.ZIN.ZIN_sr
{
    public partial class ZINPassTicketVisitorsCRView : Form
    {
         CultureInfo culture;
        ResourceManager rm;

        public ZINPassTicketVisitorsCRView(DataSet data, string enterTime)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ZINEmployeePassesCRView).Assembly);
            this.Text = rm.GetString("ZINPassesReport", culture);

            Reports.ZIN.ZIN_sr.ZINPassTicketVisitsCR_sr cr = new Reports.ZIN.ZIN_sr.ZINPassTicketVisitsCR_sr();
            cr.DataDefinition.FormulaFields["enterTime"].Text = "\"" + enterTime + "\"";
            
            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }

        private void EmployeePassesCRView_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.Zoom(1);
        }
    }
}