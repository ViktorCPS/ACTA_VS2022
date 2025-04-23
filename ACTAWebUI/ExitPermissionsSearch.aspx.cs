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

using TransferObjects;
using Common;
using Util;

namespace ACTAWebUI
{
    public partial class ExitPermissionsSearch : System.Web.UI.Page
    {
        private static CultureInfo culture;
        private static ResourceManager rm;
        private static string dateTimeFormat;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                    rm = new ResourceManager("ACTAWebUI.Resource", typeof(ExitPermissionsSearch).Assembly);
                    DateTimeFormatInfo dateTimeformat = new CultureInfo("en-US", true).DateTimeFormat;
                    dateTimeFormat = dateTimeformat.SortableDateTimePattern;

                    lbtnExit.Attributes.Add("onclick", "return LogoutByButton();");
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFrom');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbTo');");
                    btnUpdate.Attributes.Add("onclick", "return getSelectedKeys('" + updSelBox.ID.Trim() + "', '" + rm.GetString("noSelected", culture) + "', '" + resultIframe.ID.Trim() + "');");
                    btnDelete.Attributes.Add("onclick", "return getDelSelectedKeys('" + delSelBox.ID.Trim() + "', '" + rm.GetString("noSelected", culture) + "', '" + resultIframe.ID.Trim() + "');");

                    setLanguage();
                    populateTypes();
                    populateStatus();
                    populateIssued();

