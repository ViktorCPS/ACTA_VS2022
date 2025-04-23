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

namespace Reports.ZIN.ZIN_sr
{
    public partial class ZINEmployeePassesCRView : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public ZINEmployeePassesCRView(DataSet data, DateTime from, DateTime to, string selWorkingUnit,string selEmplGroup)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ZINEmployeePassesCRView).Assembly);
            this.Text = rm.GetString("ZINPassesReport", culture);

            Reports.ZIN.ZIN_sr.ZINEmployeePassesCR_sr cr = new Reports.ZIN.ZIN_sr.ZINEmployeePassesCR_sr();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmplGroup"].Text = "\"" + selEmplGroup + "\"";
            
            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }

        private void EmployeePassesCRView_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.Zoom(1);
        }
    }
}