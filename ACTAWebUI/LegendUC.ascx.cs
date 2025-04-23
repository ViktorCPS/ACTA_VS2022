using System;
using System.Collections;
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
using System.Resources;
using System.Globalization;

using Util;
using CommonWeb;
using TransferObjects;
using Common;

namespace ACTAWebUI
{
    public partial class LegendUC : System.Web.UI.UserControl
    {
        private const int legendWidth = 20;
        private const int legendDescWidth = 110;
        private const int lblSeparatorWidth = 10;
        private const int height = 14;

        private const string outWSBorderColor = "#FFCCCC";
        private const string outWSColor = "#FFE6E6";
        private const string inWSBorderColor = "#CCEBCC";
        private const string inWSColor = "#E6F5E6";

        private int company = -1;

        public int Company
        {
            get { return company; }
            set { company = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DrawLegend();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in LegendUC.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void DrawLegend()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(LegendUC).Assembly);
                
                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Dispose();
                }

                this.Controls.Clear();

                int legendCtrlNum = 0;

                Label nightWorkLbl = new Label();
                nightWorkLbl.ID = "nightWorkLbl";
                nightWorkLbl.Width = new Unit(legendWidth);
                nightWorkLbl.Height = new Unit(height);
                nightWorkLbl.BackColor = ColorTranslator.FromHtml(Constants.nightWorkLblColor);
                nightWorkLbl.CssClass = "legendLbl";
                this.Controls.Add(nightWorkLbl);

                Label nightWorkDescLbl = new Label();
                nightWorkDescLbl.ID = "nightWorkDescLbl";                
                nightWorkDescLbl.Height = new Unit(height);
                nightWorkDescLbl.CssClass = "legendLbl";
                nightWorkDescLbl.Text = rm.GetString("nightWorkDesc", culture);
                this.Controls.Add(nightWorkDescLbl);

                legendCtrlNum++;
                insertSeparator(legendCtrlNum);

                Label manualCreatedLbl = new Label();
                manualCreatedLbl.ID = "manualCreatedLbl";
                manualCreatedLbl.Width = new Unit(legendWidth - 2);
                manualCreatedLbl.Height = new Unit(height - 2);
                manualCreatedLbl.BorderColor = Color.Black;
                manualCreatedLbl.BorderStyle = BorderStyle.Solid;
                manualCreatedLbl.BorderWidth = new Unit(1);
                manualCreatedLbl.CssClass = "legendLbl";
                this.Controls.Add(manualCreatedLbl);

                Label manualCreatedDescLbl = new Label();
                manualCreatedDescLbl.ID = "manualCreatedDescLbl";                
                manualCreatedDescLbl.Height = new Unit(height);
                manualCreatedDescLbl.CssClass = "legendLbl";
                manualCreatedDescLbl.Text = rm.GetString("manualCreatedDesc", culture);
                this.Controls.Add(manualCreatedDescLbl);

                legendCtrlNum++;
                insertSeparator(legendCtrlNum);

                string url = "/ACTAWeb/ReportsWeb/BarSegment.aspx?width=" + legendWidth.ToString() + "&height=" + height.ToString();
                url += "&inWS=true&barColor=B2B2B2&legend=true";
                System.Web.UI.WebControls.Image inWSLbl = new System.Web.UI.WebControls.Image();
                inWSLbl.ID = "inWSLbl";
                inWSLbl.Width = new Unit(legendWidth);
                inWSLbl.Height = new Unit(height);                            
                inWSLbl.ImageUrl = url;                
                this.Controls.Add(inWSLbl);
                
                Label inWSDescLbl = new Label();
                inWSDescLbl.ID = "inWSDescLbl";                
                inWSDescLbl.Height = new Unit(height);
                inWSDescLbl.CssClass = "legendLbl";
                inWSDescLbl.Text = rm.GetString("inWSDesc", culture);
                this.Controls.Add(inWSDescLbl);

                legendCtrlNum++;
                insertSeparator(legendCtrlNum);

                url = "/ACTAWeb/ReportsWeb/BarSegment.aspx?width=" + legendWidth.ToString() + "&height=" + height.ToString();
                url += "&inWS=false&barColor=B2B2B2&legend=true";
                System.Web.UI.WebControls.Image outWSLbl = new System.Web.UI.WebControls.Image();
                outWSLbl.ID = "outWSLbl";
                outWSLbl.Width = new Unit(legendWidth);
                outWSLbl.Height = new Unit(height);
                outWSLbl.ImageUrl = url;                
                this.Controls.Add(outWSLbl);

