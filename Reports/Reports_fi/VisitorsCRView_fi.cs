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
    public partial class VisitorsCRView_fi : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public VisitorsCRView_fi(DataSet data, string selTag, string selWorkingUnit, string selEmployee, string selVisitor, string selVisitDescr,
            DateTime from, DateTime to)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(VisitorsCRView_fi).Assembly);
            this.Text = rm.GetString("VisitorsReports", culture);

            Reports.Reports_fi.VisitorsCR_fi cr = new Reports.Reports_fi.VisitorsCR_fi();
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