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

namespace Reports.Reports_fi
{
    public partial class ExtraHoursUsedCRView_fi : Form
    {
         CultureInfo culture;
        ResourceManager rm;

        public ExtraHoursUsedCRView_fi(DataSet data, string selWorkingUnit,
            string selEmplName, string selType,DateTime from, DateTime to)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ExtraHoursUsedCRView_fi).Assembly);
            this.Text = rm.GetString("extraHoursUsed", culture);
            
            Reports.Reports_fi.ExtraHoursUsedCR_fi cr = new Reports.Reports_fi.ExtraHoursUsedCR_fi();
            if (!from.Equals(new DateTime()) && !to.Equals(new DateTime()))
            {
                cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            }
            else
            {
                cr.DataDefinition.FormulaFields["from"].Text = "";
                cr.DataDefinition.FormulaFields["to"].Text = "";
            }
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmplName + "\"";
            cr.DataDefinition.FormulaFields["selType"].Text = "\"" +selType + "\"";
                       
            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}