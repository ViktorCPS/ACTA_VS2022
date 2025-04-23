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
using System.Resources;
using System.Globalization;
using System.Drawing;

using Common;
using TransferObjects;
using Util;
using ReportsWeb;

namespace ACTAWebUI
{
    public partial class TLLoansPage : System.Web.UI.Page
    {
        const string sessionLoanList = "TLLoansPage.LoanList";

        const string pageName = "TLLoansPage";

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

        DateTime fromDate = DateTime.Now;
        DateTime toDate = DateTime.Now;
        protected void ShowCalendarFrom(object sender, EventArgs e)
        {
            calendarFrom.Visible = true;
            btnFrom.Visible = false;
        }
        protected void ShowCalendarTo(object sender, EventArgs e)
        {
            calendarTo.Visible = true;
            btnTo.Visible = false;
        }
        protected void DataFromChange(object sender, EventArgs e)
        {
            fromDate = calendarFrom.SelectedDate;
            tbFromDate.Text = fromDate.ToString("dd.MM.yyyy.");
            calendarFrom.Visible = false;
            btnFrom.Visible = true;
        }
        protected void DataToChange(object sender, EventArgs e)
        {
            toDate = calendarTo.SelectedDate;
            tbToDate.Text = toDate.ToString("dd.MM.yyyy.");
            calendarTo.Visible = false;
            btnTo.Visible = true;
        }






