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
using System.Drawing;

using Common;
using TransferObjects;
using Util;
using ReportsWeb;

namespace ACTAWebUI
{
    public partial class EmplOnDemandAppointmentsPage : System.Web.UI.Page
    {
        const string pageName = "EmplOnDemandAppointmentsPage";

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
                // parameter in request query string is emplID (employee_id)
                if (!IsPostBack)
                {
                    btnSave.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnAdd.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnRemove.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                                        
                    EmployeeTO Empl = getEmployee();

                    if (Empl.EmployeeID != -1)
                    {
                        lblEmplData.Text = Empl.EmployeeID.ToString().Trim() + " - " + Empl.FirstAndLastName.Trim();
                        btnSave.Enabled = true;
                    }
                    else                    
                        btnSave.Enabled = false;
                        
                    setLanguage();

                    populatePositions(Empl);                    
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplOnDemandAppointmentsPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplOnDemandAppointmentsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        #region Inner Class for sorting List of Risks

        /*
		 *  Class used for sorting List of Risks
		*/

        private class ListSort : IComparer<RiskTO>
        {
            public ListSort()
            { }

            public int Compare(RiskTO x, RiskTO y)
            {
                RiskTO r1 = x;
                RiskTO r2 = y;

                return r1.RiskCode.ToUpper().CompareTo(r2.RiskCode.ToUpper());            
            }
        }

        #endregion

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplOnDemandAppointmentsPage).Assembly);

                lblPosition.Text = rm.GetString("lblPosition", culture);
                lblRisks.Text = rm.GetString("lblRisks", culture);
                lblSelectedRisks.Text = rm.GetString("lblSelectedRisks", culture);

