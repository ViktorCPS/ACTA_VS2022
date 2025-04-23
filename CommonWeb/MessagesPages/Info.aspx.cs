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
using System.IO;

using Util;
using TransferObjects;

namespace CommonWeb.MessagesPages
{
    public partial class Info : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("CommonWeb.Resource", typeof(Info).Assembly);

                    btnClose.Attributes.Add("onclick", "return closeWindow();");
                    btnClose.Text = rm.GetString("btnClose", culture);

                    if (Request.QueryString["isError"] != null && Request.QueryString["isError"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))                    
                        lblTitle.Text = rm.GetString("Error", culture);
                    else                    
                        lblTitle.Text = rm.GetString("Information", culture);

                    if (Session[Constants.sessionInfoMessage] != null)
                    {
                        lblInfoMessage.Text = Session[Constants.sessionInfoMessage].ToString().Trim().Replace(Environment.NewLine, "<br/>");
                        Session[Constants.sessionInfoMessage] = null;
                    }

                    if (Request.QueryString["systemInfo"] != null && Request.QueryString["systemInfo"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                        btnClose.Visible = false;
                    else
                        btnClose.Visible = true;
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
    }
}
