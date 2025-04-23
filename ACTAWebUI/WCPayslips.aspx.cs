using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TransferObjects;
using Util;
using System.IO;
using Common;
using System.Globalization;
using System.Resources;
using System.Net;
using System.Configuration;

namespace ACTAWebUI
{
    public partial class WCPayslips : System.Web.UI.Page
    {
        const string pageName = "WCPayslips";

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
        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);

                lblPeriod.Text = rm.GetString("lblMonth", culture);

                btnShow.Text = rm.GetString("btnShow", culture);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                EmployeeTO Empl = getEmployee();
                InitializeData(Empl);
                setLanguage();
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
                Session[Constants.sessionEmplCounters] = null;
                Session[Constants.sessionDayPairs] = null;
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);

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
        protected void InitializeData(EmployeeTO Empl)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCReportsPage).Assembly);
                foreach (KeyValuePair<int, string> pair in getMonths())
                {
                    lbMonths.Items.Add(pair.Value);
                }

                DateTime current = DateTime.Now;
                lbMonths.SelectedIndex = current.Month - 1;
                tbYear.Text = current.Year + ".";


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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "WCPayslips.", filterState);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnShow_Click(Object sender, EventArgs e)
        {
            EmployeeTO Employee = new EmployeeTO();
            try
            {
                ClearSessionValues();

                Session[Constants.sessionCounters] = null;

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCPayslips).Assembly);
                lblError.Text = "";

                if (tbYear.Text == "")
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);
                }
                else
                {
                    SaveState();

                    Employee = getEmployee();
                    Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();

                    int company = Common.Misc.getRootWorkingUnit(Employee.WorkingUnitID, WUnits);

                    //only for wc, for now this part is not in use, if not wc try to find his payslip and show if found

                    //string employeeType = "";
                    //EmployeeType emplType = new EmployeeType(Session[Constants.sessionConnection]);
                    //EmployeeTypeTO emplTypeTO = new EmployeeTypeTO();
                    //emplTypeTO.WorkingUnitID = company;
                    //emplTypeTO.EmployeeTypeID = Employee.EmployeeTypeID;
                    //emplType.EmployeeTypeTO = emplTypeTO;
                    //List<EmployeeTypeTO> listEmplTypes = new List<EmployeeTypeTO>();
                    //listEmplTypes = emplType.Search();

                    //if (listEmplTypes.Count == 1)
                    //    employeeType = listEmplTypes[0].EmployeeTypeName;


                    //if (employeeType == Enum.GetName(typeof(Constants.EmployeeTypesFIAT), 2))
                    //{
                    DateTime current = CommonWeb.Misc.createDate("01." + (lbMonths.SelectedIndex + 1) + "." + tbYear.Text);
                    if (current == new DateTime())
                    {
                        lblError.Text = rm.GetString("invalidDate", culture);
                        return;
                    }
                    string name = Employee.EmployeeID + "_" + current.ToString("yyyy_MM") + ".pdf";
                    //name = Constants.logFilePath + "AA.pdf";
                    Dictionary<int, string> dictionaryComaniesNames = new Dictionary<int, string>();
                    foreach (int comp in Enum.GetValues(typeof(Constants.FiatCompanies)))
                    {

                        string companyName = Enum.GetName(typeof(Constants.FiatCompanies), comp);
                        dictionaryComaniesNames.Add(comp, companyName);

                    }
                    if (!dictionaryComaniesNames.ContainsKey(company))
                        return;

                    name = Constants.FiatPayslipsPath + dictionaryComaniesNames[company] + "\\" + current.Year + "\\" + current.ToString("MM") + "\\" + name;

                    //if (current.Month == DateTime.Now.Month && current.Year == DateTime.Now.Year)
                    if (current.Month == DateTime.Now.AddMonths(-1).Month)
                    {
                        int paymentDay = -1;
                        int offsetPayslip = -1;
                        Common.Rule rulePaymentDay = new Common.Rule(Session[Constants.sessionConnection]);
                        rulePaymentDay.RuleTO.WorkingUnitID = company;
                        rulePaymentDay.RuleTO.EmployeeTypeID = Employee.EmployeeTypeID;
                        rulePaymentDay.RuleTO.RuleType = Constants.RuleCompanyPaymentDay;

                        List<RuleTO> rulesPaymentDay = rulePaymentDay.Search();

                        if (rulesPaymentDay.Count == 1)
                        {
                            paymentDay = rulesPaymentDay[0].RuleValue;
                        }
                        Common.Rule ruleOffset = new Common.Rule(Session[Constants.sessionConnection]);
                        ruleOffset.RuleTO.WorkingUnitID = company;
                        ruleOffset.RuleTO.EmployeeTypeID = Employee.EmployeeTypeID;
                        ruleOffset.RuleTO.RuleType = Constants.RulePayslipOffsetVisibility;

                        List<RuleTO> rulesPayslipOffset = ruleOffset.Search();

                        if (rulesPayslipOffset.Count == 1)
                        {
                            offsetPayslip = rulesPayslipOffset[0].RuleValue;
                        }
                       
                        if (paymentDay != -1 && offsetPayslip != -1)
                        {
                            //if payment day from rules is on Saturday or Sunday payment day will be Friday
                            DateTime paymentDayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, paymentDay);
                            if (paymentDayDate.DayOfWeek == DayOfWeek.Saturday)
                                paymentDayDate = paymentDayDate.AddDays(-1);
                            else if (paymentDayDate.DayOfWeek == DayOfWeek.Sunday)
                                paymentDayDate = paymentDayDate.AddDays(-2);

                            //offset from rules is how many days before payment day someone can see payslips
                            //take only working day
                            DateTime offsetDate = paymentDayDate.AddDays(-offsetPayslip);

                            if (offsetDate.DayOfWeek == DayOfWeek.Saturday)
                                offsetDate = offsetDate.AddDays(-1);
                            else if (offsetDate.DayOfWeek == DayOfWeek.Sunday)
                                offsetDate = offsetDate.AddDays(-2);
                          
                            if (DateTime.Now.Date >= offsetDate)
                            {
                                if (test.Value == "")
                                {
                                    if (File.Exists(name))
                                    {
                                        ReadPdfFile(name);
                                    }
                                    else
                                    {
                                        lblError.Text = rm.GetString("payslipNotFound", culture);
                                    }

                                }
                                else
                                {
                                    if (File.Exists(name))
                                    {
                                        ReadPdfFileAttach(name);
                                    }
                                    else
                                    {
                                        lblError.Text = rm.GetString("payslipNotFound", culture);
                                    }
                                }
                            }
                            else
                                lblError.Text = rm.GetString("lblPayslipAvailableEarliest", culture) + offsetDate.ToString("dd.MM.yyyy");
                        }
                        else
                            lblError.Text = rm.GetString("payslipNotFound", culture);
                    }
                    else if (current > DateTime.Now.Date)
                    {
                        lblError.Text = rm.GetString("lblFutureMonth", culture);
                    }
                    else
                    {
                        //if there is pdf reader (Adobe for now)
                        if (test.Value == "")
                        {
                            if (File.Exists(name))
                            {
                                ReadPdfFile(name);
                            }
                            else
                            {
                                lblError.Text = rm.GetString("payslipNotFound", culture);
                            }

                        }
                        else
                        {
                            if (File.Exists(name))
                            {
                                ReadPdfFileAttach(name);
                            }
                            else
                            {
                                lblError.Text = rm.GetString("payslipNotFound", culture);
                            }
                        }
                    }
                    //}
                    //else { return; }

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCReportsPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?emplID=" + Employee.EmployeeID, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void ReadPdfFile(string path)
        {

            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(path);

            if (buffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.TransmitFile(path);
                Response.End();

            }

        }
        private void ReadPdfFileAttach(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(path);

            if (buffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment;Filename=" + name + ".pdf");
                Response.TransmitFile(path);
                Response.End();

            }

        }

    }
}
