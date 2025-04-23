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
using System.Globalization;
using System.Resources;

using Util;
using TransferObjects;

namespace ReportsWeb
{
    public partial class ReportPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // parameters in query string should be Report (report url), Back (back url)
                if (!IsPostBack)
                {
                    setLanguage();

                    if (Request.QueryString["Report"] != null)
                        reportIframe.Attributes["src"] = Request.QueryString["Report"].Trim();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ReportPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ReportsWeb/ReportPage.aspx";
                    Response.Redirect(message, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ReportsWeb.Resource", typeof(ReportPage).Assembly);

                btnBack.Text = rm.GetString("lbtnBack", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnBack_Click(Object sender, EventArgs e)
        {
            try
            {

            //ScriptManager.RegisterStartupScript(this, GetType(), "Redirect", "window.history.back();", true);
            if (Request.QueryString["Back"] != null)
            {
                string message = Request.QueryString["Back"].ToString();

                if (Request.QueryString["emplID"] != null)
                {
                    message = message + "?emplID=" + Request.QueryString["emplID"].ToString();
                }
                Response.Redirect(message, false);
            }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ReportPage.btnBack_Click(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ReportsWeb/ReportPage.aspx";
                    Response.Redirect(message, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