        List<string> WUName = new List<string>();
        List<string> OUName = new List<string>();
        Dictionary<int, List<WorkingUnitTO>> wuDic = new Dictionary<int, List<WorkingUnitTO>>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //dropDownWU.Items.Clear();
                // if user do not want to reload selected filter and get results for that filter, parameter reloadState should be passes through url and set to false
                if (!IsPostBack)
                {
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'true');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'true');");
                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    btnTempWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnTempWUTree', 'true');");
                    cbSelectAllEmpolyees.Attributes.Add("onclick", "return selectListItems('cbSelectAllEmpolyees', 'lboxEmployees');");
                    tbEmployee.Attributes.Add("onKeyUp", "return selectItem('tbEmployee', 'lboxEmployees');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnReport.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnSave.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    

                    btnWUTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnWUTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnOrgTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnOrgTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnTempWUTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnTempWUTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnFromDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnPrevDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnPrevDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnNextDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnNextDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    cbSelectAllEmpolyees.Visible = false;

                    
                    //VIKTOR 20.11.2023 ubacio deo koda koji proziva sve wu i ou koje korisnik sistema vidi
                    try
                    {
                        WorkingUnit wUnit = new WorkingUnit(Session[Constants.sessionConnection]);
                        string wUnits = "";


                        if (Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>)
                        {
                            foreach (int wuID in (List<int>)Session[Constants.sessionLoginCategoryWUnits])
                            {
                                wUnits += wuID.ToString().Trim() + ",";
                            }

                            if (wUnits.Length > 0)
                                wUnits = wUnits.Substring(0, wUnits.Length - 1);

                            List<WorkingUnitTO> wuList = wUnit.Search(wUnits);
                            

                            foreach (WorkingUnitTO wu in wuList)
                            {
                                wuDic.Add(wu.WorkingUnitID, new List<WorkingUnitTO>());
                            }

                            foreach (WorkingUnitTO wu in wuList)
                            {
                                if (wu.ParentWorkingUID != wu.WorkingUnitID && wuDic.ContainsKey(wu.ParentWorkingUID))
                                    wuDic[wu.ParentWorkingUID].Add(wu);
                            }
                            foreach (var item in wuList)
                            {
                                bool sw=false;
                                for (int i = 0; i < wuList.Count; i++) {
                                    if (item.WorkingUnitID != wuList[i].WorkingUnitID && item.WorkingUnitID == wuList[i].ParentWorkingUID)
                                    {
                                        sw = true;
                                        break;
                                    }
                                }
                                if (!sw)
                                {
                                    WUName.Add(item.Name);
                                    //dropDownWU.Items.Add(item.Name);
                                }
                            }
                        }
                        OrganizationalUnit oUnit = new OrganizationalUnit(Session[Constants.sessionConnection]);
                        string oUnits = "";

                        if (Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>)
                        {
                            foreach (int ouID in (List<int>)Session[Constants.sessionLoginCategoryOUnits])
                            {
                                oUnits += ouID.ToString().Trim() + ",";
                            }

                            if (oUnits.Length > 0)
                                oUnits = oUnits.Substring(0, oUnits.Length - 1);

                            List<OrganizationalUnitTO> ouList = oUnit.Search(oUnits);
                            Dictionary<int, List<OrganizationalUnitTO>> ouDic = new Dictionary<int, List<OrganizationalUnitTO>>();

                            foreach (OrganizationalUnitTO ou in ouList)
                            {
                                ouDic.Add(ou.OrgUnitID, new List<OrganizationalUnitTO>());
                            }

                            foreach (OrganizationalUnitTO ou in ouList)
                            {
                                if (ou.ParentOrgUnitID != ou.OrgUnitID && ouDic.ContainsKey(ou.ParentOrgUnitID))
                                    ouDic[ou.ParentOrgUnitID].Add(ou);
                            }
                            foreach (var item in ouList)
                            {
                                bool sw = false;
                                for (int i = 0; i < ouList.Count; i++)
                                {
                                    if (item.OrgUnitID != ouList[i].OrgUnitID && item.OrgUnitID == ouList[i].ParentOrgUnitID) {
                                        sw = true;
                                        break;
                                    }
                                }
                                if (!sw)
                                {
                                    OUName.Add(item.Name);
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    foreach (var item in WUName)
                    {
                        dropDownWU.Items.Add(item);
                    }




                    if (Session[Constants.sessionFromDate] != null && Session[Constants.sessionFromDate] is DateTime)
                        tbFromDate.Text = ((DateTime)Session[Constants.sessionFromDate]).ToString(Constants.dateFormat.Trim());
                    else                    
                        tbFromDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    if (Session[Constants.sessionToDate] != null && Session[Constants.sessionToDate] is DateTime)
                        tbToDate.Text = ((DateTime)Session[Constants.sessionToDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbToDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    // populate default working unit and organizational unit info
                    // get responsibility working unit (employee_asco4.integer_value_2)
                    int defaultWUID = -1;
                    int defaultOUID = -1;
                    if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    {
                        EmployeeAsco4 emplAsco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                        emplAsco.EmplAsco4TO.EmployeeID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

                        List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();

                        if (emplAscoList.Count == 1)
                        {
                            defaultWUID = emplAscoList[0].IntegerValue2;
                            defaultOUID = emplAscoList[0].IntegerValue3;
                        }
                    }

                    if (defaultWUID != -1 && Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>
                        && ((List<int>)Session[Constants.sessionLoginCategoryWUnits]).Contains(defaultWUID))
                    {
                        populateWU(defaultWUID);
                    }
                    if (defaultOUID != -1 && Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>
                       && ((List<int>)Session[Constants.sessionLoginCategoryOUnits]).Contains(defaultOUID))
                    {
                        populateOU(defaultOUID);
                    }
                    
                    if (Session[Constants.sessionWU] == null && Session[Constants.sessionOU] == null)
                    {
                        if (defaultWUID == -1 && defaultOUID != -1)
                        {
                            
                            Menu1.Items[1].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[1]);
                            Session[Constants.sessionOU] = defaultOUID;
                        }
                        else
                        {
                            
                            Menu1.Items[0].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[0]);
                            Session[Constants.sessionWU] = defaultWUID;
                        }
                    }
                    else
                    {
                        if (Session[Constants.sessionOU] != null && Session[Constants.sessionOU] is int)
                        {
                            //foreach (var item in OUName)
                            //{
                            //    dropDownWU.Items.Add(item);
                            //}
                            populateOU((int)Session[Constants.sessionOU]);

                            Menu1.Items[1].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[1]);
                        }
                        else if (Session[Constants.sessionWU] != null && Session[Constants.sessionWU] is int)
                        {

                            //foreach (var item in WUName)
                            //{
                            //    dropDownWU.Items.Add(item);
                            //}
                            populateWU((int)Session[Constants.sessionWU]);

                            Menu1.Items[0].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[0]);
                        }
                    }

                    for (int i = 0; i < Menu1.Items.Count; i++)
                    {

                        CommonWeb.Misc.setMenuImage(Menu1, i, Menu1.Items[i].Selected);
                        CommonWeb.Misc.setMenuSeparator(Menu1, i, Menu1.Items[i].Selected);
                    }

                    setLanguage();
                                        
                    rbShow.Checked = true;
                    rbShow_CheckedChanged(this, new EventArgs());

                    // HR Legal Entity can only see data without posibility of changing
                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity)
                    {                        
                        rbMove.Visible = rbShow.Visible = btnSave.Visible = false;
                        Menu1.Enabled = lboxEmployees.Enabled = tbEmployee.Enabled = btnOrgTree.Enabled = dropDownWU.Enabled = btnWUTree.Enabled = btnTempWUTree.Enabled = !rbShow.Checked;
                    }                    

                    InitializeSQLParameters();

                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    {
                        ClearSessionValues();
                        if (Menu1.SelectedItem.Value.Equals("0"))
                        {
                            int wuID = -1;
                            if (tbUte.Attributes["id"] != null)
                                if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                                    wuID = -1;
                            populateEmployees(wuID, true);
                        }
                        else
                        {
                            int ouID = -1;
                            if (tbOrgUte.Attributes["id"] != null)
                                if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                                    ouID = -1;
                            populateEmployees(ouID, false);
                        }

                        if (Session[Constants.sessionSelectedEmplIDs] != null && Session[Constants.sessionSelectedEmplIDs] is List<string> && lboxEmployees.Enabled)
                        {
                            foreach (ListItem item in lboxEmployees.Items)
                            {
                                if (((List<string>)Session[Constants.sessionSelectedEmplIDs]).Contains(item.Value.Trim()))
                                    item.Selected = true;
                            }
                        }
                    }
                    else
                    {
                        // reload selected filter state                        
                        LoadState();
                        if (Menu1.SelectedItem.Value.Equals("0"))
                        {
                            int wuID = -1;
                            if (tbUte.Attributes["id"] != null)
                                if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                                    wuID = -1;
                            populateEmployees(wuID, true);
                        }
                        else
                        {
                            int ouID = -1;
                            if (tbOrgUte.Attributes["id"] != null)
                                if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                                    ouID = -1;
                            populateEmployees(ouID, false);
                        }

                        // do again load state to select previously selected employees in employees list
                        LoadState();

                        InitializeSQLParameters();
                    }

                    //resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                    btnShow_Click(this, new EventArgs());
                }
                else
                {
                    try
                    {
                        WorkingUnit wUnit = new WorkingUnit(Session[Constants.sessionConnection]);
                        string wUnits = "";


                        if (Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>)
                        {
                            foreach (int wuID in (List<int>)Session[Constants.sessionLoginCategoryWUnits])
                            {
                                wUnits += wuID.ToString().Trim() + ",";
                            }

                            if (wUnits.Length > 0)
                                wUnits = wUnits.Substring(0, wUnits.Length - 1);

                            List<WorkingUnitTO> wuList = wUnit.Search(wUnits);


                            foreach (WorkingUnitTO wu in wuList)
                            {
                                wuDic.Add(wu.WorkingUnitID, new List<WorkingUnitTO>());
                            }

                            foreach (WorkingUnitTO wu in wuList)
                            {
                                if (wu.ParentWorkingUID != wu.WorkingUnitID && wuDic.ContainsKey(wu.ParentWorkingUID))
                                    wuDic[wu.ParentWorkingUID].Add(wu);
                            }
                            foreach (var item in wuList)
                            {
                                bool sw = false;
                                for (int i = 0; i < wuList.Count; i++)
                                {
                                    if (item.WorkingUnitID != wuList[i].WorkingUnitID && item.WorkingUnitID == wuList[i].ParentWorkingUID)
                                    {
                                        sw = true;
                                        break;
                                    }
                                }
                                if (!sw)
                                {
                                    WUName.Add(item.Name);
                                    //dropDownWU.Items.Add(item.Name);
                                }
                            }
                        }
                        OrganizationalUnit oUnit = new OrganizationalUnit(Session[Constants.sessionConnection]);
                        string oUnits = "";

                        if (Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>)
                        {
                            foreach (int ouID in (List<int>)Session[Constants.sessionLoginCategoryOUnits])
                            {
                                oUnits += ouID.ToString().Trim() + ",";
                            }

                            if (oUnits.Length > 0)
                                oUnits = oUnits.Substring(0, oUnits.Length - 1);

                            List<OrganizationalUnitTO> ouList = oUnit.Search(oUnits);
                            Dictionary<int, List<OrganizationalUnitTO>> ouDic = new Dictionary<int, List<OrganizationalUnitTO>>();

                            foreach (OrganizationalUnitTO ou in ouList)
                            {
                                ouDic.Add(ou.OrgUnitID, new List<OrganizationalUnitTO>());
                            }

                            foreach (OrganizationalUnitTO ou in ouList)
                            {
                                if (ou.ParentOrgUnitID != ou.OrgUnitID && ouDic.ContainsKey(ou.ParentOrgUnitID))
                                    ouDic[ou.ParentOrgUnitID].Add(ou);
                            }
                            foreach (var item in ouList)
                            {
                                bool sw = false;
                                for (int i = 0; i < ouList.Count; i++)
                                {
                                    if (item.OrgUnitID != ouList[i].OrgUnitID && item.OrgUnitID == ouList[i].ParentOrgUnitID)
                                    {
                                        sw = true;
                                        break;
                                    }
                                }
                                if (!sw)
                                {
                                    OUName.Add(item.Name);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    //foreach (var item in WUName)
                    //{
                    //    dropDownWU.Items.Add(item);
                    //}
                    // if there is selected wuID in wu tree pop up, populate with that working unit
                    if (Session[Constants.sessionSelectedWUID] != null)
                    {
                        ClientScript.GetPostBackClientHyperlink(btnWUTree, "");

                        int wuID = -1;
                        if (!int.TryParse(Session[Constants.sessionSelectedWUID].ToString(), out wuID))
                            wuID = -1;

                        if (wuID != -1)
                        {
                            populateWU(wuID);
                            populateEmployees(wuID, true);
                        }
                    }
                    else if (Session[Constants.sessionSelectedOUID] != null)
                    {
                        ClientScript.GetPostBackClientHyperlink(btnOrgTree, "");

                        int ouID = -1;
                        if (!int.TryParse(Session[Constants.sessionSelectedOUID].ToString(), out ouID))
                            ouID = -1;

                        if (ouID != -1)
                        {
                            populateOU(ouID);
                            populateEmployees(ouID, false);
                        }
                    }
                    if (Session[Constants.sessionSelectedTempWUID] != null)
                    {
                        ClientScript.GetPostBackClientHyperlink(btnTempWUTree, "");

                        int wuID = -1;
                        if (!int.TryParse(Session[Constants.sessionSelectedTempWUID].ToString(), out wuID))
                            wuID = -1;

                        if (wuID != -1)
                            populateTempWU(wuID);
                    }
                }

                Session[Constants.sessionSelectedWUID] = null;
                Session[Constants.sessionSelectedOUID] = null;
                Session[Constants.sessionSelectedTempWUID] = null;
                
                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLLoansPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                //date validation
                if (fromDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFromDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                tbFromDate.Text = fromDate.AddDays(-1).ToString(Constants.dateFormat);
                tbToDate.Text = toDate.AddDays(-1).ToString(Constants.dateFormat);

                Date_Changed(this, new EventArgs());

                //btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLLoansPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLLoansPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                //date validation
                if (fromDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFromDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                tbFromDate.Text = fromDate.AddDays(1).ToString(Constants.dateFormat);
                tbToDate.Text = toDate.AddDays(1).ToString(Constants.dateFormat);

                Date_Changed(this, new EventArgs());

                //btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLLoansPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLLoansPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void InitializeSQLParameters()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrEmployee", culture) + "," + rm.GetString("hdrBranch", culture) + "," + rm.GetString("hdrEmplType", culture) + ","
                    + rm.GetString("hdrPermanentUTE", culture) + "," + rm.GetString("hdrTempUte", culture) + ","
                    + rm.GetString("hdrFromTime", culture) + "," + rm.GetString("hdrToTime", culture) + "," + rm.GetString("hdrOperater", culture);
                Session[Constants.sessionFields] = "empl, emplBranch, emplType, permUTE, tempUTE, fromDate, toDate, operater";                    
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(5, (int)Constants.FormatTypes.DateFormat);
                formating.Add(6, (int)Constants.FormatTypes.DateFormat);
                Session[Constants.sessionFieldsFormating] = formating;                
                Session[Constants.sessionFieldsFormatedValues] = null;                
                Session[Constants.sessionTables] = null;                
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionKey] = null;
                Session[Constants.sessionColTypes] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateWU(int wuID)
        {
            try
            {
                WorkingUnitTO wu = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(wuID);
                WorkingUnit wUnit = new WorkingUnit(Session[Constants.sessionConnection]);
                wUnit.WUTO = wu;
                tbWorkshop.Text = wUnit.getParentWorkingUnit().Name.Trim();
                tbUte.Attributes.Add("id", wuID.ToString());
                tbUte.Text = wu.Name.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateOU(int ouID)
        {
            try
            {
                OrganizationalUnitTO ou = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(ouID);
                tbOrg.Text = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(ou.ParentOrgUnitID).Name.Trim();
                tbOrgUte.Attributes.Add("id", ouID.ToString());
                tbOrgUte.Text = ou.Name.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateTempWU(int wuID)
        {
            try
            {
                WorkingUnitTO wu = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(wuID);
                tbTempUte.Attributes.Add("id", wuID.ToString());
                tbTempUte.Text = wu.Name.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Date_Changed(object sender, EventArgs e)
        {
            try
            {
                if (Menu1.SelectedItem.Value.Equals("0"))
                {
                    int wuID = -1;
                    if (tbUte.Attributes["id"] != null)
                        if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                            wuID = -1;
                    populateEmployees(wuID, true);
                }
                else
                {
                    int ouID = -1;
                    if (tbOrgUte.Attributes["id"] != null)
                        if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                            ouID = -1;
                    populateEmployees(ouID, false);
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLLoansPage.Date_Changed(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLLoansPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateEmployees(int ID, bool isWU)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                lblError.Text = "";
                                
                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    return;
                }

                DateTime from = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime to = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                //date validation
                if (from.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFromDate.Focus();
                    return;
                }

                if (to.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    return;
                }

                if (from > to)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    return;
                }

                List<EmployeeTO> empolyeeList = new List<EmployeeTO>();

                //int onlyEmplTypeID = -1;
                //int exceptEmplTypeID = -1;
                int emplID = -1;

                Dictionary<int, List<int>> companyVisibleTypes = new Dictionary<int, List<int>>();
                List<int> typesVisible = new List<int>();

                if (Session[Constants.sessionVisibleEmplTypes] != null && Session[Constants.sessionVisibleEmplTypes] is Dictionary<int, List<int>>)
                    companyVisibleTypes = (Dictionary<int, List<int>>)Session[Constants.sessionVisibleEmplTypes];

                // get company
                Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                int company = -1;
                if (isWU)
                    company = Common.Misc.getRootWorkingUnit(ID, wuDict);
                else
                {
                    WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                    wuXou.WUXouTO.OrgUnitID = ID;
                    List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                    if (list.Count > 0)
                        company = Common.Misc.getRootWorkingUnit(list[0].WorkingUnitID, wuDict);
                }

                if (companyVisibleTypes.ContainsKey(company))
                    typesVisible = companyVisibleTypes[company];

                if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    emplID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

                if (ID != -1)
                {
                    if (isWU)
                    {
                        if (Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>)
                        {
                            empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(Common.Misc.getWorkingUnitHierarhicly(ID, (List<int>)Session[Constants.sessionLoginCategoryWUnits], Session[Constants.sessionConnection]), emplID, typesVisible, from, to);
                        }
                    }
                    else if (!isWU)
                    {
                        if (Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>)
                        {
                            empolyeeList = new Employee(Session[Constants.sessionConnection]).SearchByOU(Common.Misc.getOrgUnitHierarhicly(ID.ToString(), (List<int>)Session[Constants.sessionLoginCategoryOUnits], Session[Constants.sessionConnection]), emplID, typesVisible, from, to);
                        }
                    }
                }

                lboxEmployees.DataSource = empolyeeList;
                lboxEmployees.DataTextField = "FirstAndLastName";
                lboxEmployees.DataValueField = "EmployeeID";
                lboxEmployees.DataBind();
                foreach (ListItem item in lboxEmployees.Items)
                {
                    item.Attributes.Add("title", item.Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lboxEmployees_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lboxEmployees.Items)
                {
                    item.Attributes.Add("title", item.Value);
                }
            }
            catch { }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                lblEmployee.Text = rm.GetString("lblEmployeeSelection", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblTempUTE.Text = rm.GetString("lblTempUTE", culture);

                cbSelectAllEmpolyees.Text = rm.GetString("lblSelectAll", culture);

                // HR Legal Entity can only see data without posibility of changing
                btnShow.Text = rm.GetString("btnShow", culture);                
                btnReport.Text = rm.GetString("btnReport", culture);
                btnSave.Text = rm.GetString("btnSave", culture);

                rbMove.Text = rm.GetString("rbMove", culture);
                rbShow.Text = rm.GetString("rbShow", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClearSessionValues()
        {
            try
            {
                if (Session[Constants.sessionFilter] != null)
                    Session[Constants.sessionFilter] = "";
                if (Session[Constants.sessionSortCol] != null)
                    Session[Constants.sessionSortCol] = "";
                if (Session[Constants.sessionSortDir] != null)
                    Session[Constants.sessionSortDir] = "";
                if (Session[Constants.sessionResultCurrentPage] != null)
                    Session[Constants.sessionResultCurrentPage] = null;

                Session[Constants.sessionItemsColors] = null;                
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

                if (Session[Constants.sessionFilterState] != null && Session[Constants.sessionFilterState] is Dictionary<string, string>)
                    filterState = (Dictionary<string, string>)Session[Constants.sessionFilterState];

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "TLLoansPage.", filterState);
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

                if (Session[Constants.sessionFilterState] != null && Session[Constants.sessionFilterState] is Dictionary<string, string>)
                {
                    filterState = (Dictionary<string, string>)Session[Constants.sessionFilterState];
                    CommonWeb.Misc.LoadState(this, "TLLoansPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                dropDownWU.Items.Clear();
                for (int i = 0; i < Menu1.Items.Count; i++)
                {
                    if (i == int.Parse(e.Item.Value))
                    {
                        Menu1.Items[i].Selected = true;
                        MultiView1.SetActiveView(MultiView1.Views[i]);
                    }

                    CommonWeb.Misc.setMenuImage(Menu1, i, Menu1.Items[i].Selected);
                    CommonWeb.Misc.setMenuSeparator(Menu1, i, Menu1.Items[i].Selected);
                }
                foreach (var item in WUName)
                {
                    dropDownWU.Items.Add(item);
                }
                // value 0 - WU tab, value 1 - OU tab                
                if (e.Item.Value.Equals("0"))
                {

                    int wuID = -1;
                    if (tbUte.Attributes["id"] != null)
                        if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                            wuID = -1;
                    populateEmployees(wuID, true);
                    Session[Constants.sessionWU] = wuID;
                    Session[Constants.sessionOU] = null;
                }
                else
                {
                    //foreach (var item in OUName)
                    //{
                    //    dropDownWU.Items.Add(item);
                    //}
                    int ouID = -1;
                    if (tbOrgUte.Attributes["id"] != null)
                        if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                            ouID = -1;
                    populateEmployees(ouID, false);
                    Session[Constants.sessionOU] = ouID;
                    Session[Constants.sessionWU] = null;
                }

                // save selected filter state
                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLLoansPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLLoansPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbMove_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                rbShow.Checked = !rbMove.Checked;
                Menu1.Enabled = lboxEmployees.Enabled = tbEmployee.Enabled = btnOrgTree.Enabled = dropDownWU.Enabled = btnWUTree.Enabled = btnTempWUTree.Enabled = rbMove.Checked;

                if (rbMove.Checked)
                    btnShow.Text = "  >  ";
                else
                    btnShow.Text = rm.GetString("btnShow", culture);

                btnSave.Visible = rbMove.Checked;
                btnReport.Visible = !rbMove.Checked;

                ClearSessionValues();
                InitializeSQLParameters();

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLLoansPage.rbMove_CheckedChange(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLLoansPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbShow_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                rbMove.Checked = !rbShow.Checked;

                Menu1.Enabled = lboxEmployees.Enabled = tbEmployee.Enabled = btnOrgTree.Enabled = dropDownWU.Enabled = btnWUTree.Enabled = btnTempWUTree.Enabled = !rbShow.Checked;

                if (!rbShow.Checked)
                    btnShow.Text = "  >  ";
                else
                    btnShow.Text = rm.GetString("btnShow", culture);

                btnSave.Visible = !rbShow.Checked;
                btnReport.Visible = rbShow.Checked;

                ClearSessionValues();
                InitializeSQLParameters();

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLLoansPage.rbShow_CheckedChange(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLLoansPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnReport_Click(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                // ClearSessionValues();
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                if (Session[Constants.sessionDataTableList] != null || Session[Constants.sessionFilter] != null)
                {
                    // Table Definition for  Reports
                    DataSet dataSet = new DataSet();
                    DataTable tableCR = new DataTable("Loans");

                    tableCR.Columns.Add("employee", typeof(System.String));                    
                    tableCR.Columns.Add("employee_branch", typeof(System.String));
                    tableCR.Columns.Add("employee_type", typeof(System.String));
                    tableCR.Columns.Add("permanent_ute", typeof(System.String));
                    tableCR.Columns.Add("temp_ute", typeof(System.String));
                    tableCR.Columns.Add("from", typeof(System.DateTime));
                    tableCR.Columns.Add("until", typeof(System.DateTime));
                    tableCR.Columns.Add("operater", typeof(System.String));

                    dataSet.Tables.Add(tableCR);
                    if (Session[Constants.sessionFilter] != null && !Session[Constants.sessionFilter].ToString().Trim().Equals(""))
                    {
                        string filter = (string)Session[Constants.sessionFilter];
                        int rowCount = 0;
                        DataTable tableLoans = new DataTable();
                        Result result = new Result(Session[Constants.sessionConnection]);
                        rowCount = result.SearchResultCount(Session[Constants.sessionTables].ToString().Trim(), Session[Constants.sessionFilter].ToString().Trim());

                        if (rowCount > 0)
                        {
                            // get all passes for search criteria for report
                            tableLoans = new Result(Session[Constants.sessionConnection]).SearchResultTable(Session[Constants.sessionFields].ToString().Trim(), Session[Constants.sessionTables].ToString().Trim(),
                                Session[Constants.sessionFilter].ToString().Trim(), Session[Constants.sessionSortCol].ToString().Trim(), Session[Constants.sessionSortDir].ToString().Trim(), 1, rowCount);
                            
                            foreach (DataRow loan in tableLoans.Rows)
                            {
                                DataRow row = tableCR.NewRow();
                                if (loan["empl"] != DBNull.Value)                                
                                    row["employee"] = loan["empl"].ToString().Trim();                                
                                if (loan["emplBranch"] != DBNull.Value)
                                    row["employee_branch"] = loan["emplBranch"].ToString().Trim();
                                if (loan["emplType"] != DBNull.Value)
                                    row["employee_type"] = loan["emplType"].ToString().Trim();
                                if (loan["permUTE"] != DBNull.Value)
                                    row["permanent_ute"] = loan["permUTE"].ToString().Trim();
                                if (loan["tempUTE"] != DBNull.Value)
                                    row["temp_ute"] = loan["tempUTE"].ToString().Trim();
                                if (loan["fromDate"] != DBNull.Value)
                                    row["from"] = loan["fromDate"].ToString().Trim();
                                if (loan["toDate"] != DBNull.Value)
                                    row["until"] = loan["toDate"].ToString().Trim();
                                if (loan["operater"] != DBNull.Value)
                                    row["operater"] = loan["operater"].ToString().Trim();
                                tableCR.Rows.Add(row);
                                tableCR.AcceptChanges();
                            }
                        }
                        else
                            lblError.Text = rm.GetString("noReportData", culture);
                    }
                    else if (Session[Constants.sessionDataTableList] != null && Session[Constants.sessionDataTableList] is List<List<object>>)
                    {
                        List<List<object>> lista = (List<List<object>>)Session[Constants.sessionDataTableList];

                        if (lista.Count > 0)
                        {
                            foreach (List<object> listObj in lista)
                            {
                                if (listObj.Count >= 8)
                                {
                                    DataRow row = tableCR.NewRow();
                                    if (listObj[0] != null)
                                        row["employee"] = listObj[0].ToString().Trim();
                                    if (listObj[1] != null)
                                        row["employee_branch"] = listObj[1].ToString().Trim();
                                    if (listObj[2] != null)
                                        row["employee_type"] = listObj[2].ToString().Trim();
                                    if (listObj[3] != DBNull.Value)
                                        row["permanent_ute"] = listObj[3].ToString().Trim();
                                    if (listObj[4] != null)
                                        row["temp_ute"] = listObj[4].ToString().Trim();
                                    if (listObj[5] != null)
                                        row["from"] = listObj[5].ToString().Trim();
                                    if (listObj[6] != null)
                                        row["until"] = listObj[6].ToString().Trim();
                                    if (listObj[7] != null)                                    
                                        row["operater"] = listObj[7].ToString().Trim();
                                    
                                    tableCR.Rows.Add(row);
                                    tableCR.AcceptChanges();
                                }
                            }
                        }
                        else
                            lblError.Text = rm.GetString("noReportData", culture);
                       
                    }

                    if (tableCR.Rows.Count == 0)
                    {
                        lblError.Text = rm.GetString("noReportData", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }

                    string employee = "*";
                    string fromDate = "";
                    string toDate = "";
                    string tempUte = "*";
                    if (lboxEmployees.SelectedIndex > 0)
                    {
                        for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                        {
                            if (lboxEmployees.Items[intEmpolyees].Selected)
                            {
                                employee = employee + ", " + lboxEmployees.Items[intEmpolyees].ToString(); ;

                            }
                        }
                        employee = employee.Substring(employee.IndexOf(',') + 1);
                    }
                    if (employee == "")
                    {
                        for (int intEmpolyees = 0; intEmpolyees < lboxEmployees.Items.Count; intEmpolyees++)
                        {
                            employee = employee + ", " + lboxEmployees.Items[intEmpolyees].ToString();
                        }
                        employee = employee.Substring(employee.IndexOf(',') + 1);
                    }
                    if(tbTempUte.Text!="")
                        tempUte = tbTempUte.Text;
                    
                    fromDate = CommonWeb.Misc.createDate(tbFromDate.Text).ToString("dd.MM.yyyy.");
                    toDate = CommonWeb.Misc.createDate(tbToDate.Text).ToString("dd.MM.yyyy.");
                    Session["TLLoansPage.employee"] = employee;
                    Session["TLLoansPage.tempUte"] = tempUte;
                    Session["TLLoansPage.fromDate"] = fromDate;
                    Session["TLLoansPage.toDate"] = toDate;
                    Session["TLLoansPage.dataSet"] = dataSet;
                    string reportURL = "";
                    if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                        reportURL = "/ACTAWeb/ReportsWeb/sr/TLLoansReport_sr.aspx";
                    else
                        reportURL = "/ACTAWeb/ReportsWeb/en/TLLoansReport_en.aspx";
                    Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/TLLoansPage.aspx&Report=" + reportURL.Trim(), false);
                    
                    // save selected filter state
                    SaveState();
                }
                else
                    lblError.Text = rm.GetString("noReportData", culture);

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLLoansPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLLoansPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                ClearSessionValues();
                lblError.Text = "";

                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionDataTableList] = null;
                Session[sessionLoanList] = null;
                
                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                //date validation
                if (fromDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFromDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;

                string errorMessage = "";
                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                Dictionary<int, EmployeeTO> employees = new Employee(Session[Constants.sessionConnection]).SearchDictionary();

                // get all employee types by company, key is employee_type, value name for that company
                Dictionary<int, Dictionary<int, string>> emplTypes = new EmployeeType(Session[Constants.sessionConnection]).SearchDictionary();

                // create list of employee loans and list of columns for priview
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("empl", typeof(string)));                
                resultColumns.Add(new DataColumn("emplBranch", typeof(string)));
                resultColumns.Add(new DataColumn("emplType", typeof(string)));
                resultColumns.Add(new DataColumn("permUTE", typeof(string)));
                resultColumns.Add(new DataColumn("tempUTE", typeof(string)));
                resultColumns.Add(new DataColumn("fromDate", typeof(DateTime)));
                resultColumns.Add(new DataColumn("toDate", typeof(DateTime)));
                resultColumns.Add(new DataColumn("operater", typeof(string)));

                List<List<object>> resultTable = new List<List<object>>();

                if (rbShow.Checked)
                {
                    string filter = "e.employee_id = el.employee_id AND e.employee_id = ea.employee_id AND e.working_unit_id = w.working_unit_id AND el.working_unit_id = tw.working_unit_id AND au.user_id = el.created_by";

                    filter += " AND (('" + fromDate.ToString(CommonWeb.Misc.getDateTimeFormatUniversal()) + "' <= el.date_start AND '" + toDate.ToString(CommonWeb.Misc.getDateTimeFormatUniversal())
                        + "' >= el.date_start) OR ('" + fromDate.ToString(CommonWeb.Misc.getDateTimeFormatUniversal()) + "' >= el.date_start AND '"
                        + toDate.ToString(CommonWeb.Misc.getDateTimeFormatUniversal()) + "' <= el.date_end) OR ('" + fromDate.ToString(CommonWeb.Misc.getDateTimeFormatUniversal())
                        + "' <= el.date_end AND '" + toDate.ToString(CommonWeb.Misc.getDateTimeFormatUniversal()) + "' >= el.date_end))";

                    if (Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>)
                    {
                        string wuIDs = "";

                        foreach (int wuID in (List<int>)Session[Constants.sessionLoginCategoryWUnits])
                        {
                            wuIDs += wuID.ToString().Trim() + ",";
                        }

                        if (wuIDs.Length > 0)
                        {
                            wuIDs = wuIDs.Substring(0, wuIDs.Length - 1);
                            filter += " AND el.working_unit_id IN (" + wuIDs.Trim() + ") AND e.working_unit_id IN (" + wuIDs.Trim() + ")";
                        }
                    }

                    string fields = "e.employee_id AS empl, e.employee_type_id AS emplType, ea.nvarchar_value_6 AS emplBranch, w.name AS permUTE, tw.name AS tempUTE, el.date_start AS fromDate, el.date_end AS toDate, au.name AS operater";
                    string tables = "employees e, employees_asco4 ea, working_units w, working_units tw, employee_loans el, appl_users au";                    
                    string sortCol = "e.last_name + ' ' + e.first_name";
                    string sortDir = Constants.sortASC;

                    Result result = new Result(Session[Constants.sessionConnection]);
                    int rowCount = result.SearchResultCount(tables.Trim(), filter.Trim());

                    if (rowCount > 0)
                    {
                        DataTable tableLoans = result.SearchResultTable(fields.Trim(), tables.Trim(), filter.Trim(), sortCol.Trim(), sortDir.Trim(), 1, rowCount);

                        foreach (DataRow loanRow in tableLoans.Rows)
                        {
                            // create table row
                            List<object> resultRow = new List<object>();
                            EmployeeTO empl = new EmployeeTO();
                            int emplCompany = -1;

                            if (loanRow["empl"] != DBNull.Value)
                            {
                                int emplID = -1;
                                if (int.TryParse(loanRow["empl"].ToString().Trim(), out emplID))
                                {
                                    if (employees.ContainsKey(emplID))
                                    {
                                        empl = employees[emplID];
                                        emplCompany = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wUnits);
                                        resultRow.Add(empl.FirstAndLastName.Trim());
                                    }
                                    else
                                        continue;
                                }
                                else
                                    continue;
                            }
                            else
                                continue;

                            if (loanRow["emplBranch"] != DBNull.Value)
                                resultRow.Add(loanRow["emplBranch"].ToString().Trim()); // branch
                            else
                                resultRow.Add("");
                            if (emplTypes.ContainsKey(emplCompany) && emplTypes[emplCompany].ContainsKey(empl.EmployeeTypeID))
                                resultRow.Add(emplTypes[emplCompany][empl.EmployeeTypeID].Trim());
                            else
                                resultRow.Add("");                            
                            if (loanRow["permUTE"] != DBNull.Value)
                                resultRow.Add(loanRow["permUTE"].ToString().Trim());
                            else
                                resultRow.Add("");
                            if (loanRow["tempUTE"] != DBNull.Value)
                                resultRow.Add(loanRow["tempUTE"].ToString().Trim());
                            else
                                resultRow.Add("");
                            if (loanRow["fromDate"] != DBNull.Value)
                            {
                                DateTime fromLoanDate = new DateTime();
                                if (DateTime.TryParse(loanRow["fromDate"].ToString().Trim(), out fromLoanDate))
                                    resultRow.Add(fromLoanDate);
                                else
                                    resultRow.Add(new DateTime());
                            }
                            else
                                resultRow.Add(new DateTime());
                            if (loanRow["toDate"] != DBNull.Value)
                            {
                                DateTime toLoanDate = new DateTime();
                                if (DateTime.TryParse(loanRow["toDate"].ToString().Trim(), out toLoanDate))
                                    resultRow.Add(toLoanDate);
                                else
                                    resultRow.Add(new DateTime());
                            }
                            else
                                resultRow.Add(new DateTime());
                            if (loanRow["operater"] != DBNull.Value)
                                resultRow.Add(loanRow["operater"].ToString().Trim());
                            else
                                resultRow.Add("");

                            resultTable.Add(resultRow);
                        }
                    }
                }
                else if (rbMove.Checked)
                {
                    if (lboxEmployees.Items.Count <= 0)
                    {
                        lblError.Text = rm.GetString("noEmployeesForReport", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }
                    //string proba = Menu1.SelectedItem.Value.ToString();
                    int tempWU = -1;
                    string WUName = dropDownWU.SelectedItem.Value;
                    WorkingUnit wUnitS = new WorkingUnit();
                    List<WorkingUnitTO> listWU = wUnitS.SearchByName(WUName);
                    WorkingUnitTO wuID = new WorkingUnitTO();
                    if (listWU.Count == 1)
                    {
                        wuID = listWU[0];
                    }
                    try {
                        tempWU = listWU[0].WorkingUnitID;
                    }
                    catch {
                        tempWU = -1;
                    }
                    //if (proba.Contains('0'))
                    //{
                    //    string WUName = dropDownWU.SelectedItem.Value;
                    //    WorkingUnit wUnitS = new WorkingUnit();
                    //    List<WorkingUnitTO> listWU = wUnitS.SearchByName(WUName);
                    //    WorkingUnitTO wuID = new WorkingUnitTO();
                    //    if (listWU.Count == 1)
                    //    {
                    //        wuID = listWU[0];
                    //    }

                    //    if (wuID.WorkingUnitID != null)
                    //    {
                    //        try
                    //        {
                    //            tempWU = wuID.WorkingUnitID;
                    //        }
                    //        catch { tempWU = -1; }
                    //        //if (!int.TryParse(tbTempUte.Attributes["id"], out tempWU))
                    //        //    tempWU = -1;
                    //    }
                    //}
                    //else if (proba.Contains('1'))
                    //{
                    //    string OUName = dropDownWU.SelectedItem.Value;
                    //    OrganizationalUnit oUnitS = new OrganizationalUnit();
                    //    List<OrganizationalUnitTO> listOU = oUnitS.SearchByName(OUName);
                    //    OrganizationalUnitTO ouID = new OrganizationalUnitTO();
                    //    if (listOU.Count == 1)
                    //    {
                    //        ouID = listOU[0];
                    //    }
                    //    if (ouID.OrgUnitID != null)
                    //    {
                    //        try
                    //        {
                    //            tempWU = ouID.OrgUnitID;
                    //        }
                    //        catch
                    //        {
                    //            tempWU = -1;
                    //        }
                    //    }
                    //}
                   

                    if (tempWU == -1)
                    {
                        lblError.Text = rm.GetString("noTempWUSelected", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }

                    //int wu = -1;
                    //if (tbUte.Attributes["id"] != null)
                    //{
                    //    if (!int.TryParse(tbUte.Attributes["id"], out wu))
                    //        wu = -1;
                    //}

                    //if (tempWU == wu)
                    //{
                    //    lblError.Text = rm.GetString("tempWUSelectedSame", culture);
                    //    writeLog(DateTime.Now, false);
                    //    return;
                    //}

                    List<EmployeeLoanTO> loans = new List<EmployeeLoanTO>();
                    Dictionary<int, EmployeeAsco4TO> employeesAdditional = getEmployeesAdditional();

                    int loanDays = (int)toDate.AddDays(1).Date.Subtract(fromDate.Date).TotalDays;

                    List<int> employeesIDs = new List<int>();

                    if (lboxEmployees.GetSelectedIndices().Length > 0)
                    {
                        List<string> idList = new List<string>();
                        foreach (int emplIndex in lboxEmployees.GetSelectedIndices())
                        {
                            if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                            {
                                int emplID = -1;
                                if (int.TryParse(lboxEmployees.Items[emplIndex].Value.Trim(), out emplID))
                                    employeesIDs.Add(emplID);
                                idList.Add(lboxEmployees.Items[emplIndex].Value.Trim());
                            }
                        }
                        Session[Constants.sessionSelectedEmplIDs] = idList;
                    }
                    else
                    {
                        foreach (ListItem item in lboxEmployees.Items)
                        {
                            int emplID = -1;
                            if (int.TryParse(item.Value.Trim(), out emplID))
                                employeesIDs.Add(emplID);
                        }
                    }

                    foreach (int emplID in employeesIDs)
                    {
                        if (emplID != -1 && employees.ContainsKey(emplID))
                        {
                            EmployeeTO empl = employees[emplID];
                            int emplCompany = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wUnits);
                            // get loan max period rule for employee
                            Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                            rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                            rule.RuleTO.WorkingUnitID = emplCompany;
                            rule.RuleTO.RuleType = Constants.RuleEmplLoanMaxPeriod;

                            List<RuleTO> loanRules = rule.Search();
                            if (loanRules.Count > 0 && loanDays > loanRules[0].RuleValue)
                            {
                                errorMessage += emplID.ToString();
                                if (employees.ContainsKey(emplID))
                                    errorMessage += " " + employees[emplID].FirstAndLastName.Trim();
                                errorMessage += ": " + rm.GetString("maxLoanPeriod", culture) + " ";
                                continue;
                            }

                            if (tempWU == empl.WorkingUnitID)
                            {
                                errorMessage += emplID.ToString();
                                if (employees.ContainsKey(emplID))
                                    errorMessage += " " + employees[emplID].FirstAndLastName.Trim();
                                errorMessage += ": " + rm.GetString("tempWUSelectedSame", culture) + " ";
                                continue;
                            }

                            loans.Add(createLoan(emplID, tempWU, fromDate.Date, toDate.Date));

                            // create table row
                            List<object> resultRow = new List<object>();

                            resultRow.Add(empl.FirstAndLastName.Trim());                            
                            if (employeesAdditional.ContainsKey(empl.EmployeeID))
                                resultRow.Add(employeesAdditional[empl.EmployeeID].NVarcharValue6); // branch
                            else
                                resultRow.Add("");
                            if (emplTypes.ContainsKey(emplCompany) && emplTypes[emplCompany].ContainsKey(empl.EmployeeTypeID))
                                resultRow.Add(emplTypes[emplCompany][empl.EmployeeTypeID].Trim());
                            else
                                resultRow.Add("");
                            if (wUnits.ContainsKey(empl.WorkingUnitID))
                                resultRow.Add(wUnits[empl.WorkingUnitID].Name);
                            else
                                resultRow.Add("");
                            if (wUnits.ContainsKey(tempWU))
                                resultRow.Add(wUnits[tempWU].Name);
                            else
                                resultRow.Add("");
                            resultRow.Add(fromDate.Date);
                            resultRow.Add(toDate.Date);
                            if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO)
                                resultRow.Add(((ApplUserTO)Session[Constants.sessionLogInUser]).Name.Trim());
                            else
                                resultRow.Add(CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]));

                            resultTable.Add(resultRow);
                        }
                    }

                    Session[sessionLoanList] = loans;
                }

                Session[Constants.sessionDataTableList] = resultTable;
                Session[Constants.sessionDataTableColumns] = resultColumns;
                Session[Constants.sessionSortCol] = "empl";
                Session[Constants.sessionSortDir] = Constants.sortASC;

                // save selected filter state
                SaveState();

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";

                // show error page pop up
                if (!errorMessage.Trim().Equals(""))
                {
                    Session[Constants.sessionInfoMessage] = errorMessage.Trim();
                    string wOptions = "dialogWidth:800px; dialogHeight:300px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                    ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/CommonWeb/MessagesPages/Info.aspx?isError=true', window, '" + wOptions.Trim() + "');", true);
                }

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLLoansPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLLoansPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private EmployeeLoanTO createLoan(int emplID, int wuID, DateTime from, DateTime to)
        {
            try
            {
                EmployeeLoanTO loan = new EmployeeLoanTO();
                loan.EmployeeID = emplID;
                loan.WorkingUnitID = wuID;
                loan.DateStart = from;
                loan.DateEnd = to;
                loan.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                return loan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(Object sender, EventArgs e)
        {
            try
            {
                // if system is closed, sign out and redirect to login page
                if (Common.Misc.getClosingEvent(DateTime.Now, Session[Constants.sessionConnection]).EventID != -1)
                    CommonWeb.Misc.LogOut(Session[Constants.sessionLogInUserLog], Session[Constants.sessionLogInUser], Session[Constants.sessionConnection], Session, Response);

                lblError.Text = "";

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLLoansPage).Assembly);

                if (Session[sessionLoanList] != null && Session[sessionLoanList] is List<EmployeeLoanTO> && ((List<EmployeeLoanTO>)Session[sessionLoanList]).Count > 0)
                {
                    EmployeeLoan loan = new EmployeeLoan(Session[Constants.sessionConnection]);

                    if (loan.BeginTransaction())
                    {
                        try
                        {
                            bool saved = true;
                            foreach (EmployeeLoanTO loanTO in (List<EmployeeLoanTO>)Session[sessionLoanList])
                            {
                                loan.EmployeeLoanTO = loanTO;
                                saved = saved && (loan.Save(false) > 0);

                                if (!saved)
                                    break;
                            }

                            if (saved)
                            {
                                loan.CommitTransaction();
                                Session[sessionLoanList] = null;
                                Session[Constants.sessionDataTableList] = null;
                                Session[Constants.sessionDataTableColumns] = null;
                                lblError.Text = rm.GetString("loansSaved", culture);

                                if (Menu1.SelectedItem.Value.Equals("0"))
                                {
                                    int wuID = -1;
                                    if (tbUte.Attributes["id"] != null)
                                        if (!int.TryParse(tbUte.Attributes["id"].Trim(), out wuID))
                                            wuID = -1;
                                    populateEmployees(wuID, true);
                                }
                                else
                                {
                                    int ouID = -1;
                                    if (tbOrgUte.Attributes["id"] != null)
                                        if (!int.TryParse(tbOrgUte.Attributes["id"].Trim(), out ouID))
                                            ouID = -1;
                                    populateEmployees(ouID, false);
                                }
                            }
                            else
                            {
                                loan.RollbackTransaction();
                                lblError.Text = rm.GetString("loansSavingFaild", culture);
                            }
                        }
                        catch
                        {
                            loan.RollbackTransaction();
                            lblError.Text = rm.GetString("loansSavingFaild", culture);
                        }
                    }
                    else
                        lblError.Text = rm.GetString("loansSavingFaild", culture);
                }
                else
                {
                    lblError.Text = rm.GetString("noLoansForSaving", culture);
                }

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLLoansPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLLoansPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        private Dictionary<int, EmployeeTO> getEmployees()
        {
            try
            {
                string emplIDs = "";
                if (lboxEmployees.GetSelectedIndices().Length > 0)
                {
                    foreach (int emplIndex in lboxEmployees.GetSelectedIndices())
                    {
                        if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                        {
                            emplIDs += lboxEmployees.Items[emplIndex].Value.Trim() + ",";
                        }
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }
                else
                {
                    foreach (ListItem item in lboxEmployees.Items)
                    {
                        emplIDs += item.Value.Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }

                return new Employee(Session[Constants.sessionConnection]).SearchDictionary(emplIDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, EmployeeAsco4TO> getEmployeesAdditional()
        {
            try
            {
                string emplIDs = "";
                if (lboxEmployees.GetSelectedIndices().Length > 0)
                {
                    foreach (int emplIndex in lboxEmployees.GetSelectedIndices())
                    {
                        if (emplIndex >= 0 && emplIndex < lboxEmployees.Items.Count)
                        {
                            emplIDs += lboxEmployees.Items[emplIndex].Value.Trim() + ",";
                        }
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }
                else
                {
                    foreach (ListItem item in lboxEmployees.Items)
                    {
                        emplIDs += item.Value.Trim() + ",";
                    }

                    if (emplIDs.Length > 0)
                        emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);
                }

                List<EmployeeAsco4TO> addList = new EmployeeAsco4(Session[Constants.sessionConnection]).Search(emplIDs);

                Dictionary<int, EmployeeAsco4TO> addDictionary = new Dictionary<int, EmployeeAsco4TO>();

                foreach (EmployeeAsco4TO emplAdd in addList)
                {
                    if (!addDictionary.ContainsKey(emplAdd.EmployeeID))
                        addDictionary.Add(emplAdd.EmployeeID, emplAdd);
                    else
                        addDictionary[emplAdd.EmployeeID] = emplAdd;
                }

                return addDictionary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        string SelectedWU = "";
        protected void dropDownWU_ItemChanged(object sender, EventArgs e)
        {
            //DropDownList ddl =  sender as DropDownList;
            SelectedWU = dropDownWU.SelectedItem.Value;
        }
    }
}

