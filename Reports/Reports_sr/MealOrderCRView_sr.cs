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
    public partial class MealOrderCRView_sr : Form
    {
            CultureInfo culture;
        ResourceManager rm;

        public MealOrderCRView_sr(DataSet data, DateTime from, DateTime to, string selCreatedBy)
        {
            try
            {
                InitializeComponent();

                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(MealsUsedCRView_sr).Assembly);
                this.Text = rm.GetString("mealsOrderReports", culture);

                Reports.Reports_sr.MealOrderCR_sr cr = new Reports.Reports_sr.MealOrderCR_sr();
                cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["selCreatedBy"].Text = "\"" + selCreatedBy + "\"";
                for (int i = 1; i <= 31; i++)
                {
                    DateTime date = from.AddDays(i - 1);
                    if (date.Date <= to.Date)
                    {
                        cr.DataDefinition.FormulaFields[i.ToString()].Text = "\"" + date.Day.ToString() + "\"";
                    }
                    else
                    {
                        cr.DataDefinition.FormulaFields[i.ToString()].Text = "";
                    }
                }

                cr.SetDataSource(data);
                this.crystalReportViewer1.ReportSource = cr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}