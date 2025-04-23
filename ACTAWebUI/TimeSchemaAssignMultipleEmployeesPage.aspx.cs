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
using System.Collections.Generic;
using TransferObjects;
using System.Text;
using Util;
using System.Resources;
using System.Globalization;
using Common;

namespace ACTAWebUI
{
    public partial class TimeSchemaAssignMultipleEmployeesPage : System.Web.UI.Page
    {
        const string vsMultipleEmplSchemaAssignSelFrom = "vsMultipleEmplSchemaAssignSelFrom";
        const string vsMultipleEmplSchemaAssignSelTo = "vsMultipleEmplSchemaAssignSelTo";
        const string sessionCycleDayList = "TimeSchemaAssignMultipleEmployeePage.CycleDayList";
        const string sessionChangedDaysList = "TimeSchemaAssignMultipleEmployeePage.ChangedDaysList";

        private DateTime SelectedFrom
        {
            get
            {
                DateTime selFrom = new DateTime();
                if (ViewState[vsMultipleEmplSchemaAssignSelFrom] != null && ViewState[vsMultipleEmplSchemaAssignSelFrom] is DateTime)
                {
                    selFrom = (DateTime)ViewState[vsMultipleEmplSchemaAssignSelFrom];
                }

                return selFrom;
            }
            set
            {
                ViewState[vsMultipleEmplSchemaAssignSelFrom] = value;
            }
        }

        private DateTime SelectedTo
        {
            get
            {
                DateTime selTo = new DateTime();
                if (ViewState[vsMultipleEmplSchemaAssignSelTo] != null && ViewState[vsMultipleEmplSchemaAssignSelTo] is DateTime)
                {
                    selTo = (DateTime)ViewState[vsMultipleEmplSchemaAssignSelTo];
                }

                return selTo;
            }
            set
            {
                ViewState[vsMultipleEmplSchemaAssignSelTo] = value;
            }
        }

        const string pageName = "TimeSchemaAssignMultipleEmployeesPage";

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
            tbFrom.Text = fromDate.ToString("dd.MM.yyyy.");
            calendarFrom.Visible = false;
            btnFrom.Visible = true;
        }
        protected void DataToChange(object sender, EventArgs e)
        {
            toDate = calendarTo.SelectedDate;
            tbTo.Text = toDate.ToString("dd.MM.yyyy.");
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
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFrom', 'false');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbTo', 'false');");

                    btnFromDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    ClearSessionValues();

                    setLanguage();
                    
                    populateMonthsAndYear();

                    populateTimeSchemas();
                    
