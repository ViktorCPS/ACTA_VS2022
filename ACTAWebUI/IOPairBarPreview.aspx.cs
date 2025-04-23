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
using System.Globalization;
using System.Resources;
using System.Drawing;

using TransferObjects;
using Common;
using Util;
using ReportsWeb;
using CommonWeb;

namespace ACTAWebUI
{
    public partial class IOPairBarPreview : System.Web.UI.Page
    {
        const string pageName = "IOPairBarPreview";

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

                // parameters in query string should be date, emplTypeID (employee_type_id), company (root working_unit_id), emplID (employee_id)
                if (!IsPostBack)
                {
                    ClearSessionValues();
                                        
                    DateTime date = new DateTime();

                    // get date from query string
                    if (Request.QueryString["date"] != null)
                        date = CommonWeb.Misc.createDate(Request.QueryString["date"].Trim());                    
                    
                    setLanguage(date);
                }

                InitializeGraphData();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairBarPreview.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/IOPairBarPreview.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage(DateTime date)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(IOPairBarPreview).Assembly);

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

                lblEmployee.Text = emplTO.LastName.Trim() + " " + emplTO.FirstName.Trim();
                lblDate.Text = date.ToString(Constants.dateFormat);

                btnCancel.Text = rm.GetString("btnCancel", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, PassTypeTO> getPassTypeDictionary(int company)
        {
            try
            {
                Dictionary<int, PassTypeTO> ptDic = new Dictionary<int, PassTypeTO>();

                if (company != -1)
                {
                    bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());
                    ptDic = new PassType(Session[Constants.sessionConnection]).SearchForCompanyDictionary(company, isAltLang);
                }

                return ptDic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<PassTypeTO> getPassTypeList(int company)
        {
            try
            {
                List<PassTypeTO> ptList = new List<PassTypeTO>();

                if (company != -1)
                {
                    bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());
                    ptList = new PassType(Session[Constants.sessionConnection]).SearchForCompany(company, isAltLang);
                }

                return ptList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<string, RuleTO> getRules(int emplTypeID, int company)
        {
            try
            {
                Dictionary<string, RuleTO> rules = new Dictionary<string, RuleTO>();

                if (emplTypeID != -1 && company != -1)
                {
                    Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                    rule.RuleTO.EmployeeTypeID = emplTypeID;
                    rule.RuleTO.WorkingUnitID = company;
                    List<RuleTO> rulesList = rule.Search();

                    foreach (RuleTO ruleTO in rulesList)
                    {
                        if (!rules.ContainsKey(ruleTO.RuleType))
                            rules.Add(ruleTO.RuleType, ruleTO);
                        else
                            rules[ruleTO.RuleType] = ruleTO;
                    }
                }

                return rules;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeGraphData()
        {
            try
            {
                int company = -1;
                int emplID = -1;
                int emplTypeID = -1;
                DateTime date = new DateTime();

                // get date, employee type and company from query string
                if (Request.QueryString["company"] != null && !int.TryParse(Request.QueryString["company"].Trim(), out company))
                    company = -1;
                if (Request.QueryString["date"] != null)
                    date = CommonWeb.Misc.createDate(Request.QueryString["date"].Trim());
                if (Request.QueryString["emplID"] != null && !int.TryParse(Request.QueryString["emplID"].Trim(), out emplID))
                    emplID = -1;
                if (Request.QueryString["emplTypeID"] != null && !int.TryParse(Request.QueryString["emplTypeID"].Trim(), out emplTypeID))
                    emplTypeID = -1;

                if (!date.Equals(new DateTime()))
                {
                    List<PassTypeTO> ptList = getPassTypeList(company);
                    Dictionary<int, PassTypeTO> ptDic = getPassTypeDictionary(company);
                    Dictionary<string, RuleTO> rules = getRules(emplTypeID, company);
                    Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                    // get time schedule and day time schema intervals for current day
                    List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplID.ToString().Trim(), date, date, null);
                    if (emplSchedules.ContainsKey(emplID))
                        timeScheduleList = emplSchedules[emplID];

                    List<WorkTimeIntervalTO> timeSchemaIntervalList = Common.Misc.getTimeSchemaInterval(date, timeScheduleList, schemas);

                    Dictionary<DateTime, List<IOPairsProcessedHistTO>> IOPairHistList = new IOPairsProcessedHist(Session[Constants.sessionConnection]).SearchIOPairsSet(emplID, date);

                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (timeSchemaIntervalList.Count > 0 && schemas.ContainsKey(timeSchemaIntervalList[0].TimeSchemaID))
                        schema = schemas[timeSchemaIntervalList[0].TimeSchemaID];

                    DrawGraphControl(IOPairHistList, date, ptDic, ptList, timeSchemaIntervalList, rules, schema);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void DrawGraphControl(Dictionary<DateTime, List<IOPairsProcessedHistTO>> IOPairHistList, DateTime date, Dictionary<int, PassTypeTO> passTypes, List<PassTypeTO> passTypesList,
            List<WorkTimeIntervalTO> timeSchemaIntervalList, Dictionary<string, RuleTO> rules, WorkTimeSchemaTO schema)
        {
            try
            {
                // set legend control
                foreach (Control ctrl in legendCtrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                legendCtrlHolder.Controls.Clear();

                int company = -1;
                if (Request.QueryString["company"] != null && !int.TryParse(Request.QueryString["company"].Trim(), out company))
                    company = -1;

                LegendUC legendCtrl = new LegendUC();
                legendCtrl.ID = "legend";
                legendCtrl.Company = company;

                legendCtrlHolder.Controls.Add(legendCtrl);

                foreach (Control ctrl in ctrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                ctrlHolder.Controls.Clear();

                int pairSetCounter = 0;

                // check pass types/locations visibility
                Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                rule.RuleTO.RuleType = Constants.RulePairsByLocation;
                List<RuleTO> locRules = rule.Search();

                bool byLocations = false;
                Dictionary<int, LocationTO> locDict = new Dictionary<int, LocationTO>();
                if (locRules.Count > 0 && locRules[0].RuleValue == Constants.yesInt)
                {
                    byLocations = true;
                    locDict = new Location(Session[Constants.sessionConnection]).SearchDict();
                }

                for (int i = IOPairHistList.Keys.Count - 1; i >= 0; i--)                
                {
                    DateTime modTime = IOPairHistList.Keys.ElementAt<DateTime>(i);

                    pairSetCounter++;

                    List<IOPairProcessedTO> pairsList = getPairsList(IOPairHistList[modTime]);

                    // insert hour line
                    HoursLineControlUC hourLine = new HoursLineControlUC();
                    hourLine.ID = "hourLine" + pairSetCounter.ToString().Trim();
                    if (rules.ContainsKey(Constants.RuleNightWork))
                        hourLine.NightWorkRule = rules[Constants.RuleNightWork];
                    hourLine.ShowOnlyPairs = true;
                    ctrlHolder.Controls.Add(hourLine);

                    Color backColor = Color.White;

                    // insert pairs bars
                    EmployeeWorkingDayViewUC emplDay = new EmployeeWorkingDayViewUC();
                    emplDay.ID = "emplDayView" + pairSetCounter.ToString().Trim();
                    emplDay.DayPairList = pairsList;
                    emplDay.DayIntervalList = timeSchemaIntervalList;
                    emplDay.PassTypes = passTypes;
                    emplDay.BackColor = backColor;
                    emplDay.Date = date;
                    emplDay.IsFirst = true;
                    emplDay.AllowChange = false;
                    emplDay.ShowOnlyPairs = true;
                    emplDay.EmplTimeSchema = schema;
                    emplDay.HistDay = true;
                    ctrlHolder.Controls.Add(emplDay);

                    // insert pairs data
                    int ctrlCounter = 0;
                    foreach (IOPairProcessedTO pair in pairsList)
                    {
                        IOPairDataUC pairData = new IOPairDataUC();
                        pairData.ID = "pairData" + ctrlCounter.ToString().Trim() + pairSetCounter.ToString().Trim();
                        pairData.PairTO = pair;
                        pairData.Rules = rules;
                        pairData.PassTypesAll = passTypesList;
                        pairData.PassTypesAllDic = passTypes;
                        pairData.LocationsDic = locDict;
                        pairData.DayIntervals = timeSchemaIntervalList;
                        pairData.AllowChange = false;
                        pairData.EmplTimeSchema = schema;
                        pairData.ByLocations = byLocations;
                        ctrlHolder.Controls.Add(pairData);

                        ctrlCounter++;
                    }

                    // insert separator label
                    Label lblSeparator = new Label();
                    lblSeparator.ID = "lblSeparator" + pairSetCounter.ToString().Trim();
                    lblSeparator.Width = new Unit(755);
                    lblSeparator.Height = new Unit(20);
                    ctrlHolder.Controls.Add(lblSeparator);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> getPairsList(List<IOPairsProcessedHistTO> pairsHistList)
        {
            try
            {
                List<IOPairProcessedTO> pairsList = new List<IOPairProcessedTO>();

                foreach (IOPairsProcessedHistTO pairHist in pairsHistList)
                {
                    pairsList.Add(new IOPairProcessedTO(pairHist));
                }

                return pairsList;
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "IOPairBarPreview.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "IOPairBarPreview.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearSessionValues();

                // close pop up window
                ClientScript.RegisterStartupScript(GetType(), "close", "window.close();", true);

                Message = "";
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairBarPreview.btnCancel_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/IOPairBarPreview.aspx", false);
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
    }
}
