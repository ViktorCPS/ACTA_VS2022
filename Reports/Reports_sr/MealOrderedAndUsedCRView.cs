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


namespace Reports.Reports_sr
{
    public partial class MealOrderedAndUsedCRView : Form
    {

        CultureInfo culture;
        ResourceManager rm;

      public MealOrderedAndUsedCRView(DataSet data, DateTime from, DateTime to, string selWorkingUnit)
        {
            try
            {
                InitializeComponent();

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(MealsUsedCRView_sr).Assembly);
                this.Text = rm.GetString("mealOrdAndUsed", culture);

                Reports.Reports_sr.MealOrderedAndUsedCR_sr cr = new Reports.Reports_sr.MealOrderedAndUsedCR_sr();
                cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";

                cr.SetDataSource(data);
                this.crystalReportViewer1.ReportSource = cr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


      private void MealOrderedAndUsedCR_sr_Load(object sender, EventArgs e)
      {
          this.crystalReportViewer1.Zoom(1);
      }

    }
}