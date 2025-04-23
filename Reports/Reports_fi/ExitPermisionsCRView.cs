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


namespace Reports.Reports_fi
{
    public partial class ExitPermisionsCRView_fi : Form
    {
         CultureInfo culture;
        ResourceManager rm;

        public ExitPermisionsCRView_fi(DataSet data, string selWorkingUnit,
            string selEmplName, string selPassType, string selState, string selIsued, DateTime from, DateTime to)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(IOPairsCRView_fi).Assembly);
            this.Text = rm.GetString("ExitPermisionsReports", culture);

            Reports.Reports_fi.ExitPermisionsCR_fi cr = new Reports.Reports_fi.ExitPermisionsCR_fi();
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