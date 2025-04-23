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
    public partial class MealsUsedCRView_sr : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public MealsUsedCRView_sr(DataSet data, DateTime from, DateTime to, string selWorkingUnit, 
            string selEmployee, string selPoint, string selMealType, string qty, string moneyAmt)
        {
            try
            {
                InitializeComponent();

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(MealsUsedCRView_sr).Assembly);
                this.Text = rm.GetString("mealsUsedReports", culture);

                Reports.Reports_sr.MealsUsedCR_sr cr = new Reports.Reports_sr.MealsUsedCR_sr();
                cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
                cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
                cr.DataDefinition.FormulaFields["selPoint"].Text = "\"" + selPoint + "\"";
                cr.DataDefinition.FormulaFields["selMealType"].Text = "\"" + selMealType + "\"";
                cr.DataDefinition.FormulaFields["qty"].Text = "\"" + qty + "\"";
                cr.DataDefinition.FormulaFields["moneyAmt"].Text = "\"" + moneyAmt + "\"";

                cr.SetDataSource(data);
                this.crystalReportViewer1.ReportSource = cr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MealsUsedCRView_sr_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.Zoom(1);
        }
    }
}