                    tbFrom.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                    tbTo.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());                    
                    
                    lblError.Text = "";                    
                    btnFromDate.Focus();

                    // if returned from result page, reload selected filter state
                    Dictionary<string, string> filterState = new Dictionary<string, string>();
                    if (Session["FilterState"] != null)
                        filterState = (Dictionary<string, string>)Session["FilterState"];
                    CommonWeb.Misc.LoadState(this, "ExitPermissionsSearch.", filterState);

                    // put sql parameters for result page in session
                    InitializeSQLParameters();
                    resultIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx";
                }
                else
                {
                    // if there are selected permissions to update, call update click
                    if (!updSelBox.Value.Trim().Equals(""))
                        btnUpdate_Click(this, new EventArgs());
                    if (!delSelBox.Value.Trim().Equals(""))
                        btnDelete_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionsSearch.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                lbtnExit.Text = rm.GetString("lbtnExit", culture);
                lbtnMenu.Text = rm.GetString("lbtnMenu", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblDateFormat.Text = rm.GetString("lblDateFormat", culture);
                lblDateExample.Text = rm.GetString("lblDateExample", culture);
                lblType.Text = rm.GetString("lblType", culture);
                lblStatus.Text = rm.GetString("lblStatus", culture);
                lblIssued.Text = rm.GetString("lblIssued", culture);
                lblExitPermissionsSearch.Text = rm.GetString("lblExitPermissionsSearch", culture);
                if (Session["LoggedInUser"] != null)
                    lblLoggedInUser.Text = Session["LoggedInUser"].ToString().Trim();
                else
                    lblLoggedInUser.Text = "";

                btnSearch.Text = rm.GetString("btnSearch", culture);
                btnUpdate.Text = rm.GetString("btnUpdate", culture);
                btnAdd.Text = rm.GetString("btnAdd", culture);
                btnDelete.Text = rm.GetString("btnDelete", culture);
                btnClear.Text = rm.GetString("btnClear", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
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
                PassType pt = new PassType();
                pt.PTypeTO.IsPass = Constants.passOnReader;
                List<PassTypeTO> ptArray = pt.Search();
                ptArray.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));

                cbType.DataSource = ptArray;
                cbType.DataTextField = "Description";
                cbType.DataValueField = "PassTypeID";

                cbType.DataBind(); // bez ovoga se ne poveze lista objekata sa drop down listom i nista se ne prikazuje

                cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateStatus()
        {
            try
            {
                ArrayList usedList = new ArrayList();
                
                //Permission verification - new state, unverified added
                usedList.Add(rm.GetString("all", culture));
                usedList.Add(rm.GetString("not_used", culture));
                usedList.Add(rm.GetString("used", culture));
                usedList.Add(rm.GetString("error", culture));
                usedList.Add(rm.GetString("unverified", culture));
                
                cbStatus.DataSource = usedList;
                cbStatus.DataBind();
                cbStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateIssued()
        {
            try
            {
                List<ApplUserTO> userArray = new ApplUser().Search();
                
                List<string> issuedByList = new List<string>();
                issuedByList.Add(rm.GetString("all", culture));
                issuedByList.Add(Session["LoggedInUser"].ToString().Trim());
                foreach (ApplUserTO user in userArray)
                {
                    issuedByList.Add(user.UserID.Trim());
                }

                issuedByList.Sort();

                cbIssued.DataSource = issuedByList;                
                cbIssued.DataBind();
                cbIssued.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbtnMenu_Click(Object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("/ACTAWeb/Default.aspx", false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionsSearch.lbtnMenu_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnClear_Click(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                
                tbFrom.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                tbTo.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                cbType.SelectedIndex = 0;
                cbStatus.SelectedIndex = 0;
                cbIssued.SelectedIndex = 0;
                
                ClearSessionValues();

                btnFromDate.Focus();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionsSearch.btnClear_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void ClearSessionValues()
        {
            try
            {
                // every session value is set to default value of corresponding type                
                if (Session["filter"] != null)
                    Session["filter"] = "";
                if (Session["sortCol"] != null)
                    Session["sortCol"] = "";
                if (Session["sortDir"] != null)
                    Session["sortDir"] = "";
                if (Session["FilterState"] != null)
                    Session["FilterState"] = new Dictionary<string, string>();
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
                Session["header"] = "ID," + rm.GetString("hdrType", culture) + "," + rm.GetString("hdrDateTime", culture) + "," + rm.GetString("hdrIssued", culture) + ","
                + rm.GetString("hdrOffset", culture) + "," + rm.GetString("hdrStatus", culture) + "," + rm.GetString("hdrDesc", culture) + "," + rm.GetString("hdrVerified", culture);
                Session["fields"] = "ep.permission_id AS id, pt.description AS pt_description, ep.start_time AS time, ep.created_by AS issued, ep.offset AS offset, ep.used AS status," 
                    + " ep.description AS description, ep.verified_by AS verified";
                Dictionary<int, int> formating = new Dictionary<int, int>();
                formating.Add(2, (int)Constants.FormatTypes.DateTimeFormat);
                Session["fieldsFormating"] = formating;
                Dictionary<int, Dictionary<string, string>> values = new Dictionary<int,Dictionary<string,string>>();                
                Dictionary<string, string> statuses = new Dictionary<string, string>();
                statuses.Add(((int)Constants.Used.No).ToString(), rm.GetString("not_used", culture));
                statuses.Add(((int)Constants.Used.Yes).ToString(), rm.GetString("used", culture));
                statuses.Add(((int)Constants.Used.Unverified).ToString(), rm.GetString("unverified", culture));
                values.Add(6, statuses);
                Session["fieldsFormatedValues"] = values;
                Session["tables"] = "exit_permissions ep, pass_types pt";
                Session["key"] = "id";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void clearSelection(HtmlInputHidden selBox)
        {
            try
            {
                selBox.Value = "";
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                if (!updSelBox.Value.Trim().Equals(Constants.updNoSelected.ToString().Trim()))
                {
                    List<string> selKeys = getSelectionValues(updSelBox);                    
                    Session["selectedKeys"] = selKeys;
                    Response.Redirect("/ACTAWeb/ACTAWebUI/ExitPermissionAdd.aspx?Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx", false);
                }

                clearSelection(updSelBox);
                Session["samePage"] = true;
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionsSearch.btnClear_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                if (!delSelBox.Value.Trim().Equals(Constants.updNoSelected.ToString().Trim()))
                {
                    List<string> selKeys = getSelectionValues(delSelBox);
                    Session["selectedKeys"] = selKeys;

                    bool usedPerm = false;

                    foreach (string id in selKeys)
                    {
                        ExitPermissionTO permTO = new ExitPermission().Find(int.Parse(id.Trim()));

                        if (permTO.Used == (int)Constants.Used.Yes)
                        {
                            usedPerm = true;
                            continue;
                        }

                        new ExitPermission().Delete(permTO.PermissionID, permTO.StartTime);
                    }

                    if (usedPerm)
                        lblError.Text = rm.GetString("usedPermDelete", culture);
                }

                clearSelection(delSelBox);
                Session["samePage"] = true;
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionsSearch.btnDelete_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                Response.Redirect("/ACTAWeb/ACTAWebUI/ExitPermissionAdd.aspx?Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx", false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionsSearch.btnClear_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnSearch_Click(Object sender, EventArgs e)
        {
            try
            {
                ClearSessionValues();
                CultureInfo ci = CultureInfo.InvariantCulture;

                lblError.Text = "";
                DateTime from = CommonWeb.Misc.createDate(tbFrom.Text.Trim());
                DateTime to = CommonWeb.Misc.createDate(tbTo.Text.Trim());
                
                int passTypeID = -1;
                string filter = "ep.employee_id = '" + Session["UserID"].ToString().Trim().ToUpper() + "' AND ";

                if (!tbFrom.Text.Trim().Equals("") && from.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFrom.Focus();
                    from = new DateTime();
                    return;
                }

                if (!tbTo.Text.Trim().Equals("") && to.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbTo.Focus();
                    to = new DateTime();
                    return;
                }

                if (!from.Equals(new DateTime()) && !(to.Equals(new DateTime())) && from.Date > to.Date)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    tbFrom.Focus();
                    return;
                }
                                
                if (!from.Equals(new DateTime()))
                {
                    if (Session["dataProvider"].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                        filter += "ep.start_time >= CONVERT(datetime,'" + from.Date.ToString("yyyy-MM-dd") + "', 111) AND ";                        
                    else if (Session["dataProvider"].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                        filter += "DATE_FORMAT(ep.start_time,'%Y-%m-%dT%h:%m:%s') >= '" + from.Date.ToString(dateTimeFormat, ci).Trim() + "' AND ";
                    
                }

                if (!to.Equals(new DateTime()))
                {
                    if (Session["dataProvider"].ToString().Trim().ToLower().Equals(Constants.dpSqlServer.Trim().ToLower()))
                        filter += "ep.start_time < CONVERT(datetime,'" + to.AddDays(1).Date.ToString("yyyy-MM-dd") + "', 111) AND ";
                    else if (Session["dataProvider"].ToString().Trim().ToLower().Equals(Constants.dpMySql.Trim().ToLower()))
                        filter += "DATE_FORMAT(ep.start_time,'%Y-%m-%dT%h:%m:%s') <= '" + to.AddDays(1).Date.ToString(dateTimeFormat, ci).Trim() + "' AND ";                    
                }

                if (cbType.SelectedIndex > 0)
                {
                    if (!int.TryParse(cbType.SelectedValue.Trim(), out passTypeID))
                    {
                        lblError.Text = rm.GetString("invalidPassType", culture);
                        cbType.Focus();
                        passTypeID = -1;
                        return;
                    }
                    else
                    {
                        filter += "ep.pass_type_id = '" + passTypeID.ToString().Trim().ToUpper() + "' AND ";
                    }
                }

                if (cbIssued.SelectedIndex > 0)
                {
                    filter += "RTRIM(LTRIM(UPPER(ep.created_by))) = N'" + cbIssued.Text.Trim().ToUpper() + "' AND ";
                }
                
                // proveriti statuse, odnosno used
                if (cbStatus.SelectedIndex > 0)
                {
                    filter += "ep.used = '" + (cbStatus.SelectedIndex - 1).ToString().Trim().ToUpper() + "' AND ";
                }

                filter += "ep.pass_type_id = pt.pass_type_id";

                // put sql statement required data in session
                Session["filter"] = filter;
                Session["sortCol"] = "ep.start_time";
                Session["sortDir"] = Constants.sortASC;

                // save selected filter state
                Session["FilterState"] = CommonWeb.Misc.SaveState(this, "ExitPermissionsSearch.", new Dictionary<string, string>());

                //Response.Redirect("/ACTAWeb/ACTAWebUI/ResultPage.aspx?Filter=/ACTAWeb/ACTAWebUI/PassesSearch.aspx&Title=" + rm.GetString("passes", culture), false);
                resultIframe.Attributes["src"] = "/ACTAWeb/ACTAWebUI/ResultPage.aspx";
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionsSearch.btnSearch_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                int rowCount = 0;
                DataTable exitPermissions = null;

                if (Session["fields"] != null && !Session["fields"].ToString().Trim().Equals("")
                    && Session["tables"] != null && !Session["tables"].ToString().Trim().Equals("")
                    && Session["filter"] != null && !Session["filter"].ToString().Trim().Equals("")
                    && Session["sortCol"] != null && !Session["sortCol"].ToString().Trim().Equals("")
                    && Session["sortDir"] != null && !Session["sortDir"].ToString().Trim().Equals(""))
                {
                    Result result = new Result();
                    rowCount = result.SearchResultCount(Session["tables"].ToString().Trim(), Session["filter"].ToString().Trim());

                    //Dictionary<int, List<string>> reportRecords = getReportRecordsValues(reportSelBox);

                    if (rowCount > 0)
                    {
                        // get all exit permissions for search criteria for report
                        exitPermissions = new Result().SearchResultTable(Session["fields"].ToString().Trim(), Session["tables"].ToString().Trim(), Session["filter"].ToString().Trim(),
                        Session["sortCol"].ToString().Trim(), Session["sortDir"].ToString().Trim(), 1, rowCount);

                        // Table Definition for Crystal Reports
                        DataSet dataSetCR = new DataSet();
                        DataTable tableCR = new DataTable("exit_permissions");
                        DataTable tableI = new DataTable("images");
                                                
                        tableCR.Columns.Add("employee", typeof(System.String));
                        tableCR.Columns.Add("pass_type", typeof(System.String));                        
                        tableCR.Columns.Add("issued", typeof(System.String));
                        tableCR.Columns.Add("date_time", typeof(System.DateTime));                        
                        tableCR.Columns.Add("imageID", typeof(byte));
                        tableCR.Columns.Add("tolerance", typeof(System.String));
                        tableCR.Columns.Add("state", typeof(System.String));
                        tableCR.Columns.Add("desc", typeof(System.String));
                        tableCR.Columns.Add("verified_by", typeof(System.String));

                        tableI.Columns.Add("imageID", typeof(byte));
                        tableI.Columns.Add("image", typeof(System.Byte[]));

                        //add logo image just once
                        DataRow rowI = tableI.NewRow();
                        rowI["image"] = Constants.LogoForReport;
                        rowI["imageID"] = 1;
                        tableI.Rows.Add(rowI);
                        tableI.AcceptChanges();

                        dataSetCR.Tables.Add(tableCR);
                        dataSetCR.Tables.Add(tableI);

                        foreach (DataRow perm in exitPermissions.Rows)
                        {
                            DataRow row = tableCR.NewRow();
                            
                            row["employee"] = lblLoggedInUser.Text;                            
                            if (perm["pt_description"] != DBNull.Value)
                                row["pass_type"] = perm["pt_description"].ToString().Trim();
                            if (perm["issued"] != DBNull.Value)
                                row["issued"] = perm["issued"].ToString().Trim();
                            if (perm["time"] != DBNull.Value)
                                row["date_time"] = perm["time"].ToString().Trim();
                            if (perm["offset"] != DBNull.Value)
                                row["tolerance"] = perm["offset"].ToString().Trim();                            
                            if (perm["status"] != DBNull.Value)
                            {
                                if (perm["status"].ToString().Trim().Equals(((int)Constants.Used.Yes).ToString()))
                                {
                                    row["state"] = rm.GetString("used", culture);
                                }
                                else if (perm["status"].ToString().Trim().Equals(((int)Constants.Used.No).ToString()))
                                {
                                    row["state"] = rm.GetString("not_used", culture);
                                }
                                else if (perm["status"].ToString().Trim().Equals(((int)Constants.Used.Unverified).ToString()))
                                {
                                    row["state"] = rm.GetString("unverified", culture);
                                }
                                else
                                    row["state"] = "";
                            }
                            if (perm["description"] != DBNull.Value)
                                row["desc"] = perm["description"].ToString().Trim();
                            if (perm["verified"] != DBNull.Value)
                                row["verified_by"] = perm["verified"].ToString().Trim();
                                                        
                            row["imageID"] = 1;

                            tableCR.Rows.Add(row);
                            tableCR.AcceptChanges();
                        }

                        if (tableCR.Rows.Count == 0)
                        {
                            lblError.Text = rm.GetString("noReportData", culture);
                            return;
                        }
                                                
                        string selPassType = "*";
                        string selStatus = "*";
                        string selIssued = "*";
                        
                        if (cbType.SelectedIndex > 0)
                            selPassType = cbType.SelectedItem.Text;
                        if (cbStatus.SelectedIndex > 0)
                            selStatus = cbStatus.SelectedItem.Text;
                        if (cbIssued.SelectedIndex > 0)
                            selIssued = cbIssued.SelectedItem.Text;
                        
                        Session["exitPermSelPassType"] = selPassType;
                        Session["exitPermSelStatus"] = selStatus;
                        Session["exitPermSelIssued"] = selIssued;                        
                        Session["exitPermDS"] = dataSetCR;
                        Session["exitPermFrom"] = tbFrom.Text;
                        Session["exitPermTo"] = tbTo.Text;

                        Session["reportName"] = rm.GetString("lblExitPermReport", culture);
                        string reportURL = "";
                        if (NotificationController.GetLanguage().Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                            reportURL = "/ACTAWeb/ReportsWeb/sr/ExitPermissionsReport_sr.aspx";
                        else
                            reportURL = "/ACTAWeb/ReportsWeb/en/ExitPermissionsReport_en.aspx";
                        Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx&Report=" + reportURL.Trim(), false);
                    }
                    else
                    {
                        lblError.Text = rm.GetString("noReportData", culture);
                    }
                }
                else
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                }

                Session["samePage"] = true;
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionsSearch.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionsSearch.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
