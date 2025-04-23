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

using Util;
using TransferObjects;
using Common;

namespace ACTAWebUI
{
    public partial class LogOutPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
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
                }
            }
            catch { }
        }
    }
}
