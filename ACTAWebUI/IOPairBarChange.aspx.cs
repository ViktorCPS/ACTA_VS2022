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
    public partial class IOPairBarChange : System.Web.UI.Page
    {
        const string pageName = "IOPairBarChange";

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
                    btnPrev.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnPrev.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnNext.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnNext.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    DateTime date = new DateTime();
                    if (Request.QueryString["date"] != null)
                        date = CommonWeb.Misc.createDate(Request.QueryString["date"].Trim());

                    if (date.Equals(new DateTime()))
                        tbDate.Text = "";
                    else
                        tbDate.Text = date.ToString(Constants.dateFormat.Trim());

                    chbDateCompany.Checked = false;
                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                        chbDateCompany.Visible = true;
                    else
                        chbDateCompany.Visible = false;

                    InitializePage();

                    if (Session[Constants.sessionMinChangingDate] != null && Session[Constants.sessionMinChangingDate] is DateTime && date.Date < ((DateTime)Session[Constants.sessionMinChangingDate]).Date)
                    {
                        string message = " INCORRECT DATE OPENING - DATE CHANGED: "
                            + date.Date.ToString(Constants.dateFormat) + " MIN CHANGED DATE: " + ((DateTime)Session[Constants.sessionMinChangingDate]).Date.ToString(Constants.dateFormat);

                        if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO)
                            message += " USER: " + ((ApplUserTO)Session[Constants.sessionLogInUser]).UserID;

                        if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                            message += " CATEGORY: " + ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).Name;

                        int emplID = -1;
                        if (Request.QueryString["emplID"] != null && !int.TryParse(Request.QueryString["emplID"].Trim(), out emplID))
                            emplID = -1;
                        message += " EMPLOYEE: " + emplID.ToString().Trim();

                        Common.Misc.writeIncorrectChangingDate(DateTime.Now.ToString(Constants.dateFormat + " " + Constants.timeFormat + message));
                    }
                }

                InitializeGraphData();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairBarChange.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/IOPairBarChange.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void InitializePage()
        {
            try
            {
                ClearSessionValues();

                int emplID = -1;
                DateTime date = new DateTime();

                // get date and emplID from query string
                if (Request.QueryString["emplID"] != null && !int.TryParse(Request.QueryString["emplID"].Trim(), out emplID))
                    emplID = -1;
                
                date = CommonWeb.Misc.createDate(tbDate.Text.Trim());

                bool forConfirmation = false;
                bool forVerification = false;
                if (Request.QueryString["confirm"] != null)
                    forConfirmation = Request.QueryString["confirm"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper());
                if (Request.QueryString["verify"] != null)
                    forVerification = Request.QueryString["verify"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper());

                if (forConfirmation || forVerification)
                    btnPrev.Visible = btnNext.Visible = false;
                else
                    btnPrev.Visible = btnNext.Visible = true;

                setLanguage(date);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setLanguage(DateTime date)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(IOPairBarChange).Assembly);

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
                lblHelpMessage.Text = rm.GetString("pair0Message", culture);

                chbDateCompany.Text = rm.GetString("chbDateCompany", culture);

                //btnPrev.Text = rm.GetString("btnPrev", culture);
                //btnNext.Text = rm.GetString("btnNext", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnUndo.Text = rm.GetString("btnUndoChanges", culture);
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
                date = CommonWeb.Misc.createDate(tbDate.Text.Trim());

                if (Request.QueryString["company"] != null && !int.TryParse(Request.QueryString["company"].Trim(), out company))
                    company = -1;                
                if (Request.QueryString["emplID"] != null && !int.TryParse(Request.QueryString["emplID"].Trim(), out emplID))
                    emplID = -1;
                if (chbDateCompany.Checked)
                    company = getDateCompany(emplID, date, company);
                if (Request.QueryString["emplTypeID"] != null && !int.TryParse(Request.QueryString["emplTypeID"].Trim(), out emplTypeID))
                    emplTypeID = -1;

                if (!date.Equals(new DateTime()))
                {
                    Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplID.ToString().Trim());
                    EmployeeAsco4TO emplAsco = new EmployeeAsco4TO();
                    if (ascoDict.ContainsKey(emplID))
                        emplAsco = ascoDict[emplID];

                    InitializeCounters(emplID, emplAsco);

                    List<PassTypeTO> ptList = getPassTypeList(company);
                    Dictionary<int, PassTypeTO> ptDic = getPassTypeDictionary(company);
                    Dictionary<string, RuleTO> rules = getRules(emplTypeID, company);
                    Dictionary<int, WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).getDictionary();

                    // get time schedule and day time schema intervals for current, previous and next day
                    List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();
                    Dictionary<int, List<EmployeeTimeScheduleTO>> emplSchedules = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedulesExactDate(emplID.ToString().Trim(), date.AddDays(-1), date.AddDays(1), null);
                    if (emplSchedules.ContainsKey(emplID))
                        timeScheduleList = emplSchedules[emplID];

                    List<WorkTimeIntervalTO> timeSchemaIntervalList = Common.Misc.getTimeSchemaInterval(date, timeScheduleList, schemas);
                    List<WorkTimeIntervalTO> prevDayIntervalList = Common.Misc.getTimeSchemaInterval(date.AddDays(-1), timeScheduleList, schemas);
                    List<WorkTimeIntervalTO> nextDayIntervalList = Common.Misc.getTimeSchemaInterval(date.AddDays(1), timeScheduleList, schemas);

                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (timeSchemaIntervalList.Count > 0 && schemas.ContainsKey(timeSchemaIntervalList[0].TimeSchemaID))
                        schema = schemas[timeSchemaIntervalList[0].TimeSchemaID];

                    List<IOPairProcessedTO> IOPairList = FindIOPairsForEmployee(emplID, date, timeSchemaIntervalList, prevDayIntervalList, nextDayIntervalList, schema, ptDic);
                    DrawGraphControl(IOPairList, date, ptDic, timeSchemaIntervalList, rules, schema, company);

                    // if date is not as original sent, check if selected previous/next day can be changed
                    DateTime reqDate = new DateTime();
                    if (Request.QueryString["date"] != null)
                        reqDate = CommonWeb.Misc.createDate(Request.QueryString["date"].Trim());
                    bool allowChange = true;

                    //VIKTOR UBACIO DA BLOKIRA IZMENU PAROVA U DANIMA KADA NE SME DA SE MENJA --- 4septembar2024
                    int tt = (int)Constants.Categories.TL;
                    int wcdr = (int)Constants.Categories.WCManager;
                    int wcss = (int)Constants.Categories.WC;
                    if ((((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == tt || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == wcdr || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == wcss) && (DateTime.Now.Month) >= (date.Month + 1) && date.Year<=DateTime.Now.Year)
                    {
                        int maxRadniDani = rules["CUTOFF DATE"].RuleValue;
                        int trenutniRadniDani = DateTime.Now.Day;
                        if (maxRadniDani < trenutniRadniDani)
                            allowChange = false;
                        if (reqDate.Month > date.Month + 1)
                            allowChange = false;
                    }

                    if (reqDate.Date != date.Date)
                    {
                        if (!emplAsco.DatetimeValue2.Equals(new DateTime()) && emplAsco.DatetimeValue2.Date <= date.Date && (emplAsco.DatetimeValue3.Equals(new DateTime()) || emplAsco.DatetimeValue3 >= date.Date))
                        {
                            List<IOPairProcessedTO> ioPairsForDay = new List<IOPairProcessedTO>();
                            foreach (IOPairProcessedTO dayPair in IOPairList)
                            {
                                if (dayPair.IOPairDate.Date == date.Date)
                                    ioPairsForDay.Add(dayPair);
                            }
                            if (CommonWeb.Misc.isVerifiedConfirmedDay(ioPairsForDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser])))
                                allowChange = false;
                        }
                        else
                            allowChange = false;
                    }
                    
                    InitializePairsDataPanel(IOPairList, ptDic, ptList, timeSchemaIntervalList, prevDayIntervalList, nextDayIntervalList, rules, schema, date, emplAsco, schemas, allowChange);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeCounters(int emplID, EmployeeAsco4TO ascoTO)
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

                Dictionary<int, EmployeeCounterValueTO> emplCounters = new Dictionary<int, EmployeeCounterValueTO>();

                if (Session[Constants.sessionEmplCounters] == null)
                {
                    if (emplID != -1)
                    {
                        EmployeeCounterValue emplCValue = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                        emplCValue.ValueTO.EmplID = emplID;
                        List<EmployeeCounterValueTO> counterValues = emplCValue.Search();

                        foreach (EmployeeCounterValueTO val in counterValues)
                        {
                            emplCounters.Add(val.EmplCounterTypeID, val);
                        }

                        Session[Constants.sessionEmplCounters] = emplCounters;
                    }
                }
                else if (Session[Constants.sessionEmplCounters] is Dictionary<int, EmployeeCounterValueTO>)
                    emplCounters = (Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters];

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
                        if (ascoTO.NVarcharValue5.Trim() != "")
                        {
                            ApplUserXApplUserCategory userXCat = new ApplUserXApplUserCategory(Session[Constants.sessionConnection]);
                            userXCat.UserXCategoryTO.UserID = ascoTO.NVarcharValue5.Trim();
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
                emplCounter.CounterValues = emplCounters;
                emplCounter.CounterNames = counterNames;
                emplCounter.EmplID = emplID;
                emplCounter.HideBH = hideBH;
                emplCounter.InsertSeparator = false;                
                counterCtrlHolder.Controls.Add(emplCounter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> FindIOPairsForEmployee(int emplID, DateTime date, List<WorkTimeIntervalTO> timeSchemaIntervalList,
            List<WorkTimeIntervalTO> prevIntervals, List<WorkTimeIntervalTO> nextIntervals, WorkTimeSchemaTO schema, Dictionary<int, PassTypeTO> passTypes)
        {
            try
            {
                List<IOPairProcessedTO> IOPairList = new List<IOPairProcessedTO>();

                if (Session[Constants.sessionDayPairs] == null)
                {
                    List<DateTime> datesList = new List<DateTime>();
                    datesList.Add(date.AddDays(-1));
                    datesList.Add(date);
                    datesList.Add(date.AddDays(1));

                    // get pairs for previous and next day in case of third shifts
                    IOPairList = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(emplID.ToString().Trim(), datesList, "");

                    // check if there exists pairs for previous and next day intervals
                    // key is interval index, value is index of first pair from interval
                    Dictionary<int, int> intervalPairsIndexesPrevDay = new Dictionary<int, int>();
                    for (int intervalIndex = 0; intervalIndex < prevIntervals.Count; intervalIndex++)
                    {
                        WorkTimeIntervalTO interval = prevIntervals[intervalIndex];
                        if (interval.StartTime.TimeOfDay.Equals(interval.EndTime.TimeOfDay))
                            continue;

                        int pairIndex = 0;
                        while (pairIndex < IOPairList.Count)
                        {
                            TimeSpan start = interval.StartTime.TimeOfDay;
                            TimeSpan end = interval.EndTime.TimeOfDay;

                            if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                            {
                                start = interval.EarliestArrived.TimeOfDay;
                                end = interval.LatestLeft.TimeOfDay;
                            }

                            if (IOPairList[pairIndex].IOPairDate.Date.Equals(date.AddDays(-1).Date)
                                && ((IOPairList[pairIndex].StartTime.TimeOfDay <= start && IOPairList[pairIndex].EndTime.TimeOfDay > start)
                                || (IOPairList[pairIndex].StartTime.TimeOfDay < end && IOPairList[pairIndex].EndTime.TimeOfDay >= end)
                                || (IOPairList[pairIndex].StartTime.TimeOfDay >= start && IOPairList[pairIndex].EndTime.TimeOfDay <= end)))
                            {
                                if (!intervalPairsIndexesPrevDay.ContainsKey(intervalIndex))
                                    intervalPairsIndexesPrevDay.Add(intervalIndex, pairIndex);
                                break;
                            }

                            pairIndex++;
                        }

                        if (pairIndex >= IOPairList.Count && !intervalPairsIndexesPrevDay.ContainsKey(intervalIndex))
                            intervalPairsIndexesPrevDay.Add(intervalIndex, -1);
                    }

                    // key is interval index, value is index of first pair from interval
                    Dictionary<int, int> intervalPairsIndexesNextDay = new Dictionary<int, int>();
                    for (int intervalIndex = 0; intervalIndex < nextIntervals.Count; intervalIndex++)
                    {
                        WorkTimeIntervalTO interval = nextIntervals[intervalIndex];
                        if (interval.StartTime.TimeOfDay.Equals(interval.EndTime.TimeOfDay))
                            continue;

                        int pairIndex = 0;
                        while (pairIndex < IOPairList.Count)
                        {
                            TimeSpan start = interval.StartTime.TimeOfDay;
                            TimeSpan end = interval.EndTime.TimeOfDay;

                            if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                            {
                                start = interval.EarliestArrived.TimeOfDay;
                                end = interval.LatestLeft.TimeOfDay;
                            }

                            if (IOPairList[pairIndex].IOPairDate.Date.Equals(date.AddDays(1).Date)
                                && ((IOPairList[pairIndex].StartTime.TimeOfDay <= start && IOPairList[pairIndex].EndTime.TimeOfDay > start)
                                || (IOPairList[pairIndex].StartTime.TimeOfDay < end && IOPairList[pairIndex].EndTime.TimeOfDay >= end)
                                || (IOPairList[pairIndex].StartTime.TimeOfDay >= start && IOPairList[pairIndex].EndTime.TimeOfDay <= end)))
                            {
                                if (!intervalPairsIndexesNextDay.ContainsKey(intervalIndex))
                                    intervalPairsIndexesNextDay.Add(intervalIndex, pairIndex);
                                break;
                            }

                            pairIndex++;
                        }

                        if (pairIndex >= IOPairList.Count && !intervalPairsIndexesNextDay.ContainsKey(intervalIndex))
                            intervalPairsIndexesNextDay.Add(intervalIndex, -1);
                    }

                    // remove all pairs from previous and next day if they do not belong to night shift or are not whole interval absence
                    IEnumerator<IOPairProcessedTO> pairEnumerator = IOPairList.GetEnumerator();
                    while (pairEnumerator.MoveNext())
                    {
                        bool remove = true;

                        // previous day
                        if (pairEnumerator.Current.IOPairDate.Date.Equals(date.AddDays(-1).Date))
                        {
                            if (pairEnumerator.Current.EndTime.Hour == 23 && pairEnumerator.Current.EndTime.Minute == 59)
                            {
                                foreach (WorkTimeIntervalTO prevInterval in prevIntervals)
                                {
                                    if (isWholeIntervalPair(pairEnumerator.Current, prevInterval, schema) && (pairEnumerator.Current.PassTypeID == Constants.absence
                                    || (passTypes.ContainsKey(pairEnumerator.Current.PassTypeID) && passTypes[pairEnumerator.Current.PassTypeID].IsPass == Constants.wholeDayAbsence)))
                                    {
                                        remove = false;
                                        break;
                                    }
                                }
                            }
                        }
                        // next day
                        else if (pairEnumerator.Current.IOPairDate.Date.Equals(date.AddDays(1).Date))
                        {
                            if (pairEnumerator.Current.StartTime.Hour == 0 && pairEnumerator.Current.StartTime.Minute == 0)
                            {
                                foreach (WorkTimeIntervalTO nextInterval in nextIntervals)
                                {
                                    if (isWholeIntervalPair(pairEnumerator.Current, nextInterval, schema) && (pairEnumerator.Current.PassTypeID == Constants.absence
                                    || (passTypes.ContainsKey(pairEnumerator.Current.PassTypeID) && passTypes[pairEnumerator.Current.PassTypeID].IsPass == Constants.wholeDayAbsence)))
                                    {
                                        remove = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                            remove = false;

                        if (remove)
                        {
                            IOPairList.Remove(pairEnumerator.Current);
                            pairEnumerator = IOPairList.GetEnumerator();
                        }
                    }

                    // key is interval index, value is index of first pair from interval
                    Dictionary<int, int> intervalPairsIndexes = new Dictionary<int, int>();                    
                    for (int intervalIndex = 0; intervalIndex < timeSchemaIntervalList.Count; intervalIndex++)
                    {
                        WorkTimeIntervalTO interval = timeSchemaIntervalList[intervalIndex];
                        if (interval.StartTime.TimeOfDay.Equals(interval.EndTime.TimeOfDay))
                            continue;
                        
                        int pairIndex = 0;
                        while (pairIndex < IOPairList.Count)
                        {
                            TimeSpan start = interval.StartTime.TimeOfDay;
                            TimeSpan end = interval.EndTime.TimeOfDay;

                            if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                            {
                                start = interval.EarliestArrived.TimeOfDay;
                                end = interval.LatestLeft.TimeOfDay;
                            }

                            if (IOPairList[pairIndex].IOPairDate.Date.Equals(date.Date)
                                && ((IOPairList[pairIndex].StartTime.TimeOfDay <= start && IOPairList[pairIndex].EndTime.TimeOfDay > start)
                                || (IOPairList[pairIndex].StartTime.TimeOfDay < end && IOPairList[pairIndex].EndTime.TimeOfDay >= end)
                                || (IOPairList[pairIndex].StartTime.TimeOfDay >= start && IOPairList[pairIndex].EndTime.TimeOfDay <= end)))
                            {
                                if (!intervalPairsIndexes.ContainsKey(intervalIndex))
                                    intervalPairsIndexes.Add(intervalIndex, pairIndex);
                                break;
                            }

                            pairIndex++;
                        }

                        if (pairIndex >= IOPairList.Count && !intervalPairsIndexes.ContainsKey(intervalIndex))
                            intervalPairsIndexes.Add(intervalIndex, -1);
                    }

                    foreach (int index in intervalPairsIndexes.Keys)
                    {
                        if (intervalPairsIndexes[index] == -1)
                        {
                            if (index >= 0 && index < timeSchemaIntervalList.Count)
                            {
                                DateTime startTime = timeSchemaIntervalList[index].StartTime;
                                DateTime endTime = timeSchemaIntervalList[index].EndTime;

                                if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                                {
                                    startTime = timeSchemaIntervalList[index].EarliestArrived;
                                    endTime = timeSchemaIntervalList[index].EarliestLeft;
                                }

                                // check if missing pair is from first or from second interval of night shift - if there are two intervals
                                if (intervalPairsIndexes.ContainsKey(index + 1))
                                {
                                    IOPairList.Insert(0, unjustifiedPair(date.Date, new DateTime(date.Year, date.Month, date.Day, startTime.Hour,
                                        startTime.Minute, 0), new DateTime(date.Year, date.Month, date.Day, endTime.Hour,
                                            endTime.Minute, 0), emplID));
                                }
                                else
                                {
                                    IOPairList.Add(unjustifiedPair(date.Date, new DateTime(date.Year, date.Month, date.Day, startTime.Hour,
                                        startTime.Minute, 0), new DateTime(date.Year, date.Month, date.Day, endTime.Hour,
                                            endTime.Minute, 0), emplID));
                                }

                                // if interval is second interval from night shift, insert unjustified to previous day night shift interval
                                if (Common.Misc.isThirdShiftEndInterval(timeSchemaIntervalList[index]))
                                {
                                    for (int i = 0; i < prevIntervals.Count; i++)
                                    {
                                        WorkTimeIntervalTO prevInterval = prevIntervals[i];
                                        if (Common.Misc.isThirdShiftBeginningInterval(prevInterval))
                                        {
                                            DateTime start = prevInterval.StartTime;
                                            DateTime end = prevInterval.EndTime;

                                            if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                                            {
                                                start = prevInterval.EarliestArrived;
                                                end = prevInterval.EarliestLeft;
                                            }

                                            if (intervalPairsIndexesPrevDay.ContainsKey(i) && intervalPairsIndexesPrevDay[i] == -1)                                                
                                                IOPairList.Insert(0, unjustifiedPair(date.AddDays(-1).Date, new DateTime(date.AddDays(-1).Year, date.AddDays(-1).Month, date.AddDays(-1).Day,
                                                    start.Hour, start.Minute, 0), new DateTime(date.AddDays(-1).Year, date.AddDays(-1).Month, date.AddDays(-1).Day,
                                                        end.Hour, end.Minute, 0), emplID));
                                        }
                                    }
                                }

                                // if interval is first interval from night shift, insert unjustified to next day night shift interval
                                if (Common.Misc.isThirdShiftBeginningInterval(timeSchemaIntervalList[index]))
                                {
                                    for (int i = 0; i < nextIntervals.Count; i++)
                                    {
                                        WorkTimeIntervalTO nextInterval = nextIntervals[i];

                                        if (Common.Misc.isThirdShiftEndInterval(nextInterval))
                                        {
                                            DateTime start = nextInterval.StartTime;
                                            DateTime end = nextInterval.EndTime;

                                            if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                                            {
                                                start = nextInterval.EarliestArrived;
                                                end = nextInterval.EarliestLeft;
                                            }

                                            if (intervalPairsIndexesNextDay.ContainsKey(i) && intervalPairsIndexesNextDay[i] == -1)                                            
                                                IOPairList.Add(unjustifiedPair(date.AddDays(1).Date, new DateTime(date.AddDays(1).Year, date.AddDays(1).Month, date.AddDays(1).Day,
                                                    start.Hour, start.Minute, 0), new DateTime(date.AddDays(1).Year, date.AddDays(1).Month, date.AddDays(1).Day,
                                                        end.Hour, end.Minute, 0), emplID));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (IOPairList.Count <= 0)
                        IOPairList.Add(overtimePair(date.Date, emplID));

                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();

                    foreach (IOPairProcessedTO pair in IOPairList)
                    {
                        dayPairs.Add(new IOPairProcessedTO(pair));
                    }

                    Session[Constants.sessionDayPairs] = dayPairs;
                }
                else
                {
                    if (Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                    {

                        List<IOPairProcessedTO> t1 = new List<IOPairProcessedTO>();
                        try
                        {

                            t1 = (List<IOPairProcessedTO>) Session[Constants.sessionDayPairs];
                        }
                        catch(Exception e)
                        {

                        }
                        IOPairList = new List<IOPairProcessedTO>();
                        foreach (IOPairProcessedTO pairTO in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                        {
                            IOPairList.Add(new IOPairProcessedTO(pairTO));
                        }
                    }
                }

                return IOPairList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isWholeIntervalPair(IOPairProcessedTO pair, WorkTimeIntervalTO interval, WorkTimeSchemaTO schema)
        {
            try
            {
                bool wholeIntervalPair = false;

                if (schema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                        && pair.EndTime.Subtract(pair.StartTime).TotalMinutes == interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                    {
                        wholeIntervalPair = true;
                    }
                }
                else if (interval.EndTime.TimeOfDay == pair.EndTime.TimeOfDay && interval.StartTime.TimeOfDay == pair.StartTime.TimeOfDay)
                    wholeIntervalPair = true;

                return wholeIntervalPair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void DrawGraphControl(List<IOPairProcessedTO> IOPairList, DateTime date, Dictionary<int, PassTypeTO> passTypes, List<WorkTimeIntervalTO> timeSchemaIntervalList, 
            Dictionary<string, RuleTO> rules, WorkTimeSchemaTO schema, int company)
        {
            try
            {
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

                foreach (Control ctrl in ctrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                ctrlHolder.Controls.Clear();

                HoursLineControlUC hourLine = new HoursLineControlUC();
                hourLine.ID = "hourLine";
                if (rules.ContainsKey(Constants.RuleNightWork))
                    hourLine.NightWorkRule = rules[Constants.RuleNightWork];
                hourLine.ShowOnlyPairs = true;
                ctrlHolder.Controls.Add(hourLine);

                Color backColor = Color.White;
                                
                EmployeeWorkingDayViewUC emplDay = new EmployeeWorkingDayViewUC();
                emplDay.ID = "emplDayView";
                emplDay.DayPairList = IOPairList;
                emplDay.DayIntervalList = timeSchemaIntervalList;
                emplDay.PassTypes = passTypes;
                emplDay.BackColor = backColor;
                emplDay.Date = date;
                emplDay.IsFirst = true;
                emplDay.AllowChange = false;
                emplDay.ShowOnlyPairs = true;
                emplDay.EmplTimeSchema = schema;
                                
                ctrlHolder.Controls.Add(emplDay);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializePairsDataPanel(List<IOPairProcessedTO> IOPairList, Dictionary<int, PassTypeTO> passTypes, List<PassTypeTO> passTypesList, 
            List<WorkTimeIntervalTO> timeSchemaIntervalList, List<WorkTimeIntervalTO> prevDayIntervalList, List<WorkTimeIntervalTO> nextDayIntervalList, 
            Dictionary<string, RuleTO> rules, WorkTimeSchemaTO timeSchema, DateTime date, EmployeeAsco4TO emplAsco, Dictionary<int, WorkTimeSchemaTO> schDict, bool allowChange)
        {
            try
            {
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

                // get confirmation types
                Dictionary<int, List<int>> confirmationTypes = new PassTypesConfirmation(Session[Constants.sessionConnection]).SearchDictionary();

                foreach (Control ctrl in pairDataCtrlHolder.Controls)
                {
                    ctrl.Dispose();
                }

                pairDataCtrlHolder.Controls.Clear();

                int ctrlCounter = 0;
                bool forConfirmation = false;
                bool forVerification = false;
                uint pairRecID = 0;
                int confirmType = -1;
                int verifyType = -1;
                if (Request.QueryString["confirm"] != null)
                    forConfirmation = Request.QueryString["confirm"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper());
                if (Request.QueryString["verify"] != null)
                    forVerification = Request.QueryString["verify"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper());
                if (Request.QueryString["confirmType"] != null)
                    if (!int.TryParse(Request.QueryString["confirmType"].Trim(), out confirmType))
                        confirmType = -1;
                if (Request.QueryString["verifyType"] != null)
                    if (!int.TryParse(Request.QueryString["verifyType"].Trim(), out verifyType))
                        verifyType = -1;
                if (Request.QueryString["pairRecID"] != null)
                    if (!uint.TryParse(Request.QueryString["pairRecID"].Trim(), out pairRecID))
                        pairRecID = 0;

                // check if employee is expat out
                Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesAll = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();
                rulesAll.Add(Constants.defaultWorkingUnitID, new Dictionary<int, Dictionary<string, RuleTO>>());
                rulesAll[Constants.defaultWorkingUnitID].Add((int)Constants.EmployeeTypesFIAT.BC, getRules((int)Constants.EmployeeTypesFIAT.BC, Constants.defaultWorkingUnitID));
                EmployeeTO emplTO = new Employee(Session[Constants.sessionConnection]).Find(emplAsco.EmployeeID.ToString().Trim());
                bool isExpatOut = Common.Misc.isExpatOut(rulesAll, emplTO);

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

                // get whole day absence by hour types
                List<int> wholeDayAbsenceByHour = new List<int>();
                foreach (string type in rules.Keys)
                {
                    if (type.StartsWith(Constants.RuleWholeDayAbsenceByHour.Trim()))
                        wholeDayAbsenceByHour.Add(rules[type].RuleValue);
                }

                foreach (IOPairProcessedTO pair in IOPairList)
                {
                    if (pair.IOPairDate.Date.Equals(date.Date))
                    {
                        IOPairDataUC pairData = new IOPairDataUC();
                        pairData.ID = "pairData" + ctrlCounter.ToString().Trim();
                        pairData.PairTO = pair;
                        pairData.Rules = rules;
                        pairData.PassTypesAll = passTypesList;
                        pairData.PassTypesAllDic = passTypes;
                        pairData.LocationsDic = locDict;
                        pairData.SchDict = schDict;
                        pairData.DayIntervals = timeSchemaIntervalList;
                        pairData.PrevDayIntervals = prevDayIntervalList;
                        pairData.NextDayIntervals = nextDayIntervalList;
                        pairData.PassTypeLimits = ptLimits;
                        pairData.ConfirmationTypes = confirmationTypes;
                        pairData.ForConfirmation = forConfirmation;
                        pairData.ConfirmType = confirmType;
                        pairData.ForVerification = forVerification;
                        pairData.VerifyType = verifyType;
                        pairData.ByLocations = byLocations;
                        pairData.WholeDayAbsenceByHour = wholeDayAbsenceByHour;
                        if (timeSchemaIntervalList.Count == 0 ||
                            (timeSchemaIntervalList.Count == 1 && timeSchemaIntervalList[0].StartTime.Hour == 0 && timeSchemaIntervalList[0].StartTime.Minute == 0))
                            pairData.IsWorkAbsenceDay = true;
                        else
                            pairData.IsWorkAbsenceDay = false;
                        if (passTypes.ContainsKey(pair.PassTypeID))
                        {
                            if (!allowChange)
                                pairData.AllowChange = false;
                            else if ((forConfirmation || forVerification) && pair.RecID != pairRecID)
                                pairData.AllowChange = false;
                            else
                                pairData.AllowChange = (Session[Constants.sessionLoginCategoryPassTypes] != null && Session[Constants.sessionLoginCategoryPassTypes] is List<int>
                                    && ((List<int>)Session[Constants.sessionLoginCategoryPassTypes]).Contains(pair.PassTypeID));
                        }
                        else
                            pairData.AllowChange = false;                      

                        pairData.EmplTimeSchema = timeSchema;
                        pairData.EmplAsco = emplAsco;
                        pairData.Empl = emplTO;
                        pairData.IsExpatOut = isExpatOut;
                        pairData.BubbleClick += new ControlEventHandler(BubbleControl_BubbleClick);
                        pairDataCtrlHolder.Controls.Add(pairData);

                        ctrlCounter++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void BubbleControl_BubbleClick(object sender, ControlEventArgs e)
        {
            try
            {
                // action on parent page
                InitializeGraphData();
                lblError.Text = e.Error.Trim();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairBarChange.BubbleControl_BubbleClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/IOPairBarChange.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void chbDateCompany_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                InitializePage();
                InitializeGraphData();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairBarChange.chbDateCompany_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/IOPairBarChange.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = new DateTime();

                date = CommonWeb.Misc.createDate(tbDate.Text.Trim());

                if (!date.Equals(new DateTime()))
                    tbDate.Text = date.AddDays(1).ToString(Constants.dateFormat.Trim());

                lblError.Text = "";
                InitializePage();
                InitializeGraphData();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairBarChange.btnNext_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/IOPairBarChange.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = new DateTime();

                date = CommonWeb.Misc.createDate(tbDate.Text.Trim());

                if (!date.Equals(new DateTime()))
                    tbDate.Text = date.AddDays(-1).ToString(Constants.dateFormat.Trim());

                lblError.Text = "";
                InitializePage();
                InitializeGraphData();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairBarChange.btnPrev_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/IOPairBarChange.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnUndo_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                InitializePage();
                InitializeGraphData();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairBarChange.btnUndo_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/IOPairBarChange.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // if system is closed, sign out and redirect to login page
                if (Common.Misc.getClosingEvent(DateTime.Now, Session[Constants.sessionConnection]).EventID != -1)
                    CommonWeb.Misc.LogOut(Session[Constants.sessionLogInUserLog], Session[Constants.sessionLogInUser], Session[Constants.sessionConnection], Session, Response);

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(IOPairBarChange).Assembly);

                DateTime dateChanging = CommonWeb.Misc.createDate(tbDate.Text.Trim());

                // if customer is FIAT and user is not HRRSC, check if there is attempt to change day less then allowed
                if (Session[Constants.sessionMinChangingDate] != null && Session[Constants.sessionMinChangingDate] is DateTime && dateChanging.Date < ((DateTime)Session[Constants.sessionMinChangingDate]).Date)
                    //&& Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    //&& ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID != (int)Constants.Categories.HRSSC)
                {
                    string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                    int cost = 0;
                    bool costum = int.TryParse(costumer, out cost);
                    if (cost == (int)Constants.Customers.FIAT)
                    {
                        string message = " INCORRECT DATE SAVING - DATE CHANGED: "
                            + dateChanging.Date.ToString(Constants.dateFormat) + " MIN CHANGED DATE: " + ((DateTime)Session[Constants.sessionMinChangingDate]).Date.ToString(Constants.dateFormat);

                        if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLogInUser] is ApplUserTO)
                            message += " USER: " + ((ApplUserTO)Session[Constants.sessionLogInUser]).UserID;

                        if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                            message += " CATEGORY: " + ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).Name;

                        int emplChangingID = -1;
                        if (Request.QueryString["emplID"] != null && !int.TryParse(Request.QueryString["emplID"].Trim(), out emplChangingID))
                            emplChangingID = -1;
                        message += " EMPLOYEE: " + emplChangingID.ToString().Trim();

                        Common.Misc.writeIncorrectChangingDate(DateTime.Now.ToString(Constants.dateFormat + " " + Constants.timeFormat + message));

                        lblError.Text = rm.GetString("changingPreviousMonthCuttOffDatePassed", culture);
                        return;
                    }
                }
                                
                int emplID = -1;
                int company = -1;              
                DateTime date = CommonWeb.Misc.createDate(tbDate.Text.Trim());
                if (Request.QueryString["emplID"] != null && !int.TryParse(Request.QueryString["emplID"].Trim(), out emplID))
                    emplID = -1;
                if (Request.QueryString["company"] != null && !int.TryParse(Request.QueryString["company"].Trim(), out company))
                    company = -1;
                if (chbDateCompany.Checked)                
                    company = getDateCompany(emplID, date, company);
                bool forVerification = false;
                uint pairRecID = 0;                
                int verifyType = -1;
                if (Request.QueryString["verify"] != null)
                    forVerification = Request.QueryString["verify"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper());
                if (Request.QueryString["verifyType"] != null)
                    if (!int.TryParse(Request.QueryString["verifyType"].Trim(), out verifyType))
                        verifyType = -1;
                if (Request.QueryString["pairRecID"] != null)
                    if (!uint.TryParse(Request.QueryString["pairRecID"].Trim(), out pairRecID))
                        pairRecID = 0;

                // validate pairs before saving
                string error = "";
                if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                {
                    List<int> sickLeaveConfirmationTypes = new List<int>();

                    // get employee
                    EmployeeTO empl = new Employee(Session[Constants.sessionConnection]).Find(emplID.ToString().Trim());

                    if (empl.EmployeeID != -1)
                    {
                        // get sick leave confirmation types
                        Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                        rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                        rule.RuleTO.WorkingUnitID = company;
                        rule.RuleTO.RuleType = Constants.RuleCompanySickLeaveNCF;
                        List<RuleTO> emplRules = rule.Search();

                        if (emplRules.Count > 0)
                        {
                            PassTypesConfirmation pt = new PassTypesConfirmation(Session[Constants.sessionConnection]);
                            pt.PTConfirmTO.PassTypeID = emplRules[0].RuleValue;

                            List<PassTypesConfirmationTO> ptConfirmedTypes = pt.Search();

                            foreach (PassTypesConfirmationTO ptCF in ptConfirmedTypes)
                            {
                                sickLeaveConfirmationTypes.Add(ptCF.ConfirmationPassTypeID);
                            }
                        }
                    }

                    error = validateDayPairs((List<IOPairProcessedTO>)Session[Constants.sessionDayPairs], sickLeaveConfirmationTypes);

                    if (!error.Trim().Equals(""))
                    {
                        lblError.Text = rm.GetString("dayPairsNotValid", culture) + " " + rm.GetString(error, culture);

                        if (error.Equals("noConfirmationDate"))
                            lblError.Text += " " + DateTime.Now.Date.ToString(Constants.dateFormat);

                        writeLog(DateTime.Now, false);
                        return;
                    }
                }

                // get all pass types
                Dictionary<int, PassTypeTO> passTypesAll = getPassTypeDictionary(company);

                EmployeeCounterValue counter = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                IOPairProcessed pair = new IOPairProcessed(Session[Constants.sessionConnection]);
                IOPairsProcessedHist pairHist = new IOPairsProcessedHist(Session[Constants.sessionConnection]);
                EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist(Session[Constants.sessionConnection]);
                bool saved = true;

                // get old counters
                EmployeeCounterValue value = new EmployeeCounterValue(Session[Constants.sessionConnection]);
                value.ValueTO.EmplID = emplID;
                List<EmployeeCounterValueTO> values = value.Search();                
                if (counter.BeginTransaction())
                {
                    try
                    {
                        //string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                        //DebugLog log = new DebugLog(logFilePath);

                        // update counters from session, updated counters insert to hist table
                        if (Session[Constants.sessionEmplCounters] != null && Session[Constants.sessionEmplCounters] is Dictionary<int, EmployeeCounterValueTO>)
                        {
                            counterHist.SetTransaction(counter.GetTransaction());

                            // update counters and move old counter values to hist table if updated
                            //log.writeLog(DateTime.Now + " IOPairBarChange: UPDATING COUNTERS STARTED \n");
                            foreach (EmployeeCounterValueTO val in values)
                            {
                                if (((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters]).ContainsKey(val.EmplCounterTypeID) 
                                    && ((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters])[val.EmplCounterTypeID].Value != val.Value)
                                {
                                    // move to hist table
                                    counterHist.ValueTO = new EmployeeCounterValueHistTO(val);
                                    counterHist.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                    saved = saved && (counterHist.Save(false) >= 0);

                                    if (!saved)
                                        break;

                                    counter.ValueTO = new EmployeeCounterValueTO();
                                    counter.ValueTO.EmplCounterTypeID = val.EmplCounterTypeID;
                                    counter.ValueTO.EmplID = val.EmplID;
                                    counter.ValueTO.Value = ((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters])[val.EmplCounterTypeID].Value;
                                    counter.ValueTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                                    saved = saved && counter.Update(false);

                                    if (!saved)
                                        break;
                                }
                            }
                            //log.writeLog(DateTime.Now + " IOPairBarChange: UPDATING COUNTERS FINISHED \n");
                        }

                        if (saved)
                        {
                            // save pairs from session, move all day pairs to hist table with alert value of 1, insert new pairs that last more then 0min
                            if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                            {
                                DateTime modifiedTime = DateTime.Now;

                                // move old pairs to hist table with alert value of 1
                                //log.writeLog(DateTime.Now + " IOPairBarChange: MOVING TO HIST STARTED \n");
                                pairHist.SetTransaction(counter.GetTransaction());
                                saved = saved && (pairHist.Save(emplID, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), modifiedTime, date, Constants.alertStatus, false) >= 0);
                                //log.writeLog(DateTime.Now + " IOPairBarChange: MOVING TO HIST FINISHED \n");

                                pair.SetTransaction(counter.GetTransaction());
                                //log.writeLog(DateTime.Now + " IOPairBarChange: DELETING PAIRS STARTED \n");
                                if (saved)
                                    saved = saved && pair.Delete(emplID, date, false);
                                //log.writeLog(DateTime.Now + " IOPairBarChange: DELETING PAIRS FINISHED \n");

                                if (saved)
                                {
                                    //log.writeLog(DateTime.Now + " IOPairBarChange: SAVING PAIRS STARTED \n");
                                    for (int i = 0; i < ((List<IOPairProcessedTO>)Session[Constants.sessionDayPairs]).Count; i++)
                                    {
                                        IOPairProcessedTO pairTO = ((List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])[i];

                                        if ((int)pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes > 0)
                                        {
                                            // empty pair, marked for delete will not be saved
                                            if (pairTO.PassTypeID == Constants.ptEmptyInterval)
                                            {
                                                // if night overtime is deleted, mark initial io_pair as processed so new night overtime will not be created again
                                                if (pairTO.OldPassTypeID != -1 && pairTO.IOPairID > 0)
                                                {
                                                    IOPair ioPair = new IOPair(Session[Constants.sessionConnection]);
                                                    ioPair.SetTransaction(counter.GetTransaction());
                                                    ioPair.PairTO = ioPair.Find(pairTO.IOPairID);
                                                    ioPair.PairTO.ProcessedGenUsed = Constants.yesInt;
                                                    saved = saved && ioPair.Update(false);

                                                    if (!saved)
                                                        break;
                                                }

                                                continue;
                                            }

                                            // move session pairs from previous and next day to hist table and save pairs from session if it is pair from db, not manually inserted pair
                                            if (!pairTO.IOPairDate.Date.Equals(date.Date) && pairTO.RecID != 0)
                                            {
                                                saved = saved && (pairHist.Save(emplID, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), modifiedTime, pairTO.IOPairDate.Date, Constants.alertStatus, false) >= 0);
                                                
                                                if (!saved)
                                                    break;
                                                                                                
                                                saved = saved && pair.Delete(pairTO.RecID.ToString().Trim(), false);

                                                if (!saved)
                                                    break;
                                            }

                                            if (forVerification)
                                            {
                                                if (i < ((List<IOPairProcessedTO>)Session[Constants.sessionDayPairs]).Count - 1 && pairTO.PassTypeID != Constants.absence
                                                && passTypesAll.ContainsKey(pairTO.PassTypeID) && passTypesAll[pairTO.PassTypeID].IsPass == Constants.wholeDayAbsence
                                                & pairTO.EndTime.Hour == 23 && pairTO.EndTime.Minute == 59)
                                                {
                                                    IOPairProcessedTO nextPair = ((List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])[i + 1];

                                                    if (nextPair.RecID == pairRecID && nextPair.PassTypeID != Constants.absence)
                                                    {
                                                        pairTO.VerificationFlag = (int)Constants.Verification.Verified;
                                                        pairTO.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                                        pairTO.VerifiedTime = modifiedTime;
                                                        pairTO.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                                    }
                                                }

                                                if (i > 0 && pairTO.PassTypeID != Constants.absence
                                                    && passTypesAll.ContainsKey(pairTO.PassTypeID) && passTypesAll[pairTO.PassTypeID].IsPass == Constants.wholeDayAbsence
                                                    & pairTO.StartTime.Hour == 0 && pairTO.StartTime.Minute == 0)
                                                {
                                                    IOPairProcessedTO prevPair = ((List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])[i - 1];

                                                    if (prevPair.RecID == pairRecID && prevPair.PassTypeID != Constants.absence)
                                                    {
                                                        pairTO.VerificationFlag = (int)Constants.Verification.Verified;
                                                        pairTO.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                                        pairTO.VerifiedTime = modifiedTime;
                                                        pairTO.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                                    }
                                                }

                                                if (pairTO.RecID == pairRecID && pairTO.PassTypeID != Constants.absence)
                                                {
                                                    pairTO.VerificationFlag = (int)Constants.Verification.Verified;
                                                    pairTO.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                                    pairTO.VerifiedTime = modifiedTime;
                                                    pairTO.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                                }
                                            }

                                            pair.IOPairProcessedTO = pairTO;                                            
                                            pair.IOPairProcessedTO.CreatedTime = modifiedTime;
                                            saved = saved && (pair.Save(false) >= 0);

                                            if (!saved)
                                                break;
                                        }
                                    }
                                    //log.writeLog(DateTime.Now + " IOPairBarChange: SAVING PAIRS FINISHED \n");
                                }
                            }

                            if (saved)
                            {
                                counter.CommitTransaction();

                                ClearSessionValues();
                                
                                if (Request.QueryString["postBackID"] != null)
                                {
                                    string jsString = "var openerWindow = window.dialogArguments; openerWindow.pageRefresh('" + Request.QueryString["postBackID"] + "', '" + Constants.pairsSavedArg + "'); window.close();";

                                    // initiate parent page post back and close pop up window
                                    ClientScript.RegisterStartupScript(GetType(), "close", "window.opener.location=window.opener.location; window.close();", true);
                                }
                            }
                            else
                            {
                                counter.RollbackTransaction();
                                lblError.Text = rm.GetString("pairsCountersSavingFailed", culture);
                            }
                        }
                    }
                    catch
                    {
                        if (counter.GetTransaction() != null)
                            counter.RollbackTransaction();

                        lblError.Text = rm.GetString("pairsCountersSavingFailed", culture);
                    }
                }
                else
                {
                    lblError.Text = rm.GetString("pairsCountersSavingFailed", culture);
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairBarChange.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/IOPairBarChange.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairBarChange.btnCancel_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/IOPairBarChange.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private string validateDayPairs(List<IOPairProcessedTO> dayPairs, List<int> sickLeaveConfirmedIDs)
        {
            try
            {
                string valid = "";

                for (int i = 0; i < dayPairs.Count; i++)
                {
                    if (dayPairs[i].StartTime.Equals(new DateTime()) || dayPairs[i].EndTime.Equals(new DateTime()))
                    {
                        valid = "notValidBeginingEnd";
                        break;
                    }

                    if (dayPairs[i].StartTime > dayPairs[i].EndTime)
                    {
                        valid = "pairEndLessThenStart";
                        break;
                    }

                    if (i > 0 && dayPairs[i - 1].EndTime > dayPairs[i].StartTime && dayPairs[i - 1].IOPairDate.Date.Equals(dayPairs[i].IOPairDate.Date))
                    {
                        valid = "overlapPairs";
                        break;
                    }

                    if (i < dayPairs.Count - 1 && dayPairs[i + 1].StartTime < dayPairs[i].EndTime && dayPairs[i + 1].IOPairDate.Date.Equals(dayPairs[i].IOPairDate.Date))
                    {
                        valid = "overlapPairs";
                        break;
                    }
                                        
                    if (sickLeaveConfirmedIDs.Contains(dayPairs[i].PassTypeID))
                    {
                        DateTime dateDesc = CommonWeb.Misc.createDate(dayPairs[i].Desc.Trim());

                        if (dateDesc.Equals(new DateTime()))
                        {
                            valid = "noConfirmationDate";
                            break;
                        }

                        if (dateDesc.Date < new DateTime(1900, 1, 1))
                        {
                            valid = "confirmationDateLessThanMinAllowed";
                            break;
                        }
                    }
                }

                return valid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IOPairProcessedTO unjustifiedPair(DateTime date, DateTime start, DateTime end, int emplID)
        {
            try
            {
                IOPairProcessedTO unjustifiedPair = new IOPairProcessedTO();
                unjustifiedPair.EmployeeID = emplID;
                unjustifiedPair.IOPairDate = date.Date;
                unjustifiedPair.StartTime = start;
                unjustifiedPair.EndTime = end;
                unjustifiedPair.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                unjustifiedPair.LocationID = Constants.locationOut;
                unjustifiedPair.Alert = Constants.alertStatusNoAlert.ToString();
                unjustifiedPair.ManualCreated = (int)Constants.recordCreated.Manualy;
                unjustifiedPair.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                unjustifiedPair.PassTypeID = Constants.absence;

                // get delay pass type
                PassTypeTO unjustifiedPT = new PassType(Session[Constants.sessionConnection]).Find(Constants.absence);
                if (unjustifiedPT.PassTypeID != -1)
                {
                    unjustifiedPair.ConfirmationFlag = unjustifiedPT.ConfirmFlag;
                    // TL and HRSSC automatically verify pairs for other employees - unjustified is verified by default, other not verified
                    if (CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], emplID))
                    {
                        if (unjustifiedPT.VerificationFlag == (int)Constants.Verification.NotVerified)
                        {
                            unjustifiedPair.VerificationFlag = (int)Constants.Verification.Verified;
                            unjustifiedPair.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                            unjustifiedPair.VerifiedTime = DateTime.Now;
                        }
                        else
                            unjustifiedPair.VerificationFlag = (int)Constants.Verification.Verified;
                    }
                    else
                        unjustifiedPair.VerificationFlag = CommonWeb.Misc.verificationFlag(unjustifiedPT, false, false);
                }
                else
                {
                    unjustifiedPair.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                    unjustifiedPair.VerificationFlag = (int)Constants.Verification.Verified;
                }

                unjustifiedPair.IOPairID = Constants.unjustifiedIOPairID;

                return unjustifiedPair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IOPairProcessedTO overtimePair(DateTime date, int emplID)
        {
            try
            {
                IOPairProcessedTO overtimePair = new IOPairProcessedTO();
                overtimePair.EmployeeID = emplID;
                overtimePair.IOPairDate = date.Date;
                overtimePair.StartTime = new DateTime();
                overtimePair.EndTime = new DateTime();
                overtimePair.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                overtimePair.LocationID = Constants.locationOut;
                overtimePair.Alert = Constants.alertStatusNoAlert.ToString();
                overtimePair.ManualCreated = (int)Constants.recordCreated.Manualy;
                overtimePair.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                overtimePair.PassTypeID = Constants.overtimeUnjustified;

                PassTypeTO overtimePT = new PassType(Session[Constants.sessionConnection]).Find(Constants.overtimeUnjustified);
                if (overtimePT.PassTypeID != -1)
                {
                    overtimePair.ConfirmationFlag = overtimePT.ConfirmFlag;
                    // TL and HRSSC automatically verify pairs for other employees - unjustified is verified by default, other not verified
                    if (CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], emplID))
                    {
                        if (overtimePT.VerificationFlag == (int)Constants.Verification.NotVerified)
                        {
                            overtimePair.VerificationFlag = (int)Constants.Verification.Verified;
                            overtimePair.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                            overtimePair.VerifiedTime = DateTime.Now;
                        }
                        else
                            overtimePair.VerificationFlag = (int)Constants.Verification.Verified;
                    }
                    else
                        overtimePair.VerificationFlag = CommonWeb.Misc.verificationFlag(overtimePT, false, false); ;
                }
                else
                {
                    overtimePair.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                    overtimePair.VerificationFlag = (int)Constants.Verification.Verified;
                }

                overtimePair.IOPairID = Constants.unjustifiedIOPairID;

                return overtimePair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int getDateCompany(int emplID, DateTime date, int company)
        {
            try
            {                
                EmployeeTO emplTO = new Employee(Session[Constants.sessionConnection]).Find(emplID.ToString().Trim());

                if (emplTO.EmployeeID != -1)
                {
                    EmployeeHistTO emplHistTO = new EmployeeHist(Session[Constants.sessionConnection]).SearchDateEmployee(date.Date, emplTO);

                    if (emplHistTO.EmployeeID != -1)
                    {
                        Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                        return Common.Misc.getRootWorkingUnit(emplHistTO.WorkingUnitID, WUnits);
                    }
                    else
                        return company;
                }
                else
                    return company;
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
