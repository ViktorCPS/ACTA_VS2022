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
using System.Globalization;
using System.Resources;

using Common;
using TransferObjects;
using Util;

namespace CommonWeb.Pickers
{
    public partial class Legend : System.Web.UI.Page
    {
        private const int legendWidth = 30;
        private const int legendDescWidth = 300;
        private const int separatorWidth = 770;
        private const int height = 15;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

                if (!Page.IsPostBack)
                {
                    btnClose.Attributes.Add("onclick", "return closeWindow();");

                    setLanguage();

                    if (Request.QueryString["company"] != null)
                    {
                        int company = -1;
                        if (!int.TryParse(Request.QueryString["company"].Trim(), out company))
                            company = -1;
                        
                        if (company != -1)                        
                            DrawLegend(company);                        
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in Legend.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("CommonWeb.Resource", typeof(Legend).Assembly);

                lblLegend.Text = rm.GetString("legend", culture);
                btnClose.Text = rm.GetString("btnClose", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawLegend(int company)
        {
            try
            {
                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());
                Dictionary<int, PassTypeTO> passTypes = new PassType(Session[Constants.sessionConnection]).SearchForCompanyDictionary(company, isAltLang);

                int legendIndex = 0;
                foreach (int ptID in passTypes.Keys)
                {
                    if (legendIndex % 2 == 0)
                    {
                        Label separatorLbl = new Label();
                        separatorLbl.ID = "separatorLbl" + legendIndex.ToString().Trim();
                        separatorLbl.Width = new Unit(separatorWidth);
                        separatorLbl.Height = new Unit(5);
                        legendCtrlHolder.Controls.Add(separatorLbl);
                    }

                    Label legendLbl = new Label();
                    legendLbl.ID = "legendLbl" + legendIndex.ToString().Trim();
                    legendLbl.Width = new Unit(legendWidth);
                    legendLbl.Height = new Unit(height);
                    legendLbl.BackColor = ColorTranslator.FromHtml(passTypes[ptID].SegmentColor);
                    legendCtrlHolder.Controls.Add(legendLbl);

                    Label legendDescLbl = new Label();
                    legendDescLbl.ID = "legendDescLbl" + legendIndex.ToString().Trim();
                    legendDescLbl.Width = new Unit(legendDescWidth);
                    legendDescLbl.Height = new Unit(height);
                    legendDescLbl.CssClass = "legendLbl";
                    if (!isAltLang)
                        legendDescLbl.Text = passTypes[ptID].DescriptionAndID;
                    else
                        legendDescLbl.Text = passTypes[ptID].DescriptionAltAndID;
                    legendCtrlHolder.Controls.Add(legendDescLbl);
                    
                    legendIndex++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
