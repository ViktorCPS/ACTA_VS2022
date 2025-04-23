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
    public partial class ZINPresencePreviewView : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public ZINPresencePreviewView(DataSet data, DateTime date, DateTime fromTime, DateTime toTime)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ZINEmployeePassesCRView).Assembly);
            this.Text = rm.GetString("ZINPresence", culture);

            Reports.ZIN.ZIN_sr.ZINPresencePreviewCR_sr cr = new Reports.ZIN.ZIN_sr.ZINPresencePreviewCR_sr();
            cr.DataDefinition.FormulaFields["selDate"].Text = "\"" + date.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selFromTime"].Text = "\"" + fromTime.ToString("HH") + "\"";
            cr.DataDefinition.FormulaFields["selToTime"].Text = "\"" + toTime.ToString("HH") + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}