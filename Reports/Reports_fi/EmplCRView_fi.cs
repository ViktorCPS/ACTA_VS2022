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

namespace Reports.Reports_fi
{
    public partial class EmplCRView_fi : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        CultureInfo culture;
        ResourceManager rm;

        public EmplCRView_fi(DataSet data, string selEmplID, string selFirstName, string selLastName, string selType, string CardNum,
          string WU, List<string> rowTexts)
        {
            InitializeComponent();

            // Language tool
            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(EmplCRView_fi).Assembly);
            this.Text = rm.GetString("ZINVisits", culture);

            Reports_fi.EmplCR_fi cr = new Reports_fi.EmplCR_fi();
            cr.DataDefinition.FormulaFields["selEmplID"].Text = "\"" + selEmplID + "\"";
            cr.DataDefinition.FormulaFields["selFirstName"].Text = "\"" + selFirstName + "\"";
            cr.DataDefinition.FormulaFields["selLastName"].Text = "\"" + selLastName + "\"";
            cr.DataDefinition.FormulaFields["selType"].Text = "\"" + selType + "\"";
            cr.DataDefinition.FormulaFields["selCardNum"].Text = "\"" + CardNum + "\"";
            cr.DataDefinition.FormulaFields["selWU"].Text = "\"" + WU + "\"";
            cr.DataDefinition.FormulaFields["row1Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row2Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row3Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row4Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row5Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row6Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row7Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row8Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row9Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row10Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row11Text"].Text = "\"\"";
            cr.DataDefinition.FormulaFields["row12Text"].Text = "\"\"";
            for (int i = 1; i < rowTexts.Count + 1; i++)
            {
                cr.DataDefinition.FormulaFields["row" + i + "Text"].Text = "\"" + rowTexts[i - 1] + "\"";
            }

            cr.SetDataSource(data);
            this.crystalReportViewer1.ReportSource = cr;
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}