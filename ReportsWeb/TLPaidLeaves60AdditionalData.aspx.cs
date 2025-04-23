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
    public partial class TLPaidLeaves60AdditionalData : System.Web.UI.Page
    {
        const string pageName = "TLPaidLeaves60AdditionalData";

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
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                
                if (!IsPostBack)
                {
                    rbStopWorking.Attributes.Add("onclick", "return checkRB('rbStopWorking', 'rbLessWorking', 'rbOther');");
                    rbLessWorking.Attributes.Add("onclick", "return checkRB('rbLessWorking', 'rbStopWorking', 'rbOther');");
                    rbOther.Attributes.Add("onclick", "return checkRB('rbOther', 'rbStopWorking', 'rbLessWorking');");

                    rbFiatStopWorking.Attributes.Add("onclick", "return check('rbFiatStopWorking', 'rbDescOther');");
                    rbDescOther.Attributes.Add("onclick", "return check('rbDescOther', 'rbFiatStopWorking');");

                    rbStopWorking.Checked = true;
                    rbFiatStopWorking.Checked = true;
                    setLanguage();
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLPaidLeaves60AdditionalData.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ReportsWeb/TLPaidLeaves60AdditionalData.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ReportsWeb.Resource", typeof(TLPaidLeaves60AdditionalData).Assembly);

                lblAddData.Text = rm.GetString("lblAddData", culture);
                lblPaidLeaveReason.Text = rm.GetString("lblPaidLeaveReason", culture);
                lblReasonDesc.Text = rm.GetString("lblReasonDesc", culture);
                
                rbFiatStopWorking.Text = rm.GetString("rbFiatStopWorking", culture);
                rbLessWorking.Text = rm.GetString("rbLessWorking", culture);                
                rbStopWorking.Text = rm.GetString("rbStopWorking", culture);
                
                btnOK.Text = rm.GetString("btnOK", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["TLPaidLeavePage.ds"] != null && Session["TLPaidLeavePage.ds"] is DataSet)
                {
                    DataSet ds = (DataSet)Session["TLPaidLeavePage.ds"];
                    if (ds.Tables.Count > 0)
                    {
                        string reason = "";
                        string description = "";

                        if (rbStopWorking.Checked)
                            reason = rbStopWorking.Text.Trim();
                        else if (rbLessWorking.Checked)
                            reason = rbLessWorking.Text.Trim();
                        else if (rbOther.Checked)
                            reason = tbReason.Text.Trim();

                        if (rbFiatStopWorking.Checked)
                            description = rbFiatStopWorking.Text.Trim();
                        else if (rbDescOther.Checked)
                            description = tbDescOther.Text.Trim();

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row.ItemArray.Length > 9)
                                row[9] = reason.ToLower();

                            if (row.ItemArray.Length > 10)
                                row[10] = description.ToLower();
                        }
                    }
                }

                string wOptions = "dialogWidth:800px; dialogHeight:600px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                        ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLPaidLeaves60ReportPage.aspx', window, '" + wOptions.Trim() + "'); window.close();", true);

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLPaidLeaves60AdditionalData.btnOK_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ReportsWeb/TLPaidLeaves60AdditionalData.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
