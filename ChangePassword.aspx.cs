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

using Common;
using TransferObjects;
using Util;
using opp;

namespace ACTAWeb
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        const string pageName = "ChangePassword";

        private DateTime LoadTime
        {
            get
            {
                DateTime loadDate = new DateTime();
                if (ViewState["loadDate"] != null && ViewState["loadDate"] is DateTime)
                {
                    loadDate = (DateTime)ViewState["loadDate"];
                }

                return loadDate;
            }
            set
            {
                if (value.Equals(new DateTime()))
                    ViewState["loadDate"] = null;
                else
                    ViewState["loadDate"] = value;
            }
        }

        private string Message
        {
            get
            {
                string message = "";
                if (ViewState["message"] != null)
                    message = ViewState["message"].ToString().Trim();

                return message;
            }
            set
            {
                if (value.Trim().Equals(""))
                    ViewState["message"] = null;
                else
                    ViewState["message"] = value;
            }
        }

        private DateTime StartLoadTime
        {
            get
            {
                DateTime startLoadDate = new DateTime();
                if (ViewState["startLoadDate"] != null && ViewState["startLoadDate"] is DateTime)
                {
                    startLoadDate = (DateTime)ViewState["startLoadDate"];
                }

                return startLoadDate;
            }
            set
            {
                if (value.Equals(new DateTime()))
                    ViewState["startLoadDate"] = null;
                else
                    ViewState["startLoadDate"] = value;
            }
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                StartLoadTime = DateTime.Now;
                LoadTime = new DateTime();
                Message = "";
            }
            catch { }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                writeLog(DateTime.Now, true);
            }
            catch { }
        }

        private void writeLog(DateTime date, bool writeToFile)
        {
            try
            {
                string writeFile = ConfigurationManager.AppSettings["writeLoadTime"];

                if (writeFile != null && writeFile.Trim().ToUpper().Equals(Constants.yes.Trim().ToUpper()))
                {
                    DebugLog log = new DebugLog(Constants.logFilePath + "LoadTime.txt");

                    if (!writeToFile)
                    {
                        string message = pageName;

                        if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                            message += "|" + ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).Name.Trim();

                        message += "|" + date.ToString("dd.MM.yyyy HH:mm:ss");

                        message += "|" + ((int)date.Subtract(StartLoadTime).TotalMilliseconds).ToString();

                        Message = message;
                        LoadTime = date;
                    }
                    else if (Message != null && !Message.Trim().Equals(""))
                    {
                        Message += "|" + ((int)date.Subtract(LoadTime).TotalMilliseconds).ToString();

                        log.writeLog(Message);
                        StartLoadTime = new DateTime();
                        LoadTime = new DateTime();
                        Message = "";
                    }
                }
            }
            catch { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    rbSr.Attributes.Add("onclick", "return check('rbSr', 'rbEn');");
                    rbEn.Attributes.Add("onclick", "return check('rbEn', 'rbSr');");

                    setLanguage();
                    
                    if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO)
                    {
                        tbPassword.Text = ((ApplUserTO)Session[Constants.sessionLogInUser]).Password;
                        tbConfirmPassword.Text = ((ApplUserTO)Session[Constants.sessionLogInUser]).Password;

                        if (((ApplUserTO)Session[Constants.sessionLogInUser]).LangCode.Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                        {
                            rbSr.Checked = true;
                            rbEn.Checked = false;
                        }
                        else
                        {
                            rbSr.Checked = false;
                            rbEn.Checked = true;
                        }
                    }

                    lblError.Text = "";                    
                    tbPassword.Focus();
                    writeLog(DateTime.Now, false);
                }                
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ChangePassword.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ChangePassword.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWeb.Resource", typeof(ChangePassword).Assembly);

                lblChangePassword.Text = rm.GetString("ChangePassword", culture);
                lblConfirmPassword.Text = rm.GetString("lblConfirmPassword", culture);
                lblPassword.Text = rm.GetString("lblNewPassword", culture);
                lblLangCode.Text = rm.GetString("lblLangCode", culture);
                if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO)
                    lblLoggedInUser.Text = ((ApplUserTO)Session[Constants.sessionLogInUser]).Name.ToString().Trim();

                rbSr.Text = rm.GetString("sr", culture);
                rbEn.Text = rm.GetString("en", culture);

                btnOK.Text = rm.GetString("btnOK", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWeb.Resource", typeof(ChangePassword).Assembly);

                lblError.Text = "";

                if (tbPassword.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noPassword", culture);
                    tbPassword.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (tbConfirmPassword.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("confirmPassword", culture);
                    tbConfirmPassword.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (!tbPassword.Text.Trim().Equals(tbConfirmPassword.Text.Trim()))
                {
                    lblError.Text = rm.GetString("wrongConfirmPassword", culture);
                    tbConfirmPassword.Text = "";
                    tbConfirmPassword.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }
                                
                if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO)
                {
                    ApplUser user = new ApplUser(Session[Constants.sessionConnection]);

                    user.UserTO = (ApplUserTO)Session[Constants.sessionLogInUser];
                    user.UserTO.Password = tbPassword.Text.Trim();

                    if (rbEn.Checked)
                        user.UserTO.LangCode = Constants.Lang_en;
                    else
                        user.UserTO.LangCode = Constants.Lang_sr;
                    
                    if (user.Update())
                    {
                        // if password is changed, go to menu page
                        Response.Redirect("/ACTAWeb/Default.aspx", false);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("passwordNotChanged", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }

                    writeLog(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ChangePassword.btnOK_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ChangePassword.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                // do not change password and go to manu page
                Response.Redirect("/ACTAWeb/Default.aspx", false);

                Message = "";
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ChangePassword.btnCancel_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ChangePassword.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }        
    }
}
