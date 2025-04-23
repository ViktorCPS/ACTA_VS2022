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
using System.IO;

using Common;
using Util;
using TransferObjects;
using CommonWeb;

namespace ACTAWebUI
{
    public partial class DefaultPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //lbtnExit.Attributes.Add("onclick", "return LogoutByButton();");
                    //logo.Attributes.Add("onclick", "return ADAMTest();");
                    
                    setLanguage();

                    populateCategories();

                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                    {
                        cbCategory.SelectedValue = ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID.ToString();

                        bool isDefaultCategory = isDefaultUserCategory(((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID);

                        chbDefault.Checked = isDefaultCategory;
                        chbDefault.Enabled = !isDefaultCategory;
                        
                        tabAreaIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TabPage.aspx";
                    }

                    // check if there is some information
                    CheckInfo();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in DefaultPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/DefaultPage.aspx" + "&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void LogOut(object sender, EventArgs e)
        {
            try
            {
                // sign out
                //if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO)
                //{
                //    string logoutMessage = ((ApplUserTO)Session[Constants.sessionLogInUser]).UserID.Trim() + "|" + ((ApplUserTO)Session[Constants.sessionLogInUser]).Name.Trim()
                //        + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + Constants.LoginType.LOG_OFF.ToString() + "|" + Constants.LoginStatus.SUCCESSFULL.ToString();

                //    string ip = "";
                //    string clientMachineName = "";
                //    // get client ip and host name
                //    try
                //    {
                //        ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                //        if (string.IsNullOrEmpty(ip))
                //        {
                //            ip = Request.ServerVariables["REMOTE_ADDR"];
                //            if (!string.IsNullOrEmpty(ip))
                //            {
                //                clientMachineName = (System.Net.Dns.GetHostEntry(ip).HostName);
                //                if (string.IsNullOrEmpty(clientMachineName))
                //                    clientMachineName = "";
                //            }
                //            else
                //                ip = "";
                //        }
                //    }
                //    catch
                //    {
                //        ip = "";
                //        clientMachineName = "";
                //    }

                //    Common.Misc.writeLoginData(logoutMessage + "|" + ip + "|" + clientMachineName);
                //}

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

                //Session.Abandon();
                //FormsAuthentication.SignOut();                
                //Response.Redirect("/ACTAWeb/Login.aspx", false);
                CommonWeb.Misc.LogOut(Session[Constants.sessionLogInUserLog], Session[Constants.sessionLogInUser], Session[Constants.sessionConnection], Session, Response);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in DefaultPage.LogOut(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/DefaultPage.aspx" + "&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void CheckInfoFile()
        {
            try
            {
                // just for WCSelfService for now
                //if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                //    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WC)
                //{
                    string infoFileName = "";

                    if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO
                        && ((ApplUserTO)Session[Constants.sessionLogInUser]).LangCode.Trim().ToUpper().Equals(Constants.Lang_en.Trim().ToUpper()))
                        infoFileName = Constants.infoENName;
                    else
                        infoFileName = Constants.infoSRName;

                    string infoPath = Constants.infoFilePath + infoFileName;

                    if (File.Exists(infoPath))
                    {
                        FileStream str = File.Open(infoPath, FileMode.Open);
                        StreamReader reader = new StreamReader(str, System.Text.Encoding.Unicode);
                        
                        // read file lines
                        string message = "";
                        string line = "";

                        while (line != null)
                        {
                            if (!line.Trim().Equals(""))
                                message += line + " ";

                            line = reader.ReadLine();
                        }

                        reader.Close();
                        str.Dispose();
                        str.Close();

                        if (!message.Trim().Equals(""))
                        {
                            Session[Constants.sessionInfoMessage] = message.Trim();
                            string wOptions = "dialogWidth:800px; dialogHeight:300px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                            ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/CommonWeb/MessagesPages/Info.aspx', window, '" + wOptions.Trim() + "');", true);
                        }
                    }
                //}
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in DefaultPage.CheckInfo(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/DefaultPage.aspx" + "&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void CheckInfo()
        {
            try
            {
                // get login user company and role
                int role = -1;
                int company = -1;

                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                    role = ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID;

                if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                {
                    List<EmployeeAsco4TO> ascoList = new EmployeeAsco4(Session[Constants.sessionConnection]).Search(((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID.ToString().Trim());

                    if (ascoList.Count > 0)
                        company = ascoList[0].IntegerValue4;
                }

                if (role == -1 || company == -1)
                    return;

                string message = "";
                bool altLang = !(Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO
                    && ((ApplUserTO)Session[Constants.sessionLogInUser]).LangCode.Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()));

                List<SystemMessageTO> msgList = new SystemMessage().Search(DateTime.Now, DateTime.Now, company.ToString().Trim(), role);

                if (msgList.Count <= 0)
                    return;

                if (altLang)
                    message = msgList[0].MessageEN.Trim();
                else
                    message = msgList[0].MessageSR.Trim();
                
                if (!message.Trim().Equals(""))
                {
                    Session[Constants.sessionInfoMessage] = message.Trim();
                    string wOptions = "dialogWidth:800px; dialogHeight:300px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                    ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/CommonWeb/MessagesPages/Info.aspx', window, '" + wOptions.Trim() + "');", true);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in DefaultPage.CheckInfo(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/DefaultPage.aspx" + "&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session[Constants.sessionUserCategories] != null && Session[Constants.sessionUserCategories] is List<ApplUserCategoryTO>)
                {
                    foreach (ApplUserCategoryTO category in (List<ApplUserCategoryTO>)Session[Constants.sessionUserCategories])
                    {
                        if (category.CategoryID.ToString().Equals(cbCategory.SelectedValue))
                        {
                            CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                            ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(DefaultPage).Assembly);

                            Session[Constants.sessionLoginCategory] = category;

                            if (category.CategoryID == (int)Constants.Categories.HRSSC)
                                lblInformation.Text = rm.GetString("lblHRSSCNoLimit", culture);
                            else
                                lblInformation.Text = "";

                            ApplUserCategoryXPassType userXPt = new ApplUserCategoryXPassType(Session[Constants.sessionConnection]);
                            userXPt.UserCategoryXPassTypeTO.CategoryID = category.CategoryID;
                            List<ApplUserCategoryXPassTypeTO> passTypes = userXPt.Search();

                            List<int> userPassTypes = new List<int>();

                            foreach (ApplUserCategoryXPassTypeTO catXPt in passTypes)
                            {
                                userPassTypes.Add(catXPt.PassTypeID);
                            }

                            Session[Constants.sessionLoginCategoryPassTypes] = userPassTypes;

                            // get visible employees types
                            Dictionary<int, List<int>> visibleTypes = new Dictionary<int, List<int>>();

                            if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                                visibleTypes = new EmployeeTypeVisibility(Session[Constants.sessionConnection]).SearchCompanyVisibleTypes(((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID);

                            Session[Constants.sessionVisibleEmplTypes] = visibleTypes;

                            // change minimal changing date
                            int company = -1;
                            int emplType = -1;
                            if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                            {
                                emplType = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeTypeID;
                                company = Common.Misc.getRootWorkingUnit(((EmployeeTO)Session[Constants.sessionLogInEmployee]).WorkingUnitID, new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary());
                            }

                            Session[Constants.sessionMinChangingDate] = Common.Misc.GetMinChangingDate(company, emplType, category.CategoryID, Session[Constants.sessionConnection]);

                            break;
                        }
                    }

                    tabAreaIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/TabPage.aspx";
                }

                int categoryId = -1;
                if (!int.TryParse(cbCategory.SelectedValue, out categoryId))
                    categoryId = -1;

                bool isDefaultCategory = isDefaultUserCategory(categoryId);
                chbDefault.Checked = isDefaultCategory;
                chbDefault.Enabled = !isDefaultCategory;

                // check if there is some information
                CheckInfo();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in DefaultPage.cbCategory_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/DefaultPage.aspx" + "&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void chbDefault_OnCheckChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbDefault.Checked)
                {
                    int categoryId = -1;
                    if (!int.TryParse(cbCategory.SelectedValue, out categoryId))
                        categoryId = -1;

                    // set selected category to default
                    if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO && categoryId != -1)
                    {
                        ApplUserXApplUserCategory userXCat = new ApplUserXApplUserCategory(Session[Constants.sessionConnection]);
                        userXCat.UserXCategoryTO.CategoryID = categoryId;
                        userXCat.UserXCategoryTO.UserID = ((ApplUserTO)Session[Constants.sessionLogInUser]).UserID;

                        userXCat.SetDefaultCategory();
                    }

                    chbDefault.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in DefaultPage.chbDefault_OnCheckChanged(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/DefaultPage.aspx" + "&Header=" + Constants.trueValue.Trim() + "&Exit=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(DefaultPage).Assembly);

                lbtnExit.Text = rm.GetString("lbtnExit", culture);
                chbDefault.Text = rm.GetString("chbDefaultCategory", culture);                
                if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO)
                    lblLoggedInUser.Text = ((ApplUserTO)Session[Constants.sessionLogInUser]).Name.ToString().Trim();
                else
                    lblLoggedInUser.Text = "";

                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                    lblInformation.Text = rm.GetString("lblHRSSCNoLimit", culture);
                else
                    lblInformation.Text = "";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateCategories()
        {
            try
            {
                if (Session[Constants.sessionUserCategories] != null && Session[Constants.sessionUserCategories] is List<ApplUserCategoryTO>)                                        
                {
                    cbCategory.DataSource = (List<ApplUserCategoryTO>)Session[Constants.sessionUserCategories];
                    cbCategory.DataTextField = "Name";
                    cbCategory.DataValueField = "CategoryID";

                    cbCategory.DataBind();
                }
                else
                    cbCategory.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isDefaultUserCategory(int categoryID)
        {
            try
            {
                bool isDefault = false;

                if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO && categoryID != -1)
                {
                    ApplUserXApplUserCategory userXCat = new ApplUserXApplUserCategory(Session[Constants.sessionConnection]);
                    userXCat.UserXCategoryTO.CategoryID = categoryID;
                    userXCat.UserXCategoryTO.UserID = ((ApplUserTO)Session[Constants.sessionLogInUser]).UserID;
                    userXCat.UserXCategoryTO.DefaultCategory = Constants.categoryDefault;

                    List<ApplUserXApplUserCategoryTO> categoryList = userXCat.Search();

                    if (categoryList.Count == 1)
                        isDefault = true;
                }

                return isDefault;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
