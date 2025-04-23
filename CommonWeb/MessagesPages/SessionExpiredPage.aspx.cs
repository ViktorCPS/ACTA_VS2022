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
using Common;

namespace CommonWeb.MessagesPages
{
    public partial class SessionExpiredPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("CommonWeb.Resource", typeof(SessionExpiredPage).Assembly);

                // close log record
                if (Session[Constants.sessionLogInUserLog] != null && Session[Constants.sessionLogInUserLog] is ApplUserLogTO)
                {
                    ApplUserLog log = new ApplUserLog(Session[Constants.sessionConnection]);
                    log.UserLogTO = (ApplUserLogTO)Session[Constants.sessionLogInUserLog];

                    if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO)
                        log.Update("", ((ApplUserTO)Session[Constants.sessionLogInUser]).UserID);
                    else
                        log.Update();
                }
                 
                FormsAuthentication.SignOut();
                lblError.Text = rm.GetString("sessionExpired", culture);
                Response.Redirect("/ACTAWeb/Login.aspx?sessionExpired=true", false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in DefaultPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim(), false);                        
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
