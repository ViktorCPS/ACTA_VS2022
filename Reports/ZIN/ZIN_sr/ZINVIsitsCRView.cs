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
    public partial class ZINVIsitsCRView : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public ZINVIsitsCRView(DataSet data, string VisitorName, string DocNum, string JMBG, string State, string Company, string CardNum,
            string VisitType, string Visitor, string WU, string Empl, string Privacy, string From, string To, List<string> rowTexts, int numOfVisits)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ZINEmployeePassesCRView).Assembly);
            this.Text = rm.GetString("ZINVisits", culture);

            Reports.ZIN.ZIN_sr.ZINVIsitsCR_sr cr = new Reports.ZIN.ZIN_sr.ZINVIsitsCR_sr();
            cr.DataDefinition.FormulaFields["selVisitorName"].Text = "\"" + VisitorName + "\"";
            cr.DataDefinition.FormulaFields["selDocNum"].Text = "\"" + DocNum + "\"";
            cr.DataDefinition.FormulaFields["selJMBG"].Text = "\"" + JMBG + "\"";
            cr.DataDefinition.FormulaFields["selState"].Text = "\"" + State + "\"";
            cr.DataDefinition.FormulaFields["selCompany"].Text = "\"" + Company + "\"";
            cr.DataDefinition.FormulaFields["selCardNum"].Text = "\"" + CardNum + "\"";
            cr.DataDefinition.FormulaFields["selVisitType"].Text = "\"" + VisitType + "\"";
            cr.DataDefinition.FormulaFields["selVisitor"].Text = "\"" + Visitor + "\"";
            cr.DataDefinition.FormulaFields["selWU"].Text = "\"" + WU + "\"";
            cr.DataDefinition.FormulaFields["selPrivacy"].Text = "\"" + Privacy + "\"";
            cr.DataDefinition.FormulaFields["selFrom"].Text = "\"" + From + "\"";
            cr.DataDefinition.FormulaFields["selEmpl"].Text = "\"" + Empl + "\"";
            cr.DataDefinition.FormulaFields["selTo"].Text = "\"" + To + "\"";
            cr.DataDefinition.FormulaFields["row1Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row2Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row3Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row4Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row5Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row6Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row7Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row8Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row9Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row10Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row11Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row12Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["visitNum"].Text = "\"" + numOfVisits + "\"";
            for (int i = 1; i < rowTexts.Count+1; i++)
            {
                cr.DataDefinition.FormulaFields["row"+i+"Text"].Text = "\""+rowTexts[i-1]+"\"";
            }

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}