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
    public partial class IOPairsCRView : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public IOPairsCRView(DataSet data, string selWorkingUnit,
            string selEmplName, string selShowPairs, string selLocation,DateTime from, DateTime to)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(IOPairsCRView).Assembly);
            this.Text = rm.GetString("IOPairsReports", culture);

            Reports.Reports_sr.IOPairsCR_sr cr = new Reports.Reports_sr.IOPairsCR_sr();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmplName + "\"";
            cr.DataDefinition.FormulaFields["selShowPairs"].Text = "\"" + selShowPairs + "\"";
            cr.DataDefinition.FormulaFields["selLocation"].Text = "\"" + selLocation + "\"";
           
            
            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}