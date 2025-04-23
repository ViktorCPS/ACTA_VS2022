using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using System.Globalization;
using System.Resources;
using Common;

namespace Reports.UNIPROM.UNIPROM_en
{
    public partial class UNIPROMDailyPreviewCRView_en : Form
    {
       CultureInfo culture;
        ResourceManager rm;

        public UNIPROMDailyPreviewCRView_en(DataSet data, string selWorkingUnit,
            string selEmplName, string selShowPairs, string selLocation,DateTime from, DateTime to,string totalPresent,
            string totalDayOff,string totalAbsent,string totalSickLeave, string totalVacation)
        {
            InitializeComponent();
            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(UNIPROMDailyPreviewCRView_en).Assembly);
            this.Text = rm.GetString("DailyPreviewTitle", culture);

            UNIPROM_sr.UNIPROMDailyPreviewCR_sr cr = new UNIPROM_sr.UNIPROMDailyPreviewCR_sr();
            cr.DataDefinition.FormulaFields["from"].Text = "\"" + from.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["to"].Text = "\"" + to.ToString("dd.MM.yyyy") + "\"";
            cr.DataDefinition.FormulaFields["selWorkingUnit"].Text = "\"" + selWorkingUnit + "\"";
            cr.DataDefinition.FormulaFields["selEmployee"].Text = "\"" + selEmplName + "\"";
            cr.DataDefinition.FormulaFields["selShowPairs"].Text = "\"" + selShowPairs + "\"";
            cr.DataDefinition.FormulaFields["selLocation"].Text = "\"" + selLocation + "\"";
            cr.DataDefinition.FormulaFields["totalAbsent"].Text = "\"" + totalAbsent + "\"";
            cr.DataDefinition.FormulaFields["totalDayOff"].Text = "\"" + totalDayOff + "\"";
            cr.DataDefinition.FormulaFields["totalPresente"].Text = "\"" + totalPresent + "\"";
            cr.DataDefinition.FormulaFields["totalSickLeave"].Text = "\"" + totalSickLeave + "\"";
            cr.DataDefinition.FormulaFields["totalVacation"].Text = "\"" + totalVacation + "\"";
            
            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
    }
}