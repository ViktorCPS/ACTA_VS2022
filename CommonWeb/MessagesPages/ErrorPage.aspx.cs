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
using System.Resources;
using System.Globalization;

using Util;
using TransferObjects;
using Common;

namespace CommonWeb.MessagesPages
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["Error"] != null)
                    {
                        lblError.Text = Request.QueryString["Error"].ToString();
                    }
                    if (Request.QueryString["Back"] != null)
                    {
                        lbtnBack.Visible = true;
                    }
                    else
                    {
                        lbtnBack.Visible = false;
                    }
                    if (Request.QueryString["Exit"] != null)
                    {
                        lbtnExit.Visible = true;
                    }
                    else
                    {
                        lbtnExit.Visible = false;
                    }
                    if (Request.QueryString["Header"] != null && Request.QueryString["Header"].ToString().Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    {
                        hdrTable.Visible = false;
                    }
                    else
                    {
                        hdrTable.Visible = true;
                    }
                                        
                    setLanguage();
                }                
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=" + ex.Message.Replace('\n', ' ').Trim()
                        + "&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("CommonWeb.Resource", typeof(ErrorPage).Assembly);
                        
                lbtnBack.Text = rm.GetString("lbtnBack", culture);
                if (Request.QueryString["Back"] != null)
                    lbtnBack.ToolTip = Request.QueryString["Back"].ToString();
                lbtnExit.Text = rm.GetString("lbtnExit", culture);
                lblErrorTitle.Text = rm.GetString("Error", culture);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbtnBack_Click(Object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["Back"] != null)
                {
                    string message = Request.QueryString["Back"].ToString();
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=" + ex.Message.Replace('\n', ' ').Trim()
                        + "&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void LogOut(object sender, EventArgs e)
        {
            try
            {
                // close log record
                //if (Session[Constants.sessionLogInUserLog] != null && Session[Constants.sessionLogInUserLog] is ApplUserLogTO)
                //{
                //    ApplUserLog log = new ApplUserLog(Session[Constants.sessionConnection]);
                //    log.UserLogTO = (ApplUserLogTO)Session[Constants.sessionLogInUserLog];

                //    if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO)
                //        log.Update("", ((ApplUserTO)Session[Constants.sessionLogInUser]).UserID);
                //    else
                //        log.Update();
                //}

                //// sign out
                //Session.Abandon();
                //FormsAuthentication.SignOut();
                //Response.Redirect("/ACTAWeb/Login.aspx", false);
                CommonWeb.Misc.LogOut(Session[Constants.sessionLogInUserLog], Session[Constants.sessionLogInUser], Session[Constants.sessionConnection], Session, Response);
            }
            catch (Exception ex)
            {
                Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ErrorPage.LogOut(): " + ex.Message.Replace('\n', ' ').Trim()
                    + "&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
            }
        }
    }
}
