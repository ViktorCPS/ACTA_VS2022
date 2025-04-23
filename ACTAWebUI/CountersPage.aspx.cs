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

using TransferObjects;
using Common;
using CommonWeb;
using Util;

namespace ACTAWebUI
{
    public partial class CountersPage : System.Web.UI.Page
    {
        const string pageName = "CountersPage";

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
                InitializeCountersPanel();
                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    string message = "/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in CountersPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim()
                        + "&Back=/ACTAWeb/ACTAWebUI/CountersPage.aspx&Header=" + Constants.falseValue.Trim();
                    Response.Redirect(message, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void InitializeCountersPanel()
        {
            try
            {
                Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = new Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>();

                // get counters for passed employees and employees data
                if (Session[Constants.sessionCountersEmployees] != null)
                {
                    if (Session[Constants.sessionCounters] != null && Session[Constants.sessionCounters] is Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>)
                        emplCounters = (Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>)Session[Constants.sessionCounters];
                    else
                    {
                        emplCounters = new EmployeeCounterValue(Session[Constants.sessionConnection]).SearchValuesOrderedByName(Session[Constants.sessionCountersEmployees].ToString().Trim());
                        Session[Constants.sessionCounters] = emplCounters;
                    }

                    Dictionary<int, EmployeeTO> employees = new Employee(Session[Constants.sessionConnection]).SearchDictionary(Session[Constants.sessionCountersEmployees].ToString().Trim());

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

                    bool readOnly = true;

                    if (Request.QueryString["readOnly"] != null && Request.QueryString["readOnly"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                        readOnly = false;

                    foreach (Control ctrl in ctrlHolder.Controls)
                    {
                        ctrl.Dispose();
                    }

                    ctrlHolder.Controls.Clear();

                    int ctrlCounter = 0;

                    List<int> supervisorsList = new List<int>();
                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                            && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                        supervisorsList = new List<int>();
                    else
                    {
                        Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                        rule.RuleTO.EmployeeTypeID = (int)Constants.EmployeeTypesFIAT.BC;
                        rule.RuleTO.WorkingUnitID = Constants.defaultWorkingUnitID;
                        rule.RuleTO.RuleType = Constants.RuleHideBH;
                        List<RuleTO> ruleList = rule.Search();

                        if (ruleList.Count > 0 && ruleList[0].RuleValue == Constants.yesInt)
                            supervisorsList = new ApplUserXApplUserCategory(Session[Constants.sessionConnection]).SearchSupervisors(Session[Constants.sessionCountersEmployees].ToString().Trim());
                    }

                    foreach (int emplID in emplCounters.Keys)
                    {
                        // draw employee data label
                        if (employees.ContainsKey(emplID))
                        {
                            string emplData = employees[emplID].FirstAndLastName + " (" + employees[emplID].EmployeeID.ToString().Trim() + ")";

                            Label lblSeparator1 = new Label();
                            lblSeparator1.ID = "lblSeparator1" + ctrlCounter.ToString().Trim();
                            lblSeparator1.Width = new Unit(950);
                            lblSeparator1.Height = new Unit(10);
                            ctrlHolder.Controls.Add(lblSeparator1);

                            Label lblEmplData = new Label();
                            lblEmplData.ID = "lblEmplData" + ctrlCounter.ToString().Trim();
                            lblEmplData.Width = new Unit(300);
                            lblEmplData.CssClass = "emplDataLbl";
                            lblEmplData.Text = emplData.Trim();
                            ctrlHolder.Controls.Add(lblEmplData);

                            Label lblSeparator2 = new Label();
                            lblSeparator2.ID = "lblSeparator2" + ctrlCounter.ToString().Trim();
                            lblSeparator2.Width = new Unit(950);
                            lblSeparator2.Height = new Unit(5);
                            ctrlHolder.Controls.Add(lblSeparator2);
                        }
                        EmployeeCounterValueTO emplC = new EmployeeCounterValueTO();
                        // add counter
                        CounterUC emplCounter = new CounterUC();
                        emplCounter.ID = "emplCounter" + ctrlCounter.ToString().Trim();
                        emplCounter.CounterValues = emplCounters[emplID];
                        
                        /*
                        if (emplCounters[emplID].Values[].)
                        {
                            emplCounter.ReadOnly = true;
                            
                        }
                        else
                        {
                            emplCounter.ReadOnly = readOnly;
                        }
                         */
                        /*
                        for(int i=0;i<3;i++)
                        {
                            //emplCounter.ReadOnly=
                        }
                        */
                        emplCounter.CounterNames = counterNames;
                        emplCounter.ReadOnly = readOnly;
                        emplCounter.EmplID = emplID;
                        emplCounter.HideBH = supervisorsList.Contains(emplID);
                        emplCounter.BubbleClick += new ControlEventHandler(BubbleControl_BubbleClick);
                        ctrlHolder.Controls.Add(emplCounter);

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
                InitializeCountersPanel();

                // show error page pop up
                if (!e.Error.Trim().Equals(""))
                {
                    Session[Constants.sessionInfoMessage] = e.Error.Trim();
                    string wOptions = "dialogWidth:800px; dialogHeight:300px; edge:Raised; center:Yes; resizable:Yes; status:Yes;";
                    ClientScript.RegisterStartupScript(GetType(), "infoPopup", "window.open('/ACTAWeb/CommonWeb/MessagesPages/Info.aspx?isError=true', window, '" + wOptions.Trim() + "');", true);
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in CountersPage.BubbleControl_BubbleClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/CountersPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
