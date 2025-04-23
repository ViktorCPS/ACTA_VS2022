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
    public partial class WCSelfTMDataPage : System.Web.UI.Page
    {
        const string pageName = "WCSelfTMDataPage";

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
                // clear day pairs and employee counters so bars and counters could be presented properly
                ClearSessionValues();

                EmployeeTO Empl = getEmployee();
                // parameter in request query string is emplID (employee_id) and Back if Back button should be visble and functional
                if (!IsPostBack)
                {
                    btnBack.Visible = Request.QueryString["Back"] != null && !Request.QueryString["Back"].Trim().Equals("");
                    btnBack.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnNext.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnVerifyAll.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnPrev.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    
                    btnPrev.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnPrev.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnNext.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnNext.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    setLanguage();
                    
                    InitializeData(Empl);

                    rbAllData.Checked = true;
                    rbVerificationData.Checked = false;
                    btnVerifyAll.Visible = false;

                    if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager))
                    {
                        rbAllData.Visible = rbVerificationData.Visible = btnVerifyAll.Visible = false;
                    }

                    rbPassType.Checked = true;
                    rbLocation.Checked = false;

                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager)
                        rbPassType.Visible = rbLocation.Visible = false;
                    else
                    {
                        // check pass types/locations switch visibility
                        Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                        rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                        rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                        rule.RuleTO.RuleType = Constants.RulePairsByLocation;
                        List<RuleTO> locRules = rule.Search();

                        if (locRules.Count > 0 && locRules[0].RuleValue == Constants.yesInt)
                            rbPassType.Visible = rbLocation.Visible = true;
                        else
                            rbPassType.Visible = rbLocation.Visible = false;
                    }

                    tbMonth.Text = DateTime.Now.Date.ToString(Constants.monthFormat.Trim());

                    // reload selected filter state
                    if (!(Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper())))
                        LoadState();

                    InitializeGraphData(Empl);
                }
                else
                {
                    if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"].Equals(Constants.pairsSavedArg))
                    {
                        InitializeGraphData(Empl);
                    }
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCSelfTMDataPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void ClearSessionValues()
        {
            try
            {
                // clean Session
                Session[Constants.sessionEmplCounters] = null;
                Session[Constants.sessionDayPairs] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCSelfTMDataPage).Assembly);

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
                lblHoursCalc.Text = rm.GetString("lblHoursCalc", culture);
                lblPayroll.Text = rm.GetString("lblPayrollID", culture);

                btnBack.Text = rm.GetString("btnBack", culture);
                btnVerifyAll.Text = rm.GetString("btnVerifyAll", culture);
                
                rbAllData.Text = rm.GetString("rbAllData", culture);
                rbVerificationData.Text = rm.GetString("rbVerificationData", culture);
                rbPassType.Text = rm.GetString("rbPassType", culture);
                rbLocation.Text = rm.GetString("rbLocation", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string emplTypes(EmployeeTO employee)
        {
            try
            {
                string empl = "";
                // get selected company
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
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCSelfTMDataPage).Assembly);
                    
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

                    // create dinamically asco data
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

        private void InitializeGraphData(EmployeeTO Empl)
        {
            try
            {
                selectedPairs.Value = "";

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCSelfTMDataPage).Assembly);

                Dictionary<int, PassTypeTO> PassTypes = getTypes();
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();                
                Dictionary<int, string> Months = getMonths();
                Dictionary<int, LocationTO> locDict = new Dictionary<int, LocationTO>();
                if (rbLocation.Checked)
                    locDict = new Location(Session[Constants.sessionConnection]).SearchDict();

                int hrsscCutoffDate = -1;

                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC/* && Session[Constants.sessionLogInEmployee] != null
                    && Session[Constants.sessionLogInEmployee] is EmployeeTO*/)
                {
                    //int hrsscCompany = Common.Misc.getRootWorkingUnit(((EmployeeTO)Session[Constants.sessionLogInEmployee]).WorkingUnitID, WUnits);

                    //if (hrsscCompany != -1)
                    //{
                    //    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                    //    rule.RuleTO.WorkingUnitID = hrsscCompany;
                    //    rule.RuleTO.EmployeeTypeID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeTypeID;
                    //    rule.RuleTO.RuleType = Constants.RuleHRSSCCutOffDate;

                    //    List<RuleTO> rules = rule.Search();

                    //    if (rules.Count == 1)
                    //    {
                    //        hrsscCutoffDate = rules[0].RuleValue;
                    //    }
                    //}
                    Common.Rule ruleCutOff = new Common.Rule(Session[Constants.sessionConnection]);
                    ruleCutOff.RuleTO.WorkingUnitID = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);
                    ruleCutOff.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                    ruleCutOff.RuleTO.RuleType = Constants.RuleHRSSCCutOffDate;
                    List<RuleTO> rulesListCutOff = ruleCutOff.Search();
                    hrsscCutoffDate = rulesListCutOff[0].RuleValue;
                }
                

                DateTime month = new DateTime();

                int workingDaysNum = Common.Misc.countWorkingDays(DateTime.Now.Date, Session[Constants.sessionConnection]);
                if (hrsscCutoffDate != -1 && workingDaysNum <= hrsscCutoffDate)
                    month = CommonWeb.Misc.createDate("01." + tbMonth.Text.Trim()).AddMonths(-1);
                else
                    month = CommonWeb.Misc.createDate("01." + tbMonth.Text.Trim());

                if (Months.ContainsKey(month.Month))
                    lblGraphPeriod.Text = Months[month.Month] + " " + month.Year.ToString().Trim();

                DateTime prevMonth = month.AddMonths(-1);
                DateTime nextMonth = month.AddMonths(1);

                DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);

                // get employee rules
                Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();

                int company = -1;
                if (Empl.EmployeeID != -1)
                    company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);

                Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
                if (Empl.EmployeeID != -1)
                {
                    if (company != -1)
                    {
                        Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                        rule.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                        rule.RuleTO.WorkingUnitID = company;                        
                        List<RuleTO> rulesList = rule.Search();

                        foreach (RuleTO ruleTO in rulesList)
                        {
                            if (!emplRules.ContainsKey(ruleTO.RuleType))
                                emplRules.Add(ruleTO.RuleType, ruleTO);
                        }
                    }

                    ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(Empl.EmployeeID.ToString().Trim());
                }

                EmployeeAsco4TO asco = new EmployeeAsco4TO();
                if (ascoDict.ContainsKey(Empl.EmployeeID))
                    asco = ascoDict[Empl.EmployeeID];

                InitializeCounters(Empl, asco, emplRules);

                // set legend control
                foreach (Control ctrl in legendCtrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                legendCtrlHolder.Controls.Clear();

                LegendUC legendCtrl = new LegendUC();
                legendCtrl.ID = "legend";
                legendCtrl.Company = company;

                legendCtrlHolder.Controls.Add(legendCtrl);

                // set hour line control
                foreach (Control ctrl in hourLineCtrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                hourLineCtrlHolder.Controls.Clear();

                // find night hour rule
                RuleTO nightWorkRule = new RuleTO();

                if (emplRules.ContainsKey(Constants.RuleNightWork))
                    nightWorkRule = emplRules[Constants.RuleNightWork];                

                HoursLineControlUC hourLine = new HoursLineControlUC();
                hourLine.NightWorkRule = nightWorkRule;
                hourLine.ID = "hourLine";
                hourLine.FirstLblText = rm.GetString("lblDate", culture);
                hourLine.SecondLblText = rm.GetString("lblTotal", culture);
                hourLineCtrlHolder.Controls.Add(hourLine);

                //Dictionary<int, Dictionary<DateTime, List<PassTO>>> Passes = FindPassesForEmployee(month, Empl);

                List<IOPairProcessedTO> IOPairList = FindIOPairsForEmployee(month, Empl);

                DrawGraphControl(month, IOPairList, PassTypes, WUnits, locDict, Empl, hrsscCutoffDate, workingDaysNum);

                InitializeHourCalc(IOPairList, PassTypes, locDict);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeCounters(EmployeeTO Empl, EmployeeAsco4TO asco, Dictionary<string, RuleTO> emplRules)
        {
            try
            {
                foreach (Control ctrl in counterCtrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                counterCtrlHolder.Controls.Clear();

                Dictionary<int, string> counterNames = new Dictionary<int, string>();
                List<EmployeeCounterTypeTO> counterTypes = new EmployeeCounterType(Session[Constants.sessionConnection]).Search();

                foreach (EmployeeCounterTypeTO type in counterTypes)
                {
                    if (!counterNames.ContainsKey(type.EmplCounterTypeID))
                    {
                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            counterNames.Add(type.EmplCounterTypeID, type.Name.Trim());
                        else
                            counterNames.Add(type.EmplCounterTypeID, type.NameAlt.Trim());
                    }
                }

                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValues(Empl.EmployeeID.ToString().Trim());

                int reserved = -1;

                if (asco.IntegerValue10 != Constants.yesInt && emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && emplRules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeaveReservation))
                {
                    DateTime startDate = new DateTime();
                    
                    startDate = new DateTime(DateTime.Now.Year, emplRules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleDateTime1.Month, emplRules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleDateTime1.Day);

                    if (startDate.Date > DateTime.Now)
                        startDate = startDate.AddYears(-1).Date;

                    reserved = emplRules[Constants.RuleCompanyCollectiveAnnualLeaveReservation].RuleValue 
                        - Common.Misc.SearchCollectiveAnnualLeave(new IOPairProcessedTO(), Empl.EmployeeID, emplRules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue, startDate.Date, 
                        new List<DateTime>(), new List<IOPairProcessedTO>(), emplRules, Session[Constants.sessionConnection]);

                    if (reserved < 0)
                        reserved = 0;
                }

                // add counter
                bool hideBH = false;
                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                    hideBH = false;
                else
                {
                    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                    rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                    rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                    rule.RuleTO.RuleType = Constants.RuleHideBH;
                    List<RuleTO> ruleList = rule.Search();

                    if (ruleList.Count > 0 && ruleList[0].RuleValue == Constants.yesInt)
                    {
                        // check if employee is supervisor
                        if (asco.NVarcharValue5.Trim() != "")
                        {
                            ApplUserXApplUserCategory userXCat = new ApplUserXApplUserCategory(Session[Constants.sessionConnection]);
                            userXCat.UserXCategoryTO.UserID = asco.NVarcharValue5.Trim();
                            List<ApplUserXApplUserCategoryTO> catList = userXCat.Search();

                            foreach (ApplUserXApplUserCategoryTO catTO in catList)
                            {
                                if (catTO.CategoryID == (int)Constants.Categories.TL)
                                {
                                    hideBH = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                CounterUC emplCounter = new CounterUC();
                emplCounter.ID = "emplCounter";
                if (emplCounters.ContainsKey(Empl.EmployeeID))
                    emplCounter.CounterValues = emplCounters[Empl.EmployeeID];
                emplCounter.CounterNames = counterNames;
                emplCounter.EmplID = Empl.EmployeeID;
                emplCounter.ReservedALDays = reserved;
                emplCounter.HideBH = hideBH;
                counterCtrlHolder.Controls.Add(emplCounter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeHourCalc(List<IOPairProcessedTO> IOPairList, Dictionary<int, PassTypeTO> PassTypes, Dictionary<int, LocationTO> Locations)
        {
            try
            {
                foreach (Control ctrl in typeCtrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                typeCtrlHolder.Controls.Clear();

                Dictionary<int, TimeSpan> typesCounter = new Dictionary<int, TimeSpan>();
                Dictionary<int, TimeSpan> locationsCounter = new Dictionary<int, TimeSpan>();

                foreach (IOPairProcessedTO iopair in IOPairList)
                {
                    if (!iopair.StartTime.Date.Equals(new DateTime()) && !iopair.EndTime.Date.Equals(new DateTime()))
                    {
                        TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                        if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                            duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                        if (typesCounter.ContainsKey(iopair.PassTypeID))
                            typesCounter[iopair.PassTypeID] = typesCounter[iopair.PassTypeID].Add(duration);
                        else
                            typesCounter.Add(iopair.PassTypeID, duration);
                        if (locationsCounter.ContainsKey(iopair.LocationID))
                            locationsCounter[iopair.LocationID] = locationsCounter[iopair.LocationID].Add(duration);
                        else
                            locationsCounter.Add(iopair.LocationID, duration);
                    }
                }

                if (rbLocation.Checked)
                {
                    // if there is no pairs and calculated hours per location, panel title lbl is not visible
                    lblHoursCalc.Visible = (locationsCounter.Count > 0);

                    int counter = 0;
                    foreach (int locID in locationsCounter.Keys)
                    {
                        if (locationsCounter[locID].TotalMinutes > 0)
                        {
                            // make location name label
                            Label locDescLbl = new Label();
                            locDescLbl.ID = "locDescLbl" + counter.ToString().Trim();
                            locDescLbl.Width = new Unit(typePanel.Width.Value - 30);
                            if (Locations.ContainsKey(locID))
                                locDescLbl.Text = Locations[locID].Name.Trim();
                            locDescLbl.CssClass = "contentLblLeft";
                            typeCtrlHolder.Controls.Add(locDescLbl);

                            // make location counter label
                            Label locCounterLbl = new Label();
                            locCounterLbl.ID = "locCounterLbl" + counter.ToString().Trim();
                            locCounterLbl.Width = new Unit(typePanel.Width.Value - 30);
                            int hours = (int)locationsCounter[locID].TotalHours;
                            int minutes = (int)locationsCounter[locID].TotalMinutes - hours * 60;
                            if (hours > 0)
                                locCounterLbl.Text += hours.ToString().Trim() + "h";
                            if (minutes > 0)
                                locCounterLbl.Text += minutes.ToString().Trim() + "min";
                            locCounterLbl.CssClass = "counterMiddleLblLeft";
                            typeCtrlHolder.Controls.Add(locCounterLbl);

                            counter++;
                        }
                    }
                }
                else
                {
                    // if there is no pairs and calculated hours per pass type, panel title lbl is not visible
                    lblHoursCalc.Visible = (typesCounter.Count > 0);

                    int counter = 0;
                    foreach (int ptID in typesCounter.Keys)
                    {
                        if (typesCounter[ptID].TotalMinutes > 0)
                        {
                            // make pass type name label
                            Label typeDescLbl = new Label();
                            typeDescLbl.ID = "ptDescLbl" + counter.ToString().Trim();
                            typeDescLbl.Width = new Unit(typePanel.Width.Value - 30);
                            if (PassTypes.ContainsKey(ptID))
                                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                                    typeDescLbl.Text = PassTypes[ptID].Description.Trim();
                                else
                                    typeDescLbl.Text = PassTypes[ptID].DescAlt.Trim();
                            typeDescLbl.CssClass = "contentLblLeft";
                            typeCtrlHolder.Controls.Add(typeDescLbl);

                            // make pass type counter label
                            Label typeCounterLbl = new Label();
                            typeCounterLbl.ID = "ptCounterLbl" + counter.ToString().Trim();
                            typeCounterLbl.Width = new Unit(typePanel.Width.Value - 30);
                            int hours = (int)typesCounter[ptID].TotalHours;
                            int minutes = (int)typesCounter[ptID].TotalMinutes - hours * 60;
                            if (hours > 0)
                                typeCounterLbl.Text += hours.ToString().Trim() + "h";
                            if (minutes > 0)
                                typeCounterLbl.Text += minutes.ToString().Trim() + "min";
                            typeCounterLbl.CssClass = "counterMiddleLblLeft";
                            typeCtrlHolder.Controls.Add(typeCounterLbl);

                            counter++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, PassTypeTO> getTypes()
        {
            try
            {
                return new PassType(Session[Constants.sessionConnection]).SearchDictionary();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, string> getMonths()
        {
            try
            {                
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCSelfTMDataPage).Assembly);

                Dictionary<int, string> months = new Dictionary<int, string>();

                if (!months.ContainsKey(1))
                    months.Add(1, rm.GetString("Jan", culture));
                if (!months.ContainsKey(2))
                    months.Add(2, rm.GetString("Feb", culture));
                if (!months.ContainsKey(3))
                    months.Add(3, rm.GetString("Mar", culture));
                if (!months.ContainsKey(4))
                    months.Add(4, rm.GetString("Apr", culture));
                if (!months.ContainsKey(5))
                    months.Add(5, rm.GetString("May", culture));
                if (!months.ContainsKey(6))
                    months.Add(6, rm.GetString("Jun", culture));
                if (!months.ContainsKey(7))
                    months.Add(7, rm.GetString("Jul", culture));
                if (!months.ContainsKey(8))
                    months.Add(8, rm.GetString("Aug", culture));
                if (!months.ContainsKey(9))
                    months.Add(9, rm.GetString("Sep", culture));
                if (!months.ContainsKey(10))
                    months.Add(10, rm.GetString("Oct", culture));
                if (!months.ContainsKey(11))
                    months.Add(11, rm.GetString("Nov", culture));
                if (!months.ContainsKey(12))
                    months.Add(12, rm.GetString("Dec", culture));

                return months;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                        
        private List<IOPairProcessedTO> FindIOPairsForEmployee(DateTime month, EmployeeTO Empl)
        {
            try
            {
                List<IOPairProcessedTO> IOPairList = new List<IOPairProcessedTO>();
                List<DateTime> datesList = new List<DateTime>();
                DateTime day = month;
                while (day < month.AddMonths(1))
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }
                                
                if (Empl.EmployeeID != -1)
                    IOPairList = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(Empl.EmployeeID.ToString().Trim(), datesList, "");
                
                return IOPairList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void DrawGraphControl(DateTime month, List<IOPairProcessedTO> IOPairList, Dictionary<int, PassTypeTO> PassTypes, Dictionary<int, WorkingUnitTO> WUnits, 
            Dictionary<int, LocationTO> locDict, EmployeeTO Empl, int hrsscCutoffDate, int workingDaysNum)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCSelfTMDataPage).Assembly);

                //get cuttoff date
                int cutoffDate = -1;                

                //get wcdr cuttoff date - Empl is logged in employee
                int wcdrCutoffDate = -1;
                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager /*&& Session[Constants.sessionLogInEmployee] != null
                    && Session[Constants.sessionLogInEmployee] is EmployeeTO*/)
                {
                    //int company = Common.Misc.getRootWorkingUnit(((EmployeeTO)Session[Constants.sessionLogInEmployee]).WorkingUnitID, WUnits);

                    //if (company != -1)
                    //{
                    //    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                    //    rule.RuleTO.WorkingUnitID = company;
                    //    rule.RuleTO.EmployeeTypeID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeTypeID;
                    //    rule.RuleTO.RuleType = Constants.RuleWCDRCutOffDate;

                    //    List<RuleTO> rules = rule.Search();

                    //    if (rules.Count == 1)
                    //    {
                    //        wcdrCutoffDate = rules[0].RuleValue;
                    //    }
                    //}
                    Common.Rule ruleCutOff = new Common.Rule(Session[Constants.sessionConnection]);
                    ruleCutOff.RuleTO.WorkingUnitID = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);
                    ruleCutOff.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                    ruleCutOff.RuleTO.RuleType = Constants.RuleWCDRCutOffDate;
                    List<RuleTO> rulesListCutOff = ruleCutOff.Search();
                    wcdrCutoffDate = rulesListCutOff[0].RuleValue;
                }                

                if (Empl.EmployeeID != -1)
                {
                    int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);

                    if (company != -1)
                    {
                        Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                        rule.RuleTO.WorkingUnitID = company;
                        rule.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                        rule.RuleTO.RuleType = Constants.RuleCutOffDate;

                        List<RuleTO> rules = rule.Search();

                        if (rules.Count == 1)
                        {
                            cutoffDate = rules[0].RuleValue;
                        }
                    }

                    // create dictionary with pairs for specific day
                    Dictionary<DateTime, List<IOPairProcessedTO>> dayPairs = new Dictionary<DateTime, List<IOPairProcessedTO>>();
                    Dictionary<DateTime, List<IOPairProcessedTO>> dayPairsForVerification = new Dictionary<DateTime, List<IOPairProcessedTO>>();

                    foreach (IOPairProcessedTO pair in IOPairList)
                    {
                        if (!dayPairs.ContainsKey(pair.IOPairDate.Date))
                            dayPairs.Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                        dayPairs[pair.IOPairDate.Date].Add(pair);

                        if (rbVerificationData.Checked && pair.VerificationFlag == (int)Constants.Verification.NotVerified)
                        {
                            if (!dayPairsForVerification.ContainsKey(pair.IOPairDate.Date))
                                dayPairsForVerification.Add(pair.IOPairDate.Date, new List<IOPairProcessedTO>());

                            dayPairsForVerification[pair.IOPairDate.Date].Add(pair);
                        }
                    }

                    // dispose and clear existing controls
                    foreach (Control ctrl in ctrlHolder.Controls)
                    {
                        ctrl.Dispose();
                    }

                    ctrlHolder.Controls.Clear();

                    // get schedules for all employees
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(Empl.EmployeeID.ToString().Trim(), month.Date, month.AddMonths(1).AddDays(-1).Date, null);

                    List<EmployeeTimeScheduleTO> timeSchedules = new List<EmployeeTimeScheduleTO>();

                    if (emplSchedules.ContainsKey(Empl.EmployeeID))
                        timeSchedules = emplSchedules[Empl.EmployeeID];

                    // get all time schemas
                    Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                    // get all alerts
                    Dictionary<int, Dictionary<DateTime, int>> alerts = new IOPairsProcessedHist(Session[Constants.sessionConnection]).SearchAlerts(Empl.EmployeeID.ToString(), month.Date, month.AddMonths(1).AddDays(-1).Date);

                    Dictionary<int, EmployeeAsco4TO> emplAddData = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(Empl.EmployeeID.ToString().Trim());
                    
                    // get employee hiring and termination dates
                    DateTime emplHiringDate = new DateTime();
                    DateTime emplTerminationDate = new DateTime();

                    if (emplAddData.ContainsKey(Empl.EmployeeID))
                    {
                        emplHiringDate = emplAddData[Empl.EmployeeID].DatetimeValue2;

                        if (Empl.Status.Trim().ToUpper().Equals(Constants.statusRetired.Trim().ToUpper()))
                            emplTerminationDate = emplAddData[Empl.EmployeeID].DatetimeValue3;
                    }

                    int currentIndex = 0;
                    DateTime currDay = new DateTime(month.Year, month.Month, 1, 0, 0, 0); // start with first day in month and go through all month days
                    DateTime nextMonth = currDay.AddMonths(1);

                    while (currDay.Date < nextMonth.Date)
                    {
                        List<IOPairProcessedTO> ioPairsForDay = new List<IOPairProcessedTO>();
                        List<IOPairProcessedTO> ioPairsForDayToVerify = new List<IOPairProcessedTO>();

                        if (dayPairs.ContainsKey(currDay.Date))
                            ioPairsForDay = dayPairs[currDay.Date];

                        if (rbVerificationData.Checked && dayPairsForVerification.ContainsKey(currDay.Date))
                            ioPairsForDayToVerify = dayPairsForVerification[currDay.Date];

                        int hours = 0;
                        int min = 0;
                        if (rbVerificationData.Checked)
                        {
                            foreach (IOPairProcessedTO iopair in ioPairsForDayToVerify)
                            {
                                if (!iopair.StartTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && !iopair.EndTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                                {
                                    if (int.Parse(iopair.StartTime.ToString("mm")) == 0)
                                    {
                                        hours += int.Parse(iopair.EndTime.ToString("HH")) - int.Parse(iopair.StartTime.ToString("HH"));
                                    }
                                    else
                                    {
                                        hours += int.Parse(iopair.EndTime.ToString("HH")) - int.Parse(iopair.StartTime.ToString("HH")) - 1;
                                        min += 60 - int.Parse(iopair.StartTime.ToString("mm"));
                                    }

                                    min += int.Parse(iopair.EndTime.ToString("mm"));
                                }

                                while (min >= 60)
                                {
                                    hours++;
                                    min -= 60;
                                }
                            }
                        }
                        else
                        {
                            foreach (IOPairProcessedTO iopair in ioPairsForDay)
                            {
                                if (!iopair.StartTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)) && !iopair.EndTime.Date.Equals(new DateTime(1, 1, 1, 0, 0, 0)))
                                {
                                    if (int.Parse(iopair.StartTime.ToString("mm")) == 0)
                                    {
                                        hours += int.Parse(iopair.EndTime.ToString("HH")) - int.Parse(iopair.StartTime.ToString("HH"));
                                    }
                                    else
                                    {
                                        hours += int.Parse(iopair.EndTime.ToString("HH")) - int.Parse(iopair.StartTime.ToString("HH")) - 1;
                                        min += 60 - int.Parse(iopair.StartTime.ToString("mm"));
                                    }

                                    min += int.Parse(iopair.EndTime.ToString("mm"));

                                    // add one minute for first interval of third shift
                                    if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                                        min++;
                                }

                                while (min >= 60)
                                {
                                    hours++;
                                    min -= 60;
                                }
                            }
                        }

                        string timeString = ""; //text for summ time cell
                        if (hours == 0) { timeString += "00h"; }
                        else
                        {
                            if (hours < 10) { timeString += "0"; }
                            timeString += hours + "h";
                        }
                        if (min == 0) { timeString += "00min"; }
                        else
                        {
                            if (min < 10) { timeString += "0"; }
                            timeString += min + "min";
                        }

                        List<WorkTimeIntervalTO> timeSchemaIntervalList = Common.Misc.getTimeSchemaInterval(currDay.Date, timeSchedules, schemas);

                        WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                        if (timeSchemaIntervalList.Count > 0 && schemas.ContainsKey(timeSchemaIntervalList[0].TimeSchemaID))
                            schema = schemas[timeSchemaIntervalList[0].TimeSchemaID];

                        string day = "";
                        switch (currDay.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                day = rm.GetString("mon", culture);
                                break;
                            case DayOfWeek.Tuesday:
                                day = rm.GetString("tue", culture);
                                break;
                            case DayOfWeek.Wednesday:
                                day = rm.GetString("wed", culture);
                                break;
                            case DayOfWeek.Thursday:
                                day = rm.GetString("thu", culture);
                                break;
                            case DayOfWeek.Friday:
                                day = rm.GetString("fri", culture);
                                break;
                            case DayOfWeek.Saturday:
                                day = rm.GetString("sat", culture);
                                break;
                            case DayOfWeek.Sunday:
                                day = rm.GetString("sun", culture);
                                break;
                            default:
                                day = "";
                                break;
                        }

                        Color backColor = Color.White;
                        bool isAltCtrl = false;

                        if (currentIndex % 2 != 0)
                        {
                            backColor = ColorTranslator.FromHtml(Constants.emplDayViewAltColor);
                            isAltCtrl = true;
                        }
                        
                        EmployeeWorkingDayViewUC emplDay = new EmployeeWorkingDayViewUC();
                        emplDay.ID = "emplDayView" + currentIndex.ToString();
                        if (rbVerificationData.Checked)
                            emplDay.DayPairList = ioPairsForDayToVerify;
                        else
                            emplDay.DayPairList = ioPairsForDay;
                        emplDay.DayIntervalList = timeSchemaIntervalList;
                        emplDay.PassTypes = PassTypes;
                        emplDay.Locations = locDict;
                        emplDay.WUnits = WUnits;
                        emplDay.Empl = Empl;
                        emplDay.BackColor = backColor;
                        emplDay.Date = currDay;
                        emplDay.SecondData = timeString.Trim();
                        emplDay.FirstData = currDay.ToString(Constants.dateMonthFormat) + day;
                        emplDay.ShowReallocated = true;
                        if (currentIndex == 0)
                            emplDay.IsFirst = true;
                        emplDay.IsAltCtrl = isAltCtrl;
                        emplDay.EmplTimeSchema = schema;
                        emplDay.ByLocations = rbLocation.Checked;
                        emplDay.PostBackCtrlID = btnPrev.ID;
                        emplDay.ShowAlert = alerts.ContainsKey(Empl.EmployeeID) && alerts[Empl.EmployeeID].ContainsKey(currDay.Date) && alerts[Empl.EmployeeID][currDay.Date] > 0;

                        if (!emplHiringDate.Equals(new DateTime()) && emplHiringDate.Date <= currDay.Date && (emplTerminationDate.Equals(new DateTime()) || emplTerminationDate >= currDay.Date))
                        {
                            if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                                && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                            {
                                if (currDay.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                                    emplDay.AllowChange = true;
                                else if (hrsscCutoffDate != -1 && workingDaysNum <= hrsscCutoffDate
                                    && currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                                    emplDay.AllowChange = true;
                                else
                                    emplDay.AllowChange = false;
                            }
                            else if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager)
                            {
                                //9.1.2018 Miodrag / WC Direct Responsible uloga moze da menja parove iz pogleda na parove jednog zaposlenog
                                if (CommonWeb.Misc.isVerifiedConfirmedDay(ioPairsForDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                    //emplDay.AllowVerify = false;
                                    emplDay.AllowChange = false;
                                else if (currDay.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                                    //emplDay.AllowVerify = true;
                                    emplDay.AllowChange = true;
                                else if (wcdrCutoffDate != -1 && workingDaysNum <= wcdrCutoffDate
                                    && currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))                               
                                    //emplDay.AllowVerify = true;
                                    emplDay.AllowChange = true;
                                else
                                    //emplDay.AllowVerify = false;
                                    emplDay.AllowChange = false;

                                if (rbVerificationData.Checked && emplDay.AllowVerify)
                                {
                                    foreach (IOPairProcessedTO pair in ioPairsForDayToVerify)
                                    {
                                        // check if it belongs to previous day
                                        if (Session[Constants.sessionLoginCategoryPassTypes] != null && Session[Constants.sessionLoginCategoryPassTypes] is List<int>
                                            && ((List<int>)Session[Constants.sessionLoginCategoryPassTypes]).Contains(pair.PassTypeID))
                                            selectedPairs.Value += pair.RecID.ToString().Trim() + ",";
                                    }
                                }
                            }
                            else
                            {
                                if (CommonWeb.Misc.isVerifiedConfirmedDay(ioPairsForDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                    emplDay.AllowChange = false;
                                else if (currDay.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                                    emplDay.AllowChange = true;
                                else if (cutoffDate != -1 && workingDaysNum <= cutoffDate
                                    && currDay.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                                    emplDay.AllowChange = true;
                                else
                                    emplDay.AllowChange = false;
                            }
                        }
                        else
                            emplDay.AllowChange = emplDay.AllowConfirm = emplDay.AllowUndoVerify = emplDay.AllowVerify = false;

                        ctrlHolder.Controls.Add(emplDay);

                        currentIndex++;
                        currDay = currDay.AddDays(1);
                    }

                    if (selectedPairs.Value.Trim().Length > 0)
                        selectedPairs.Value = selectedPairs.Value.Substring(0, selectedPairs.Value.Length - 1);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "WCSelfTMDataPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "WCSelfTMDataPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rbLocation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbLocation.Checked)
                    rbPassType.Checked = !rbLocation.Checked;

                setVerifyAllVisibility();

                InitializeGraphData(getEmployee());

                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCSelfTMDataPage.rbLocation_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        protected void rbPassType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbPassType.Checked)
                    rbLocation.Checked = !rbPassType.Checked;

                setVerifyAllVisibility();

                InitializeGraphData(getEmployee());

                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCSelfTMDataPage.rbPassType_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                // change date in tbMonth, add one month
                tbMonth.Text = CommonWeb.Misc.createDate("01." + tbMonth.Text.Trim()).AddMonths(1).ToString(Constants.monthFormat.Trim());

                setVerifyAllVisibility();

                InitializeGraphData(getEmployee());

                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCSelfTMDataPage.btnNext_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                // change date in tbMonth, add one month
                tbMonth.Text = CommonWeb.Misc.createDate("01." + tbMonth.Text.Trim()).AddMonths(-1).ToString(Constants.monthFormat.Trim());

                setVerifyAllVisibility();

                InitializeGraphData(getEmployee());

                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCSelfTMDataPage.btnPrev_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "Redirect", "window.history.back();", true);
                if (Request.QueryString["Back"] != null && !Request.QueryString["Back"].Trim().Equals(""))
                    Response.Redirect(Request.QueryString["Back"].Trim() + "?reloadState=false", false);

                Message = "";
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCSelfTMDataPage.btnBack_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
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

        protected void rbAllData_ChackedChanged(Object sender, EventArgs e)
        {
            try
            {                                
                rbVerificationData.Checked = !rbAllData.Checked;

                btnVerifyAll.Visible = !rbAllData.Checked;

                InitializeGraphData(getEmployee());

                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCSelfTMDataPage.rbMove_ChackedChange(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbVerificationData_ChackedChanged(Object sender, EventArgs e)
        {
            try
            {
                rbAllData.Checked = !rbVerificationData.Checked;

                setVerifyAllVisibility();
                
                InitializeGraphData(getEmployee());

                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCSelfTMDataPage.rbVerificationData_ChackedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnVerifyAll_Click(Object sender, EventArgs e)
        {
            try
            {
                // if system is closed, sign out and redirect to login page
                if (Common.Misc.getClosingEvent(DateTime.Now, Session[Constants.sessionConnection]).EventID != -1)
                    CommonWeb.Misc.LogOut(Session[Constants.sessionLogInUserLog], Session[Constants.sessionLogInUser], Session[Constants.sessionConnection], Session, Response);

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCSelfTMDataPage).Assembly);

                if (selectedPairs.Value.Trim().Length > 0)
                {
                    // get pairs by day to be moved to hist table
                    Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> pairsToVerifyEmplDaySet = new IOPairProcessed(Session[Constants.sessionConnection]).SearchPairsToVerifyEmplDaySet(selectedPairs.Value);

                    // create new pairs by day to be inserted
                    Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> verifiedPairsDaySet = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();

                    DateTime modifiedTime = DateTime.Now;
                    string histRecIDs = "";
                    foreach (int emplID in pairsToVerifyEmplDaySet.Keys)
                    {
                        if (!verifiedPairsDaySet.ContainsKey(emplID))
                            verifiedPairsDaySet.Add(emplID, new Dictionary<DateTime, List<IOPairProcessedTO>>());

                        foreach (DateTime date in pairsToVerifyEmplDaySet[emplID].Keys)
                        {
                            if (!verifiedPairsDaySet[emplID].ContainsKey(date))
                                verifiedPairsDaySet[emplID].Add(date, new List<IOPairProcessedTO>());

                            foreach (IOPairProcessedTO pairTO in pairsToVerifyEmplDaySet[emplID][date])
                            {
                                histRecIDs += pairTO.RecID.ToString().Trim() + ",";

                                IOPairProcessedTO newPair = new IOPairProcessedTO(pairTO);
                                newPair.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                newPair.CreatedTime = modifiedTime;

                                if (selectedPairs.Value.Contains(newPair.RecID.ToString()))
                                {
                                    newPair.VerificationFlag = (int)Constants.Verification.Verified;
                                    newPair.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                    newPair.VerifiedTime = modifiedTime;
                                }

                                verifiedPairsDaySet[emplID][date].Add(newPair);
                            }
                        }
                    }

                    if (histRecIDs.Length > 0)
                        histRecIDs = histRecIDs.Substring(0, histRecIDs.Length - 1);

                    IOPairProcessed pair = new IOPairProcessed(Session[Constants.sessionConnection]);
                    IOPairsProcessedHist pairHist = new IOPairsProcessedHist(Session[Constants.sessionConnection]);

                    if (pair.BeginTransaction())
                    {
                        try
                        {
                            // save pairs from session, move all day pairs to hist table with alert value of 1, insert new pairs that last more then 0min
                            bool saved = true;
                            if (histRecIDs.Length > 0)
                            {
                                // move old pairs to hist table with alert value of 1
                                pairHist.SetTransaction(pair.GetTransaction());
                                saved = saved && (pairHist.Save(histRecIDs, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), modifiedTime, Constants.alertStatus, false) >= 0);

                                if (saved)
                                    saved = saved && pair.Delete(histRecIDs, false);

                                if (saved)
                                {
                                    foreach (int emplID in verifiedPairsDaySet.Keys)
                                    {
                                        foreach (DateTime date in verifiedPairsDaySet[emplID].Keys)
                                        {
                                            foreach (IOPairProcessedTO pairTO in verifiedPairsDaySet[emplID][date])
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
                                    }
                                }

                                if (saved)
                                {
                                    pair.CommitTransaction();
                                    selectedPairs.Value = "";
                                    InitializeGraphData(getEmployee());
                                }
                                else                                
                                    pair.RollbackTransaction();
                            }
                        }
                        catch
                        {
                            if (pair.GetTransaction() != null)
                                pair.RollbackTransaction();
                        }
                    }
                }
                
                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCSelfTMDataPage.btnVerifyAll_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCSelfTMDataPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setVerifyAllVisibility()
        {
            try
            {
                if (rbVerificationData.Checked)
                {
                    DateTime month = CommonWeb.Misc.createDate("01." + tbMonth.Text.Trim());

                    if (month < new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                        btnVerifyAll.Visible = false;
                    else
                    {
                        int cutoffDate = -1;

                        EmployeeTO Empl = getEmployee();
                        if (Empl.EmployeeID != -1)
                        {
                            Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                            int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);

                            if (company != -1)
                            {
                                Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                                rule.RuleTO.WorkingUnitID = company;
                                rule.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                                rule.RuleTO.RuleType = Constants.RuleCutOffDate;

                                List<RuleTO> rules = rule.Search();

                                if (rules.Count == 1)
                                {
                                    cutoffDate = rules[0].RuleValue;
                                }
                            }
                        }

                        if (cutoffDate != -1 && Common.Misc.countWorkingDays(DateTime.Now.Date, Session[Constants.sessionConnection]) > cutoffDate && month < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                            btnVerifyAll.Visible = false;
                        else
                            btnVerifyAll.Visible = true;
                    }
                }
                else
                    btnVerifyAll.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
