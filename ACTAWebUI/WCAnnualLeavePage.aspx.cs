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
    public partial class WCAnnualLeavePage : System.Web.UI.Page
    {
        const string sessionPairs = "WCAnnualLeavePage.PairsListByEmployee";
        const string sessionOldPairs = "WCAnnualLeavePage.OldPairsListByEmployee";
        const string sessionCounters = "WCAnnualLeavePage.CounterListByEmployee";

        const string pageName = "WCAnnualLeavePage";

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





        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // if user do not want to reload selected filter and get results for that filter, parameter reloadState should be passes through url and set to false
                if (!IsPostBack)
                {
                    ClearPageSessionValues();

                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'false');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'false');");
                    rbYes.Attributes.Add("onclick", "return check('rbYes', 'rbNo');");
                    rbNo.Attributes.Add("onclick", "return check('rbNo', 'rbYes');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnSave.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

                    btnFromDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnPrevDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnPrevDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnNextDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnNextDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    if (Session[Constants.sessionFromDate] != null && Session[Constants.sessionFromDate] is DateTime)
                        tbFromDate.Text = ((DateTime)Session[Constants.sessionFromDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbFromDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    if (Session[Constants.sessionToDate] != null && Session[Constants.sessionToDate] is DateTime)
                        tbToDate.Text = ((DateTime)Session[Constants.sessionToDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbToDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    EmployeeTO Empl = getEmployee();

                    InitializeData(Empl);

                    Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                    populatePassTypes(Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits), Empl.EmployeeTypeID);

                    setLanguage();

                    string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                    int cost = 0;
                    bool costum = int.TryParse(costumer, out cost);
                    bool isHyatt = (cost == (int)Constants.Customers.Hyatt);

                    if (isHyatt)
                    {
                        btnPrint.Visible = true;
                        chbReport.Visible = true;
                        cbPassType.AutoPostBack = true;                        
                    }
                    else
                    {
                        btnPrint.Visible = false;
                        chbReport.Checked = false;
                        chbReport.Visible = false;
                        cbPassType.AutoPostBack = false;
                    }

                    InitializeSQLParameters();

                    rbNo.Checked = true;
                    rbNo.Visible = rbYes.Visible = lblOverwritePairs.Visible = false;

                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                        ClearSessionValues();
                    else
                        // reload selected filter state                        
                        LoadState();

                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?showSelection=false";
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCAnnualLeavePage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCAnnualLeavePage).Assembly);

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

                //btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCAnnualLeavePage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCAnnualLeavePage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCAnnualLeavePage).Assembly);

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

                //btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCAnnualLeavePage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCAnnualLeavePage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private string emplTypes(EmployeeTO employee)
        {
            try
            {
                string empl = "";
                //// get selected company
                int company = -1;

                int wuID = employee.WorkingUnitID;
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                company = Common.Misc.getRootWorkingUnit(wuID, WUnits);
                if (company == -1)
                {
                    WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                    wuXou.WUXouTO.OrgUnitID = employee.OrgUnitID;
                    List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                    if (list.Count > 0)
                    {
                        WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                        company = Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, WUnits);
                    }
                    else
                        company = -1;
                }
                List<EmployeeTypeTO> listEmplTypes = new List<EmployeeTypeTO>();
                listEmplTypes = new EmployeeType(Session[Constants.sessionConnection]).Search();
                foreach (EmployeeTypeTO emplType in listEmplTypes)
                {
                    if (emplType.EmployeeTypeID == employee.EmployeeTypeID && emplType.WorkingUnitID == company)
                    {
                        empl = emplType.EmployeeTypeName;
                        break;
                    }
                }

                return empl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeData(EmployeeTO Empl)
        {
            try
            {
                // get employee data
                if (Empl.EmployeeID != -1)
                {
                    CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCAnnualLeavePage).Assembly);

                    tbFirstName.Text = Empl.FirstName.Trim();
                    tbLastName.Text = Empl.LastName.Trim();
                    tbPayroll.Text = Empl.EmployeeID.ToString().Trim();
                    // get additional employee data
                    EmployeeAsco4 asco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                    asco.EmplAsco4TO.EmployeeID = Empl.EmployeeID;
                    List<EmployeeAsco4TO> ascoList = asco.Search();

                    EmployeeAsco4TO ascoTO = new EmployeeAsco4TO();
                    if (ascoList.Count > 0)
                        ascoTO = ascoList[0];

                    tbStringone.Text = emplTypes(Empl);

                    // tbStringone.Text = ascoList[0].NVarcharValue2.Trim();
                    tbBranch.Text = ascoTO.NVarcharValue6.Trim();

                    // get employee working unit - UTE
                    WorkingUnit wu = new WorkingUnit(Session[Constants.sessionConnection]);
                    WorkingUnitTO tempWU = wu.FindWU(Empl.WorkingUnitID);
                    tbUTE.Text = tempWU.Code.Trim();
                    tbWUnit.Text = tempWU.Name.Trim();
                    tbWUnit.ToolTip = tempWU.Description.Trim() + "(" + tempWU.Code.Trim() + ")";

                    // get workshop (parent of UTE)
                    wu.WUTO = tempWU;
                    tempWU = wu.getParentWorkingUnit();
                    tbWorkgroup.Text = tempWU.Code.Trim();

                    // get cost centar
                    wu.WUTO = tempWU;
                    tempWU = wu.getParentWorkingUnit();
                    tbCostCentar.Text = tempWU.Code.Trim();

                    // get plant
                    wu.WUTO = tempWU;
                    tempWU = wu.getParentWorkingUnit();
                    tbPlant.Text = tempWU.Code.Trim();

                    // get organizational unit
                    OrganizationalUnitTO ou = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(Empl.OrgUnitID);
                    tbOUnit.Text = ou.Code.Trim();
                    tbOUnit.ToolTip = ou.Name.Trim();

                    // get FS responsible persons
                    List<EmployeeTO> emplWUResList = new Employee(Session[Constants.sessionConnection]).SearchEmployeesWUResponsible(Empl.WorkingUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);
                    string responsibility = rm.GetString("FSResPerson", culture) + Environment.NewLine;
                    foreach (EmployeeTO empl in emplWUResList)
                    {
                        responsibility += empl.FirstAndLastName.Trim() + Environment.NewLine;
                    }

                    // get OU responsible persons
                    List<EmployeeTO> emplOUResList = new Employee(Session[Constants.sessionConnection]).SearchEmployeesOUResponsible(Empl.OrgUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);
                    responsibility += rm.GetString("OUResPerson", culture) + Environment.NewLine;
                    foreach (EmployeeTO empl in emplOUResList)
                    {
                        responsibility += empl.FirstAndLastName.Trim() + Environment.NewLine;
                    }

                    tbStringone.ToolTip = responsibility.Trim();

                    string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                    int cost = 0;
                    bool costum = int.TryParse(costumer, out cost);
                    bool isFiat = (cost == (int)Constants.Customers.FIAT);

                    rowBranch.Visible = rowBranchtlbl.Visible = rowPlant.Visible = rowPlantlbl.Visible = rowCC.Visible = rowCClbl.Visible = rowWG.Visible = rowWGlbl.Visible = rowUTE.Visible = rowUTElbl.Visible = isFiat;
                    rowWU.Visible = rowWUlbl.Visible = !isFiat;

                    // create dinamically asco data (except Branch(nvarchar_value_6) which is already shown)
                    Dictionary<string, string> ascoMetadata = new EmployeeAsco4Metadata(Session[Constants.sessionConnection]).GetMetadataWebValues(CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]));

                    int counter = 0;
                    foreach (string col in ascoMetadata.Keys)
                    {
                        if (ascoMetadata[col].Trim() == "")
                            continue;

                        // make row separator label
                        Label separatorLbl = new Label();
                        separatorLbl.ID = "separatorLbl" + counter.ToString().Trim();
                        separatorLbl.Width = new Unit(105);
                        separatorLbl.Height = new Unit(5);
                        separatorLbl.CssClass = "contentLblLeft";
                        ascoCtrlHolder.Controls.Add(separatorLbl);

                        // make asco name label
                        Label ascoNameLbl = new Label();
                        ascoNameLbl.ID = "ascoNameLbl" + counter.ToString().Trim();
                        ascoNameLbl.Width = new Unit(105);
                        ascoNameLbl.Text = ascoMetadata[col].Trim() + ":";
                        ascoNameLbl.CssClass = "contentLblLeft";
                        ascoCtrlHolder.Controls.Add(ascoNameLbl);

                        // make pass type counter label
                        TextBox ascoTb = new TextBox();
                        ascoTb.ID = "ascoLbl" + counter.ToString().Trim();
                        ascoTb.Width = new Unit(105);
                        ascoTb.Text = Common.Misc.getAscoValue(ascoTO, col);
                        ascoTb.ReadOnly = true;
                        ascoTb.CssClass = "contentTbDisabled";
                        ascoCtrlHolder.Controls.Add(ascoTb);

                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populatePassTypes(int company, int emplTypeID)
        {
            try
            {
                List<PassTypeTO> typeList = new List<PassTypeTO>();
                Dictionary<int, PassTypeTO> passTypesAll = new PassType(Session[Constants.sessionConnection]).SearchDictionary();
                if (company != -1 && emplTypeID != -1 && Session[Constants.sessionLoginCategoryPassTypes] != null && Session[Constants.sessionLoginCategoryPassTypes] is List<int>)
                {
                    // add annual leave
                    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                    rule.RuleTO.EmployeeTypeID = emplTypeID;
                    rule.RuleTO.WorkingUnitID = company;
                    rule.RuleTO.RuleType = Constants.RuleCompanyAnnualLeave;
                    List<RuleTO> ruleList = rule.Search();

                    if (ruleList.Count > 0 && passTypesAll.ContainsKey(ruleList[0].RuleValue)
                        && ((List<int>)Session[Constants.sessionLoginCategoryPassTypes]).Contains(ruleList[0].RuleValue))
                    {
                        typeList.Add(passTypesAll[ruleList[0].RuleValue]);
                    }

                    // add not confirmed sick leave
                    rule.RuleTO.RuleType = Constants.RuleCompanySickLeaveNCF;
                    ruleList = rule.Search();

                    if (ruleList.Count > 0 && passTypesAll.ContainsKey(ruleList[0].RuleValue)
                        && ((List<int>)Session[Constants.sessionLoginCategoryPassTypes]).Contains(ruleList[0].RuleValue))
                    {
                        typeList.Add(passTypesAll[ruleList[0].RuleValue]);
                    }
                }

                cbPassType.DataSource = typeList;
                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                {
                    cbPassType.DataTextField = "DescriptionAndID";
                }
                else
                    cbPassType.DataTextField = "DescriptionAltAndID";
                cbPassType.DataValueField = "PassTypeID";
                cbPassType.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cbPassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbPassType.SelectedValue != null && (chbReport.Visible || btnPrint.Visible))
                {
                    EmployeeTO Empl = getEmployee();
                    Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                    int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);

                    // get employee annual leave
                    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                    rule.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                    rule.RuleTO.WorkingUnitID = company;
                    rule.RuleTO.RuleType = Constants.RuleCompanyAnnualLeave;
                    List<RuleTO> ruleList = rule.Search();

                    bool isAnnualLeaveSelected = false;
                    if (ruleList.Count > 0)
                    {
                        isAnnualLeaveSelected = (ruleList[0].RuleValue.ToString().Trim() == cbPassType.SelectedValue.Trim());
                    }

                    chbReport.Enabled = btnPrint.Enabled = isAnnualLeaveSelected;

                    if (!isAnnualLeaveSelected)
                        chbReport.Checked = false;
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCAnnualLeavePage.cbPassType_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCAnnualLeavePage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCAnnualLeavePage).Assembly);

                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblOverwritePairs.Text = rm.GetString("lblOverwritePairs", culture);
                lblEmplData.Text = rm.GetString("lblEmplData", culture);
                lblFirstName.Text = rm.GetString("lblFirstName", culture);
                lblLastName.Text = rm.GetString("lblLastName", culture);
                lblStringone.Text = rm.GetString("lblEmployeeType", culture);
                lblPlant.Text = rm.GetString("lblPlant", culture);
                lblCostCentar.Text = rm.GetString("lblCostCentar", culture);
                lblWorkgroup.Text = rm.GetString("lblWorkgroup", culture);
                lblUTE.Text = rm.GetString("lblUTE", culture);
                lblWUnit.Text = rm.GetString("lblWUnit", culture);
                lblBranch.Text = rm.GetString("lblBranch", culture);
                lblOUnit.Text = rm.GetString("lblOUnit", culture);
                lblPayroll.Text = rm.GetString("lblPayrollID", culture);
                lblPassType.Text = rm.GetString("lblPassType", culture);

                rbYes.Text = rm.GetString("yes", culture);
                rbNo.Text = rm.GetString("no", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                chbReport.Text = rm.GetString("chbPrintAnnualReport", culture);
                btnPrint.Text = rm.GetString("btnPrintAnnualReport", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeSQLParameters()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCAnnualLeavePage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrPassType", culture) + "," + rm.GetString("hdrDate", culture) + ","
                    + rm.GetString("hdrStartTime", culture) + "," + rm.GetString("hdrEndTime", culture) + "," + rm.GetString("hdrHours", culture);
                Session[Constants.sessionFields] = "pt, pairdate, pairstart, pairend, hours";
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(1, (int)Constants.FormatTypes.DateFormat);
                formating.Add(2, (int)Constants.FormatTypes.TimeFormat);
                formating.Add(3, (int)Constants.FormatTypes.TimeFormat);
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

                Session[Constants.sessionResultCurrentPage] = null;
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionItemsColors] = null;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private EmployeeTO getEmployee()
        {
            try
            {
                EmployeeTO emplTO = new EmployeeTO();
                int emplID = -1;

                if (Request.QueryString["emplID"] != null)
                {
                    if (!int.TryParse(Request.QueryString["emplID"], out emplID))
                        emplID = -1;
                }

                if (emplID != -1)
                {
                    emplTO = new Employee(Session[Constants.sessionConnection]).Find(emplID.ToString().Trim());
                }

                return emplTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClearPageSessionValues()
        {
            try
            {
                Session[sessionPairs] = null;
                Session[sessionOldPairs] = null;
                Session[sessionCounters] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCAnnualLeavePage).Assembly);

                ClearSessionValues();
                ClearPageSessionValues();

                lblError.Text = "";

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
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate.Date < new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                {
                    lblError.Text = rm.GetString("changingLessPreviousMonth", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;

                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                EmployeeTO empl = getEmployee();

                // get asco record
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(empl.EmployeeID.ToString().Trim());
                EmployeeAsco4TO emplAsco = new EmployeeAsco4TO();
                if (ascoDict.ContainsKey(empl.EmployeeID))
                    emplAsco = ascoDict[empl.EmployeeID];

                // cutt of date rule must be checked
                int company = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wUnits);
                int cutoffDate = -1;

                if (company != -1)
                {
                    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                    rule.RuleTO.WorkingUnitID = company;
                    rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                    rule.RuleTO.RuleType = Constants.RuleCutOffDate;

                    List<RuleTO> rulesList = rule.Search();

                    if (rulesList.Count == 1)
                    {
                        cutoffDate = rulesList[0].RuleValue;
                    }
                }

                if (fromDate.Date < new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                {
                    lblError.Text = rm.GetString("changingLessPreviousMonth", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }                
                else if (((cutoffDate != -1 && Common.Misc.countWorkingDays(DateTime.Now.Date, Session[Constants.sessionConnection]) > cutoffDate) || cutoffDate == -1) 
                    && fromDate.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                {
                    lblError.Text = rm.GetString("changingPreviousMonthCuttOffDatePassed", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }                

                // pass type validation
                if (cbPassType.SelectedIndex < 0)
                {
                    lblError.Text = rm.GetString("noSelectedPassType", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                int ptID = -1;
                if (!int.TryParse(cbPassType.SelectedValue.Trim(), out ptID))
                {
                    lblError.Text = rm.GetString("noSelectedPassType", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                PassTypeTO pt = new PassType(Session[Constants.sessionConnection]).Find(ptID);

                if (pt.PassTypeID == -1)
                {
                    lblError.Text = rm.GetString("noSelectedPassType", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                // validate passtypes limit occassionaly
                if (pt.LimitOccasionID != -1)
                {
                    // get limit value
                    PassTypeLimitTO limit = new PassTypeLimit(Session[Constants.sessionConnection]).Find(pt.LimitOccasionID);

                    if (limit.Type.Trim().ToUpper().Equals(Constants.ptLimitTypeOccassionaly.Trim().ToUpper()) && limit.Value < ((int)toDate.Date.Subtract(fromDate.Date).TotalDays + 1))
                    {
                        lblError.Text = rm.GetString("ptLimitOccassionalyReached", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }
                }

                // create list of columns for list privew of pairs for inserting
                List<DataColumn> resultColumns = new List<DataColumn>();
                resultColumns.Add(new DataColumn("pt", typeof(string)));
                resultColumns.Add(new DataColumn("pairdate", typeof(DateTime)));
                resultColumns.Add(new DataColumn("pairstart", typeof(DateTime)));
                resultColumns.Add(new DataColumn("pairend", typeof(DateTime)));
                resultColumns.Add(new DataColumn("hours", typeof(string)));

                // get all selected employees for inserting pairs                
                // if overwritting pairs is selected, all existing pairs for selected days will be moved to hist table so get all selected employees
                // if overwritting pairs is not selected, remove all employees that has at least one pair different than unjustified for one of selected days
                // get all pairs for selected employees for selected days - get one day more for third shifts                
                List<DateTime> datesList = new List<DateTime>();
                DateTime currDate = fromDate.Date;
                while (currDate <= toDate.AddDays(1).Date)
                {
                    datesList.Add(currDate);
                    currDate = currDate.AddDays(1);
                }

                // get all rules
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();

                // get rules for employeeID and its company
                Dictionary<string, RuleTO> rules = new Dictionary<string, RuleTO>();

                if (emplRules.ContainsKey(company) && emplRules[company].ContainsKey(empl.EmployeeTypeID))
                    rules = emplRules[company][empl.EmployeeTypeID];

                //Common.Rule emplRule = new Common.Rule(Session[Constants.sessionConnection]);
                //emplRule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                //emplRule.RuleTO.WorkingUnitID = company;

                //List<RuleTO> emplRules = emplRule.Search();

                //foreach (RuleTO ruleTO in emplRules)
                //{
                //    if (!rules.ContainsKey(ruleTO.RuleType))
                //        rules.Add(ruleTO.RuleType, ruleTO);
                //    else
                //        rules[ruleTO.RuleType] = ruleTO;
                //}

                if (rbNo.Checked)
                {
                    List<IOPairProcessedTO> emplPairsList = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(empl.EmployeeID.ToString(), datesList, "");

                    List<int> emplPairIDs = new List<int>();

                    int holidayPT = -1;
                    int personalHolidayPT = -1;

                    if (rules.ContainsKey(Constants.RuleHolidayPassType))
                        holidayPT = rules[Constants.RuleHolidayPassType].RuleValue;

                    if (rules.ContainsKey(Constants.RulePersonalHolidayPassType))
                        personalHolidayPT = rules[Constants.RulePersonalHolidayPassType].RuleValue;

                    //**********night shift
                    foreach (IOPairProcessedTO pair in emplPairsList)
                    {
                        if (pair.PassTypeID != Constants.absence && pair.PassTypeID != holidayPT && pair.PassTypeID != personalHolidayPT
                            && !emplPairIDs.Contains(pair.EmployeeID) && ((pair.IOPairDate.Date > fromDate.Date && pair.IOPairDate.Date <= toDate.Date)
                            || (pair.IOPairDate.Date == fromDate.Date && !(pair.StartTime.Hour == 0 && pair.StartTime.Minute == 0))
                            || (pair.IOPairDate.Date > toDate.Date && pair.StartTime.Hour == 0 && pair.StartTime.Minute == 0)))
                            emplPairIDs.Add(pair.EmployeeID);
                    }

                    // remove from employee list employees that has at least one pair in selected period                    
                    if (emplPairIDs.Contains(empl.EmployeeID))
                    {
                        lblError.Text = rm.GetString("noAbsenceAllowedEmployees", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }
                }

                // get time schedules and time schemas for selected employees for selected days, add one day for third shifts
                Dictionary<int, List<EmployeeTimeScheduleTO>> employeeSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(empl.EmployeeID.ToString(), fromDate.Date, toDate.AddDays(1).Date, null);
                List<EmployeeTimeScheduleTO> emplSchedules = new List<EmployeeTimeScheduleTO>();
                if (employeeSchedules.ContainsKey(empl.EmployeeID))
                    emplSchedules = employeeSchedules[empl.EmployeeID];

                string schemaID = "";
                foreach (EmployeeTimeScheduleTO employeeTimeSchedule in emplSchedules)
                {
                    schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                }
                if (!schemaID.Equals(""))
                {
                    schemaID = schemaID.Substring(0, schemaID.Length - 2);
                }

                List<WorkTimeSchemaTO> timeSchema = new TimeSchema(Session[Constants.sessionConnection]).Search(schemaID);
                Dictionary<int, WorkTimeSchemaTO> schemas = new Dictionary<int, WorkTimeSchemaTO>();
                foreach (WorkTimeSchemaTO sch in timeSchema)
                {
                    if (!schemas.ContainsKey(sch.TimeSchemaID))
                        schemas.Add(sch.TimeSchemaID, sch);
                }

                // get employee holidays
                List<DateTime> personalHolidays = new List<DateTime>();
                List<DateTime> emplHolidays = employeeHolidays(ref personalHolidays, fromDate.Date, toDate.Date, empl.EmployeeID, emplSchedules, schemas, empl, emplRules);

                // get old pairs for employees for selected days, and for next day becouse of third shifts
                List<IOPairProcessedTO> oldPairs = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(empl.EmployeeID.ToString().Trim(), datesList, "");

                // create dictionary of old pairs list for each employee by date
                Dictionary<DateTime, List<IOPairProcessedTO>> emplOldPairs = new Dictionary<DateTime, List<IOPairProcessedTO>>();
                foreach (IOPairProcessedTO oldPair in oldPairs)
                {
                    if (!emplOldPairs.ContainsKey(oldPair.IOPairDate.Date))
                        emplOldPairs.Add(oldPair.IOPairDate.Date, new List<IOPairProcessedTO>());

                    emplOldPairs[oldPair.IOPairDate.Date].Add(oldPair);
                }

                // get old counters
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplOldCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(empl.EmployeeID.ToString().Trim());

                // get all pass types
                Dictionary<int, PassTypeTO> passTypesAll = new PassType(Session[Constants.sessionConnection]).SearchDictionary();

                // get pass types limits
                List<PassTypeLimitTO> ptLimitsList = new PassTypeLimit(Session[Constants.sessionConnection]).Search();
                Dictionary<int, PassTypeLimitTO> ptLimits = new Dictionary<int, PassTypeLimitTO>();

                foreach (PassTypeLimitTO limit in ptLimitsList)
                {
                    if (!ptLimits.ContainsKey(limit.PtLimitID))
                        ptLimits.Add(limit.PtLimitID, limit);
                    else
                        ptLimits[limit.PtLimitID] = limit;
                }

                List<List<object>> resultTable = new List<List<object>>();

                List<IOPairProcessedTO> emplPairs = new List<IOPairProcessedTO>();
                List<IOPairProcessedTO> emplValidPairs = new List<IOPairProcessedTO>();

                string error = "";

                // go through one day more if employee worked third shift for last selected day                
                // old pairs which will be moved to hist table
                List<IOPairProcessedTO> oldPairsList = new List<IOPairProcessedTO>();

                DateTime date = fromDate.Date;
                Dictionary<DateTime, WorkTimeSchemaTO> daySchemas = new Dictionary<DateTime, WorkTimeSchemaTO>();
                Dictionary<DateTime, List<WorkTimeIntervalTO>> dayIntervalsList = new Dictionary<DateTime,List<WorkTimeIntervalTO>>();
                bool nightFlexyFound = false;
                while (date <= toDate.AddDays(1).Date)
                {
                    DateTime dayBegining = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    DateTime dayEnd = new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);

                    // get intervals for employee/date                    
                    List<WorkTimeIntervalTO> timeSchemaIntervalList = Common.Misc.getTimeSchemaInterval(date.Date, emplSchedules, schemas);

                    if (!dayIntervalsList.ContainsKey(date.Date))
                        dayIntervalsList.Add(date.Date, timeSchemaIntervalList);
                    else
                        dayIntervalsList[date.Date] = timeSchemaIntervalList;
                    
                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (timeSchemaIntervalList.Count > 0 && schemas.ContainsKey(timeSchemaIntervalList[0].TimeSchemaID))
                        schema = schemas[timeSchemaIntervalList[0].TimeSchemaID];

                    if (schema.Type == Constants.schemaTypeNightFlexi)
                    {
                        nightFlexyFound = true;
                        break;
                    }

                    if (!daySchemas.ContainsKey(date.Date))
                        daySchemas.Add(date.Date, schema);
                    else
                        daySchemas[date.Date] = schema;

                    // whole day absences
                    // validate pairs and insert pairs if valid for all intervals
                    foreach (WorkTimeIntervalTO interval in timeSchemaIntervalList)
                    {
                        // do not insert pairs for interval of 0 minutes
                        if (interval.StartTime.TimeOfDay.Equals(interval.EndTime.TimeOfDay))
                            continue;

                        // first day - do not insert second night shift interval, it belongs to previous day
                        if (date.Date.Equals(fromDate.Date) && Common.Misc.isThirdShiftEndInterval(interval))
                        {
                            // remove old pairs from this interval
                            removeIntervalPairs(ref emplOldPairs, date.Date, interval, schema);
                            continue;
                        }

                        // last day - insert second night shift interval from next day
                        if (date.Date.Equals(toDate.AddDays(1).Date) && !Common.Misc.isThirdShiftEndInterval(interval))
                        {
                            // remove old pairs from this interval
                            removeIntervalPairs(ref emplOldPairs, date.Date, interval, schema);
                            continue;
                        }

                        // do not insert annual or paid leaves if day is employees holiday
                        if ((rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && pt.PassTypeID == rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                            || (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && pt.PassTypeID == rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue) 
                            || pt.LimitCompositeID != -1 || pt.LimitElementaryID != -1 || pt.LimitOccasionID != -1)
                        {
                            if (!schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()))
                            {
                                if (emplHolidays.Contains(date.AddDays(-1).Date) && Common.Misc.isThirdShiftEndInterval(interval))
                                {
                                    removeIntervalPairs(ref emplOldPairs, date.Date, interval, schema);
                                    continue;
                                }

                                if (emplHolidays.Contains(date.Date) && !Common.Misc.isThirdShiftEndInterval(interval))
                                {
                                    removeIntervalPairs(ref emplOldPairs, date.Date, interval, schema);
                                    continue;
                                }
                            }
                            else
                            {
                                if (personalHolidays.Contains(date.AddDays(-1).Date) && Common.Misc.isThirdShiftEndInterval(interval))
                                {
                                    removeIntervalPairs(ref emplOldPairs, date.Date, interval, schema);
                                    continue;
                                }

                                if (personalHolidays.Contains(date.Date) && !Common.Misc.isThirdShiftEndInterval(interval))
                                {
                                    removeIntervalPairs(ref emplOldPairs, date.Date, interval, schema);
                                    continue;
                                }
                            }
                        }

                        DateTime start = CommonWeb.Misc.getIntervalStart(interval, new List<IOPairProcessedTO>(), schema, new DateTime(), new Dictionary<int,PassTypeTO>());
                        DateTime end = CommonWeb.Misc.getIntervalEnd(interval, new List<IOPairProcessedTO>(), schema, new DateTime(), new Dictionary<int,PassTypeTO>());

                        start = new DateTime(date.Year, date.Month, date.Day, start.Hour, start.Minute, 0);
                        end = new DateTime(date.Year, date.Month, date.Day, end.Hour, end.Minute, 0);

                        IOPairProcessedTO pair = createPair(empl.EmployeeID, pt, date.Date, start, end);                            

                        emplPairs.Add(pair);
                    }

                    if (emplOldPairs.ContainsKey(date.Date))
                        oldPairsList.AddRange(emplOldPairs[date.Date]);

                    date = date.AddDays(1);
                }

                if (nightFlexyFound)
                {
                    lblError.Text = rm.GetString("nightFlexyChangesNotAllowed", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                // old counters
                Dictionary<int, EmployeeCounterValueTO> oldCounters = new Dictionary<int, EmployeeCounterValueTO>();
                if (emplOldCounters.ContainsKey(empl.EmployeeID))
                    oldCounters = emplOldCounters[empl.EmployeeID];

                if (emplPairs.Count > 0)
                {
                    // get annual leaves from from and to week
                    List<IOPairProcessedTO> fromWeekAnnualPairs = new List<IOPairProcessedTO>();
                    List<IOPairProcessedTO> toWeekAnnualPairs = new List<IOPairProcessedTO>();
                                        
                    IOPairProcessed annualPair = new IOPairProcessed(Session[Constants.sessionConnection]);
                    
                    string ptIDs = Common.Misc.getAnnualLeaveTypesString(rules);
                    fromWeekAnnualPairs = annualPair.SearchWeekPairs(empl.EmployeeID, fromDate.Date, false, ptIDs, null);
                    toWeekAnnualPairs = annualPair.SearchWeekPairs(empl.EmployeeID, toDate.Date, false, ptIDs, null);

                    // remove pairs that are in selected period
                    IEnumerator<IOPairProcessedTO> pairEnumerator = fromWeekAnnualPairs.GetEnumerator();
                    while (pairEnumerator.MoveNext())
                    {
                        if (pairEnumerator.Current.IOPairDate.Date >= fromDate.Date)
                        {
                            fromWeekAnnualPairs.Remove(pairEnumerator.Current);
                            pairEnumerator = fromWeekAnnualPairs.GetEnumerator();
                        }
                    }

                    pairEnumerator = toWeekAnnualPairs.GetEnumerator();
                    while (pairEnumerator.MoveNext())
                    {
                        if (pairEnumerator.Current.IOPairDate.Date <= toDate.Date)
                        {
                            toWeekAnnualPairs.Remove(pairEnumerator.Current);
                            pairEnumerator = toWeekAnnualPairs.GetEnumerator();
                        }
                    }

                    Dictionary<int, int> paidLeavesElementaryPairsDict = new Dictionary<int, int>();
                    error = Common.Misc.validatePairsPassType(empl.EmployeeID, emplAsco, fromDate.Date, toDate.Date, emplPairs, oldPairsList, new List<IOPairProcessedTO>(), ref oldCounters, rules,
                                passTypesAll, ptLimits, schemas, daySchemas, dayIntervalsList, fromWeekAnnualPairs, toWeekAnnualPairs, paidLeavesElementaryPairsDict, 
                                new List<IOPairProcessedTO>(), new List<DateTime>(), null, Session[Constants.sessionConnection], CommonWeb.Misc.checkLimit(Session[Constants.sessionLoginCategory]), false, true, false);
                    //error = validatePairsPassType(fromDate.Date, toDate.Date, emplPairs, oldPairsList, ref oldCounters, rules, passTypesAll, ptLimits, daySchemas,
                    //    fromWeekAnnualPairs, toWeekAnnualPairs);

                    if (error.Trim().Equals(""))
                    {
                        if (emplOldCounters.ContainsKey(empl.EmployeeID))
                            emplOldCounters[empl.EmployeeID] = oldCounters;

                        emplValidPairs.AddRange(emplPairs);
                    }
                    else
                    {
                        lblError.Text = rm.GetString(error.Trim(), culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }
                }

                foreach (IOPairProcessedTO pair in emplValidPairs)
                {
                    resultTable.Add(createResultRow(empl, pair));
                }

                Session[sessionPairs] = emplValidPairs;
                Session[sessionOldPairs] = emplOldPairs;
                Session[sessionCounters] = emplOldCounters;
                Session[Constants.sessionDataTableList] = resultTable;
                Session[Constants.sessionDataTableColumns] = resultColumns;

                // save selected filter state
                SaveState();

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCAnnualLeavePage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCAnnualLeavePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
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
                Session["TLAnnualLeavePage.ds"] = null;
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCAnnualLeavePage).Assembly);

                if (Session[sessionPairs] != null && Session[sessionPairs] is List<IOPairProcessedTO>)
                {
                    // get old counters
                    EmployeeTO empl = getEmployee();
                    Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplOldCompareCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(empl.EmployeeID.ToString().Trim());

                    IOPairProcessed pair = new IOPairProcessed(Session[Constants.sessionConnection]);

                    if (pair.BeginTransaction())
                    {
                        try
                        {
                            EmployeeCounterValue counter = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                            IOPairsProcessedHist pairHist = new IOPairsProcessedHist(Session[Constants.sessionConnection]);
                            EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist(Session[Constants.sessionConnection]);
                            bool saved = true;

                            // get old pairs for this employee
                            List<IOPairProcessedTO> oldPairs = new List<IOPairProcessedTO>();
                            if (Session[sessionOldPairs] != null && Session[sessionOldPairs] is Dictionary<DateTime, List<IOPairProcessedTO>>)
                            {
                                foreach (DateTime date in ((Dictionary<DateTime, List<IOPairProcessedTO>>)Session[sessionOldPairs]).Keys)
                                {
                                    oldPairs.AddRange(((Dictionary<DateTime, List<IOPairProcessedTO>>)Session[sessionOldPairs])[date]);
                                }
                            }

                            // get counters for this employee
                            Dictionary<int, EmployeeCounterValueTO> counters = new Dictionary<int, EmployeeCounterValueTO>();
                            if (Session[sessionCounters] != null && Session[sessionCounters] is Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>
                                && ((Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>)Session[sessionCounters]).ContainsKey(empl.EmployeeID))
                                counters = ((Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>)Session[sessionCounters])[empl.EmployeeID];

                            // get compare counters
                            Dictionary<int, EmployeeCounterValueTO> oldCounters = new Dictionary<int, EmployeeCounterValueTO>();
                            if (emplOldCompareCounters.ContainsKey(empl.EmployeeID))
                                oldCounters = emplOldCompareCounters[empl.EmployeeID];

                            // get new pairs for this employee
                            List<IOPairProcessedTO> newPairs = (List<IOPairProcessedTO>)Session[sessionPairs];

                            // move old pairs to hist table
                            string recIDs = "";
                            if (oldPairs.Count > 0)
                            {
                                foreach (IOPairProcessedTO oldPair in oldPairs)
                                {
                                    recIDs += oldPair.RecID.ToString().Trim() + ",";
                                }

                                if (recIDs.Length > 0)
                                    recIDs = recIDs.Substring(0, recIDs.Length - 1);
                            }

                            if (recIDs.Length > 0)
                            {
                                pairHist.SetTransaction(pair.GetTransaction());
                                saved = saved && (pairHist.Save(recIDs, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), DateTime.Now, Constants.alertStatus, false) >= 0);

                                if (saved)
                                    saved = saved && pair.Delete(recIDs, false);
                            }

                            if (saved)
                            {
                                foreach (IOPairProcessedTO pairTO in newPairs)
                                {
                                    if ((int)pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes > 0)
                                    {
                                        pair.IOPairProcessedTO = pairTO;
                                        saved = saved && (pair.Save(false) >= 0);

                                        if (!saved)
                                            break;
                                    }
                                }
                            }

                            if (saved)
                            {
                                // update counters from session, updated counters insert to hist table
                                counterHist.SetTransaction(pair.GetTransaction());
                                counter.SetTransaction(pair.GetTransaction());
                                // update counters and move old counter values to hist table if updated
                                foreach (int type in counters.Keys)
                                {
                                    if (oldCounters.ContainsKey(type) && oldCounters[type].Value != counters[type].Value)
                                    {
                                        // move to hist table
                                        counterHist.ValueTO = new EmployeeCounterValueHistTO(oldCounters[type]);
                                        counterHist.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                        saved = saved && (counterHist.Save(false) >= 0);

                                        if (!saved)
                                            break;

                                        counter.ValueTO = new EmployeeCounterValueTO(counters[type]);
                                        //counter.ValueTO.EmplCounterTypeID = type;
                                        //counter.ValueTO.EmplID = empl.EmployeeID;
                                        //counter.ValueTO.Value = counters[type];
                                        counter.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                                        saved = saved && counter.Update(false);

                                        if (!saved)
                                            break;
                                    }
                                }
                            }

                            if (saved)
                            {
                                pair.CommitTransaction();
                                //ClearPageSessionValues();
                                //ClearSessionValues();
                                lblError.Text = rm.GetString("pairsSaved", culture);
                                if (chbReport.Checked)
                                {
                                    
                                    
                                    Dictionary<int, WorkingUnitTO> wuDict = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                                    Dictionary<int, List<IOPairProcessedTO>> ioPairs = new Dictionary<int, List<IOPairProcessedTO>>();
                                    List<IOPairProcessedTO> pairs = (List<IOPairProcessedTO>)Session[sessionPairs];  //(Dictionary<int, List<IOPairProcessedTO>>)Session[sessionPairs];
                                    ioPairs.Add(empl.EmployeeID, pairs);
                                    Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>(); // (Dictionary<int, EmployeeTO>)Session["TLAnnualLeavePage.employeeDict"];
                                    employees.Add(empl.EmployeeID, empl);
                                    // get asco data
                                    Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(empl.EmployeeID.ToString());
                                 //   Dictionary<int, EmployeeAsco4TO> ascoDict = (Dictionary<int, EmployeeAsco4TO>)Session["TLAnnualLeavePage.employeeAsco4Dict"];
                                    Dictionary<int, int> emplAfterAL = new Dictionary<int, int>();

                                    DataSet ds = new DataSet();
                                    DataTable table = new DataTable();
                                    table.Columns.Add("employee", typeof(string));
                                    table.Columns.Add("working_unit", typeof(string));
                                    table.Columns.Add("inital_an", typeof(string));
                                    table.Columns.Add("length", typeof(string));
                                    table.Columns.Add("start", typeof(string));
                                    table.Columns.Add("end", typeof(string));
                                    table.Columns.Add("from_previous", typeof(string));
                                    table.Columns.Add("from_current", typeof(string));
                                    table.Columns.Add("total_left", typeof(string));
                                    table.Columns.Add("previous_left", typeof(string));
                                    table.Columns.Add("current_left", typeof(string));
                                    table.Columns.Add("hiring_date", typeof(string));
                                    table.Columns.Add("contract", typeof(string));
                                    table.Columns.Add("printDate", typeof(string));
                                    ds.Tables.Add(table);

                                    RuleTO ruleTo = new RuleTO();
                                    ruleTo.RuleType = Constants.RulePrintPaidLeaveReportOffset;
                                    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                                    rule.RuleTO = ruleTo;
                                    List<RuleTO> rulesList = rule.Search();

                                    foreach (KeyValuePair<int, List<IOPairProcessedTO>> IoPair in ioPairs)
                                    {
                                        int annual_leave_count = 0;
                                        foreach (IOPairProcessedTO pairProc in IoPair.Value)
                                        {
                                            if (!pairProc.StartTime.ToString("HH:mm").Equals("00:00"))
                                                annual_leave_count++;
                                        }

                                        if (annual_leave_count > 0)
                                        {
                                            DataRow row = table.NewRow();
                                            if (employees.ContainsKey(IoPair.Key))
                                            {
                                                row[0] = employees[IoPair.Key].FirstAndLastName;
                                                if (wuDict.ContainsKey(employees[IoPair.Key].WorkingUnitID))
                                                    row[1] = wuDict[employees[IoPair.Key].WorkingUnitID].Description;
                                            }

                                            if (emplOldCompareCounters.ContainsKey(IoPair.Key))
                                            {
                                                if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                                                    row[2] = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                                                if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                                {
                                                    int previous_year = 0;
                                                    int current_year = 0;
                                                    int used = 0;
                                                    if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter))
                                                        previous_year = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value;

                                                    if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                                                        current_year = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                                                    if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                                        used = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value;


                                                    used += annual_leave_count;
                                                    int from_current = 0;
                                                    int from_previous = 0;
                                                    int current_left = 0;
                                                    int prev_left = previous_year - used;

                                                    int previous_left = 0;

                                                    int iopairused = previous_year - used;
                                                    if (iopairused < 0)
                                                    {
                                                        from_previous = iopairused + annual_leave_count;
                                                        if (from_previous < 0)
                                                        {
                                                            from_previous = 0;
                                                            from_current = annual_leave_count;
                                                        }
                                                        else
                                                            from_current = Math.Abs(iopairused);

                                                        previous_left = 0;
                                                        current_left = current_year + iopairused;
                                                    }
                                                    else
                                                    {
                                                        current_left = current_year;
                                                        previous_left = iopairused;
                                                        int count = iopairused - annual_leave_count;
                                                        //if (count < 0)
                                                        //{
                                                        //    if (Math.Abs(count) > annual_leave_count)
                                                        //    {
                                                        //        from_previous = 0;
                                                        //        from_current = annual_leave_count;
                                                        //    }
                                                        //    else
                                                        //    {
                                                        from_previous = annual_leave_count;
                                                        from_current = annual_leave_count - from_previous;
                                                        //    }

                                                        //}
                                                        //else
                                                        //{
                                                        //from_previous = annual_leave_count;
                                                        //from_current = 0;

                                                        //}
                                                    }
                                                    row[6] = from_previous;
                                                    row[7] = from_current;

                                                    row[8] = previous_left + current_left;
                                                    row[9] = previous_left;
                                                    row[10] = current_left;

                                                }
                                            }
                                            row[3] = annual_leave_count;
                                            row[4] = IoPair.Value[0].IOPairDate.ToString("dd.MM.yyyy.");
                                            row[5] = IoPair.Value[IoPair.Value.Count - 1].IOPairDate.ToString("dd.MM.yyyy.");


                                            if (ascoDict.ContainsKey(IoPair.Key))
                                            {
                                                row[11] = ascoDict[IoPair.Key].DatetimeValue5.ToString("dd.MM.yyyy.");
                                                if (!ascoDict[IoPair.Key].NVarcharValue10.Equals("")) row[12] = ascoDict[IoPair.Key].NVarcharValue10;
                                                else row[12] = "      ";
                                            }


                                            DateTime offsetDate = new DateTime();
                                            if (rulesList.Count > 0)
                                            {

                                                if (DateTime.Now.Date <= IoPair.Value[0].IOPairDate.Date)
                                                {
                                                    if (IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue).Date < DateTime.Now.Date)
                                                        offsetDate = DateTime.Now;
                                                    else
                                                        offsetDate = IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue);
                                                }
                                                else
                                                {

                                                    offsetDate = IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue);
                                                }
                                            }
                                            else
                                            {
                                                if (DateTime.Now < IoPair.Value[0].IOPairDate)
                                                    offsetDate = DateTime.Now;
                                                else offsetDate = IoPair.Value[0].IOPairDate;
                                            }
                                            row[13] = offsetDate.ToString("dd.MM.yyyy");

                                            if (ascoDict.ContainsKey(IoPair.Key))
                                            {
                                                int positionID = ascoDict[IoPair.Key].IntegerValue6;
                                                if (positionID != -1)
                                                {
                                                    EmployeePosition posit = new EmployeePosition();
                                                    posit.EmplPositionTO.PositionID = positionID;
                                                    List<EmployeePositionTO> position = posit.SearchEmployeePositions();
                                                    row[14] = position[0].PositionCodeTitleSR;
                                                }
                                                else
                                                {
                                                    row[14] = "/";
                                                }
                                            }

                                            table.Rows.Add(row);

                                        }
                                    }
                                    ClearPageSessionValues();
                                    ClearSessionValues();
                                    if (table.Rows.Count > 0)
                                    {
                                        string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                                        int cost = 0;
                                        bool costum = int.TryParse(costumer, out cost);
                                        bool isHyatt = (cost == (int)Constants.Customers.Hyatt);

                                        Session["TLAnnualLeavePage.ds"] = ds;

                                        string wOptions = "dialogWidth:800px; dialogHeight:600px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                                        if (isHyatt)
                                            ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReportHyatt.aspx', window, '" + wOptions.Trim() + "');", true);

                                    }
                                }
                            }
                            else
                            {
                                if (pair.GetTransaction() != null)
                                    pair.RollbackTransaction();
                                lblError.Text = rm.GetString("pairsSavingFaild", culture);
                            }
                        }
                        catch
                        {
                            if (pair.GetTransaction() != null)
                                pair.RollbackTransaction();
                            lblError.Text = rm.GetString("pairsSavingFaild", culture);
                        }
                    }
                    else
                        lblError.Text = rm.GetString("pairsSavingFaild", culture);
                }
                else
                {
                    lblError.Text = rm.GetString("noPairsForSaving", culture);
                }

                ClearPageSessionValues();
                ClearSessionValues();
                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCAnnualLeavePage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCAnnualLeavePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrint_Click(Object sender, EventArgs e)
        {
            try
            {
                Session["TLAnnualLeavePage.ds"] = null;
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLAnnualLeavePage).Assembly);

                ClearSessionValues();
                ClearPageSessionValues();

                lblError.Text = "";
                EmployeeTO empl = getEmployee();

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
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
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

                //List<EmployeeTO> emplList = new Employee(Session[Constants.sessionConnection]).Search(empl.EmployeeID.ToString());
                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                // get asco data
                Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(empl.EmployeeID.ToString());

                List<DateTime> datesList = new List<DateTime>();
                DateTime currDate = fromDate.Date;
                while (currDate < toDate.AddDays(1).Date)
                {
                    datesList.Add(currDate);
                    currDate = currDate.AddDays(1);
                }
                // dictioanry of employees
                Dictionary<int, EmployeeTO> employees = new Dictionary<int, EmployeeTO>();
                if (!employees.ContainsKey(empl.EmployeeID))
                        employees.Add(empl.EmployeeID, empl);

                List<IOPairProcessedTO> emplPairsList = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(empl.EmployeeID.ToString(), datesList, "");
                Dictionary<int, List<IOPairProcessedTO>> emplValidPairs = new Dictionary<int, List<IOPairProcessedTO>>();
                Dictionary<int, int> emplAfterAL = new Dictionary<int, int>();
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();
                // get all pass types
                Dictionary<int, PassTypeTO> passTypesAll = new PassType(Session[Constants.sessionConnection]).SearchDictionary();
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplOldCompareCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(empl.EmployeeID.ToString());
                if (emplPairsList.Count <= 0)
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    return;
                }


                foreach (IOPairProcessedTO pair in emplPairsList)
                {

                    // get rules for employeeID and its company
                    Dictionary<string, RuleTO> rules = new Dictionary<string, RuleTO>();

                    int company = -1;
                    int emplTypeID = -1;
                    if (employees.ContainsKey(pair.EmployeeID))
                    {
                        company = Common.Misc.getRootWorkingUnit(employees[pair.EmployeeID].WorkingUnitID, wUnits);
                        emplTypeID = employees[pair.EmployeeID].EmployeeTypeID;
                    }
                    if (company != -1 && emplTypeID != -1 && emplRules.ContainsKey(company) && emplRules[company].ContainsKey(emplTypeID))
                        rules = emplRules[company][emplTypeID];

                    int emplID = pair.EmployeeID;


                    // get company annualLeave
                    PassTypeTO ptAL = new PassTypeTO();
                    if (rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && passTypesAll.ContainsKey(rules[Constants.RuleCompanyAnnualLeave].RuleValue))
                    {
                        ptAL = passTypesAll[rules[Constants.RuleCompanyAnnualLeave].RuleValue];
                    }
                    else
                    {
                        lblError.Text = rm.GetString("noCompanyAnnualLeave", culture);
                        writeLog(DateTime.Now, false);
                        return;
                    }

                    PassTypeTO ptCAL = new PassTypeTO();
                    if (rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && passTypesAll.ContainsKey(rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue))
                    {
                        ptCAL = passTypesAll[rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue];
                    }
                    if (pair.PassTypeID == ptAL.PassTypeID || pair.PassTypeID == ptCAL.PassTypeID)
                    {

                        if (!emplValidPairs.ContainsKey(pair.EmployeeID))
                            emplValidPairs.Add(pair.EmployeeID, new List<IOPairProcessedTO>());

                        emplValidPairs[pair.EmployeeID].Add(pair);
                    }
                    if (!emplAfterAL.ContainsKey(pair.EmployeeID))
                        emplAfterAL.Add(pair.EmployeeID, 0);

                    int alAfterAl = new IOPairProcessed(Session[Constants.sessionConnection]).SearchCollectiveAnnualLeaves(pair.EmployeeID, ptAL.PassTypeID, toDate.AddDays(1), new List<DateTime>());
                    int alAfterCal = new IOPairProcessed(Session[Constants.sessionConnection]).SearchCollectiveAnnualLeaves(pair.EmployeeID, ptCAL.PassTypeID, toDate.AddDays(1), new List<DateTime>());

                    emplAfterAL[pair.EmployeeID] = alAfterAl + alAfterCal;
                }

                if (emplValidPairs.Count <= 0)
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    return;
                }
                DataSet ds = new DataSet();
                DataTable table = new DataTable();
                table.Columns.Add("employee", typeof(string));

                table.Columns.Add("working_unit", typeof(string));
                table.Columns.Add("inital_an", typeof(string));
                table.Columns.Add("length", typeof(string));
                table.Columns.Add("start", typeof(string));
                table.Columns.Add("end", typeof(string));
                table.Columns.Add("from_previous", typeof(string));
                table.Columns.Add("from_current", typeof(string));
                table.Columns.Add("total_left", typeof(string));
                table.Columns.Add("previous_left", typeof(string));
                table.Columns.Add("current_left", typeof(string));
                table.Columns.Add("hiring_date", typeof(string));
                table.Columns.Add("contract", typeof(string));
                table.Columns.Add("printDate", typeof(string));
                ds.Tables.Add(table);


                RuleTO ruleTo = new RuleTO();
                ruleTo.RuleType = Constants.RulePrintPaidLeaveReportOffset;
                Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                rule.RuleTO = ruleTo;
                List<RuleTO> rulesList = rule.Search();

                foreach (KeyValuePair<int, List<IOPairProcessedTO>> IoPair in emplValidPairs)
                {
                    int annual_leave_count = 0;
                    foreach (IOPairProcessedTO pairProc in IoPair.Value)
                    {
                        if (!pairProc.StartTime.ToString("HH:mm").Equals("00:00") || (int)pairProc.EndTime.Subtract(pairProc.StartTime).TotalHours == Constants.dayDurationStandardShift)
                            annual_leave_count++;
                    }

                    if (annual_leave_count > 0)
                    {
                        DataRow row = table.NewRow();
                        if (employees.ContainsKey(IoPair.Key))
                        {
                            row[0] = employees[IoPair.Key].FirstAndLastName;
                            if (wUnits.ContainsKey(employees[IoPair.Key].WorkingUnitID))
                                row[1] = wUnits[employees[IoPair.Key].WorkingUnitID].Description;
                        }

                        if (emplOldCompareCounters.ContainsKey(IoPair.Key))
                        {
                            if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                                row[2] = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;
                            int previous_year = 0;
                            int current_year = 0;
                            int used = 0;
                            if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter))
                                previous_year = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value;

                            if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                                current_year = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;

                            if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                                used = emplOldCompareCounters[IoPair.Key][(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value;

                            if (emplAfterAL.ContainsKey(IoPair.Key))
                                used -= emplAfterAL[IoPair.Key];

                            if (emplOldCompareCounters[IoPair.Key].ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                            {
                                //  used += IoPair.Value.Count;
                                int from_current = 0;
                                int from_previous = 0;
                                int previous_left = 0;
                                int current_left = 0;

                                int iopairused = previous_year - used;

                                if (iopairused <= 0)
                                {
                                    from_previous = iopairused + annual_leave_count;
                                    if (from_previous < 0)
                                    {
                                        from_previous = 0;
                                        from_current = annual_leave_count;
                                    }
                                    else
                                        from_current = Math.Abs(iopairused);

                                    previous_left = 0;
                                    current_left = current_year + iopairused;
                                }
                                else
                                {
                                    current_left = current_year;
                                    previous_left = iopairused;
                                    int count = iopairused - annual_leave_count;
                                    //if (count < 0)
                                    //{
                                    //    if (Math.Abs(count) > annual_leave_count)
                                    //    {
                                    //        from_previous = 0;
                                    //        from_current = annual_leave_count;
                                    //    }
                                    //    else
                                    //    {
                                    from_previous = annual_leave_count;
                                    from_current = annual_leave_count - from_previous;
                                    //    }

                                    //}
                                    //else
                                    //{
                                    //from_previous = annual_leave_count;
                                    //from_current = 0;

                                    //}
                                }
                                row[6] = from_previous;
                                row[7] = from_current;

                                row[8] = previous_left + current_left;
                                row[9] = previous_left;
                                row[10] = current_left;

                            }
                        }
                        row[3] = annual_leave_count;
                        row[4] = IoPair.Value[0].IOPairDate.ToString("dd.MM.yyyy");
                        row[5] = IoPair.Value[IoPair.Value.Count - 1].IOPairDate.ToString("dd.MM.yyyy");


                        if (ascoDict.ContainsKey(IoPair.Key))
                        {
                            row[11] = ascoDict[IoPair.Key].DatetimeValue5.ToString("dd.MM.yyyy");
                            if (!ascoDict[IoPair.Key].NVarcharValue10.Equals("")) row[12] = ascoDict[IoPair.Key].NVarcharValue10;
                            else row[12] = "      ";
                        }

                        DateTime offsetDate = new DateTime();
                        if (rulesList.Count > 0)
                        {

                            if (DateTime.Now.Date <= IoPair.Value[0].IOPairDate.Date)
                            {
                                if (IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue).Date < DateTime.Now.Date)
                                    offsetDate = DateTime.Now;
                                else
                                    offsetDate = IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue);
                            }
                            else
                            {

                                offsetDate = IoPair.Value[0].IOPairDate.AddDays(-rulesList[0].RuleValue);
                            }
                        }
                        else
                        {
                            if (DateTime.Now < IoPair.Value[0].IOPairDate)
                                offsetDate = DateTime.Now;
                            else offsetDate = IoPair.Value[0].IOPairDate;
                        }
                        row[13] = offsetDate.ToString("dd.MM.yyyy");

                        if (ascoDict.ContainsKey(IoPair.Key))
                        {
                            int positionID = ascoDict[IoPair.Key].IntegerValue6;
                            if (positionID != -1)
                            {
                                EmployeePosition posit = new EmployeePosition();
                                posit.EmplPositionTO.PositionID = positionID;
                                List<EmployeePositionTO> position = posit.SearchEmployeePositions();
                                row[14] = position[0].PositionCodeTitleSR;
                            }
                            else
                            {
                                row[14] = "/";
                            }
                        }

                        table.Rows.Add(row);
                    }
                }
                if (table.Rows.Count > 0)
                {

                    string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                    int cost = 0;
                    bool costum = int.TryParse(costumer, out cost);
                    bool isHyatt = (cost == (int)Constants.Customers.Hyatt);

                    Session["TLAnnualLeavePage.ds"] = ds;

                    string wOptions = "dialogWidth:800px; dialogHeight:600px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";

                    if (isHyatt)
                        ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/ReportsWeb/TLAnnualLeaveReportHyatt.aspx', window, '" + wOptions.Trim() + "');", true);

                }
                else
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                }
            }
            catch (Exception ex)
            {

                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCAnnualLeavePage.btnPrint_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLAnnualLeavePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }



        private List<object> createResultRow(EmployeeTO empl, IOPairProcessedTO pair)
        {
            try
            {
                // create result row
                List<object> resultRow = new List<object>();
                int totalMinutes = 0;
                if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                    totalMinutes = (int)pair.EndTime.AddMinutes(1).Subtract(pair.StartTime).TotalMinutes;
                else
                    totalMinutes = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;
                int hours = totalMinutes / 60;
                int minutes = totalMinutes % 60;
                resultRow.Add(pair.PTDesc.Trim());
                resultRow.Add(pair.IOPairDate.Date);
                resultRow.Add(pair.StartTime);
                resultRow.Add(pair.EndTime);

                resultRow.Add(hours.ToString().Trim() + "h" + minutes.ToString().Trim() + "min");

                return resultRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string validatePair(IOPairProcessedTO pair, int minimalPresence, Dictionary<string, RuleTO> rules)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCAnnualLeavePage).Assembly);

                if (pair.StartTime >= pair.EndTime)
                    return rm.GetString("pairInvalidDuration", culture);

                if (rules.ContainsKey(Constants.RuleCompanyRegularWork) && pair.PassTypeID == rules[Constants.RuleCompanyRegularWork].RuleValue)
                {
                    // validate duration due to minimal presence
                    int pairDuration = ((int)pair.EndTime.TimeOfDay.TotalMinutes - (int)pair.StartTime.TimeOfDay.TotalMinutes);

                    // if it is last pair from first night shift interval, add one minute
                    if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                        pairDuration++;

                    if (pairDuration < minimalPresence)
                        return rm.GetString("pairLessThenMinimum", culture);
                }

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rbYes_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbNo.Checked = !rbYes.Checked;
                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCAnnualLeavePage.rbYes_CheckedChange(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCAnnualLeavePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbNo_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbYes.Checked = !rbNo.Checked;
                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCAnnualLeavePage.rbNo_CheckedChange(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCAnnualLeavePage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private bool pairInInterval(IOPairProcessedTO pair, WorkTimeIntervalTO interval, WorkTimeSchemaTO schema)
        {
            try
            {
                if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                        && pair.EndTime.Subtract(pair.StartTime).TotalMinutes <= interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                        return true;
                    else
                        return false;
                }
                else if (pair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void removeIntervalPairs(ref Dictionary<DateTime, List<IOPairProcessedTO>> emplOldPairs, DateTime date, WorkTimeIntervalTO interval, WorkTimeSchemaTO schema)
        {
            try
            {
                if (emplOldPairs.ContainsKey(date.Date))
                {
                    IEnumerator<IOPairProcessedTO> pairEnumerator = emplOldPairs[date.Date].GetEnumerator();

                    while (pairEnumerator.MoveNext())
                    {
                        if (pairInInterval(pairEnumerator.Current, interval, schema))
                        {
                            emplOldPairs[date.Date].Remove(pairEnumerator.Current);
                            pairEnumerator = emplOldPairs[date.Date].GetEnumerator();
                        }
                    }
                }
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "WCAnnualLeavePage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "WCAnnualLeavePage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IOPairProcessedTO createPair(int emplID, PassTypeTO pt, DateTime date, DateTime start, DateTime end)
        {
            try
            {
                IOPairProcessedTO pair = new IOPairProcessedTO();
                pair.EmployeeID = emplID;
                pair.IOPairDate = date.Date;
                pair.StartTime = start;
                pair.EndTime = end;
                pair.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                pair.LocationID = Constants.locationOut;
                pair.Alert = Constants.alertStatusNoAlert.ToString();
                pair.ManualCreated = (int)Constants.recordCreated.Manualy;
                pair.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                pair.PassTypeID = pt.PassTypeID;
                pair.ConfirmationFlag = pt.ConfirmFlag;

                // TL and HRSSC automatically verify pairs for other employees - unjustified is verified by default, other not verified
                if (CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], emplID))
                {
                    if (pt.VerificationFlag == (int)Constants.Verification.NotVerified)
                    {
                        pair.VerificationFlag = (int)Constants.Verification.Verified;
                        pair.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                        pair.VerifiedTime = DateTime.Now;
                    }
                    else
                        pair.VerificationFlag = (int)Constants.Verification.Verified;
                }
                else
                    pair.VerificationFlag = CommonWeb.Misc.verificationFlag(pt, false, false);

                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                    pair.PTDesc = pt.Description.Trim();
                else
                    pair.PTDesc = pt.DescAlt.Trim();

                pair.IOPairID = Constants.unjustifiedIOPairID;

                return pair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<DateTime> employeeHolidays(ref List<DateTime> personalHolidays, DateTime startTime, DateTime endTime, int emplID, List<EmployeeTimeScheduleTO> emplSchedules, 
            Dictionary<int, WorkTimeSchemaTO> schemas, EmployeeTO empl, Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict)
        {
            try
            {
                List<DateTime> emplHolidays = new List<DateTime>();

                string holidayType = "";

                // check if there are personal holidays for selected period and selected employees, no transfering holidays for personal holidays                
                List<EmployeeAsco4TO> ascoList = new EmployeeAsco4(Session[Constants.sessionConnection]).Search(emplID.ToString().Trim());
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(startTime, endTime, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, Session[Constants.sessionConnection], nationalTransferableHolidays);

                // get personal holidays in selected period
                foreach (EmployeeAsco4TO asco in ascoList)
                {
                    // expat out does not have holidays
                    if (Common.Misc.isExpatOut(rulesDict, empl))
                        continue;

                    holidayType = asco.NVarcharValue1.Trim();

                    if (!holidayType.Trim().Equals(""))
                    {
                        // if category is IV, find holiday date
                        if (holidayType.Trim().Equals(Constants.holidayTypeIV.Trim()))
                        {
                            DateTime personalHolidayStartDate = new DateTime(startTime.Year, asco.DatetimeValue1.Month, asco.DatetimeValue1.Day, 0, 0, 0);
                            DateTime personalHolidayEndDate = new DateTime(endTime.Year, asco.DatetimeValue1.Month, asco.DatetimeValue1.Day, 0, 0, 0);
                            if (startTime.Date <= personalHolidayStartDate.Date && endTime.Date >= personalHolidayStartDate.Date)
                            {
                                emplHolidays.Add(personalHolidayStartDate.Date);
                                personalHolidays.Add(personalHolidayStartDate.Date);
                            }
                            else if (startTime.Date <= personalHolidayEndDate.Date && endTime.Date >= personalHolidayEndDate.Date)
                            {
                                emplHolidays.Add(personalHolidayEndDate);
                                personalHolidays.Add(personalHolidayEndDate);
                            }
                        }
                        else
                        {
                            // if category is I, II or III, check if pair date is personal holiday
                            if (personalHolidayDays.ContainsKey(holidayType.Trim()))
                            {
                                emplHolidays.AddRange(personalHolidayDays[holidayType.Trim()]);
                                personalHolidays.AddRange(personalHolidayDays[holidayType.Trim()]);
                            }
                        }
                    }
                }

                // add national holidays to all selected employees, industrial time schemas do not transfer holidays                
                if (emplID != -1 && !Common.Misc.isExpatOut(rulesDict, empl))
                {                    
                    foreach (DateTime natHoliday in nationalHolidaysDays)
                    {
                        emplHolidays.Add(natHoliday.Date);
                    }

                    // add transfered holidays to employees who does not have industrial schema for that day
                    foreach (DateTime natHolidaySunday in nationalHolidaysSundays)
                    {
                        bool isIndustrial = false;

                        if (Common.Misc.getTimeSchema(natHolidaySunday.Date, emplSchedules, schemas).Type.Trim().ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()))
                            isIndustrial = true;

                        if (!isIndustrial)
                            emplHolidays.Add(natHolidaySunday.Date);
                    }
                }

                return emplHolidays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
