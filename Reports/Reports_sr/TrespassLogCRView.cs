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
    public partial class TrespassLogCRView : System.Windows.Forms.Form
    {
        CultureInfo culture;
        ResourceManager rm;

        public TrespassLogCRView(DataSet data, string selLocation,
            string selGate, string selReader, string selDirection, string selEmployee, string selEvent, DateTime from, DateTime to)
        {
            try
            {
                InitializeComponent();
                // Language tool
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("Reports.ReportResource", typeof(TrespassLogCRView).Assembly);
                this.Text = rm.GetString("TrespassLogs", culture);

                Reports.Reports_sr.TrespassLogsCR_sr cr = new Reports.Reports_sr.TrespassLogsCR_sr();
                cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
                cr.DataDefinition.FormulaFields["selLocation"].Text = "\"" + selLocation + "\"";
                cr.DataDefinition.FormulaFields["selGate"].Text = "\"" + selGate + "\"";
                cr.DataDefinition.FormulaFields["selReader"].Text = "\"" + selReader + "\"";
                cr.DataDefinition.FormulaFields["selDirection"].Text = "\"" + selDirection + "\"";
                cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmployee + "\"";
                cr.DataDefinition.FormulaFields["selEvent"].Text = "\"" + selEvent + "\"";


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