                btnSave.Text = rm.GetString("btnSave", culture);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populatePositions(EmployeeTO empl)
        {
            try
            {
                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                List<EmployeePositionTO> posList = new List<EmployeePositionTO>();
                int company = -1;
                int position = -1;
                if (empl.EmployeeID != -1)
                {
                    EmployeeAsco4 asco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                    asco.EmplAsco4TO.EmployeeID = empl.EmployeeID;
                    List<EmployeeAsco4TO> ascoList = asco.Search();

                    if (ascoList.Count > 0)
                    {
                        company = ascoList[0].IntegerValue4;
                        position = ascoList[0].IntegerValue6;
                    }

                    EmployeePosition pos = new EmployeePosition(Session[Constants.sessionConnection]);
                    pos.EmplPositionTO.WorkingUnitID = company;
                    pos.EmplPositionTO.Status = Constants.statusActive;
                    posList = pos.SearchEmployeePositions();

                    EmployeePositionTO allPos = new EmployeePositionTO();
                    allPos.PositionCode = "*";

                    posList.Insert(0, allPos);
                }

                cbPosition.DataSource = posList;
                if (!isAltLang)
                    cbPosition.DataTextField = "PositionCodeTitleSR";
                else
                    cbPosition.DataTextField = "PositionCodeTitleEN";
                cbPosition.DataValueField = "PositionID";
                cbPosition.DataBind();

                cbPosition.SelectedValue = position.ToString().Trim();
                populateRisks();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cbPosition_SelectedIndexChanged(Object sender, EventArgs e)
        {
            try
            {
                populateRisks();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplOnDemandAppointmentsPage.cbPosition_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplOnDemandAppointmentsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateRisks()
        {
            try
            {
                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                string posIDs = "";

                if (cbPosition.SelectedIndex > 0)
                    posIDs = cbPosition.SelectedValue.Trim();
                else
                {
                    foreach (ListItem item in cbPosition.Items)
                    {
                        if (item.Value != "-1")
                            posIDs += item.Value + ",";
                    }

                    if (posIDs.Length > 0)
                        posIDs = posIDs.Substring(0, posIDs.Length - 1);
                }

                List<RiskTO> riskList = new EmployeePositionXRisk(Session[Constants.sessionConnection]).SearchRisks(posIDs);

                // remove risks that are already selected
                List<string> selRisks = new List<string>();
                foreach (ListItem item in lbSelectedRisks.Items)
                {
                    selRisks.Add(item.Value);
                }

                List<RiskTO> risks = new List<RiskTO>();

                foreach (RiskTO risk in riskList)
                {
                    if (selRisks.Contains(risk.RiskID.ToString()))
                        continue;

                    risks.Add(risk);
                }

                lbRisks.DataSource = risks;
                if (!isAltLang)
                    lbRisks.DataTextField = "RiskCodeDescSR";
                else
                    lbRisks.DataTextField = "RiskCodeDescEN";
                lbRisks.DataValueField = "RiskID";
                lbRisks.DataBind();

                foreach (ListItem item in lbRisks.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private RiskTO createRisk(ListItem item, bool isAltLang)
        {
            try
            {
                RiskTO riskTO = new RiskTO();
                
                int startIndex = item.Text.IndexOf('(');
                int endIndex = item.Text.IndexOf(')');

                if (startIndex < 0)
                    startIndex = -1;

                if (endIndex < 0)
                    endIndex = item.Text.Length;

                riskTO.RiskCode = item.Text.Substring(startIndex + 1, endIndex - startIndex - 1);
                
                if (!isAltLang)
                    riskTO.DescSR = item.Text.Substring(endIndex + 1);
                else
                    riskTO.DescEN = item.Text.Substring(endIndex + 1);
                int riskID = -1;
                if (!int.TryParse(item.Value, out riskID))
                    riskID = -1;
                riskTO.RiskID = riskID;

                return riskTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAdd_Click(Object sender, EventArgs e)
        {
            try
            {
                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());
                
                List<RiskTO> selRisks = new List<RiskTO>();

                // get selected risks
                foreach (int index in lbRisks.GetSelectedIndices())
                {
                    if (index >= 0 && index < lbRisks.Items.Count)
                    {
                        selRisks.Add(createRisk(lbRisks.Items[index], isAltLang));
                    }
                }

                // add them to selected risks
                foreach (ListItem item in lbSelectedRisks.Items)
                {
                    selRisks.Add(createRisk(item, isAltLang));
                }

                selRisks.Sort(new ListSort());

                lbSelectedRisks.DataSource = selRisks;
                if (!isAltLang)
                    lbSelectedRisks.DataTextField = "RiskCodeDescSR";
                else
                    lbSelectedRisks.DataTextField = "RiskCodeDescEN";
                lbSelectedRisks.DataValueField = "RiskID";
                lbSelectedRisks.DataBind();

                foreach (ListItem item in lbSelectedRisks.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }

                populateRisks();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplOnDemandAppointmentsPage.btnAdd_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplOnDemandAppointmentsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnRemove_Click(Object sender, EventArgs e)
        {
            try
            {
                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                List<RiskTO> selRisks = new List<RiskTO>();
                
                // remove selected risks
                foreach (ListItem item in lbSelectedRisks.Items)
                {
                    if (item.Selected)
                        continue;

                    selRisks.Add(createRisk(item, isAltLang));
                }

                selRisks.Sort(new ListSort());

                lbSelectedRisks.DataSource = selRisks;
                if (!isAltLang)
                    lbSelectedRisks.DataTextField = "RiskCodeDescSR";
                else
                    lbSelectedRisks.DataTextField = "RiskCodeDescEN";
                lbSelectedRisks.DataValueField = "RiskID";
                lbSelectedRisks.DataBind();

                foreach (ListItem item in lbSelectedRisks.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }

                populateRisks();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplOnDemandAppointmentsPage.btnRemove_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplOnDemandAppointmentsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void lbSelectedRisks_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbSelectedRisks.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
            }
            catch { }
        }

        protected void lbRisks_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbRisks.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
            }
            catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplOnDemandAppointmentsPage).Assembly);

                lblError.Text = "";

                // create record to save / update
                MedicalCheckVisitHdrTO hdrTO = new MedicalCheckVisitHdrTO();

                if (lbSelectedRisks.Items.Count <= 0)
                {
                    lblError.Text = rm.GetString("noRiskSelected", culture);                    
                    return;
                }

                hdrTO.EmployeeID = getEmployee().EmployeeID;
                if (hdrTO.EmployeeID == -1)
                {
                    lblError.Text = rm.GetString("noEmplSelected", culture);                 
                    return;
                }
                                
                hdrTO.FlagEmail = Constants.noInt;
                hdrTO.FlagEmailCratedTime = new DateTime();
                hdrTO.ScheduleDate = Constants.dateTimeNullValue();
                hdrTO.PointID = Constants.defaultMedicalCheckPointId;

                hdrTO.Status = Constants.MedicalCheckVisitStatus.DEMANDED.ToString();                
                hdrTO.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                Dictionary<int, Dictionary<string, List<int>>> scheduledRisks = new MedicalCheckVisitDtl(Session[Constants.sessionConnection]).SearchScheduledVisits(hdrTO.EmployeeID.ToString().Trim(), Constants.VisitType.R.ToString());

                string riskScheduled = "";

                foreach (ListItem item in lbSelectedRisks.Items)
                {                    
                    MedicalCheckVisitDtlTO dtlTO = new MedicalCheckVisitDtlTO();
                    dtlTO.CheckID = createRisk(item, false).RiskID;
                    dtlTO.CreatedBy = hdrTO.CreatedBy;                    
                    dtlTO.Type = Constants.VisitType.R.ToString().Trim();

                    if (scheduledRisks.ContainsKey(hdrTO.EmployeeID) && scheduledRisks[hdrTO.EmployeeID].ContainsKey(Constants.VisitType.R.ToString())
                        && scheduledRisks[hdrTO.EmployeeID][Constants.VisitType.R.ToString()].Contains(dtlTO.CheckID))
                    {
                        riskScheduled = item.Text;
                        break;
                    }

                    hdrTO.VisitDetails.Add(dtlTO);
                }

                if (!riskScheduled.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("riskScheduled", culture) + " " + riskScheduled.Trim();
                    return;
                }

                hdrTO.FlagChange = Constants.noInt;

                MedicalCheckVisitHdr hdr = new MedicalCheckVisitHdr(Session[Constants.sessionConnection]);
                hdr.VisitHdrTO = hdrTO;
                
                // save new record                
                if (hdr.Save(true))
                {
                    lblError.Text = rm.GetString("recordSaved", culture);

                    lbSelectedRisks.DataSource = new List<RiskTO>();
                    lbSelectedRisks.DataTextField = "RiskCode";
                    lbSelectedRisks.DataValueField = "RiskID";
                    lbSelectedRisks.DataBind();

                    populateRisks();
                }
                else
                    lblError.Text = rm.GetString("recordNotSaved", culture);                

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplOnDemandAppointmentsPage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplOnDemandAppointmentsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void SaveState()
        {
            try
            {
                Dictionary<string, string> filterState = new Dictionary<string, string>();

                if (Session[Constants.sessionFilterState] != null && Session[Constants.sessionFilterState] is Dictionary<string, string>)
                    filterState = (Dictionary<string, string>)Session[Constants.sessionFilterState];

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "EmplOnDemandAppointmentsPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "EmplOnDemandAppointmentsPage.", filterState);
                }
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
    }
}
