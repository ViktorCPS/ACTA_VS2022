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

namespace Reports.GSK.GSK_sr
{
    public partial class GSKVacationRequestCRView : Form
    {
         CultureInfo culture;
        ResourceManager rm;

        public GSKVacationRequestCRView(DataSet data, string employee, string superior, string requested, string dateFrom, string dateTo, string replacement, string telephone,
            string fromPY, string due, string taken, string balance, string ReturnDate)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(GSKVacationRequestCRView).Assembly);
            this.Text = rm.GetString("GSKVacations", culture);

            Reports.GSK.GSK_sr.GSKVacationRequestCR_rs cr = new Reports.GSK.GSK_sr.GSKVacationRequestCR_rs();
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + employee + "\"";
            cr.DataDefinition.FormulaFields["selSuperior"].Text = "\"" + superior + "\"";
            cr.DataDefinition.FormulaFields["selRequested"].Text = "\"" + requested + "\"";
            cr.DataDefinition.FormulaFields["selDateFrom"].Text = "\"" + dateFrom + "\"";
            cr.DataDefinition.FormulaFields["selDateTo"].Text = "\"" + dateTo + "\"";
            cr.DataDefinition.FormulaFields["selReplacement"].Text = "\"" + replacement + "\"";
            cr.DataDefinition.FormulaFields["selTelephone"].Text = "\"" + telephone + "\"";
            cr.DataDefinition.FormulaFields["selFromPY"].Text = "\"" + fromPY + "\"";
            cr.DataDefinition.FormulaFields["selDue"].Text = "\"" + due + "\"";
            cr.DataDefinition.FormulaFields["selTaken"].Text = "\"" + taken + "\"";
            cr.DataDefinition.FormulaFields["selReturnDate"].Text = "\"" + ReturnDate + "\"";
            cr.DataDefinition.FormulaFields["selBalance"].Text = "\"" + balance + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}