                    InitializeSQLParameters();
                    SetTimeShema(CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text));
                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/SmallResultPage.aspx?showSelection=false";
                                     
                    populateEmployees();

                    if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager
                        || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity)))
                    {
                        btnAssign.Visible = btnSave.Visible = selectionTable.Visible = true;

                        if (!checkCutOffDate(CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text)))
                        {
                            btnAssign.Enabled = false;
                            btnSave.Enabled = false;
                            writeError("cutOffDayPessed");
                        }
                    }
                    else
                    {
                        btnAssign.Visible = btnSave.Visible = selectionTable.Visible = false;
                    }

                    chbCheckLaborLaw.Visible = Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC;
                }

                errorLabel.Text = "";
                setCalendar(CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text), SelectedFrom, SelectedTo);

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignMultipleEmployeePage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignMultipleEmployeesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private bool checkCutOffDate(DateTime dateTime)
        {
            try
            {
                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                {
                    if ((((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID != (int)Constants.Categories.WCManager
                            && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID != (int)Constants.Categories.HRLegalEntity))
                    {                        
                        if (dateTime.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1))
                            return false;
                        else if (dateTime.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                        {
                            // get dictionary of all rules, key is company and value are rules by employee type id
                            Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();
                            Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                            int cutOffDate = -1;

                            if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                            {
                                EmployeeTO empl = (EmployeeTO)Session[Constants.sessionLogInEmployee];
                                int emplCompany = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wUnits);

                                if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(empl.EmployeeTypeID))
                                {
                                    if (Session[Constants.sessionLogInUser] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                                    {
                                        string ruleType = "";
                                        if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                                            ruleType = Constants.RuleHRSSCCutOffDate;
                                        else
                                            ruleType = Constants.RuleCutOffDate;

                                        if (emplRules[emplCompany][empl.EmployeeTypeID].ContainsKey(ruleType))
                                            cutOffDate = emplRules[emplCompany][empl.EmployeeTypeID][ruleType].RuleValue;
                                    }
                                }
                            }

                            if (cutOffDate >= 0 && Common.Misc.countWorkingDays(DateTime.Now.Date, Session[Constants.sessionConnection]) <= cutOffDate)
                                return true;
                            else
                                return false;
                        }
                        else
                            return true;
                    }
                    else
                        return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        private List<string> getSelectionValues(HtmlInputHidden selBox)
        {
            try
            {
                List<string> selKeys = new List<string>();

                string[] selectedKeys = selBox.Value.Trim().Split(Constants.delimiter);

                foreach (string key in selectedKeys)
                {
                    if (!key.Trim().Equals("") && !selKeys.Contains(key))
                        selKeys.Add(key);
                }

                return selKeys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void populateMonthsAndYear()
        {
            try
            {
                foreach (KeyValuePair<int, string> pair in getMonths())
                {
                    cbMonths.Items.Add(pair.Value);
                }
                DateTime current = DateTime.Now;
                cbMonths.SelectedIndex = current.Month - 1;
                tbYear.Text = current.Year + ".";
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignMultipleEmployeesPage).Assembly);

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

        private List<EmployeeTO> getEmployees()
        {
            try
            {
                List<EmployeeTO> emplListTO = new List<EmployeeTO>();
                string emplID = "";
                
                if (Request.QueryString["emplIDs"] != null)
                {                  
                        emplID = Request.QueryString["emplIDs"];
                }

                //emplID = getEmplString();

                if (emplID != "")
                {
                    emplListTO = new Employee(Session[Constants.sessionConnection]).Search(emplID);
                }

                return emplListTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, EmployeeAsco4TO> getEmployeesAddData()
        {
            try
            {
                Dictionary<int, EmployeeAsco4TO> emplAddList = new Dictionary<int,EmployeeAsco4TO>();
                string emplID = "";

                if (Request.QueryString["emplIDs"] != null)
                {
                    emplID = Request.QueryString["emplIDs"];
                }

                //emplID = getEmplString();

                if (emplID != "")
                {
                    emplAddList = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplID);
                }

                return emplAddList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string getEmplString()
        {
            try
            {
                string infoPath = Constants.logFilePath + "Employees.txt";
                string emplIDs = "";
                if (System.IO.File.Exists(infoPath))
                {
                    System.IO.FileStream str = System.IO.File.Open(infoPath, System.IO.FileMode.Open);
                    System.IO.StreamReader reader = new System.IO.StreamReader(str);

                    // read file lines                        
                    string line = "";

                    while (line != null)
                    {
                        if (!line.Trim().Equals(""))
                            emplIDs += line.Replace("\r", "").Replace("\n", "") + ",";

                        line = reader.ReadLine();
                    }

                    reader.Close();
                    str.Dispose();
                    str.Close();
                }

                if (!emplIDs.Trim().Equals(""))
                    emplIDs = emplIDs.Substring(0, emplIDs.Length - 1);

                return emplIDs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        private void populateTimeSchemas()
        {
            try
            {               
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignMultipleEmployeesPage).Assembly);

                //get time schemas not retired and for exect company
                List<EmployeeTO> emplList = getEmployees();
                EmployeeTO selEmpl = new EmployeeTO();
                if (emplList.Count > 0)
                    selEmpl = emplList[0];

                Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                int company = Common.Misc.getRootWorkingUnit(selEmpl.WorkingUnitID, wUnits);
                
                TimeSchema timeSchema = new TimeSchema(Session[Constants.sessionConnection]);
                timeSchema.TimeSchemaTO.WorkingUnitID = company;

                List<WorkTimeSchemaTO> tsArray = timeSchema.Search();
                List<WorkTimeSchemaTO> tsArrayNew = new List<WorkTimeSchemaTO>();
                
                foreach (WorkTimeSchemaTO wts in tsArray)
                {
                    if (wts.Status != Constants.statusRetired)
                    {
                        wts.Name = wts.TimeSchemaID.ToString() + "-" + wts.Name;
                        tsArrayNew.Add(wts);
                    }
                }

                WorkTimeSchemaTO ts = new WorkTimeSchemaTO();
                ts.Name = rm.GetString("all", culture);
                tsArrayNew.Insert(0, ts);

                cbTimeSchema.DataSource = tsArrayNew;
                cbTimeSchema.DataTextField = "Name";
                cbTimeSchema.DataValueField = "TimeSchemaID";

                cbTimeSchema.DataBind();
                cbTimeSchema.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateEmployees()
        {
            try
            {
                List<EmployeeTO> empolyeeList = getEmployees();
             
                lboxEmployees.DataSource = empolyeeList;
                lboxEmployees.DataTextField = "FirstAndLastName";
                lboxEmployees.DataValueField = "EmployeeID";
                lboxEmployees.DataBind();
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignMultipleEmployeesPage).Assembly);

                lblEmplData.Text = rm.GetString("lblEmployeesData", culture);                
                lblTimeSchema.Text = rm.GetString("lblTimeSchema", culture);
                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblSelection.Text = rm.GetString("lblSelection", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblWorkingDay.Text = rm.GetString("lblWorkingDay", culture);
                lblWeekend.Text = rm.GetString("lblWeekend", culture);
                lblNationalHoliday.Text = rm.GetString("lblNationalHoliday", culture);
                Mon.Text = rm.GetString("hdrMon", culture);
                Tue.Text = rm.GetString("hdrTue", culture);
                Wed.Text = rm.GetString("hdrWed", culture);
                Thu.Text = rm.GetString("hdrThu", culture);
                Fri.Text = rm.GetString("hdrFri", culture);
                Sat.Text = rm.GetString("hdrSat", culture);
                Sun.Text = rm.GetString("hdrSun", culture);

                btnAssign.Text = rm.GetString("btnAssign", culture);
                btnBack.Text = rm.GetString("btnBack", culture);
                btnSave.Text = rm.GetString("btnSave", culture);
                btnSelect.Text = rm.GetString("btnSelect", culture);

                chbCheckLaborLaw.Text = rm.GetString("chbCheckLaborLaw", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetTimeShema(DateTime month)
        {
            try
            {
                Dictionary<DateTime, cycleDay> dict = new Dictionary<DateTime, cycleDay>();
                if (Session[sessionCycleDayList] != null && Session[sessionCycleDayList] is Dictionary<DateTime, cycleDay>)
                {
                    dict = (Dictionary<DateTime, cycleDay>)Session[sessionCycleDayList];
                }

                DateTime startDate = new DateTime(month.Year, month.Month, 1);
                DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));
                                
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundaysDays = new List<DateTime>();
                List<DateTime> personalHolidayDays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDaysAll = new Dictionary<string, List<DateTime>>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(month, month.AddMonths(1).AddDays(-1), personalHolidayDaysAll, nationalHolidaysDays, nationalHolidaysSundays, Session[Constants.sessionConnection], nationalTransferableHolidays);
                nationalHolidaysDays.AddRange(nationalHolidaysSundays);

                DateTime date = startDate.Date;
                while (date.Date <= endDate.Date)
                {                    
                    if (!dict.ContainsKey(date.Date))
                        dict.Add(date.Date, new cycleDay());
                    if (nationalHolidaysDays.Contains(date.Date))
                        dict[date.Date].Color = Constants.calendarNationalHolidayDayColor;
                    date = date.AddDays(1);                
                }
                
                Session[sessionCycleDayList] = dict;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Draw calendar for the selected month and year.
        /// </summary>
        /// <param name="month"></param>
        private void setCalendar(DateTime month, DateTime selDayFrom, DateTime selDayTo)
        {
            try
            {
                Dictionary<DateTime, cycleDay> timeSchemaDict = new Dictionary<DateTime, cycleDay>();
                if (Session[sessionCycleDayList] != null && Session[sessionCycleDayList] is Dictionary<DateTime, cycleDay>)
                {
                    timeSchemaDict = (Dictionary<DateTime, cycleDay>)Session[sessionCycleDayList];
                }
             
                int currentMonth = month.Month;
                ctrlHolder.Controls.Clear();

                DateTime startDate = new DateTime(month.Year, month.Month, 1);
                DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));

                addHrisontalSpace();
                addSpace();

                int i = 1;
                
                int dayOfWeek = (int)startDate.DayOfWeek;
                if (dayOfWeek == 0)
                    dayOfWeek = 7;
                
                while (i < dayOfWeek)
                {
                    DayOfCalendarUC hourLine = new DayOfCalendarUC();
                    hourLine.ID = "dayHr" + i.ToString().Trim();
                    hourLine.Transprent = true;
                    ctrlHolder.Controls.Add(hourLine);
                    addSpace();
                    i++;
                }

                List<DayOfCalendarUC> controls = new List<DayOfCalendarUC>();
                int ctrlCounter = 0;

                DateTime date = startDate.Date;
                while (date.Date <= endDate.Date)
                {
                    DayOfCalendarUC dayOfCalendar = new DayOfCalendarUC();
                    dayOfCalendar.ID = "day" + ctrlCounter.ToString().Trim();
                    dayOfCalendar.Date = date;
                    dayOfCalendar.Transprent = false;
                    if (timeSchemaDict.ContainsKey(date.Date))
                        dayOfCalendar.Description = timeSchemaDict[date.Date].Description;
                    if (dayOfCalendar.Date.DayOfWeek == DayOfWeek.Sunday || dayOfCalendar.Date.DayOfWeek == DayOfWeek.Saturday)
                    {
                        dayOfCalendar.Color = Constants.calendarWeekEndDayColor;
                    }
                    else
                    {
                        dayOfCalendar.Color = Constants.calendarDayColor;
                    }
                    if (date.Date >= selDayFrom.Date && date.Date <= selDayTo.Date)
                        dayOfCalendar.Selected = true;
                    if (timeSchemaDict.ContainsKey(date.Date))
                    {
                        dayOfCalendar.SchemaID = timeSchemaDict[date.Date].Schema.TimeSchemaID;
                        dayOfCalendar.CycleDay = timeSchemaDict[date.Date].Day;
                        if (timeSchemaDict[date.Date].Color.Length > 0)
                            dayOfCalendar.Color = timeSchemaDict[date.Date].Color;
                    }

                    controls.Add(dayOfCalendar);

                    date = date.AddDays(1);
                    ctrlCounter++;
                }

                foreach (DayOfCalendarUC dayOfCalendar in controls)
                {
                    if (dayOfCalendar.Date.DayOfWeek == DayOfWeek.Monday && dayOfCalendar.Date.Day > 1)
                    {
                        addSpace();
                    }
                    ctrlHolder.Controls.Add(dayOfCalendar);
                    addSpace();
                    if (dayOfCalendar.Date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        addHrisontalSpace();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void addHrisontalSpace()
        {
            try
            {
                Label space = new Label();
                space.Width = 790;
                space.Height = 10;
                ctrlHolder.Controls.Add(space);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void addSpace()
        {
            try
            {
                Label space = new Label();
                space.Width = 10;
                space.Height = 50;
                ctrlHolder.Controls.Add(space);
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignMultipleEmployeesPage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrCycleDay", culture) + "," + rm.GetString("hdrDescription", culture) + "," + rm.GetString("hdrInterval", culture) + "," + rm.GetString("hdrStartTime", culture) + "," + rm.GetString("hdrEndTime", culture);
                Session[Constants.sessionFields] = "cycle_day,description, interval_ord_num AS interval, start_time, end_time";
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(3, (int)Constants.FormatTypes.TimeFormat);
                formating.Add(4, (int)Constants.FormatTypes.TimeFormat);
                Session[Constants.sessionFieldsFormating] = formating;
                Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionTables] = "time_schema_dtl";
                Session[Constants.sessionKey] = "cycle_day";
                Session[Constants.sessionColTypes] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void dateSelection_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);

                Session[sessionCycleDayList] = null;
                Session[sessionChangedDaysList] = null;

                SelectedTo = SelectedFrom = new DateTime();
                tbTo.Text = tbFrom.Text = "";

                if (dt == new DateTime())
                {
                    writeError("mothUnregular");
                }
                else
                {
                    SetTimeShema(dt);
                    setCalendar(dt, SelectedFrom, SelectedTo);                    
                }

                if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager
                        || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity)))
                {
                    if (!checkCutOffDate(CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text)))
                    {
                        btnAssign.Enabled = false;
                        btnSave.Enabled = false;
                        writeError("cutOffDayPessed");
                    }
                    else
                        btnAssign.Enabled = btnSave.Enabled = true;
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignMultipleEmployeesPage.dateSelection_TextChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignMultipleEmployeesPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void getSelDays(DateTime dateTime)
        {
            try
            {
                if (tbFrom.Text == "" || tbTo.Text == "")
                {
                    SelectedTo = SelectedFrom = new DateTime();
                }
                else
                {
                    DateTime startDate = new DateTime(dateTime.Year, dateTime.Month, 1);
                    DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));

                    DateTime selFrom = CommonWeb.Misc.createDate(tbFrom.Text.Trim());
                    DateTime selTo = CommonWeb.Misc.createDate(tbTo.Text.Trim());

                    if ((!tbFrom.Text.Trim().Equals("") && selFrom.Equals(new DateTime())) || (!tbTo.Text.Trim().Equals("") && selTo.Equals(new DateTime())))
                    {
                        SelectedFrom = SelectedTo = new DateTime();
                        writeError("invalidDateFormat");
                        return;
                    }
                    else
                    {
                        if (selFrom > selTo)
                        {
                            SelectedFrom = SelectedTo = new DateTime();
                            writeError("selectionToLessThanFrom");
                            return;
                        }

                        if (selFrom.Date < startDate.Date || selTo.Date > endDate.Date)
                        {
                            SelectedFrom = SelectedTo = new DateTime();
                            writeError("selectionInsideStartEnd");
                            return;
                        }

                        List<DateTime> lockedDays = getLockedDays(selFrom, selTo);

                        if (lockedDays.Count > 0)
                        {
                            DateTime currDate = selFrom.Date;
                            string locked = "";
                            while (currDate.Date <= selTo.Date)
                            {
                                if (lockedDays.Contains(currDate.Date))
                                    locked += currDate.ToString(Constants.dateFormat);

                                currDate = currDate.AddDays(1);
                            }

                            if (!locked.Trim().Equals(""))
                            {
                                SelectedFrom = SelectedTo = new DateTime();
                                writeError("selectionLockedDays", " " + locked);
                                return;
                            }
                        }

                        // get hiring and termination dates
                        Dictionary<int, EmployeeAsco4TO> emplAddData = getEmployeesAddData();
                        bool correctSelection = true;
                        foreach (int emplID in emplAddData.Keys)
                        {
                            DateTime emplHiringDate = emplAddData[emplID].DatetimeValue2;
                            DateTime emplTerminationDate = emplAddData[emplID].DatetimeValue3;

                            if (!(!emplHiringDate.Equals(new DateTime()) && emplHiringDate.Date <= selFrom.Date && (emplTerminationDate.Equals(new DateTime()) || emplTerminationDate > selTo.Date)))
                                correctSelection = false;
                        }

                        if (correctSelection)
                        {
                            SelectedFrom = selFrom;
                            SelectedTo = selTo;
                        }
                        else
                        {
                            SelectedFrom = SelectedTo = new DateTime();
                            writeError("selectionNonActiveEmployeesDays");
                            return;
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                btnAssign.Enabled = true;
                btnSave.Enabled = true;

                writeError("");

                DateTime month = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);

                getSelDays(month);
                setCalendar(month, SelectedFrom, SelectedTo);

                if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                        && (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager
                        || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity)))
                {
                    if (!checkCutOffDate(CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text)))
                    {
                        btnAssign.Enabled = false;
                        btnSave.Enabled = false;
                        writeError("cutOffDayPessed");
                    }
                    else
                        btnAssign.Enabled = btnSave.Enabled = true;
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignMultipleEmployeesPage.btnSelect_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignMultipleEmployeesPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        private void writeError(string message)
        {
            try
            {
                if (message != "")
                {
                    CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignMultipleEmployeesPage).Assembly);

                    errorLabel.Text = rm.GetString(message, culture);
                }
                else
                {
                    errorLabel.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void writeError(string message, string addMessage)
        {
            try
            {
                if (message != "")
                {
                    CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                    ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignMultipleEmployeesPage).Assembly);

                    errorLabel.Text = rm.GetString(message, culture) + addMessage;
                }
                else
                {
                    errorLabel.Text = addMessage;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "Redirect", "window.history.back();", true);
                Session[sessionCycleDayList] = null;
                Session[sessionChangedDaysList] = null;

                if (Request.QueryString["Back"] != null && !Request.QueryString["Back"].Trim().Equals(""))
                    Response.Redirect(Request.QueryString["Back"].Trim() + "?reloadState=false", false);

                Message = "";
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignMultipleEmployeesPage.btnBack_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignMultipleEmployeesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TimeSchemaAssignMultipleEmployeesPage).Assembly);
                DateTime dateTime = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                List<string> selKeys = getSelectionValues(SelBox);
    
                int day = -1;
                if (cbTimeSchema.SelectedIndex <= 0)
                {
                    errorLabel.Text = rm.GetString("selectSchema", culture);
                }
                else if (selKeys.Count <= 0 || !int.TryParse(selKeys[0], out day))
                {
                    errorLabel.Text = rm.GetString("selectOneDay", culture);
                }
                else
                {
                    if (SelectedTo.Equals(new DateTime()) || SelectedFrom.Equals(new DateTime()))
                    {
                        errorLabel.Text = rm.GetString("selectOneDayCalendar", culture);
                    }
                    else
                    {
                        Dictionary<DateTime, cycleDay> timeSchemaDict = new Dictionary<DateTime, cycleDay>();
                        if (Session[sessionCycleDayList] != null && Session[sessionCycleDayList] is Dictionary<DateTime, cycleDay>)
                        {
                            timeSchemaDict = (Dictionary<DateTime, cycleDay>)Session[sessionCycleDayList];
                        }
                                                
                        int newTimeSchema = int.Parse(cbTimeSchema.SelectedValue);
                        List<WorkTimeSchemaTO> schemas = new TimeSchema(Session[Constants.sessionConnection]).Search(newTimeSchema.ToString().Trim());
                        WorkTimeSchemaTO newSchema = new WorkTimeSchemaTO();
                        if (schemas.Count > 0)
                            newSchema = schemas[0];

                        if (day >= 0 && newTimeSchema >= 0)
                        {
                            for (DateTime i = SelectedFrom.Date; i <= SelectedTo.Date; i = i.AddDays(1))
                            {
                                if (!timeSchemaDict.ContainsKey(i.Date))
                                    timeSchemaDict.Add(i.Date, new cycleDay());

                                timeSchemaDict[i.Date].Schema = newSchema;
                                timeSchemaDict[i.Date].Day = day;
                                timeSchemaDict[i.Date].StartDay = day - 1;
                                timeSchemaDict[i.Date].CycleDuration = newSchema.CycleDuration;
                                if (newSchema.Days.ContainsKey(day - 1) && newSchema.Days[day - 1].Keys.Count > 0 && newSchema.Days[day - 1][0].Description.Length > 0)
                                {
                                    timeSchemaDict[i.Date].Description = newSchema.Days[day - 1][0].Description;
                                }

                                day = (day == newSchema.CycleDuration ? 1 : day + 1);
                            }                            
                        }

                        Session[sessionCycleDayList] = timeSchemaDict;

                        for (DateTime dateSel = SelectedFrom.Date; dateSel <= SelectedTo.Date; dateSel = dateSel.AddDays(1))
                        {
                            List<DateTime> SelectedDates = new List<DateTime>();
                            if (Session[sessionChangedDaysList] != null && Session[sessionChangedDaysList] is List<DateTime>)
                                SelectedDates = ((List<DateTime>)Session[sessionChangedDaysList]);

                            if (!SelectedDates.Contains(dateSel))
                                SelectedDates.Add(dateSel);

                            Session[sessionChangedDaysList] = SelectedDates;
                        }
                        
                        setCalendar(dateTime, SelectedFrom, SelectedTo);
                    }
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignMultipleEmployeesPage.btnAssign_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignMultipleEmployeesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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

                Dictionary<DateTime, cycleDay> dict = new Dictionary<DateTime, cycleDay>();
                if (Session[sessionCycleDayList] != null && Session[sessionCycleDayList] is Dictionary<DateTime, cycleDay>)
                {
                    dict = (Dictionary<DateTime, cycleDay>)Session[sessionCycleDayList];
                }

                DateTime startDate = CommonWeb.Misc.createDate("01." + (cbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));

                // check if there is assigned schema to change
                bool assignStatus = false;
                DateTime maxDate = new DateTime();
                foreach (DateTime day in dict.Keys)
                {
                    if (dict[day].Schema.TimeSchemaID != -1)                    
                        assignStatus = true;

                    if (day.Date > maxDate.Date)
                        maxDate = day.Date;
                }

                List<DateTime> datesList = new List<DateTime>();
                if (Session[sessionChangedDaysList] != null && Session[sessionChangedDaysList] is List<DateTime>)
                {
                    datesList = (List<DateTime>)Session[sessionChangedDaysList];
                }
                if (datesList.Count <= 0)
                {
                    assignStatus = false;
                    writeError("noChangesDetected");
                }

                if (assignStatus)
                {
                    List<EmployeeTO> emplList = getEmployees();

                    //create employee string
                    string emplString = "";
                    foreach (EmployeeTO employee in emplList)
                    {
                        emplString += employee.EmployeeID.ToString() + ", ";
                    }
                    if (emplString.Length > 0)
                        emplString = emplString.Substring(0, emplString.Length - 2);

                    Dictionary<int, EmployeeTO> emplDict = new Employee(Session[Constants.sessionConnection]).SearchDictionary(emplString);
                    Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4(Session[Constants.sessionConnection]).SearchDictionary(emplString);
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Common.Rule(Session[Constants.sessionConnection]).SearchWUEmplTypeDictionary();

                    EmployeesTimeSchedule emplTimeSchedule = new EmployeesTimeSchedule(Session[Constants.sessionConnection]);

                    // get old schedules for all employees before changing any data
                    Dictionary<int, List<EmployeeTimeScheduleTO>> oldEmplSchedules = emplTimeSchedule.SearchEmployeesSchedulesExactDate(emplString.Trim(), startDate.Date, endDate.AddDays(1).Date, null);
                    //DebugLog log = new DebugLog(Constants.logFilePath + "Schema.txt");

                    DateTime startReprocessingDate = startDate.Date;
                    List<DateTime> dateReprocessList = new List<DateTime>();

                    // if first day of month is changing and first day is leaving third shift from previous month, reprocess from second day
                    if (datesList.Contains(startDate.Date) && !checkCutOffDate(startDate.Date.AddDays(-1)) && dict.ContainsKey(startDate.Date) && dict[startDate.Date].Schema.Days.ContainsKey(dict[startDate.Date].StartDay))
                    {
                        foreach (int num in dict[startDate.Date].Schema.Days[dict[startDate.Date].StartDay].Keys)
                        {
                            if (dict[startDate.Date].Schema.Days[dict[startDate.Date].StartDay][num].StartTime.TimeOfDay == new TimeSpan(0, 0, 0)
                                && dict[startDate.Date].Schema.Days[dict[startDate.Date].StartDay][num].EndTime.TimeOfDay != new TimeSpan(0, 0, 0))
                            {
                                startReprocessingDate = startReprocessingDate.AddDays(1).Date;
                                break;
                            }
                        }
                    }

                    foreach (DateTime date in datesList)
                    {
                        if (date.Date != startDate.Date || startReprocessingDate.Date == startDate.Date)
                            dateReprocessList.Add(date);
                    }

                    bool trans = emplTimeSchedule.BeginTransaction();
                    bool isSaved = true;
                    //int counter = 0;
                    if (trans)
                    {
                        try
                        {
                            #region Reprocess Dates

                            IDbTransaction transaction = emplTimeSchedule.GetTransaction();

                            //list of datetime for each employee
                            Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();

                            foreach (EmployeeTO employeeObj in emplList)
                            {
                                emplDateWholeDayList.Add(employeeObj.EmployeeID, dateReprocessList);
                            }

                            if (dateReprocessList.Count > 0)
                                isSaved = isSaved && Common.Misc.ReprocessPairsAndRecalculateCounters(emplString, startReprocessingDate.Date, endDate.Date, transaction, emplDateWholeDayList, Session[Constants.sessionConnection], CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]));

                            #endregion
                            //log.writeLog("data reprocessed");
                            if (isSaved)
                            {
                                if (isSaved)
                                {
                                    Dictionary<int, WorkTimeSchemaTO> schemas = Common.Misc.getSchemas(emplTimeSchedule.GetTransaction(), Session[Constants.sessionConnection]);
                                    
                                    foreach (EmployeeTO employee in emplList)
                                    {
                                        //counter++;
                                        //log.writeLog(counter.ToString().Trim() + ": " + employee.EmployeeID.ToString().Trim());
                                        int prevSchemaID = -1;
                                        int expectedStartDay = -1;
                                        List<EmployeeTimeScheduleTO> emplSchedule = new List<EmployeeTimeScheduleTO>();

                                        if (oldEmplSchedules.ContainsKey(employee.EmployeeID))
                                            emplSchedule = oldEmplSchedules[employee.EmployeeID];

                                        // delete time schedule for selected days                                        
                                        foreach (DateTime date in datesList)
                                        {
                                            isSaved = isSaved && emplTimeSchedule.DeleteFromToSchedule(employee.EmployeeID, date, date, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), false);

                                            if (!isSaved)
                                                break;
                                        }

                                        if (!isSaved)
                                            break;

                                        foreach (DateTime date in dict.Keys)
                                        {
                                            if (dict[date].Schema.TimeSchemaID == -1)
                                                continue;

                                            cycleDay day = dict[date];

                                            // if schema is different, or if schema is the same but cycle day is not the expected one save new record
                                            if ((day.Schema.TimeSchemaID != prevSchemaID) ||
                                                ((day.Schema.TimeSchemaID == prevSchemaID) && (day.Day != expectedStartDay)))
                                            {
                                                isSaved = isSaved && (emplTimeSchedule.Save(employee.EmployeeID, date.Date, day.Schema.TimeSchemaID, day.StartDay, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), false) > 0 ? true : false);

                                                if (!isSaved)
                                                    break;

                                                prevSchemaID = day.Schema.TimeSchemaID;
                                            }

                                            expectedStartDay = (day.StartDay == day.CycleDuration - 1 ? 0 : day.StartDay + 1) + 1;

                                            // if next day is not changed, insert old employee schedule for that day if there is no already schedule assigned for that particular day
                                            if (!datesList.Contains(date.AddDays(1)))
                                            {
                                                prevSchemaID = -1;
                                                expectedStartDay = -1;
                                                bool scheduleExists = false;

                                                // get old schedule for next day
                                                int schIndex = -1;
                                                for (int scheduleIndex = 0; scheduleIndex < emplSchedule.Count; scheduleIndex++)
                                                {
                                                    if (date.AddDays(1).Date.Equals(emplSchedule[scheduleIndex].Date.Date))
                                                    {
                                                        scheduleExists = true;
                                                        break;
                                                    }

                                                    if (date.AddDays(1).Date > emplSchedule[scheduleIndex].Date.Date)
                                                    {
                                                        schIndex = scheduleIndex;
                                                    }
                                                }

                                                if (!scheduleExists && schIndex >= 0)
                                                {
                                                    WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                                                    EmployeeTimeScheduleTO ets = emplSchedule[schIndex];
                                                    int startDay = ets.StartCycleDay;
                                                    int schemaID = ets.TimeSchemaID;

                                                    if (employee.Status.ToUpper().Trim().Equals(Constants.statusRetired.ToUpper().Trim()) && date.Date >= maxDate.Date)
                                                        schemaID = Constants.defaultSchemaID;

                                                    if (schemas.ContainsKey(schemaID))
                                                        sch = schemas[schemaID];

                                                    if (sch.TimeSchemaID != -1)
                                                    {
                                                        int cycleDuration = sch.CycleDuration;

                                                        TimeSpan ts = new TimeSpan(date.AddDays(1).Date.Ticks - ets.Date.Date.Ticks);
                                                        int dayNum = (startDay + (int)ts.TotalDays) % cycleDuration;

                                                        int insert = emplTimeSchedule.Save(employee.EmployeeID, date.AddDays(1).Date,
                                                            schemaID, dayNum, CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]), false);

                                                        isSaved = (insert > 0 ? true : false) && isSaved;
                                                    }
                                                }
                                            }
                                        }

                                        if (!isSaved)
                                            break;
                                                                                
                                        IOPair ioPair = new IOPair(Session[Constants.sessionConnection]);
                                        if ((startDate.Year < DateTime.Now.Year) ||
                                                ((startDate.Year == DateTime.Now.Year) && (startDate.Month <= DateTime.Now.Month)))
                                        {
                                            if (endDate.Date > DateTime.Now.Date)
                                                endDate = DateTime.Now.Date;

                                            //ioPair.recalculatePause(tbEmployeeID.Text, new DateTime(month.Year, month.Month, 1), (new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1)).AddDays(-1));
                                            ioPair.recalculatePause(employee.EmployeeID.ToString(), startDate.Date, endDate.Date, emplTimeSchedule.GetTransaction(), Session[Constants.sessionConnection]);
                                        }                                        
                                    }
                                }
                            }

                            if (isSaved)
                            {
                                // validate new employee schedule
                                bool validFundHrs = true;
                                //log.writeLog("validate schedule");
                                //DateTime invalidDate = Common.Misc.isValidTimeSchedule(emplString.Trim(), startDate.Date, endDate.Date, emplTimeSchedule.GetTransaction(), 
                                //    Session[Constants.sessionConnection], CommonWeb.Misc.checkLimit(Session[Constants.sessionLoginCategory]), ref validFundHrs);
                                DateTime invalidDate = Common.Misc.isValidTimeSchedule(emplDict, ascoDict, rulesDict, emplString.Trim(), startDate.Date, endDate.Date, 
                                    emplTimeSchedule.GetTransaction(), Session[Constants.sessionConnection], false, ref validFundHrs, chbCheckLaborLaw.Checked);
                                if (invalidDate.Equals(new DateTime()))
                                {
                                    emplTimeSchedule.CommitTransaction();
                                    //writeError("emplScheduleSaved", " - " + counter.ToString().Trim() + " employees");
                                    writeError("emplScheduleSaved");
                                }
                                else
                                {
                                    emplTimeSchedule.RollbackTransaction();
                                    if (validFundHrs)
                                        writeError("notValidScheduleAssigned", " " + invalidDate.Date.AddDays(-1).ToString(Constants.dateFormat) + "/" + invalidDate.Date.ToString(Constants.dateFormat));
                                    else
                                        writeError("notValidFundHrs", " " + invalidDate.Date.ToString(Constants.dateFormat) + "-" + invalidDate.AddDays(6).Date.ToString(Constants.dateFormat));
                                }
                            }
                            else
                            {
                                emplTimeSchedule.RollbackTransaction();
                                writeError("emplScheduleNotSaved");
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                if (emplTimeSchedule.GetTransaction() != null)
                                    emplTimeSchedule.RollbackTransaction();
                                Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignMultipleEmployeesPage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignMultipleEmployeesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                            }
                            catch (System.Threading.ThreadAbortException) { }
                        }
                    }
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignMultipleEmployeesPage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignMultipleEmployeesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        protected void cbTimeSchema_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session[Constants.sessionFilter] = null;
                if (cbTimeSchema.SelectedIndex > 0)
                    Session[Constants.sessionFilter] = "time_schema_id = " + cbTimeSchema.SelectedValue;
                Session[Constants.sessionSortCol] = "cycle_day";
                Session[Constants.sessionSortDir] = Constants.sortASC;

                resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/SmallResultPage.aspx?showSelection=false";

                SelBox.Value = "";

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TimeSchemaAssignMultipleEmployeesPage.cbTimeSchema_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TimeSchemaAssignMultipleEmployeesPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
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

                Session[sessionCycleDayList] = null;
                Session[sessionChangedDaysList] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<DateTime> getLockedDays(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<DateTime> lockedDays = new List<DateTime>();

                if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO &&
                    ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC))
                {
                    string emplID = "";

                    if (Request.QueryString["emplIDs"] != null)
                    {
                        emplID = Request.QueryString["emplIDs"];

                        if (!emplID.Trim().Equals(""))
                        {
                            Dictionary<int, List<DateTime>> lockedDaysDict = new EmployeeLockedDay(Session[Constants.sessionConnection]).SearchLockedDays(emplID.ToString().Trim(), "", startDate.Date, endDate.Date);

                            foreach (int id in lockedDaysDict.Keys)
                            {
                                lockedDays.AddRange(lockedDaysDict[id]);
                            }
                        }
                    }
                }

                return lockedDays;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private class cycleDay
        {
            private WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
            private int day = -1;
            private string color = "";
            private int startDay = -1;
            private int cycleDuration = -1;
            private string description = "";

            public string Description
            {
                get { return description; }
                set { description = value; }
            }
            public int CycleDuration
            {
                get { return cycleDuration; }
                set { cycleDuration = value; }
            }

            public int StartDay
            {
                get { return startDay; }
                set { startDay = value; }
            }

            public string Color
            {
                get { return color; }
                set { color = value; }
            }

            public int Day
            {
                get { return day; }
                set { day = value; }
            }

            public WorkTimeSchemaTO Schema
            {
                get { return schema; }
                set { schema = value; }
            }
        }
    }
}
