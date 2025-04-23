using System;
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
using System.ComponentModel;
using System.Drawing;
using System.Web.SessionState;
using System.Security.Principal;
using System.Globalization;
using System.Resources;
using System.IO;
using System.DirectoryServices.AccountManagement;

using Common;
using Util;
using TransferObjects;

namespace ACTAWeb
{
    public partial class Login : System.Web.UI.Page
    {
        const int correctUserPassword = 0;
        const int noUserName = 1;
        const int noPassword = 2;
        const int incorrectUser = 3;
        const int incorrectPassword = 4;
        const int inactiveUser = 5;
        const int ADAMFailed = 6;
        const int noTag = 7;
        const int unknownTag = 8;
        const int inactiveTag = 9;
                
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // close connection if exists
                    if (Session[Constants.sessionConnection] != null)
                    {
                        DBConnectionManager.Instance.CloseDBConnection(Session[Constants.sessionConnection]);
                    }

                    // clear session if forms authentication timeout expired, not session timeout
                    Session.Clear();

                    LogOut();

                    CultureInfo culture = CommonWeb.Misc.getCulture(null);
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWeb.Resource", typeof(Login).Assembly);

                    setLanguage(culture, rm, false);

                    if (Session[Constants.sessionConnection] == null)
                    {
                        Object connection = DBConnectionManager.Instance.MakeNewDBConnection();
                        Session[Constants.sessionConnection] = connection;
                    }

