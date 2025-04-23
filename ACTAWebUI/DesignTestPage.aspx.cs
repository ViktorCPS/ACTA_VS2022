using System;
using System.Collections;
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
using System.Resources;
using System.Globalization;

using Common;

namespace ACTAWebUI
{
    public partial class DesignTestPage : System.Web.UI.Page
    {
        private static CultureInfo culture;
        private static ResourceManager rm;
        private static string dateTimeFormat;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {                    
                    culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                    rm = new ResourceManager("ACTAWebUI.Resource", typeof(PassesSearch).Assembly);
                    DateTimeFormatInfo dateTimeformat = new CultureInfo("en-US", true).DateTimeFormat;
                    dateTimeFormat = dateTimeformat.SortableDateTimePattern;

                    lbtnExit.Attributes.Add("onclick", "return LogoutByButton();");
                    
                    setLanguage();

                    lblError.Text = "";
                    
                    // if returned from result page, reload selected filter state
                    LoadState();

                    resultIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TabPage.aspx";
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in DesignTestPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/DesignTestPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                lbtnExit.Text = rm.GetString("lbtnExit", culture);
                //lbtnMenu.Text = rm.GetString("lbtnMenu", culture);
                if (Session["LoggedInUser"] != null)
                    lblLoggedInUser.Text = Session["LoggedInUser"].ToString().Trim();
                else
                    lblLoggedInUser.Text = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveState()
        {
            try
            {
                Dictionary<string, string> filterState = new Dictionary<string, string>();

                if (Session["FilterState"] != null)
                    filterState = (Dictionary<string, string>)Session["FilterState"];

                Session["FilterState"] = CommonWeb.Misc.SaveState(this, "DesignTestPage.", filterState);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadState()
        {
            try
            {
                Dictionary<string, string> filterState = new Dictionary<string, string>();

                if (Session["FilterState"] != null)
                {
                    filterState = (Dictionary<string, string>)Session["FilterState"];
                    CommonWeb.Misc.LoadState(this, "DesignTestPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
