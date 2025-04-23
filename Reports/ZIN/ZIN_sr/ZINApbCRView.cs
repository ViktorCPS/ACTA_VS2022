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
    public partial class ZINApbCRView : Form
    {
          CultureInfo culture;
        ResourceManager rm;

        public ZINApbCRView(DataSet data,string point, string employee, string cardNum,string printCopy,string reason,string direction)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(ZINEmployeePassesCRView).Assembly);
            this.Text = rm.GetString("ZINPassesReport", culture);

            Reports.ZIN.ZIN_sr.ZINApbCR_sr cr = new Reports.ZIN.ZIN_sr.ZINApbCR_sr();
            cr.DataDefinition.FormulaFields["point"].Text = "\"" + point + "\"";
            cr.DataDefinition.FormulaFields["employee"].Text = "\"" + employee + "\"";
            cr.DataDefinition.FormulaFields["cardNum"].Text = "\"" + cardNum + "\"";
            cr.DataDefinition.FormulaFields["printCopy"].Text = "\"" + printCopy + "\"";
            cr.DataDefinition.FormulaFields["reason"].Text = "\"" + reason + "\"";
            cr.DataDefinition.FormulaFields["direction"].Text = "\"" + direction + "\"";

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }
        
    }
}