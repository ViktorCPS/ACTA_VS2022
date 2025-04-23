using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;

using Util;
using CommonWeb;
using TransferObjects;

namespace ACTAWebUI
{
    public partial class HoursLineControlUC : System.Web.UI.UserControl
    {
        private int startHour = 0;
        private int endHour = 23;
        private bool showOnlyPairs = false;
        private string firstLblText = "";
        private string secondLblText = "";

        private const double firstLblWidth = 135;
        private const double secondLblWidth = 70;
        private const int height = 15;

        private RuleTO nightWorkRule = new RuleTO();

        public int StartHour
        {
            get { return startHour; }
            set { startHour = value; }
        }

        public int EndHour
        {
            get { return endHour; }
            set { endHour = value; }
        }

        public RuleTO NightWorkRule
        {
            get { return nightWorkRule; }
            set { nightWorkRule = value; }
        }

        public bool ShowOnlyPairs
        {
            get { return showOnlyPairs; }
            set { showOnlyPairs = value; }
        }

        public string FirstLblText
        {
            get { return firstLblText; }
            set { firstLblText = value; }
        }

        public string SecondLblText
        {
            get { return secondLblText; }
            set { secondLblText = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {             
                DrawHourLine();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in HoursLineControlUC.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void DrawHourLine()
        {
            try
            {
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Dispose();
                }

                this.Controls.Clear();
                
                int lblCounter = 0;

                if (!ShowOnlyPairs)
                {                    
                    Label firstLbl = new Label();
                    firstLbl.ID = "firstLbl";
                    firstLbl.Width = new Unit(firstLblWidth + 2);
                    firstLbl.Height = new Unit(height);
                    firstLbl.BackColor = ColorTranslator.FromHtml(Constants.hourLineColor);
                    firstLbl.Text = FirstLblText;
                    firstLbl.CssClass = "contentLblLeft";
                    this.Controls.Add(firstLbl);

                    Label secondLbl = new Label();
                    secondLbl.ID = "secondLbl";
                    secondLbl.Width = new Unit(secondLblWidth + 2);
                    secondLbl.Height = new Unit(height);
                    secondLbl.BackColor = ColorTranslator.FromHtml(Constants.hourLineColor);
                    secondLbl.Text = SecondLblText;
                    secondLbl.CssClass = "contentLblLeft";
                    this.Controls.Add(secondLbl);
                }

                for (int hour = startHour; hour <= endHour; hour++)
                {
                    Label hourLbl = new Label();
                    hourLbl.ID = "lbl" + lblCounter.ToString();
                    hourLbl.Width = new Unit(Constants.minutWidth * 60);
                    hourLbl.Height = new Unit(height);
                    hourLbl.CssClass = "contentHdrLbl";
                    if (nightWorkRule.RuleID != -1 && (hour < nightWorkRule.RuleDateTime2.Hour || hour >= nightWorkRule.RuleDateTime1.Hour))
                        hourLbl.BackColor = ColorTranslator.FromHtml(Constants.nightWorkLblColor);
                    else
                        hourLbl.BackColor = ColorTranslator.FromHtml(Constants.hourLineColor);
                    hourLbl.Text = hour.ToString("D2"); // with leading zero
                    this.Controls.Add(hourLbl);
                    lblCounter++;                    
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}