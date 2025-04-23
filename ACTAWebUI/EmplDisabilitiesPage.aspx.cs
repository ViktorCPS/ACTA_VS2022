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
    public partial class EmplDisabilitiesPage : System.Web.UI.Page
    {
        const string pageName = "EmplDisabilitiesPage";

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
                    ClearSessionValues();

                    btnStartDate.Attributes.Add("onclick", "return calendarPicker('tbStartDate', 'false');");
                    btnEndDate.Attributes.Add("onclick", "return calendarPicker('tbEndDate', 'false');");
                    btnSave.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnDelete.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    tbDisability.Attributes.Add("onKeyUp", "return selectItem('tbDisability', 'lbDisability');");

                    btnStartDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnStartDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnEndDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnEndDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    EmployeeTO Empl = getEmployee();

                    setLanguage();

                    InitializeSQLParameters(Empl);

                    int company = -1;
                    if (Empl.EmployeeID != -1)
                    {
                        lblEmplData.Text = Empl.EmployeeID.ToString().Trim() + " - " + Empl.FirstAndLastName.Trim();

                        EmployeeAsco4 asco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                        asco.EmplAsco4TO.EmployeeID = Empl.EmployeeID;
                        List<EmployeeAsco4TO> ascoList = asco.Search();

                        if (ascoList.Count > 0)
                            company = ascoList[0].IntegerValue4;
                    }
                    else
                        tableSaveUpdate.Visible = lblNewEntry.Visible = false;

                    populateDisabilities(company);

                    string resultParameter = "";
                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                    {
                        if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckSupervisor
                            || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckWCDR
                            || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckLE)
                        {
                            tableSaveUpdate.Visible = lblNewEntry.Visible = false;
                            resultParameter = "&showSelection=false";
                        }
                    }

                    InitializeSelectedRecord();

                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=200" + resultParameter;
                }

                if (!SelRecord.Value.Trim().Equals(""))
                {
                    setSelection();
                    InitializeSelectedRecord();
                    SelRecord.Value = "";
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplDisabilitiesPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplDisabilitiesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplDisabilitiesPage).Assembly);

                lblStartDate.Text = rm.GetString("lblStartTime", culture);
                lblEndDate.Text = rm.GetString("lblEndTime", culture);
                lblType.Text = rm.GetString("lblType", culture);
                lblNote.Text = rm.GetString("lblNote", culture);
                lblDisability.Text = rm.GetString("lblDisability", culture);
                lblNewEntry.Text = rm.GetString("newEntry", culture);

                rbPermanent.Text = rm.GetString("rbPermanent", culture);
                rbTemporary.Text = rm.GetString("rbTemporary", culture);

                btnSave.Text = rm.GetString("btnSave", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateDisabilities(int company)
        {
            try
            {
                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                MedicalCheckDisability disability = new MedicalCheckDisability(Session[Constants.sessionConnection]);
                disability.DisabilityTO.WorkingUnitID = company;
                List<MedicalCheckDisabilityTO> disabilities = disability.SearchMedicalCheckDisabilities();

                lbDisability.DataSource = disabilities;
                if (!isAltLang)
                    lbDisability.DataTextField = "DisabilityCodeDescSR";
                else
                    lbDisability.DataTextField = "DisabilityCodeDescEN";
                lbDisability.DataValueField = "DisabilityID";
                lbDisability.DataBind();

                foreach (ListItem item in lbDisability.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbDisability_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbDisability.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
            }
            catch { }
        }

        private void InitializeSelectedRecord()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplDisabilitiesPage).Assembly);

                lblError.Text = "";
                tbDisability.Text = "";

                if (SelBox.Value.Trim().Equals(""))
                {
                    lblNewEntry.Text = rm.GetString("newEntry", culture);
                    btnSave.Text = rm.GetString("btnSave", culture);

                    // set to initial values
                    tbStartDate.Text = DateTime.Now.ToString(Constants.dateFormat);
                    tbEndDate.Text = "";
                    tbEndDate.Enabled = false;
                    btnEndDate.Enabled = false;
                    rbPermanent.Checked = true;
                    rbTemporary.Checked = false;
                    tbNote.Text = "";

                    foreach (ListItem item in lbDisability.Items)
                    {
                        item.Selected = false;
                    }

                    lbDisability.Enabled = true;
                    tbDisability.Enabled = true;
                }
                else
                {
                    lblNewEntry.Text = rm.GetString("updateEntry", culture);
                    btnSave.Text = rm.GetString("btnUpdate", culture);

                    // set to last selected record values                    
                    uint recID = 0;
                    // remove last separator, and get last selected row
                    string selectedValues = SelBox.Value.Substring(0, SelBox.Value.Length - 1);
                    if (uint.TryParse(selectedValues.Substring(selectedValues.LastIndexOf(Constants.delimiter) + 1), out recID) && recID > 0)
                    {
                        EmployeeXMedicalCheckDisability dis = new EmployeeXMedicalCheckDisability(Session[Constants.sessionConnection]);
                        dis.EmplXDisabilityTO.RecID = recID;
                        List<EmployeeXMedicalCheckDisabilityTO> list = dis.SearchEmployeeXMedicalCheckDisabilities();

                        if (list.Count > 0)
                        {
                            if (list[0].Type.Trim().ToUpper().Equals(Constants.MedicalCheckDisabilityType.PERMANENT.ToString().Trim().ToUpper()))
                            {
                                rbPermanent.Checked = true;
                                rbTemporary.Checked = false;
                                tbEndDate.Text = "";
                                tbEndDate.Enabled = false;
                                btnEndDate.Enabled = false;
                            }
                            else
                            {
                                rbTemporary.Checked = true;
                                rbPermanent.Checked = false;
                                tbEndDate.Text = list[0].DateEnd.ToString(Constants.dateFormat);
                                tbEndDate.Enabled = true;
                                btnEndDate.Enabled = true;
                            }
                            tbStartDate.Text = list[0].DateStart.ToString(Constants.dateFormat);
                            tbNote.Text = list[0].Note.Trim();
                                                        
                            foreach (ListItem item in lbDisability.Items)
                            {
                                item.Selected = item.Value.Equals(list[0].DisabilityID.ToString().Trim());

                                if (item.Selected)
                                    tbDisability.Text = item.Text.Trim();
                            }                            

                            lbDisability.Enabled = false;
                            tbDisability.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeSQLParameters(EmployeeTO empl)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplDisabilitiesPage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrID", culture) + "," + rm.GetString("hdrDisability", culture) + "," + rm.GetString("hdrStartTime", culture)
                    + "," + rm.GetString("hdrEndTime", culture) + "," + rm.GetString("hdrType", culture) + "," + rm.GetString("hdrNote", culture) + "," + rm.GetString("hdrOperater", culture);
                Session[Constants.sessionFields] = "ed.rec_id AS id| d.disability_code AS disability_code| ed.date_start AS date_start| ed.date_end AS date_end| ed.type AS type| ed.note AS note|u.name AS user_name";
                Session[Constants.sessionColTypes] = null;
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(2, (int)Constants.FormatTypes.DateFormat);
                formating.Add(3, (int)Constants.FormatTypes.DateFormat);
                Session[Constants.sessionFieldsFormating] = formating;
                Dictionary<int, Dictionary<string, string>> values = new Dictionary<int, Dictionary<string, string>>();
                Dictionary<string, string> formatValues = new Dictionary<string, string>();
                formatValues.Add(Constants.MedicalCheckDisabilityType.PERMANENT.ToString(), rm.GetString(Constants.MedicalCheckDisabilityType.PERMANENT.ToString(), culture));
                formatValues.Add(Constants.MedicalCheckDisabilityType.TEMPORARY.ToString(), rm.GetString(Constants.MedicalCheckDisabilityType.TEMPORARY.ToString(), culture));
                values.Add(5, formatValues);
                Session[Constants.sessionFieldsFormatedValues] = values;
                Session[Constants.sessionTables] = "employees_x_medical_chk_disabilities ed, medical_chk_disabilities d, appl_users u";
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionKey] = "id";
                Session[Constants.sessionFilter] = "ed.employee_id = '" + empl.EmployeeID.ToString().Trim() + "' AND ed.created_by = u.user_id AND ed.disability_id = d.disability_id";
                Session[Constants.sessionSortCol] = "ed.date_start";
                Session[Constants.sessionSortDir] = Constants.sortASC;
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

        protected void rbPermanent_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                if (rbPermanent.Checked)
                {
                    rbTemporary.Checked = !rbPermanent.Checked;
                    tbEndDate.Text = "";
                    tbEndDate.Enabled = false;
                    btnEndDate.Enabled = false;

                    setSelection();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplDisabilitiesPage.rbPermanent_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplDisabilitiesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void rbTemporary_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                if (rbTemporary.Checked)
                {
                    rbPermanent.Checked = !rbTemporary.Checked;
                    tbEndDate.Text = DateTime.Now.ToString(Constants.dateFormat);
                    tbEndDate.Enabled = true;
                    btnEndDate.Enabled = true;

                    setSelection();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplDisabilitiesPage.rbTemporary_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplDisabilitiesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplDisabilitiesPage).Assembly);

                lblError.Text = "";

                // create record to save / update
                EmployeeXMedicalCheckDisabilityTO emplDisTO = new EmployeeXMedicalCheckDisabilityTO();

                // get rec_id
                uint recID = 0;
                if (!SelBox.Value.Trim().Equals(""))
                {
                    // remove last separator, and get last selected row
                    string selectedValues = SelBox.Value.Substring(0, SelBox.Value.Length - 1);
                    if (!uint.TryParse(selectedValues.Substring(selectedValues.LastIndexOf(Constants.delimiter) + 1), out recID))
                        recID = 0;
                }                

                emplDisTO.EmployeeID = getEmployee().EmployeeID;
                if (emplDisTO.EmployeeID == -1)
                {
                    lblError.Text = rm.GetString("noEmplSelected", culture);
                    setSelection();
                    return;
                }
                emplDisTO.DateStart = CommonWeb.Misc.createDate(tbStartDate.Text.Trim());
                if (emplDisTO.DateStart.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("noDateStart", culture);
                    setSelection();
                    return;
                }

                EmployeeXMedicalCheckDisability dis = new EmployeeXMedicalCheckDisability(Session[Constants.sessionConnection]);
                dis.EmplXDisabilityTO = emplDisTO;
                List<EmployeeXMedicalCheckDisabilityTO> list = dis.SearchEmployeeXMedicalCheckDisabilities();

                bool disExist = false;
                foreach (EmployeeXMedicalCheckDisabilityTO disTO in list)
                {
                    if (disTO.RecID != recID)
                    {
                        disExist = true;
                        break;
                    }
                }

                if (disExist)
                {
                    lblError.Text = rm.GetString("disExist", culture);
                    setSelection();
                    return;
                }
                
                if (rbPermanent.Checked)
                    emplDisTO.Type = Constants.MedicalCheckDisabilityType.PERMANENT.ToString().Trim();
                else
                    emplDisTO.Type = Constants.MedicalCheckDisabilityType.TEMPORARY.ToString().Trim();

                if (rbTemporary.Checked)
                {
                    emplDisTO.DateEnd = CommonWeb.Misc.createDate(tbEndDate.Text.Trim());
                    if (emplDisTO.DateStart.Equals(new DateTime()))
                    {
                        lblError.Text = rm.GetString("noDateEnd", culture);
                        setSelection();
                        return;
                    }
                    if (emplDisTO.DateStart.Date > emplDisTO.DateEnd.Date)
                    {
                        lblError.Text = rm.GetString("startAfterEnd", culture);
                        setSelection();
                        return;
                    }
                }

                emplDisTO.Note = tbNote.Text.Trim();
                                
                if (lbDisability.GetSelectedIndices().Length <= 0)                    
                {
                    lblError.Text = rm.GetString("noDisabilitySelected", culture);
                    setSelection();
                    return;
                }

                int disID = -1;
                int.TryParse(lbDisability.SelectedValue.Trim(), out disID);
                emplDisTO.DisabilityID = disID;
                emplDisTO.RecID = recID;
                emplDisTO.FlagEmail = Constants.noInt;
                emplDisTO.FlagEmailCratedTime = new DateTime();
                                
                dis.EmplXDisabilityTO = emplDisTO;
                if (emplDisTO.RecID == 0)
                {
                    // save new record
                    dis.EmplXDisabilityTO.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                    if (dis.Save(true) > 0)
                    {
                        SelBox.Value = "";

                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=200";

                        InitializeSelectedRecord();

                        lblError.Text = rm.GetString("recordSaved", culture);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("recordNotSaved", culture);
                        setSelection();
                    }
                }
                else
                {
                    // update record
                    dis.EmplXDisabilityTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                    dis.EmplXDisabilityTO.ModifiedTime = new DateTime();
                    if (dis.Update(true))
                    {
                        SelBox.Value = "";

                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=200";

                        InitializeSelectedRecord();

                        lblError.Text = rm.GetString("recordUpdated", culture);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("recordNotUpdated", culture);
                        setSelection();
                    }
                }                

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplDisabilitiesPage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplDisabilitiesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplDisabilitiesPage).Assembly);

                lblError.Text = "";

                // get ids of records for deletion
                if (SelBox.Value.Trim().Equals(""))
                    lblError.Text = rm.GetString("noRecordToDelete", culture);
                else
                {
                    string recIDs = SelBox.Value.Replace(Constants.delimiter, ',');
                    recIDs = recIDs.Substring(0, recIDs.Length - 1);
                    if (new EmployeeXMedicalCheckDisability(Session[Constants.sessionConnection]).Delete(recIDs, true))
                    {
                        SelBox.Value = "";

                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=200";

                        InitializeSelectedRecord();

                        lblError.Text = rm.GetString("recordsDeleted", culture);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("recordsNotDeleted", culture);
                        setSelection();
                    }
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplDisabilitiesPage.btnDelete_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplDisabilitiesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPostBack_Click(object sender, EventArgs e)
        {
            try
            {
                setSelection();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplDisabilitiesPage.btnPostBack_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplDisabilitiesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "EmplDisabilitiesPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "EmplDisabilitiesPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setSelection()
        {
            try
            {
                // put selection in session
                string[] selectedList = SelBox.Value.Trim().Split(Constants.delimiter);
                List<string> selKeys = new List<string>();
                foreach (string key in selectedList)
                {
                    if (!key.Equals(""))
                        selKeys.Add(key);
                }
                Session[Constants.sessionSelectedKeys] = selKeys;
                Session[Constants.sessionSamePage] = true;
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