                Label outWSDescLbl = new Label();
                outWSDescLbl.ID = "outWSDescLbl";
                outWSDescLbl.Height = new Unit(height);                
                outWSDescLbl.CssClass = "legendLbl";
                outWSDescLbl.Text = rm.GetString("outWSDesc", culture);
                this.Controls.Add(outWSDescLbl);

                legendCtrlNum++;
                insertSeparator(legendCtrlNum);

                url = "/ACTAWeb/ReportsWeb/BarSegment.aspx?width=" + legendWidth.ToString() + "&height=" + height.ToString();
                url += "&inWS=true&barColor=B2B2B2&legend=true&verify=true";
                System.Web.UI.WebControls.Image verificationLbl = new System.Web.UI.WebControls.Image();
                verificationLbl.ID = "verificationLbl";
                verificationLbl.Width = new Unit(legendWidth);
                verificationLbl.Height = new Unit(height);
                verificationLbl.ImageUrl = url;                
                this.Controls.Add(verificationLbl);

                Label verificationDescLbl = new Label();
                verificationDescLbl.ID = "verificationDescLbl";                
                verificationDescLbl.Height = new Unit(height);
                verificationDescLbl.CssClass = "legendLbl";
                verificationDescLbl.Text = rm.GetString("verificationDesc", culture);
                this.Controls.Add(verificationDescLbl);

                legendCtrlNum++;
                insertSeparator(legendCtrlNum);

                url = "/ACTAWeb/ReportsWeb/BarSegment.aspx?width=" + legendWidth.ToString() + "&height=" + height.ToString();
                url += "&inWS=true&barColor=B2B2B2&legend=true&confirm=true";
                System.Web.UI.WebControls.Image confirmationLbl = new System.Web.UI.WebControls.Image();
                confirmationLbl.ID = "confirmationLbl";
                confirmationLbl.Width = new Unit(legendWidth);
                confirmationLbl.Height = new Unit(height);
                confirmationLbl.ImageUrl = url;                
                this.Controls.Add(confirmationLbl);

                Label confirmationDescLbl = new Label();
                confirmationDescLbl.ID = "confirmationDescLbl";
                confirmationDescLbl.Height = new Unit(height);                
                confirmationDescLbl.CssClass = "legendLbl";
                confirmationDescLbl.Text = rm.GetString("confirmationDesc", culture);
                this.Controls.Add(confirmationDescLbl);

                legendCtrlNum++;
                insertSeparator(legendCtrlNum);

                PassTypeTO pt = new PassType(Session[Constants.sessionConnection]).Find(Constants.absence);
                if (pt.PassTypeID != -1)
                {
                    bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());
                    url = "/ACTAWeb/ReportsWeb/BarSegment.aspx?width=" + legendWidth.ToString() + "&height=" + height.ToString();
                    url += "&inWS=true&barColor=" + pt.SegmentColor.Replace('#', ' ').Trim() + "&legend=true";
                    System.Web.UI.WebControls.Image unjustifiedLbl = new System.Web.UI.WebControls.Image();
                    confirmationLbl.ID = "unjustifiedLbl";
                    unjustifiedLbl.Width = new Unit(legendWidth);
                    unjustifiedLbl.Height = new Unit(height);
                    unjustifiedLbl.ImageUrl = url;                    
                    this.Controls.Add(unjustifiedLbl);

                    Label unjustifiedDescLbl = new Label();
                    unjustifiedDescLbl.ID = "unjustifiedDescLbl";
                    unjustifiedDescLbl.Height = new Unit(height);                    
                    unjustifiedDescLbl.CssClass = "legendLbl";
                    if (isAltLang)
                        unjustifiedDescLbl.Text = pt.DescAlt.Trim().ToLower();
                    else
                        unjustifiedDescLbl.Text = pt.Description.Trim().ToLower();
                    this.Controls.Add(unjustifiedDescLbl);

                    legendCtrlNum++;
                    insertSeparator(legendCtrlNum);
                }

                ImageButton legendBtn = new ImageButton();
                legendBtn.ID = "legendBtn";
                legendBtn.Width = new Unit(height);
                legendBtn.Height = new Unit(height);
                legendBtn.ImageUrl = "/ACTAWeb/CommonWeb/images/legend.png";                
                legendBtn.Attributes.Add("onclick", "return legendWindow('" + company.ToString().Trim() + "');");
                legendBtn.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                legendBtn.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                this.Controls.Add(legendBtn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void insertSeparator(int legendCtrlNum)
        {
            try
            {
                Label separator = new Label();
                separator.ID = "separator" + legendCtrlNum.ToString().Trim();
                separator.Width = new Unit(lblSeparatorWidth);
                separator.CssClass = "legendLbl";
                this.Controls.Add(separator);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}