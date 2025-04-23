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
    public partial class VisitorsCRView_en : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public VisitorsCRView_en(DataSet data, string selTag, string selWorkingUnit, string selEmployee, string selVisitor, string selVisitDescr,
            DateTime from, DateTime to)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(VisitorsCRView_en).Assembly);
            this.Text = rm.GetString("VisitorsReports", culture);

            Reports.Reports_en.VisitorsCR_en cr = new Reports.Reports_en.VisitorsCR_en();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selTag"].Text = "\"" + selTag + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
            cr.DataDefinition.FormulaFields["selVisitor"].Text = "\"" + selVisitor + "\"";
            cr.DataDefinition.FormulaFields["selVisitDesc"].Text = "\"" + selVisitDescr + "\"";


            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}