                    if (Session[Constants.sessionConnection] == null)
                    {
                        rbTM.Visible = rbFiat.Visible = lblFiatLoginSR.Visible = lblFiatLoginEN.Visible = false;
                        // if there is no database connection do not allow starting application (logging in)
                        lblError.Text = rm.GetString("noDBConn", culture);
                        btnOK.Enabled = false;
                        btnOK.CssClass = "contentBtnDisabled";
                        btnReadTag.Enabled = false;
                        btnReadTag.CssClass = "contentBtnDisabled";
                    }
                    else
                    {
                        if (CheckAppAvailable())
                        {
                            string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                            int cost = 0;
                            bool costum = int.TryParse(costumer, out cost);
                            bool isFiat = (cost == (int)Constants.Customers.FIAT);

                            if (isFiat)
                            {
                                setLanguage(culture, rm, isFiat);
                                rbTM.Checked = false;
                                rbFiat.Checked = true;
                                rbTM.Visible = rbFiat.Visible = false;
                                lblFiatLoginSR.Visible = lblFiatLoginEN.Visible = true;
                                chbChangePassword.Visible = false;
                            }
                            else
                            {
                                rbTM.Checked = true;
                                rbFiat.Checked = false;
                                rbTM.Visible = rbFiat.Visible = lblFiatLoginSR.Visible = lblFiatLoginEN.Visible = false;
                                chbChangePassword.Visible = true;
                            }

                            if (Request.QueryString["sessionExpired"] != null && Request.QueryString["sessionExpired"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                                lblError.Text = rm.GetString("sessionExpired", culture);
                            else
                                lblError.Text = "";

                            tbUserName.Focus();

                            // set data provider in Session
                            string connectionString = ConfigurationManager.AppSettings["connectionString"];

                            // decrypt connection string
                            byte[] buffer = Convert.FromBase64String(connectionString);
                            connectionString = Util.Misc.decrypt(buffer);

                            int startIndex = connectionString.IndexOf(Constants.dataProvider.Trim());
                            if (startIndex >= 0)
                            {
                                int endIndex = connectionString.IndexOf(";", startIndex);

                                if (endIndex >= startIndex)
                                {
                                    // take data provider value
                                    // data provider part of the connection string is like "data provider=mysql;" and we need "mysql"
                                    // or string is like "data provider=sqlserver;" and we need "sqlserver"                                
                                    string dataProvider = connectionString.Substring(startIndex + Constants.dataProvider.Trim().Length, endIndex - startIndex - Constants.dataProvider.Trim().Length);
                                    Session[Constants.sessionDataProvider] = dataProvider;
                                }
                            }
                        }
                    }

                    // set visibility depending if app is started from read tag ip address or not
                    string ip = "";

                    // get client ip and host name
                    try
                    {
                        ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        if (string.IsNullOrEmpty(ip))
                        {
                            ip = Request.ServerVariables["REMOTE_ADDR"];
                            if (string.IsNullOrEmpty(ip))
                                ip = "";
                        }
                    }
                    catch
                    {
                        ip = "";
                    }
                    
                    if (ip.Trim() != "" && Constants.ReadTagIPList.Contains(ip.Trim()))
                    {
                        lblUserName.Visible = false;
                        tbUserName.Visible = false;
                        lblPassword.Visible = false;
                        tbPassword.Visible = false;
                        chbChangePassword.Visible = false;
                        lblFiatLoginEN.Visible = lblFiatLoginSR.Visible = false;
                        rbFiat.Visible = rbTM.Visible = false;
                        btnOK.Visible = false;
                        btnReadTag.Visible = true;
                    }
                    else
                        btnReadTag.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        
        private void LogOut()
        {
            try
            {
                // sign out                
                FormsAuthentication.SignOut();                
            }
            catch { }
        }
        
        protected void btnOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(null);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWeb.Resource", typeof(Login).Assembly);

                lblError.Text = "";

                // 25.01.2013. Sanja - SYS always log with TM password, not through ADAM
                bool checkAdam = rbFiat.Checked;
                if (tbUserName.Text.Trim().Equals(Constants.sysUser.Trim()))
                    checkAdam = false;

                int checkMsg = CheckPassword(tbUserName.Text.Trim(), tbPassword.Text.Trim(), Session, Request, chbChangePassword.Checked, checkAdam, false);
                                
                if (checkMsg != 0)
                {
                    switch (checkMsg)
                    {
                        case noUserName:
                            lblError.Text = rm.GetString("noUserName", culture);
                            tbUserName.Focus();
                            break;
                        case noPassword:
                            lblError.Text = rm.GetString("noPassword", culture);
                            tbPassword.Focus();
                            break;
                        case incorrectUser:
                            lblError.Text = rm.GetString("unknownUserName", culture);                            
                            tbUserName.Focus();
                            break;
                        case incorrectPassword:
                            lblError.Text = rm.GetString("incorrectPassword", culture);
                            tbPassword.Focus();
                            break;
                        case inactiveUser:
                            lblError.Text = rm.GetString("inactiveUser", culture);
                            tbPassword.Text = "";
                            tbUserName.Focus();
                            break;
                        case ADAMFailed:
                            lblFiatLoginSR.Text = rm.GetString("FiatLoginInfoSR", culture);
                            lblFiatLoginEN.Text = rm.GetString("FiatLoginInfoEN", culture);
                            rbFiat.Visible = rbTM.Visible = true;
                            lblError.Text = rm.GetString("ADAMFailed", culture);
                            tbUserName.Focus();
                            break;
                    }
                }
                else if (chbChangePassword.Checked)
                {
                    try
                    {
                        // redirect to page for changing password
                        Response.Redirect("/ACTAWeb/ChangePassword.aspx", false);
                    }
                    catch (System.Threading.ThreadAbortException) { }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnReadTag_Click(object sender, System.EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(null);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWeb.Resource", typeof(Login).Assembly);

                lblError.Text = "";

                // get ip client
                string ip = "";

                // get client ip and host name
                try
                {
                    ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (string.IsNullOrEmpty(ip))
                    {
                        ip = Request.ServerVariables["REMOTE_ADDR"];
                        if (string.IsNullOrEmpty(ip))
                            ip = "";
                    }
                }
                catch
                {
                    ip = "";
                }

                // delete all tags previously shown for this ip
                ApplUserLoginRFID rfid = new ApplUserLoginRFID(Session[Constants.sessionConnection]);
                rfid.Delete(ip.Trim(), DateTime.Now.AddSeconds(-Constants.rfidLoginTimeSec), true);

                // get tag shown
                string tagID = rfid.SearchLoginRFID(ip.Trim()).TagID.Trim();

                int checkMsg = CheckPassword(tagID, "", Session, Request, false, false, true);

                if (checkMsg != 0)
                {
                    switch (checkMsg)
                    {
                        case noTag:
                            lblError.Text = rm.GetString("noTag", culture);
                            btnReadTag.Focus();                            
                            break;
                        case unknownTag:
                            lblError.Text = rm.GetString("unknownTag", culture);
                            btnReadTag.Focus();
                            break;
                        case inactiveTag:
                            lblError.Text = rm.GetString("inactiveTag", culture);
                            btnReadTag.Focus();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void setLanguage(CultureInfo culture, ResourceManager rm, bool isFiat)
        {
            try
            {
                if (isFiat)
                {
                    lblLogin.Text = rm.GetString("FiatLogin", culture);
                    lblFiatLoginSR.Text = rm.GetString("FiatInitialLoginSR", culture);
                    lblFiatLoginEN.Text = rm.GetString("FiatInitialLoginEN", culture);
                    lblInfoSR.Text = rm.GetString("lblLoginInfoSR", culture);
                    lblInfoENG.Text = rm.GetString("lblLoginInfoENG", culture);
                    lblPassword.Text = rm.GetString("lblFiatPassword", culture);
                }
                else
                {
                    lblLogin.Text = rm.GetString("Login", culture);
                    lblPassword.Text = rm.GetString("lblPassword", culture);
                }
                lblUserName.Text = rm.GetString("lblUserName", culture);                

                rbFiat.Text = "FIAT " + rm.GetString("Login", culture).ToLower();
                rbTM.Text = "TM " + rm.GetString("Login", culture).ToLower();
                
                btnOK.Text = rm.GetString("btnOK", culture);
                btnReadTag.Text = rm.GetString("btnReadTag", culture);
                
                chbChangePassword.Text = rm.GetString("chbChangePassword", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static int CheckPassword(IPrincipal user, HttpSessionState session, HttpRequest req)
        {
            int checkMsg = -1;
            try
            {
                string[] userInfo = user.Identity.Name.Split('|');
                if (userInfo.Length == 2)
                {
                    string userID = userInfo[0];
                    string password = userInfo[1];
                    checkMsg = CheckPassword(userID, password, session, req, true, false, false, false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return checkMsg;
        }

        // 0 if userdID/password are correct, greater than 0 if there is error. 1 - no user name, 2 - no password, 3 - incorrect user, 4 - incorrect password, 5 - inactive user, 6 - ADAM failed
        // 7 - no tag 8 - unknown tag, 9 - inactive tag
        private static int CheckPassword(string userID, string password, HttpSessionState session, HttpRequest req, bool fromCookie, bool changePassword, bool checkADAM, bool tagLogin)
        {
            try
            {
                if (userID.Trim().Equals(""))
                {
                    if (!tagLogin)
                        return noUserName;
                    else
                        return noTag;
                }

                if (!tagLogin && password.Trim().Equals(""))
                {
                    return noPassword;
                }

                string loginMessage = userID.Trim();
                string ip = "";
                string clientMachineName = "";

                // get client ip and host name
                try
                {
                    ip = req.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (string.IsNullOrEmpty(ip))
                    {
                        ip = req.ServerVariables["REMOTE_ADDR"];
                        if (!string.IsNullOrEmpty(ip))
                        {
                            clientMachineName = (System.Net.Dns.GetHostEntry(ip).HostName);
                            if (string.IsNullOrEmpty(clientMachineName))
                                clientMachineName = "";
                        }
                        else
                            ip = "";
                    }
                }
                catch
                {
                    ip = "";
                    clientMachineName = "";
                }
                
                // create user log
                ApplUserLog log = new ApplUserLog(session[Constants.sessionConnection]);
                ApplUserLogTO logTO = new ApplUserLogTO();
                logTO.UserID = userID;
                logTO.Host = clientMachineName;
                logTO.LoginChanel = Constants.UserLoginChanel.WEB.ToString();

                if (checkADAM)
                {
                    logTO.LoginType = Constants.UserLoginType.FIAT.ToString();

                    if (!UsrAuth(userID.Trim(), password.Trim()))
                    {
                        loginMessage += "|" + userID.Trim() + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + Constants.LoginType.LOG_ON.ToString() + "|" + Constants.LoginStatus.ADAM_FAILED.ToString();
                        Common.Misc.writeLoginData(loginMessage + "|" + ip + "|" + clientMachineName);
                        logTO.LoginStatus = Constants.UserLoginStatus.FAILED.ToString();
                        log.UserLogTO = logTO;
                        log.Insert();
                        return ADAMFailed;
                    }
                }
                else
                    logTO.LoginType = Constants.UserLoginType.TM.ToString();

                if (tagLogin)
                {
                    // check if tag exists and is active                    
                    TagTO loginTag = null;
                    
                    uint tagID = 0;

                    if (uint.TryParse(userID.Trim(), out tagID))
                    {
                        Tag tag = new Tag(session[Constants.sessionConnection]);
                        tag.TgTO.TagID = tagID;

                        List<TagTO> tagList = tag.Search();

                        foreach (TagTO tagTO in tagList)
                        {
                            if (tagTO.TagID == tagID)
                            {
                                loginTag = tagTO;
                                break;
                            }
                        }
                    }

                    if (loginTag == null)
                    {
                        loginMessage += "|" + "N/A" + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + Constants.LoginType.LOG_ON.ToString() + "|" + Constants.LoginStatus.FAILED_UNKNOWN_TAG.ToString();
                        Common.Misc.writeLoginData(loginMessage + "|" + ip + "|" + clientMachineName);
                        logTO.LoginStatus = Constants.UserLoginStatus.FAILED.ToString();
                        log.UserLogTO = logTO;
                        log.Insert();
                        return unknownTag;
                    }

                    // get employee and employee asco4
                    EmployeeTO tagEmpl = new Employee(session[Constants.sessionConnection]).Find(loginTag.OwnerID.ToString().Trim());

                    if (loginTag.Status.Trim().ToUpper() != Constants.statusActive.Trim().ToUpper() || tagEmpl.Status.Trim().ToUpper() != Constants.statusActive.Trim().ToUpper())
                    {
                        loginMessage += "|" + loginTag.TagID.ToString().Trim() + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + Constants.LoginType.LOG_ON.ToString() + "|" + Constants.LoginStatus.FAILED_INACTIVE_TAG.ToString();
                        Common.Misc.writeLoginData(loginMessage + "|" + ip + "|" + clientMachineName);
                        logTO.LoginStatus = Constants.UserLoginStatus.FAILED.ToString();
                        log.UserLogTO = logTO;
                        log.Insert();
                        return inactiveTag;
                    }

                    // change user login log
                    logTO.UserID = loginTag.OwnerID.ToString().Trim();

                    // get employee asco4
                    Dictionary<int, EmployeeAsco4TO> tagAsco4Dict = new EmployeeAsco4(session[Constants.sessionConnection]).SearchDictionary(loginTag.OwnerID.ToString().Trim());
                    EmployeeAsco4TO tagAsco4 = new EmployeeAsco4TO();
                    if (tagAsco4Dict.ContainsKey(loginTag.OwnerID))
                        tagAsco4 = tagAsco4Dict[loginTag.OwnerID];

                    // create login user
                    ApplUserTO tagUser = new ApplUserTO();
                    tagUser.UserID = loginTag.OwnerID.ToString().Trim();
                    tagUser.Status = Constants.statusActive;
                    tagUser.LangCode = Constants.Lang_sr;
                    tagUser.Name = tagEmpl.FirstAndLastName;

                    // correct tag and employee, put user data in session
                    loginMessage += "|" + tagUser.Name.Trim() + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + Constants.LoginType.LOG_ON.ToString() + "|";
                    if (checkADAM)
                        loginMessage += Constants.LoginStatus.SUCCESSFULL_ADAM.ToString();
                    else
                        loginMessage += Constants.LoginStatus.SUCCESSFULL_TM.ToString();
                    Common.Misc.writeLoginData(loginMessage + "|" + ip + "|" + clientMachineName);
                    logTO.LoginStatus = Constants.UserLoginStatus.SUCCESSFUL.ToString();
                    log.UserLogTO = logTO;
                    logTO = log.Insert();
                    session[Constants.sessionLogInUser] = tagUser;
                    session[Constants.sessionLogInUserLog] = logTO;

                    int company = tagAsco4.IntegerValue4;
                    int emplType = tagEmpl.EmployeeTypeID;
                    int category = (int)Constants.Categories.BCSelfService;

                    session[Constants.sessionLogInEmployee] = tagEmpl;

                    // get login user categories
                    List<ApplUserCategoryTO> tagCategories = new List<ApplUserCategoryTO>();
                    ApplUserCategoryTO tagCategory = new ApplUserCategory(session[Constants.sessionConnection]).Find(category);
                    tagCategories.Add(tagCategory);
                    session[Constants.sessionUserCategories] = tagCategories;

                    session[Constants.sessionLoginCategory] = tagCategory;

                    // get allowed pass types for default category
                    ApplUserCategoryXPassType userXPt = new ApplUserCategoryXPassType(session[Constants.sessionConnection]);
                    userXPt.UserCategoryXPassTypeTO.CategoryID = tagCategory.CategoryID;
                    List<ApplUserCategoryXPassTypeTO> passTypes = userXPt.Search();

                    List<int> userPassTypes = new List<int>();

                    foreach (ApplUserCategoryXPassTypeTO catXPt in passTypes)
                    {
                        userPassTypes.Add(catXPt.PassTypeID);
                    }

                    session[Constants.sessionLoginCategoryPassTypes] = userPassTypes;

                    session[Constants.sessionMinChangingDate] = Common.Misc.GetMinChangingDate(company, emplType, category, session[Constants.sessionConnection]);
                }
                else
                {
                    ApplUserTO logInUser = new ApplUser(session[Constants.sessionConnection]).Find(userID.Trim());

                    if (checkADAM)
                        password = logInUser.Password;

                    if (logInUser.UserID.Trim().Equals(""))
                    {
                        loginMessage += "|" + "N/A" + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + Constants.LoginType.LOG_ON.ToString() + "|" + Constants.LoginStatus.FAILED_INCORRECT_USER.ToString();
                        Common.Misc.writeLoginData(loginMessage + "|" + ip + "|" + clientMachineName);
                        logTO.LoginStatus = Constants.UserLoginStatus.FAILED.ToString();
                        log.UserLogTO = logTO;
                        log.Insert();
                        return incorrectUser;
                    }

                    if (!logInUser.Password.Equals(password))
                    {
                        loginMessage += "|" + logInUser.Name.Trim() + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + Constants.LoginType.LOG_ON.ToString() + "|" + Constants.LoginStatus.FAILED_INCORRECT_PASSWORD.ToString();
                        Common.Misc.writeLoginData(loginMessage + "|" + ip + "|" + clientMachineName);
                        logTO.LoginStatus = Constants.UserLoginStatus.FAILED.ToString();
                        log.UserLogTO = logTO;
                        log.Insert();
                        return incorrectPassword;
                    }

                    if (!logInUser.Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper()))
                    {
                        loginMessage += "|" + logInUser.Name.Trim() + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + Constants.LoginType.LOG_ON.ToString() + "|" + Constants.LoginStatus.FAILED_INACTIVE_USER.ToString();
                        Common.Misc.writeLoginData(loginMessage + "|" + ip + "|" + clientMachineName);
                        logTO.LoginStatus = Constants.UserLoginStatus.FAILED.ToString();
                        log.UserLogTO = logTO;
                        log.Insert();
                        return inactiveUser;
                    }

                    // correct user name and password, put user data in session
                    loginMessage += "|" + logInUser.Name.Trim() + "|" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "|" + Constants.LoginType.LOG_ON.ToString() + "|";
                    if (checkADAM)
                        loginMessage += Constants.LoginStatus.SUCCESSFULL_ADAM.ToString();
                    else
                        loginMessage += Constants.LoginStatus.SUCCESSFULL_TM.ToString();
                    Common.Misc.writeLoginData(loginMessage + "|" + ip + "|" + clientMachineName);
                    logTO.LoginStatus = Constants.UserLoginStatus.SUCCESSFUL.ToString();
                    log.UserLogTO = logTO;
                    logTO = log.Insert();
                    session[Constants.sessionLogInUser] = logInUser;
                    session[Constants.sessionLogInUserLog] = logTO;

                    int company = -1;
                    int emplType = -1;
                    int category = -1;

                    // find log in employee and put employee object in session, user_id of employee is in employees_asco4.nvarchar_value_5
                    EmployeeAsco4 emplAsco = new EmployeeAsco4(session[Constants.sessionConnection]);
                    emplAsco.EmplAsco4TO.NVarcharValue5 = userID;

                    List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();

                    if (emplAscoList.Count == 1)
                    {
                        company = emplAscoList[0].IntegerValue4;

                        EmployeeTO empl = new Employee(session[Constants.sessionConnection]).Find(emplAscoList[0].EmployeeID.ToString());

                        emplType = empl.EmployeeTypeID;

                        if (empl.EmployeeID != -1)
                            session[Constants.sessionLogInEmployee] = empl;
                    }

                    // get login user categories
                    List<ApplUserCategoryTO> categories = new ApplUserCategory(session[Constants.sessionConnection]).SearchLoginCategories(logInUser.UserID, false);
                    session[Constants.sessionUserCategories] = categories;

                    // get default category
                    List<ApplUserCategoryTO> defaultCategories = new ApplUserCategory(session[Constants.sessionConnection]).SearchLoginCategories(logInUser.UserID, true);
                    if (defaultCategories.Count > 0)
                    {
                        session[Constants.sessionLoginCategory] = defaultCategories[0];

                        category = defaultCategories[0].CategoryID;

                        // get allowed pass types for default category
                        ApplUserCategoryXPassType userXPt = new ApplUserCategoryXPassType(session[Constants.sessionConnection]);
                        userXPt.UserCategoryXPassTypeTO.CategoryID = defaultCategories[0].CategoryID;
                        List<ApplUserCategoryXPassTypeTO> passTypes = userXPt.Search();

                        List<int> userPassTypes = new List<int>();

                        foreach (ApplUserCategoryXPassTypeTO catXPt in passTypes)
                        {
                            userPassTypes.Add(catXPt.PassTypeID);
                        }

                        session[Constants.sessionLoginCategoryPassTypes] = userPassTypes;
                    }

                    // get allowed wunits
                    ApplUsersXWU userXwu = new ApplUsersXWU(session[Constants.sessionConnection]);
                    userXwu.AuXWUnitTO.UserID = logInUser.UserID;
                    List<ApplUsersXWUTO> wUnits = userXwu.Search();

                    List<int> userWUnits = new List<int>();
                    foreach (ApplUsersXWUTO wu in wUnits)
                    {
                        userWUnits.Add(wu.WorkingUnitID);
                    }

                    session[Constants.sessionLoginCategoryWUnits] = userWUnits;

                    // get allowed ounits
                    List<int> userOUnits = new List<int>();
                    ApplUserXOrgUnit userXou = new ApplUserXOrgUnit(session[Constants.sessionConnection]);
                    userXou.AuXOUnitTO.UserID = logInUser.UserID;
                    List<ApplUserXOrgUnitTO> orgList = userXou.Search();
                    foreach (ApplUserXOrgUnitTO org in orgList)
                    {
                        userOUnits.Add(org.OrgUnitID);
                    }

                    session[Constants.sessionLoginCategoryOUnits] = userOUnits;

                    // get visible employees types
                    Dictionary<int, List<int>> visibleTypes = new Dictionary<int, List<int>>();

                    if (session[Constants.sessionLoginCategory] != null && session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                        visibleTypes = new EmployeeTypeVisibility(session[Constants.sessionConnection]).SearchCompanyVisibleTypes(((ApplUserCategoryTO)session[Constants.sessionLoginCategory]).CategoryID);

                    session[Constants.sessionVisibleEmplTypes] = visibleTypes;

                    session[Constants.sessionMinChangingDate] = Common.Misc.GetMinChangingDate(company, emplType, category, session[Constants.sessionConnection]);
                }

                if (!fromCookie)
                {
                    // go to menu page
                    FormsAuthentication.RedirectFromLoginPage(userID + "|" + password, true);
                }

                return correctUserPassword;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckAppAvailableFile()
        {
            bool available = true;
            try
            {
                string infoFileSRName = Constants.infoAppSRName.Trim();
                string infoFileENName = Constants.infoAppENName.Trim();

                string infoSRPath = Constants.infoFilePath + infoFileSRName;
                string infoENPath = Constants.infoFilePath + infoFileENName;

                string messageSR = "";
                string messageEN = "";

                if (File.Exists(infoSRPath))
                {
                    FileStream str = File.Open(infoSRPath, FileMode.Open);
                    StreamReader reader = new StreamReader(str, System.Text.Encoding.Unicode);

                    // read file lines                    
                    string line = "";
                    
                    while (line != null)
                    {
                        if (!line.Trim().Equals(""))
                            messageSR += line + " ";

                        line = reader.ReadLine();
                    }

                    reader.Close();
                    str.Dispose();
                    str.Close();
                }

                if (File.Exists(infoENPath))
                {
                    FileStream str = File.Open(infoENPath, FileMode.Open);
                    StreamReader reader = new StreamReader(str, System.Text.Encoding.Unicode);

                    // read file lines                    
                    string line = "";

                    while (line != null)
                    {
                        if (!line.Trim().Equals(""))
                            messageEN += line + " ";

                        line = reader.ReadLine();
                    }

                    reader.Close();
                    str.Dispose();
                    str.Close();
                }

                if (!messageSR.Trim().Equals("") || !messageEN.Trim().Equals(""))
                {
                    lblInfoENG.Text = lblInfoSR.Text = "";
                    if (!messageSR.Trim().Equals(""))
                    {
                        available = false;
                        lblInfoSR.Text = messageSR.Trim();
                    }

                    if (!messageEN.Trim().Equals(""))
                    {
                        available = false;
                        lblInfoENG.Text = messageEN.Trim();
                    }
                }

                if (!available)
                {
                    rbTM.Visible = rbFiat.Visible = lblFiatLoginSR.Visible = lblFiatLoginEN.Visible = false;
                    btnOK.Enabled = false;
                    btnOK.CssClass = "contentBtnDisabled";
                }
            }
            catch (Exception ex)
            {
                try
                {
                    throw ex;
                }
                catch (System.Threading.ThreadAbortException) { }
            }

            return available;
        }

        private bool CheckAppAvailable()
        {
            bool available = true;
            try
            {
                string messageSR = "";
                string messageEN = "";

                SystemClosingEventTO closingEventTO = Common.Misc.getClosingEvent(DateTime.Now, Session[Constants.sessionConnection]);
                messageSR = closingEventTO.MessageSR.Trim();
                messageEN = closingEventTO.MessageEN.Trim();
                
                if (!messageSR.Trim().Equals("") || !messageEN.Trim().Equals(""))
                {
                    lblInfoENG.Text = lblInfoSR.Text = "";
                    if (!messageSR.Trim().Equals(""))
                    {
                        available = false;
                        lblInfoSR.Text = messageSR.Trim().Replace(Environment.NewLine, "<br/>");
                    }

                    if (!messageEN.Trim().Equals(""))
                    {
                        available = false;
                        lblInfoENG.Text = messageEN.Trim().Replace(Environment.NewLine, "<br/>");
                    }
                }

                if (!available)
                {
                    rbTM.Visible = rbFiat.Visible = lblFiatLoginSR.Visible = lblFiatLoginEN.Visible = false;
                    btnOK.Enabled = false;
                    btnOK.CssClass = "contentBtnDisabled";
                    btnReadTag.Enabled = false;
                    btnReadTag.CssClass = "contentBtnDisabled";
                }
            }
            catch (Exception ex)
            {
                try
                {
                    throw ex;
                }
                catch (System.Threading.ThreadAbortException) { }
            }

            return available;
        }

        public int CheckPassword(string userID, string password, HttpSessionState session, HttpRequest req, bool changePassword, bool checkAdam, bool tagLogin)
        {
            try
            {
                return CheckPassword(userID, password, session, req, false, changePassword, checkAdam, tagLogin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool UsrAuth(string usr, string pwd)
        {
            bool status = false;
            try
            {
                PrincipalContext ctx = new PrincipalContext(
                                          ContextType.ApplicationDirectory,
                                          "vldap.fg.local:636",
                                          "CN=fgadam,DC=fg,DC=local",
                                          "A004904", "RssPX1suJcf9");

                string connectedServer = ctx.ConnectedServer;

                status = ctx.ValidateCredentials(usr, pwd);
            }
            catch
            {
                status = false;
            }

            return status;
        }

        protected void rbTM_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(null);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWeb.Resource", typeof(Login).Assembly);

                rbFiat.Checked = !rbTM.Checked;
                if (rbFiat.Checked)
                    lblPassword.Text = rm.GetString("lblFiatPassword", culture);
                else
                    lblPassword.Text = rm.GetString("lblTMPassword", culture);
                chbChangePassword.Visible = rbTM.Checked;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void rbFiat_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(null);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWeb.Resource", typeof(Login).Assembly);

                rbTM.Checked = !rbFiat.Checked;
                if (rbFiat.Checked)
                    lblPassword.Text = rm.GetString("lblFiatPassword", culture);
                else
                    lblPassword.Text = rm.GetString("lblTMPassword", culture);
                chbChangePassword.Visible = !rbFiat.Checked;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
