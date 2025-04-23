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
    public partial class EmplVaccinesPage : System.Web.UI.Page
    {
        const string pageName = "EmplVaccinesPage";

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

                    btnDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'false');");                    
                    btnSave.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    btnDelete.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

                    btnDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    
                    EmployeeTO Empl = getEmployee();

                    setLanguage();

                    InitializeSQLParameters(Empl);

                    populateTypes();
                    populateRotation();

                    if (Empl.EmployeeID != -1)
                    {
                        lblEmplData.Text = Empl.EmployeeID.ToString().Trim() + " - " + Empl.FirstAndLastName.Trim();
                    }
                    else
                        tableSaveUpdate.Visible = lblNewEntry.Visible = false;
                                        
                    if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
                    {
                        if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckSupervisor
                            || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckWCDR
                            || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.MedicalCheckLE)
                        {
                            tableSaveUpdate.Visible = lblNewEntry.Visible = false;                    
                        }
                    }

                    InitializeSelectedRecord();

                    resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=250";
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplVaccinesPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplVaccinesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVaccinesPage).Assembly);

                lblDatePerformed.Text = rm.GetString("lblDate", culture);
                lblType.Text = rm.GetString("lblType", culture);
                lblNextVaccine.Text = rm.GetString("lblNextVaccine", culture);
                lblDesc.Text = rm.GetString("lblDesc", culture);
                lblNewEntry.Text = rm.GetString("newEntry", culture);

                btnSave.Text = rm.GetString("btnSave", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeSelectedRecord()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVaccinesPage).Assembly);

                lblError.Text = "";

                if (SelBox.Value.Trim().Equals(""))
                {
                    lblNewEntry.Text = rm.GetString("newEntry", culture);
                    btnSave.Text = rm.GetString("btnSave", culture);

                    // set to initial values
                    tbDate.Text = DateTime.Now.ToString(Constants.dateFormat);                    
                    if (cbType.Items.Count > 0)
                        cbType.SelectedIndex = 0;
                    if (cbRotation.Items.Count > 0)
                        cbRotation.SelectedIndex = 0;

                    tbDesc.Text = "";

                    cbType.Enabled = true;
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
                        EmployeeXVaccine emplVac = new EmployeeXVaccine(Session[Constants.sessionConnection]);
                        emplVac.EmplXVaccineTO.RecID = recID;
                        List<EmployeeXVaccineTO> list = emplVac.SearchEmployeeXVaccines();

                        if (list.Count > 0)
                        {                            
                            Vaccine vac = new Vaccine(Session[Constants.sessionConnection]);
                            vac.VaccineTO.VaccineID = list[0].VaccineID;
                            List<VaccineTO> vacList = vac.SearchVaccines();

                            if (vacList.Count > 0)
                            {
                                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                                    tbDesc.Text = vacList[0].DescSR.Trim();
                                else
                                    tbDesc.Text = vacList[0].DescEN.Trim();
                            }

                            tbDate.Text = list[0].DatePerformed.ToString(Constants.dateFormat);
                            cbType.SelectedValue = list[0].VaccineID.ToString().Trim();

                            if (list[0].Rotation == -1)
                                cbRotation.SelectedIndex = 0;
                            else
                                cbRotation.SelectedValue = list[0].Rotation.ToString().Trim();

                            cbType.Enabled = false;
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVaccinesPage).Assembly);

                Session[Constants.sessionHeader] = rm.GetString("hdrID", culture) + "," + rm.GetString("hdrDate", culture) + "," + rm.GetString("hdrType", culture);
                Session[Constants.sessionFields] = "ev.rec_id AS id| ev.date_performed AS date_perforemd| v.vaccine_type AS vaccine_type";
                Session[Constants.sessionColTypes] = null;
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(1, (int)Constants.FormatTypes.DateFormat);                
                Session[Constants.sessionFieldsFormating] = formating;
                Session[Constants.sessionFieldsFormatedValues] = null;
                Session[Constants.sessionTables] = "employee_x_vaccines ev, vaccines v";
                Session[Constants.sessionDataTableList] = null;
                Session[Constants.sessionDataTableColumns] = null;
                Session[Constants.sessionKey] = "id";
                Session[Constants.sessionFilter] = "ev.employee_id = '" + empl.EmployeeID.ToString().Trim() + "' AND ev.vaccine_id = v.vaccine_id";
                Session[Constants.sessionSortCol] = "ev.date_performed";
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

        private void populateTypes()
        {
            try
            {
                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());

                List<VaccineTO> vacList = new Vaccine(Session[Constants.sessionConnection]).SearchVaccines();
                
                cbType.DataSource = vacList;
                if (!isAltLang)
                    cbType.DataTextField = "VaccineTypeDescSR";
                else
                    cbType.DataTextField = "VaccineTypeDescEN";
                cbType.DataValueField = "VaccineID";
                cbType.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateRotation()
        {
            try
            {
                List<string> rotation = new List<string>();

                for (int i = 0; i < 25; i++)
                {
                    rotation.Add(i.ToString().Trim());
                }

                cbRotation.DataSource = rotation;
                cbRotation.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVaccinesPage).Assembly);

                lblError.Text = "";

                // create record to save / update
                EmployeeXVaccineTO emplVacTO = new EmployeeXVaccineTO();

                // get rec_id
                uint recID = 0;
                if (!SelBox.Value.Trim().Equals(""))
                {
                    // remove last separator, and get last selected row
                    string selectedValues = SelBox.Value.Substring(0, SelBox.Value.Length - 1);
                    if (!uint.TryParse(selectedValues.Substring(selectedValues.LastIndexOf(Constants.delimiter) + 1), out recID))
                        recID = 0;
                }
                emplVacTO.RecID = recID;

                emplVacTO.EmployeeID = getEmployee().EmployeeID;
                if (emplVacTO.EmployeeID == -1)
                {
                    lblError.Text = rm.GetString("noEmplSelected", culture);
                    setSelection();
                    return;
                }

                emplVacTO.DatePerformed = CommonWeb.Misc.createDate(tbDate.Text.Trim());
                if (emplVacTO.DatePerformed.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("noDate", culture);
                    setSelection();
                    return;
                }

                int vacID = -1;
                if (cbType.SelectedItem != null && int.TryParse(cbType.SelectedItem.Value.Trim(), out vacID))
                    emplVacTO.VaccineID = vacID;
                if (emplVacTO.VaccineID == -1)
                {
                    lblError.Text = rm.GetString("noVaccineSelected", culture);
                    setSelection();
                    return;
                }                               
                
                int rotation = -1;
                if (cbRotation.SelectedItem != null)
                {
                    if(!int.TryParse(cbRotation.SelectedItem.Text.Trim(), out rotation))
                        rotation = -1;
                }
                                
                if (rotation > 0)
                    emplVacTO.Rotation = rotation;

                emplVacTO.RotationFlagUsed = Constants.noInt;

                EmployeeXVaccine vac = new EmployeeXVaccine(Session[Constants.sessionConnection]);
                vac.EmplXVaccineTO = emplVacTO;
                if (emplVacTO.RecID == 0)
                {
                    // save new record
                    vac.EmplXVaccineTO.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                    if (vac.Save(true) > 0)
                    {
                        SelBox.Value = "";

                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=250";

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
                    vac.EmplXVaccineTO.ModifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                    vac.EmplXVaccineTO.ModifiedTime = new DateTime();
                    if (vac.Update(true))
                    {
                        SelBox.Value = "";

                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=250";

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplVaccinesPage.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplVaccinesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmplVaccinesPage).Assembly);

                lblError.Text = "";

                // get ids of records for deletion
                if (SelBox.Value.Trim().Equals(""))
                    lblError.Text = rm.GetString("noRecordToDelete", culture);
                else
                {
                    string recIDs = SelBox.Value.Replace(Constants.delimiter, ',');
                    recIDs = recIDs.Substring(0, recIDs.Length - 1);
                    if (new EmployeeXVaccine(Session[Constants.sessionConnection]).Delete(recIDs, true))
                    {
                        SelBox.Value = "";

                        resultIFrame.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx?height=250";

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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplVaccinesPage.btnDelete_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplVaccinesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmplVaccinesPage.btnPostBack_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/EmplVaccinesPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "EmplVaccinesPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "EmplVaccinesPage.", filterState);
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
