using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;

//using SecuritySwitch;
using Util;
using Common;

namespace ACTAWeb
{
    public class Global : System.Web.HttpApplication
    {
        //string session = "";
        
        protected void Application_Start(object sender, EventArgs e)
        {
         
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                // If it says it is a new session, but an existing cookie exists, then it must 
                // have timed out (can't use the cookie collection because even on first 
                // request it already contains the cookie (request and response
                // seem to share the collection)
                string szCookieHeader = Request.Headers["Cookie"];
                if ((szCookieHeader != null) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                {
                    // if user is stil authenticated, redirect with message of session timeout expired
                    if (User.Identity.IsAuthenticated)
                        Response.Redirect(@"~/Login.aspx?sessionExpired=true", false);
                    else
                        Response.Redirect(@"~/Login.aspx", false);
                }
                else
                    Response.Redirect(@"~/Login.aspx", false);    
            }
        }

      
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            
        }

        //protected void SecuritySwitch_EvaluateRequest(object sender, EvaluateRequestEventArgs e)
        //{
        //    e.ExpectedSecurity = SecuritySwitch.Configuration.RequestSecurity.Secure;
        //}
      
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                string error = Server.GetLastError().ToString().Trim();
                DebugLog log = new DebugLog(Constants.logFilePath);
                log.writeLog(DateTime.Now + " Application error: " + error + "\n");

                try
                {
                    
                    Response.Redirect(@"~/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error occured! Contact administrator!&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
            catch (Exception ex)
            {
                try
                {
                    //Tamara 28.10.2019.
                   // Response.Redirect(@"~/CommonWeb/MessagesPages/ErrorPage.aspx?Error=" + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                if (Session[Constants.sessionConnection] != null)
                {
                    DBConnectionManager.Instance.CloseDBConnection(Session[Constants.sessionConnection]);
                }
            }
            catch { }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            
        }
    }
}