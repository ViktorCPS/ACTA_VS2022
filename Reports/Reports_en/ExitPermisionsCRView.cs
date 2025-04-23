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
    public partial class ExitPermisionsCRView_en : Form
    {
         CultureInfo culture;
        ResourceManager rm;

        public ExitPermisionsCRView_en(DataSet data, string selWorkingUnit,
            string selEmplName, string selPassType, string selState, string selIsued, DateTime from, DateTime to)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(IOPairsCRView_en).Assembly);
            this.Text = rm.GetString("ExitPermisionsReports", culture);

            Reports.Reports_en.ExitPermisionsCR_en cr = new Reports.Reports_en.ExitPermisionsCR_en();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmplName + "\"";
            cr.DataDefinition.FormulaFields["selPassType"].Text = "\"" + selPassType + "\"";
            cr.DataDefinition.FormulaFields["selState"].Text = "\"" + selState + "\"";
            cr.DataDefinition.FormulaFields["selIsued"].Text = "\"" + selIsued + "\"";
            
            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}