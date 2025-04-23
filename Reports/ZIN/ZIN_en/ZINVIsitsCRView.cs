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

namespace Reports.ZIN.ZIN_en
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
            rm = new ResourceManager("Reports.ReportResource", typeof(ZINEmployeePassesCRView_en).Assembly);
            this.Text = rm.GetString("ZINVisits", culture);

            Reports.ZIN.ZIN_en.ZINVisitsCR_en cr = new Reports.ZIN.ZIN_en.ZINVisitsCR_en();
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

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}