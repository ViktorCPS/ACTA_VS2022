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
    public partial class MealEmployeeScheduleCRView_sr : Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public MealEmployeeScheduleCRView_sr(DataSet data, DateTime from, DateTime to, string selWorkingUnit,
            string selEmployee, string selMealType, bool orderExists)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(MealEmployeeScheduleCRView_sr).Assembly);
            this.Text = rm.GetString("mealsEmployeeScheduleReports", culture);

            Reports.Reports_sr.MealEmployeeScheduleCR_sr cr = new Reports.Reports_sr.MealEmployeeScheduleCR_sr();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
            cr.DataDefinition.FormulaFields["selMealType"].Text = "\"" + selMealType + "\"";
            cr.SetParameterValue("orderExists", orderExists);

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;  
        }

        private void MealsUsedCRView_sr_Load(object sender, EventArgs e)
        {
            this.crystalReportViewer1.Zoom(1);
        }
    
    }
}
