using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Resources;
using System.Drawing.PieChart.WebControl;
using System.Data;
using System.Drawing;
using System.Collections;

using Util;
using TransferObjects;
using Common;
using System.Configuration;

namespace ACTAWebUI
{
    public partial class WUStatisticalReportsPage : System.Web.UI.Page
    {
        const string pageName = "TLWUStatisticalReportsPage";

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
            tbFromDate.Text = fromDate.ToString("dd.MM.yyyy.");
            calendarFrom.Visible = false;
            btnFrom.Visible = true;
        }
        protected void DataToChange(object sender, EventArgs e)
        {
            toDate = calendarTo.SelectedDate;
            tbToDate.Text = toDate.ToString("dd.MM.yyyy.");
            calendarTo.Visible = false;
            btnTo.Visible = true;
        }




        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClearSessionValues();
                if (!IsPostBack)
                {
                    btnFromDate.Attributes.Add("onclick", "return calendarPicker('tbFromDate', 'false');");
                    btnToDate.Attributes.Add("onclick", "return calendarPicker('tbToDate', 'false');");
                    btnWUTree.Attributes.Add("onclick", "return wUnitsPicker('btnWUTree', 'false');");
                    btnOrgTree.Attributes.Add("onclick", "return oUnitsPicker('btnOrgTree', 'false');");
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");

                    btnWUTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnWUTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnOrgTree.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnOrgTree.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnFromDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnFromDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnToDate.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnToDate.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnPrevDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnPrevDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    btnNextDayPeriod.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    btnNextDayPeriod.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");

                    if (Session[Constants.sessionFromDate] != null && Session[Constants.sessionFromDate] is DateTime)
                        tbFromDate.Text = ((DateTime)Session[Constants.sessionFromDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbFromDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());

                    if (Session[Constants.sessionToDate] != null && Session[Constants.sessionToDate] is DateTime)
                        tbToDate.Text = ((DateTime)Session[Constants.sessionToDate]).ToString(Constants.dateFormat.Trim());
                    else
                        tbToDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());


                    lbPassTypes.Enabled = false;
                    if (Session["TLWUStatistical.single"] != null)
                    {
                        if ((int)Session["TLWUStatistical.single"] == 1)
                        {
                            lbPassTypes.Enabled = false;
                            rbSingle.Checked = true;
                            rbMultiple.Checked = false;
                        }
                        else
                        {
                            lbPassTypes.Enabled = true;
                            rbMultiple.Checked = true;
                            rbSingle.Checked = false;
                        }
                    }


                    int defaultWUID = -1;
                    int defaultOUID = -1;
                    if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    {
                        EmployeeAsco4 emplAsco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                        emplAsco.EmplAsco4TO.EmployeeID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

                        List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();

                        if (emplAscoList.Count == 1)
                        {
                            defaultWUID = emplAscoList[0].IntegerValue2;
                            defaultOUID = emplAscoList[0].IntegerValue3;
                        }
                    }

                    if (defaultWUID != -1 && Session[Constants.sessionLoginCategoryWUnits] != null && Session[Constants.sessionLoginCategoryWUnits] is List<int>
                        && ((List<int>)Session[Constants.sessionLoginCategoryWUnits]).Contains(defaultWUID))
                    {
                        populateWU(defaultWUID);
                    }
                    if (defaultOUID != -1 && Session[Constants.sessionLoginCategoryOUnits] != null && Session[Constants.sessionLoginCategoryOUnits] is List<int>
                       && ((List<int>)Session[Constants.sessionLoginCategoryOUnits]).Contains(defaultOUID))
                    {
                        populateOU(defaultOUID);
                    }

                    if (Session[Constants.sessionWU] == null && Session[Constants.sessionOU] == null)
                    {
                        if (defaultWUID == -1 && defaultOUID != -1)
                        {
                            Menu1.Items[1].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[1]);
                            populateUTE(defaultOUID.ToString(), false);
                            Session[Constants.sessionOU] = defaultOUID;
                        }
                        else
                        {
                            Menu1.Items[0].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[0]);
                            populateUTE(defaultWUID.ToString(), true);
                            Session[Constants.sessionWU] = defaultWUID;
                        }
                    }
                    else
                    {
                        if (Session[Constants.sessionOU] != null && Session[Constants.sessionOU] is int)
                        {
                            defaultOUID = (int)Session[Constants.sessionOU];
                            populateOU((int)Session[Constants.sessionOU]);
                            populateUTE(Session[Constants.sessionOU].ToString(), false);
                            List<WorkingUnitXOrganizationalUnitTO> listwUnitxOUnitTO = new List<WorkingUnitXOrganizationalUnitTO>();
                            WorkingUnitXOrganizationalUnitTO wUnitxOUnitTO = new WorkingUnitXOrganizationalUnitTO();
                            wUnitxOUnitTO.OrgUnitID = defaultOUID;
                            WorkingUnitXOrganizationalUnit wUnitxOUnit = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                            wUnitxOUnit.WUXouTO = wUnitxOUnitTO;
                            listwUnitxOUnitTO = wUnitxOUnit.Search();

                            if (listwUnitxOUnitTO.Count > 0)
                            {
                                string oneComp = listwUnitxOUnitTO[0].WorkingUnitID.ToString();
                                if (oneComp != "-1")
                                {
                                    populatePassTypes(oneComp);
                                }
                            }
                            Menu1.Items[1].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[1]);

                        }
                        else if (Session[Constants.sessionWU] != null && Session[Constants.sessionWU] is int)
                        {
                            defaultWUID = (int)Session[Constants.sessionWU];
                            populateWU((int)Session[Constants.sessionWU]);
                            populateUTE(Session[Constants.sessionWU].ToString(), true);
                            populatePassTypes(Session[Constants.sessionWU].ToString());
                            Menu1.Items[0].Selected = true;
                            MultiView1.SetActiveView(MultiView1.Views[0]);
                        }

                    }

                    for (int i = 0; i < Menu1.Items.Count; i++)
                    {
                        CommonWeb.Misc.setMenuImage(Menu1, i, Menu1.Items[i].Selected);
                        CommonWeb.Misc.setMenuSeparator(Menu1, i, Menu1.Items[i].Selected);
                    }

                    setLanguage();

                    if (Request.QueryString["reloadState"] != null && Request.QueryString["reloadState"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    {
                        ClearSessionValues();
                        if (Menu1.Items[0].Selected)
                        {
                            populatePassTypes(defaultWUID.ToString());
                        }
                        else
                        {
                            List<WorkingUnitXOrganizationalUnitTO> listwUnitxOUnitTO = new List<WorkingUnitXOrganizationalUnitTO>();
                            WorkingUnitXOrganizationalUnitTO wUnitxOUnitTO = new WorkingUnitXOrganizationalUnitTO();
                            wUnitxOUnitTO.OrgUnitID = defaultOUID;
                            WorkingUnitXOrganizationalUnit wUnitxOUnit = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                            wUnitxOUnit.WUXouTO = wUnitxOUnitTO;
                            listwUnitxOUnitTO = wUnitxOUnit.Search();

                            if (listwUnitxOUnitTO.Count > 0)
                            {
                                string oneComp = listwUnitxOUnitTO[0].WorkingUnitID.ToString();
                                if (oneComp != "-1")
                                {
                                    populatePassTypes(oneComp);
                                }
                            }
                        }
                    }
                    else // reload selected filter state
                    {
                        LoadState();
                        if (Menu1.Items[0].Selected)
                        {
                            populateWU(defaultWUID);
                            populateUTE(defaultWUID.ToString(), true);
                            populatePassTypes(defaultWUID.ToString());
                        }
                        else
                        {
                            lbPassTypes.Items.Clear();
                            List<WorkingUnitXOrganizationalUnitTO> listwUnitxOUnitTO = new List<WorkingUnitXOrganizationalUnitTO>();
                            WorkingUnitXOrganizationalUnitTO wUnitxOUnitTO = new WorkingUnitXOrganizationalUnitTO();
                            wUnitxOUnitTO.OrgUnitID = defaultOUID;
                            WorkingUnitXOrganizationalUnit wUnitxOUnit = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                            wUnitxOUnit.WUXouTO = wUnitxOUnitTO;
                            listwUnitxOUnitTO = wUnitxOUnit.Search();

                            if (listwUnitxOUnitTO.Count > 0)
                            {
                                string oneComp = listwUnitxOUnitTO[0].WorkingUnitID.ToString();
                                if (oneComp != "-1")
                                {
                                    populatePassTypes(oneComp);
                                }
                            }
                            populateOU(defaultOUID);
                            populateUTE(defaultOUID.ToString(), false);
                        }

                        LoadState();
                    }
                    if (Session["TLWUStatistical.selectedUTE"] != null)
                    {
                        if (lboxUTE.Items.Count > 0)
                        {
                            foreach (int index in (int[])Session["TLWUStatistical.selectedUTE"])
                            {
                                lboxUTE.Items[index].Selected = true;
                            }
                        }
                    } if (Session["TLWUStatistical.selectedPassType"] != null)
                    {
                        if (lbPassTypes.Items.Count > 0)
                        {
                            foreach (int index in (int[])Session["TLWUStatistical.selectedPassType"])
                            {
                                lbPassTypes.Items[index].Selected = true;
                            }
                        }
                    }
                    btnShow_Click(this, new EventArgs());
                }
                else
                {
                    if (Session[Constants.sessionSelectedWUID] != null)
                    {
                        ClientScript.GetPostBackClientHyperlink(btnWUTree, "");
                        lblError.Text = "";
                        int wuID = -1;
                        if (!int.TryParse(Session[Constants.sessionSelectedWUID].ToString(), out wuID))
                            wuID = -1;

                        if (wuID != -1)
                        {
                            populateWU(wuID);
                            populateUTE(wuID.ToString(), true);
                            populatePassTypes(wuID.ToString());
                        }
                    }
                    else if (Session[Constants.sessionSelectedOUID] != null)
                    {
                        ClientScript.GetPostBackClientHyperlink(btnOrgTree, "");
                        lblError.Text = "";
                        int ouID = -1;
                        if (!int.TryParse(Session[Constants.sessionSelectedOUID].ToString(), out ouID))
                            ouID = -1;

                        if (ouID != -1)
                        {
                            List<WorkingUnitXOrganizationalUnitTO> listwUnitxOUnitTO = new List<WorkingUnitXOrganizationalUnitTO>();
                            WorkingUnitXOrganizationalUnitTO wUnitxOUnitTO = new WorkingUnitXOrganizationalUnitTO();
                            wUnitxOUnitTO.OrgUnitID = ouID;
                            WorkingUnitXOrganizationalUnit wUnitxOUnit = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                            wUnitxOUnit.WUXouTO = wUnitxOUnitTO;
                            listwUnitxOUnitTO = wUnitxOUnit.Search();

                            if (listwUnitxOUnitTO.Count > 0)
                            {
                                string oneComp = listwUnitxOUnitTO[0].WorkingUnitID.ToString();
                                if (oneComp != "-1")
                                {
                                    populatePassTypes(oneComp);
                                }
                            }
                            populateOU(ouID);
                            populateUTE(ouID.ToString(), false);
                        }
                    }
                }
                Session[Constants.sessionSelectedWUID] = null;
                Session[Constants.sessionSelectedOUID] = null;
                Session["TLWUStatistical.selectedPassType"] = null;
                Session["TLWUStatistical.selectedUTE"] = null;
                Session["TLWUStatistical.single"] = null;

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLWUStatisticalReportsPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnPrevDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WUStatisticalReportsPage).Assembly);

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                //date validation
                if (fromDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFromDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                tbFromDate.Text = fromDate.AddDays(-1).ToString(Constants.dateFormat);
                tbToDate.Text = toDate.AddDays(-1).ToString(Constants.dateFormat);

                //btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLWUStatisticalReportsPage.btnPrevDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnNextDayPeriod_Click(object sender, EventArgs e)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WUStatisticalReportsPage).Assembly);

                //check inserted date
                if (tbFromDate.Text.Trim().Equals("") || tbToDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDateInterval", culture);

                    if (tbFromDate.Text.Trim().Equals(""))
                        tbFromDate.Focus();
                    else if (tbToDate.Text.Trim().Equals(""))
                        tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                //date validation
                if (fromDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    tbFromDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    tbToDate.Focus();
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                tbFromDate.Text = fromDate.AddDays(1).ToString(Constants.dateFormat);
                tbToDate.Text = toDate.AddDays(1).ToString(Constants.dateFormat);

                //btnShow_Click(this, new EventArgs());

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLWUStatisticalReportsPage.btnNextDayPeriod_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx&Header=" + Constants.trueValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateOU(int ouID)
        {
            try
            {
                OrganizationalUnitTO ou = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(ouID);
                tbOrg.Attributes.Add("id", ouID.ToString());
                tbOrg.Text = ou.Name.Trim();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populatePassTypes(string ID)
        {
            try
            {
                List<PassTypeTO> passTypeList = new List<PassTypeTO>();
                int company = Common.Misc.getRootWorkingUnit(int.Parse(ID), getWUnits());
                foreach (KeyValuePair<int, PassTypeTO> pair in getTypes(company))
                {
                    passTypeList.Add(pair.Value);
                }
                lbPassTypes.DataSource = passTypeList;

                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                {
                    lbPassTypes.DataTextField = "DescriptionAndID";
                }
                else
                {
                    lbPassTypes.DataTextField = "DescriptionAltAndID";
                }
                lbPassTypes.DataValueField = "PassTypeID";
                lbPassTypes.DataBind();

                foreach (ListItem item in lbPassTypes.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbPassTypes_PreRender(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in lbPassTypes.Items)
                {
                    item.Attributes.Add("title", item.Text);
                }
            }
            catch { }
        }

        private void populateWU(int wuID)
        {
            try
            {
                WorkingUnitTO wu = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(wuID);
                tbWorkshop.Text = wu.Name.Trim();
                tbWorkshop.Attributes.Add("id", wu.WorkingUnitID.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateUTE(string ID, bool isWU)
        {
            try
            {
                if (isWU)
                {
                    int id = -1;
                    if (int.TryParse(ID, out id))
                    {
                        List<WorkingUnitTO> AllWUList = new List<WorkingUnitTO>();
                        List<WorkingUnitTO> List = new List<WorkingUnitTO>();

                        WorkingUnitTO defaultWU = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(int.Parse(ID));

                        List<WorkingUnitTO> listChildren = new WorkingUnit(Session[Constants.sessionConnection]).SearchChildWU(ID);
                        AllWUList.Add(defaultWU);
                        AllWUList.AddRange(listChildren);

                        bool isParent = false;
                        if (listChildren.Count > 0)
                            isParent = true;

                        while (isParent)
                        {
                            foreach (WorkingUnitTO Child in listChildren)
                            {
                                List<WorkingUnitTO> wuChildList = new WorkingUnit(Session[Constants.sessionConnection]).SearchChildWU(Child.WorkingUnitID.ToString());
                                List.AddRange(wuChildList);
                            }

                            if (List.Count > 0)
                                isParent = true;
                            else
                                isParent = false;

                            listChildren.Clear();
                            listChildren.AddRange(List);
                            AllWUList.AddRange(List);
                            List.Clear();
                        }

                        lboxUTE.DataSource = AllWUList;
                        lboxUTE.DataTextField = "Name";
                        lboxUTE.DataValueField = "WorkingUnitID";
                        lboxUTE.DataBind();
                    }
                }
                else if (!isWU)
                {
                    int id = -1;
                    if (int.TryParse(ID, out id))
                    {
                        List<OrganizationalUnitTO> AllOUList = new List<OrganizationalUnitTO>();
                        List<OrganizationalUnitTO> List = new List<OrganizationalUnitTO>();

                        OrganizationalUnitTO defaultWU = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(int.Parse(ID));

                        List<OrganizationalUnitTO> listChildren = new OrganizationalUnit(Session[Constants.sessionConnection]).SearchChildOU(ID);
                        AllOUList.Add(defaultWU);
                        AllOUList.AddRange(listChildren);

                        bool isParent = false;
                        if (listChildren.Count > 0)
                            isParent = true;

                        while (isParent)
                        {
                            foreach (OrganizationalUnitTO Child in listChildren)
                            {
                                List<OrganizationalUnitTO> wuChildList = new OrganizationalUnit(Session[Constants.sessionConnection]).SearchChildOU(Child.OrgUnitID.ToString());
                                List.AddRange(wuChildList);
                            }

                            if (List.Count > 0)
                                isParent = true;
                            else
                                isParent = false;

                            listChildren.Clear();
                            listChildren.AddRange(List);
                            AllOUList.AddRange(List);
                            List.Clear();
                        }
                        lboxUTE.DataSource = AllOUList;
                        lboxUTE.DataTextField = "Name";
                        lboxUTE.DataValueField = "OrgUnitID";
                        lboxUTE.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                RadioButton rb = sender as RadioButton;
                if (rb == rbSingle)
                {
                    rbMultiple.Checked = !rbSingle.Checked;
                    lbPassTypes.Enabled = false;
                    // lboxUTE.SelectionMode = ListSelectionMode.Single;

                }
                else if (rb == rbMultiple)
                {
                    rbSingle.Checked = !rbMultiple.Checked;
                    lbPassTypes.Enabled = true;
                    // lboxUTE.SelectionMode = ListSelectionMode.Multiple;
                }

                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLWUStatisticalReportsPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void setLanguage()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WUStatisticalReportsPage).Assembly);

                lblPeriod.Text = rm.GetString("lblPeriod", culture);
                lblPassTypes.Text = rm.GetString("lblPassType", culture);
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                rbSingle.Text = rm.GetString("rbSingle", culture);
                rbMultiple.Text = rm.GetString("rbMultiple", culture);
                lblUTE.Text = rm.GetString("lblWorkingUnitSelection", culture);

                btnShow.Text = rm.GetString("btnShow", culture);
                btnReport.Text = rm.GetString("btnReport", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, WorkingUnitTO> getWUnits()
        {
            try
            {
                Dictionary<int, WorkingUnitTO> wUnits = new Dictionary<int, WorkingUnitTO>();
                List<WorkingUnitTO> wuList = new WorkingUnit(Session[Constants.sessionConnection]).Search();

                foreach (WorkingUnitTO wu in wuList)
                {
                    if (!wUnits.ContainsKey(wu.WorkingUnitID))
                        wUnits.Add(wu.WorkingUnitID, wu);
                    else
                        wUnits[wu.WorkingUnitID] = wu;
                }

                return wUnits;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, OrganizationalUnitTO> getOUnits()
        {
            try
            {
                Dictionary<int, OrganizationalUnitTO> oUnits = new Dictionary<int, OrganizationalUnitTO>();
                List<OrganizationalUnitTO> ouList = new OrganizationalUnit(Session[Constants.sessionConnection]).Search();

                foreach (OrganizationalUnitTO ou in ouList)
                {
                    if (!oUnits.ContainsKey(ou.OrgUnitID))
                        oUnits.Add(ou.OrgUnitID, ou);
                    else
                        oUnits[ou.OrgUnitID] = ou;
                }

                return oUnits;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, PassTypeTO> getTypes(int company)
        {
            try
            {
                bool isAltLang = !CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper());
                return new PassType(Session[Constants.sessionConnection]).SearchForCompanyDictionary(company, isAltLang);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnShow_Click(Object sender, EventArgs e)
        {
            try
            {
                SaveState();
                for (int i = 1; i < 5; i++)
                {
                    Session["TLWUStatistical.dataTable" + i] = null;
                    Session["TLWUStatistical.realised" + i] = null;
                    Session["TLWUStatistical.planned" + i] = null;
                    Session["TLWUStatistical.title" + i] = null;

                }
                lblError.Text = "";
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WUStatisticalReportsPage).Assembly);

                int numOfTables = 0;
                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());

                int company = -1;
                if (Menu1.SelectedItem.Value.Equals("0"))
                {
                    int wuID = -1;
                    if (tbWorkshop.Attributes["id"] != null)
                        if (!int.TryParse(tbWorkshop.Attributes["id"], out wuID))
                            wuID = -1;

                    company = Common.Misc.getRootWorkingUnit(wuID, getWUnits());
                }
                else
                {
                    int ouID = -1;
                    if (tbOrg.Attributes["id"] != null)
                        if (!int.TryParse(tbOrg.Attributes["id"], out ouID))
                            ouID = -1;

                    if (ouID >= 0)
                    {
                        WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                        wuXou.WUXouTO.OrgUnitID = ouID;
                        List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                        if (list.Count > 0)
                        {
                            WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                            company = Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, getWUnits());
                        }
                        else
                            company = -1;
                    }
                    else
                        company = -1;
                }

                Dictionary<int, PassTypeTO> passTypesDictionary = getTypes(company);

                List<PassTypeTO> passTypesList = new List<PassTypeTO>();
                //get list from dictionary passTypeDictionary
                foreach (KeyValuePair<int, PassTypeTO> pair in passTypesDictionary)
                {
                    passTypesList.Add(pair.Value);
                }
                //date validation
                if (fromDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidFromDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (toDate.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (fromDate > toDate)
                {
                    lblError.Text = rm.GetString("invalidFromToDate", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                Session[Constants.sessionFromDate] = fromDate;
                Session[Constants.sessionToDate] = toDate;

                // get selected wunits
                string wulIDs = "";
                if (Menu1.Items[0].Selected && tbWorkshop.Text == "")
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                if (Menu1.Items[1].Selected && tbOrg.Text == "")
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }

                if (lboxUTE.GetSelectedIndices().Length <= 0)
                {
                    if (Menu1.Items[0].Selected)
                        wulIDs = tbWorkshop.Attributes["id"];
                    else
                        wulIDs = tbOrg.Attributes["id"];
                }
                else
                {
                    foreach (int wuIndex in lboxUTE.GetSelectedIndices())
                    {
                        if (wuIndex >= 0 && wuIndex < lboxUTE.Items.Count)
                            wulIDs += lboxUTE.Items[wuIndex].Value.Trim() + ",";
                    }

                    if (wulIDs.Length > 0)
                        wulIDs = wulIDs.Substring(0, wulIDs.Length - 1);

                }
                if (wulIDs == null || wulIDs == "")
                {
                    //lblError.Text = rm.GetString("no ute", culture);
                    lblError.Text = "";
                    writeLog(DateTime.Now, false);
                    return;
                }

                List<IOPairProcessedTO> IOPairsSelWU = new List<IOPairProcessedTO>();
                if (rbSingle.Checked)
                {
                    
                    if (Menu1.Items[0].Selected)
                    {
                        if (lboxUTE.GetSelectedIndices().Length > 1)
                        {
                            if (lboxUTE.GetSelectedIndices().Length > 4)
                            {
                                lblError.Text = rm.GetString("lblUteSelection", culture);
                                writeLog(DateTime.Now, false);
                                return;
                            }
                            foreach (string wu in wulIDs.Split(','))
                            {
                                
                                List<WorkingUnitTO> listWU = new List<WorkingUnitTO>();
                                WorkingUnitTO wuTO = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(int.Parse(wu));
                                listWU.Add(wuTO);
                                listWU = new WorkingUnit(Session[Constants.sessionConnection]).FindAllChildren(listWU);

                                string wuids = "";
                                foreach (WorkingUnitTO wuS in listWU)
                                {
                                    wuids = wuids + wuS.WorkingUnitID + ",";
                                }
                                if (wuids.Length > 0)
                                    wuids = wuids.Substring(0, wuids.Length - 1);

                                List<EmployeeTO> listEmpl = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(wuids, -1, null, fromDate, toDate);
                                IOPairsSelWU = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmpl);
                              
                                Dictionary<int, string> passTypeIOPairsOneSelWU = TotalHourCalc(IOPairsSelWU, passTypesDictionary);
                              

                                if (passTypeIOPairsOneSelWU.Count > 0)
                                {
                                    CalcWorkOnHoliday(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);                                 
                                    CalcNightWork(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);
                                    //CalcRotaryWork(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);
                                    
                                    double planned = CalcPlannedTime(listEmpl, fromDate, toDate);
                                    double realised = CalcRealisedTime(listEmpl, passTypesList, passTypeIOPairsOneSelWU, company);
                             
                                    DrawStatistics(passTypeIOPairsOneSelWU, listEmpl, fromDate, toDate, realised, planned, passTypesList, 2, wuTO.Name, numOfTables);
                                    numOfTables++;
                                }
                                else
                                {
                                    lblError.Text += rm.GetString("noReportData", culture) + ": " + wuTO.Name + ". ";
                                }
                            }
                        }
                        else
                        {
                            List<WorkingUnitTO> listWU = new List<WorkingUnitTO>();
                            WorkingUnitTO wuTO = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(int.Parse(wulIDs));
                            listWU.Add(wuTO);
                            listWU = new WorkingUnit(Session[Constants.sessionConnection]).FindAllChildren(listWU);

                            string wuids = "";
                            foreach (WorkingUnitTO wuS in listWU)
                            {
                                wuids = wuids + wuS.WorkingUnitID + ",";
                            }
                            if (wuids.Length > 0)
                                wuids = wuids.Substring(0, wuids.Length - 1);

                            List<EmployeeTO> listEmpl = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(wuids, -1, null, fromDate, toDate);

                            IOPairsSelWU = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmpl);
                      
                            Dictionary<int, string> passTypeIOPairsOneSelWU = TotalHourCalc(IOPairsSelWU, passTypesDictionary);
                

                            if (passTypeIOPairsOneSelWU.Count > 0)
                            {
                                CalcWorkOnHoliday(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);
                                CalcNightWork(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);
                                CalcRotaryWork(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);
                                double planned = CalcPlannedTime(listEmpl, fromDate, toDate);
                                double realised = CalcRealisedTime(listEmpl, passTypesList, passTypeIOPairsOneSelWU, company);
                                DrawStatistics(passTypeIOPairsOneSelWU, listEmpl, fromDate, toDate, realised, planned, passTypesList, 1, wuTO.Name, numOfTables);
                                numOfTables++;
                            }
                            else
                            {
                                lblError.Text = rm.GetString("noReportData", culture);
                            }
                        }
                    }
                    else
                    {
                        if (lboxUTE.GetSelectedIndices().Length > 1)
                        {
                            if (lboxUTE.GetSelectedIndices().Length > 4)
                            {
                                lblError.Text = rm.GetString("lblUteSelection", culture);
                                writeLog(DateTime.Now, false);
                                return;
                            }
                            foreach (string wu in wulIDs.Split(','))
                            {
                                List<OrganizationalUnitTO> listWU = new List<OrganizationalUnitTO>();
                                OrganizationalUnitTO wuTO = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(int.Parse(wu));
                                listWU.Add(wuTO);
                                listWU = new OrganizationalUnit(Session[Constants.sessionConnection]).FindAllChildren(listWU);

                                string wuids = "";
                                foreach (OrganizationalUnitTO wuS in listWU)
                                {
                                    wuids = wuids + wuS.OrgUnitID + ",";
                                }
                                if (wuids.Length > 0)
                                    wuids = wuids.Substring(0, wuids.Length - 1);

                                List<EmployeeTO> listEmpl = new Employee(Session[Constants.sessionConnection]).SearchByOU(wuids, -1, null, fromDate, toDate);

                                IOPairsSelWU = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmpl);
                                Dictionary<int, string> passTypeIOPairsOneSelWU = TotalHourCalc(IOPairsSelWU, passTypesDictionary);

                                if (passTypeIOPairsOneSelWU.Count > 0)
                                {
                                    CalcWorkOnHoliday(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);
                                    CalcNightWork(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);
                                    CalcRotaryWork(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);

                                    double planned = CalcPlannedTime(listEmpl, fromDate, toDate);
                                    double realised = CalcRealisedTime(listEmpl, passTypesList, passTypeIOPairsOneSelWU, company);

                                    DrawStatistics(passTypeIOPairsOneSelWU, listEmpl, fromDate, toDate, realised, planned, passTypesList, 2, wuTO.Name, numOfTables);
                                    numOfTables++;
                                }
                                else
                                {
                                    lblError.Text += rm.GetString("noReportData", culture) + ": " + wuTO.Name + ". ";
                                }
                            }
                        }
                        else
                        {
                            List<OrganizationalUnitTO> listWU = new List<OrganizationalUnitTO>();
                            OrganizationalUnitTO wuTO = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(int.Parse(wulIDs));
                            listWU.Add(wuTO);
                            listWU = new OrganizationalUnit(Session[Constants.sessionConnection]).FindAllChildren(listWU);

                            string wuids = "";
                            foreach (OrganizationalUnitTO wuS in listWU)
                            {
                                wuids = wuids + wuS.OrgUnitID + ",";
                            }
                            if (wuids.Length > 0)
                                wuids = wuids.Substring(0, wuids.Length - 1);

                            List<EmployeeTO> listEmpl = new Employee(Session[Constants.sessionConnection]).SearchByOU(wuids, -1, null, fromDate, toDate);

                            IOPairsSelWU = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmpl);
                            Dictionary<int, string> passTypeIOPairsOneSelWU = TotalHourCalc(IOPairsSelWU, passTypesDictionary);

                            if (passTypeIOPairsOneSelWU.Count > 0)
                            {
                                CalcWorkOnHoliday(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);
                                CalcNightWork(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);
                                CalcRotaryWork(listEmpl, company, IOPairsSelWU, fromDate, toDate, passTypeIOPairsOneSelWU, -1);

                                double planned = CalcPlannedTime(listEmpl, fromDate, toDate);
                                double realised = CalcRealisedTime(listEmpl, passTypesList, passTypeIOPairsOneSelWU, company);
                                DrawStatistics(passTypeIOPairsOneSelWU, listEmpl, fromDate, toDate, realised, planned, passTypesList, 1, wuTO.Name, numOfTables); numOfTables++;

                            }
                            else
                            {
                                lblError.Text = rm.GetString("noReportData", culture);
                            }
                        }
                    }
                }
                else
                {

                    if (Menu1.Items[0].Selected)
                    {
                        if (lbPassTypes.GetSelectedIndices().Length > 1)
                        {
                            if (lbPassTypes.GetSelectedIndices().Length <= 4)
                            {
                                foreach (int index in lbPassTypes.GetSelectedIndices())//if selected more than one pass_type
                                {
                                    string wul = tbWorkshop.Attributes["id"];
                                    int rootWU = company;
                                    string selectedwu = "";
                                    foreach (int ind in lboxUTE.GetSelectedIndices())
                                    {
                                        selectedwu += lboxUTE.Items[ind].Value + ",";

                                    }
                                    if (selectedwu.Length > 0)
                                        selectedwu = selectedwu.Remove(selectedwu.LastIndexOf(","));
                                    List<WorkingUnitTO> listSelectedWU = new List<WorkingUnitTO>();

                                    foreach (string wu in wulIDs.Split(','))
                                    {
                                        WorkingUnitTO wuTOSel = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(int.Parse(wu));
                                        listSelectedWU.Add(wuTOSel);
                                    }
                                    listSelectedWU = new WorkingUnit(Session[Constants.sessionConnection]).FindAllChildren(listSelectedWU);

                                    Dictionary<int, WorkingUnitTO> dictionarySelected = new Dictionary<int, WorkingUnitTO>();
                                    foreach (WorkingUnitTO selected in listSelectedWU)
                                    {
                                        if (!dictionarySelected.ContainsKey(selected.WorkingUnitID))
                                            dictionarySelected.Add(selected.WorkingUnitID, selected);

                                    }
                                    List<WorkingUnitTO> listWUWhole = new List<WorkingUnitTO>();
                                    WorkingUnitTO wuTO = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(rootWU);
                                    listWUWhole.Add(wuTO);
                                    listWUWhole = new WorkingUnit(Session[Constants.sessionConnection]).FindAllChildren(listWUWhole);
                                    List<WorkingUnitTO> listWU = new List<WorkingUnitTO>();
                                    foreach (WorkingUnitTO root in listWUWhole)
                                    {

                                        if (!dictionarySelected.ContainsKey(root.WorkingUnitID))
                                        {
                                            listWU.Add(root);
                                        }

                                    }
                                    string wuids = "";
                                    foreach (WorkingUnitTO wuS in listWU)
                                    {

                                        if (wul != wuS.WorkingUnitID.ToString())
                                            wuids = wuids + wuS.WorkingUnitID + ",";

                                    }
                                    if (wuids.Length > 0)
                                        wuids = wuids.Substring(0, wuids.Length - 1);

                                    List<EmployeeTO> listEmpl = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(wuids, -1, null, fromDate, toDate);

                                    List<IOPairProcessedTO> IOPairsRoot = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmpl);
                                    Dictionary<int, string> passTypeIOPairsRoot = TotalHourCalc(IOPairsRoot, passTypesDictionary);
                                    if (IOPairsRoot.Count > 0)
                                    {
                                        CalcWorkOnHoliday(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.Items[index].Value));
                                        CalcNightWork(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.Items[index].Value));
                                        CalcRotaryWork(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.Items[index].Value));
                                    }
                                    Dictionary<int, Dictionary<int, string>> checkDictionary = new Dictionary<int, Dictionary<int, string>>();
                                    Dictionary<int, string> ioPairsForSelPassType = new Dictionary<int, string>();

                                    double realisedSel = 0;
                                    double plannedSel = 0;
                                    foreach (string wu in wulIDs.Split(','))
                                    {

                                        List<WorkingUnitTO> listOne = new List<WorkingUnitTO>();

                                        WorkingUnitTO wuTOSel = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(int.Parse(wu));

                                        listOne.Add(wuTOSel);

                                        listOne = new WorkingUnit(Session[Constants.sessionConnection]).FindAllChildren(listOne);

                                        string wu1 = "";
                                        foreach (WorkingUnitTO wuto in listOne)
                                        {
                                            wu1 += wuto.WorkingUnitID + ",";
                                        }
                                        if (wu1.Length > 0)
                                            wu1 = wu1.Remove(wu1.LastIndexOf(','));

                                        List<EmployeeTO> listEmplSel = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(wu1, -1, null, fromDate, toDate);

                                        List<IOPairProcessedTO> IOPairsSel = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmplSel);
                                        Dictionary<int, string> passTypeAllIOPairsOneSelWU = TotalHourCalc(IOPairsSel, passTypesDictionary);
                                        if (IOPairsSel.Count > 0)
                                        {
                                            CalcWorkOnHoliday(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.Items[index].Value));
                                            CalcNightWork(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.Items[index].Value));
                                            CalcRotaryWork(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.Items[index].Value));
                                        } realisedSel += CalcRealisedTime(listEmplSel, passTypesList, passTypeAllIOPairsOneSelWU, company);
                                        plannedSel += CalcPlannedTime(listEmplSel, fromDate, toDate);

                                        checkDictionary.Add(wuTOSel.WorkingUnitID, passTypeAllIOPairsOneSelWU);
                                    }
                                    bool isValid = false;
                                    foreach (KeyValuePair<int, Dictionary<int, string>> checkpair in checkDictionary)
                                    {
                                        if (checkpair.Value.ContainsKey(int.Parse(lbPassTypes.Items[index].Value)) && checkpair.Value[int.Parse(lbPassTypes.Items[index].Value)] != "0.00")
                                        {
                                            isValid = true;

                                        }
                                        else
                                        { isValid = false; }

                                    }
                                    if (isValid || passTypeIOPairsRoot.ContainsKey(int.Parse(lbPassTypes.Items[index].Value)))
                                    {


                                        foreach (KeyValuePair<int, Dictionary<int, string>> checkpair in checkDictionary)
                                        {
                                            if (checkpair.Value.ContainsKey(int.Parse(lbPassTypes.Items[index].Value)))
                                            {
                                                ioPairsForSelPassType.Add(checkpair.Key, checkpair.Value[int.Parse(lbPassTypes.Items[index].Value)]);
                                            }
                                            else
                                            {
                                                ioPairsForSelPassType.Add(checkpair.Key, "0");
                                            }
                                        }
                                        //add data for chart, other ute from company
                                        if (passTypeIOPairsRoot.ContainsKey(int.Parse(lbPassTypes.Items[index].Value)))
                                        {
                                            ioPairsForSelPassType.Add(-1, passTypeIOPairsRoot[int.Parse(lbPassTypes.Items[index].Value)]);
                                        }
                                        else
                                        {
                                            ioPairsForSelPassType.Add(-1, "0");
                                        }
                                        //draw chart

                                        DrawStatisticsMultiple(ioPairsForSelPassType, fromDate, toDate, realisedSel, plannedSel, 2, lbPassTypes.Items[index].Text, listSelectedWU, null, numOfTables);
                                        numOfTables++;
                                    }
                                    else
                                    {
                                        lblError.Text += rm.GetString("noReportData", culture) + ": " + lbPassTypes.Items[index].Text + ". ";
                                    }


                                }
                            }
                            else
                            {
                                lblError.Text = rm.GetString("lblPassTypeSelection", culture);
                                writeLog(DateTime.Now, false);
                                return;
                            }

                        }
                        else if (lbPassTypes.GetSelectedIndices().Length == 1)
                        {

                            string wul = tbWorkshop.Attributes["id"];
                            int rootWU = company;
                            string selectedwu = "";
                            foreach (int ind in lboxUTE.GetSelectedIndices())
                            {
                                selectedwu += lboxUTE.Items[ind].Value + ",";

                            }
                            if (selectedwu.Length > 0)
                                selectedwu = selectedwu.Remove(selectedwu.LastIndexOf(","));
                            List<WorkingUnitTO> listSelectedWU = new List<WorkingUnitTO>();

                            foreach (string wu in wulIDs.Split(','))
                            {
                                WorkingUnitTO wuTOSel = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(int.Parse(wu));
                                listSelectedWU.Add(wuTOSel);
                            }
                            listSelectedWU = new WorkingUnit(Session[Constants.sessionConnection]).FindAllChildren(listSelectedWU);

                            Dictionary<int, WorkingUnitTO> dictionarySelected = new Dictionary<int, WorkingUnitTO>();
                            foreach (WorkingUnitTO selected in listSelectedWU)
                            {
                                if (!dictionarySelected.ContainsKey(selected.WorkingUnitID))
                                    dictionarySelected.Add(selected.WorkingUnitID, selected);

                            }
                            List<WorkingUnitTO> listWUWhole = new List<WorkingUnitTO>();
                            WorkingUnitTO wuTO = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(rootWU);
                            listWUWhole.Add(wuTO);
                            listWUWhole = new WorkingUnit(Session[Constants.sessionConnection]).FindAllChildren(listWUWhole);
                            List<WorkingUnitTO> listWU = new List<WorkingUnitTO>();
                            foreach (WorkingUnitTO root in listWUWhole)
                            {

                                if (!dictionarySelected.ContainsKey(root.WorkingUnitID))
                                {
                                    listWU.Add(root);
                                }

                            }
                            string wuids = "";
                            foreach (WorkingUnitTO wuS in listWU)
                            {

                                if (wul != wuS.WorkingUnitID.ToString())
                                    wuids = wuids + wuS.WorkingUnitID + ",";

                            }
                            if (wuids.Length > 0)
                                wuids = wuids.Substring(0, wuids.Length - 1);

                            List<EmployeeTO> listEmpl = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(wuids, -1, null, fromDate, toDate);

                            List<IOPairProcessedTO> IOPairsRoot = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmpl);
                            Dictionary<int, string> passTypeIOPairsRoot = TotalHourCalc(IOPairsRoot, passTypesDictionary);
                            if (IOPairsRoot.Count > 0)
                            {
                                CalcWorkOnHoliday(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.SelectedValue));
                                CalcNightWork(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.SelectedValue));
                                CalcRotaryWork(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.SelectedValue));
                            }
                            Dictionary<int, Dictionary<int, string>> checkDictionary = new Dictionary<int, Dictionary<int, string>>();
                            Dictionary<int, string> ioPairsForSelPassType = new Dictionary<int, string>();

                            double realisedSel = 0;
                            double plannedSel = 0;

                            foreach (string wu in wulIDs.Split(','))
                            {
                                List<WorkingUnitTO> listOne = new List<WorkingUnitTO>();

                                WorkingUnitTO wuTOSel = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(int.Parse(wu));

                                listOne.Add(wuTOSel);

                                listOne = new WorkingUnit(Session[Constants.sessionConnection]).FindAllChildren(listOne);

                                string wu1 = "";
                                foreach (WorkingUnitTO wuto in listOne)
                                {
                                    wu1 += wuto.WorkingUnitID + ",";
                                }
                                if (wu1.Length > 0)
                                    wu1 = wu1.Remove(wu1.LastIndexOf(','));
                                List<EmployeeTO> listEmplSel = new Employee(Session[Constants.sessionConnection]).SearchByWULoans(wu1, -1, null, fromDate, toDate);

                                List<IOPairProcessedTO> IOPairsSel = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmplSel);
                                Dictionary<int, string> passTypeAllIOPairsOneSelWU = TotalHourCalc(IOPairsSel, passTypesDictionary);
                                if (IOPairsSel.Count > 0)
                                {
                                    CalcWorkOnHoliday(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.SelectedValue));
                                    CalcNightWork(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.SelectedValue));
                                    CalcRotaryWork(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.SelectedValue));
                                }
                                realisedSel += CalcRealisedTime(listEmplSel, passTypesList, passTypeAllIOPairsOneSelWU, company);
                                plannedSel += CalcPlannedTime(listEmplSel, fromDate, toDate);

                                checkDictionary.Add(wuTOSel.WorkingUnitID, passTypeAllIOPairsOneSelWU);
                            }
                            bool isValid = false;
                            foreach (KeyValuePair<int, Dictionary<int, string>> checkpair in checkDictionary)
                            {

                                if (checkpair.Value.ContainsKey(int.Parse(lbPassTypes.SelectedValue)))
                                {
                                    if (checkpair.Value.ContainsKey(int.Parse(lbPassTypes.SelectedValue)) && checkpair.Value[int.Parse(lbPassTypes.SelectedValue)] != "0.00")
                                        isValid = true;

                                }
                                else
                                { isValid = false; }

                            }
                            if (isValid || passTypeIOPairsRoot.ContainsKey(int.Parse(lbPassTypes.SelectedValue)))
                            {

                                foreach (KeyValuePair<int, Dictionary<int, string>> checkpair in checkDictionary)
                                {
                                    if (checkpair.Value.ContainsKey(int.Parse(lbPassTypes.SelectedValue)))
                                    {
                                        ioPairsForSelPassType.Add(checkpair.Key, checkpair.Value[int.Parse(lbPassTypes.SelectedValue)]);
                                    }
                                    else
                                    {
                                        ioPairsForSelPassType.Add(checkpair.Key, "0");
                                    }
                                }
                                //add data for chart, other ute from company
                                if (passTypeIOPairsRoot.ContainsKey(int.Parse(lbPassTypes.SelectedValue)))
                                {
                                    ioPairsForSelPassType.Add(-1, passTypeIOPairsRoot[int.Parse(lbPassTypes.SelectedValue)]);
                                }
                                else
                                {
                                    ioPairsForSelPassType.Add(-1, "0");
                                }
                                //draw chart
                                DrawStatisticsMultiple(ioPairsForSelPassType, fromDate, toDate, realisedSel, plannedSel, 1, lbPassTypes.SelectedItem.Text, listSelectedWU, null, numOfTables);
                                numOfTables++;


                            }
                            else
                                lblError.Text = rm.GetString("noReportData", culture);
                        }
                        else
                        {
                            lblError.Text = rm.GetString("noSelectedPassType", culture);
                            writeLog(DateTime.Now, false);
                            return;
                        }
                    }
                    else
                    {
                        if (lbPassTypes.GetSelectedIndices().Length > 1)
                        {
                            if (lbPassTypes.GetSelectedIndices().Length <= 4)
                            {
                                foreach (int index in lbPassTypes.GetSelectedIndices())//if selected more than one pass_type
                                {
                                    string wul = tbOrg.Attributes["id"];
                                    int rootWU = company;
                                    string selectedwu = "";
                                    foreach (int ind in lboxUTE.GetSelectedIndices())
                                    {
                                        selectedwu += lboxUTE.Items[ind].Value + ",";

                                    }
                                    if (selectedwu.Length > 0)
                                        selectedwu = selectedwu.Remove(selectedwu.LastIndexOf(","));

                                    //FIND ALL SEL.ORG UNIT AND THEIR CHILDREN
                                    List<OrganizationalUnitTO> listSelectedWU = new List<OrganizationalUnitTO>();

                                    foreach (string wu in wulIDs.Split(','))
                                    {
                                        OrganizationalUnitTO wuTOSel = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(int.Parse(wu));
                                        listSelectedWU.Add(wuTOSel);
                                    }
                                    listSelectedWU = new OrganizationalUnit(Session[Constants.sessionConnection]).FindAllChildren(listSelectedWU);

                                    Dictionary<int, OrganizationalUnitTO> dictionarySelected = new Dictionary<int, OrganizationalUnitTO>();
                                    foreach (OrganizationalUnitTO selected in listSelectedWU)
                                    {
                                        if (!dictionarySelected.ContainsKey(selected.OrgUnitID))
                                            dictionarySelected.Add(selected.OrgUnitID, selected);

                                    }

                                    //FIND FOR ALL COMPANY, OR ROOT WU SELECTED, BUT WITHOUT SELECTED
                                    List<OrganizationalUnitTO> listOUWhole = new List<OrganizationalUnitTO>();
                                    OrganizationalUnitTO wuTO = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(rootWU);
                                    listOUWhole.Add(wuTO);
                                    listOUWhole = new OrganizationalUnit(Session[Constants.sessionConnection]).FindAllChildren(listOUWhole);
                                    List<OrganizationalUnitTO> listOU = new List<OrganizationalUnitTO>();
                                    foreach (OrganizationalUnitTO root in listOUWhole)
                                    {
                                        if (!dictionarySelected.ContainsKey(root.OrgUnitID))
                                        {
                                            listOU.Add(root);
                                        }

                                    }
                                    string wuids = "";
                                    foreach (OrganizationalUnitTO wuS in listOU)
                                    {

                                        if (wul != wuS.OrgUnitID.ToString())
                                            wuids = wuids + wuS.OrgUnitID + ",";

                                    }
                                    if (wuids.Length > 0)
                                        wuids = wuids.Substring(0, wuids.Length - 1);

                                    List<EmployeeTO> listEmpl = new Employee(Session[Constants.sessionConnection]).SearchByOU(wuids, -1, null, fromDate, toDate);

                                    List<IOPairProcessedTO> IOPairsRoot = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmpl);
                                    Dictionary<int, string> passTypeIOPairsRoot = TotalHourCalc(IOPairsRoot, passTypesDictionary);
                                    if (IOPairsRoot.Count > 0)
                                    {
                                        CalcWorkOnHoliday(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot,int.Parse(lbPassTypes.Items[index].Value));
                                        CalcNightWork(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.Items[index].Value));
                                        CalcRotaryWork(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.Items[index].Value));
                                    }
                                    Dictionary<int, Dictionary<int, string>> checkDictionary = new Dictionary<int, Dictionary<int, string>>();
                                    Dictionary<int, string> ioPairsForSelPassType = new Dictionary<int, string>();

                                    double realisedSel = 0;
                                    double plannedSel = 0;
                                    foreach (string wu in wulIDs.Split(','))
                                    {
                                        OrganizationalUnitTO ouTOSel = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(int.Parse(wu));

                                        List<OrganizationalUnitTO> listOne = new List<OrganizationalUnitTO>();
                                        listOne.Add(ouTOSel);

                                        listOne = new OrganizationalUnit(Session[Constants.sessionConnection]).FindAllChildren(listOne);
                                        wuids = "";
                                        foreach (OrganizationalUnitTO wuS in listOne)
                                        {
                                            wuids = wuids + wuS.OrgUnitID + ",";
                                        }
                                        if (wuids.Length > 0)
                                            wuids = wuids.Substring(0, wuids.Length - 1);

                                        List<EmployeeTO> listEmplSel = new Employee(Session[Constants.sessionConnection]).SearchByOU(wuids, -1, null, fromDate, toDate);

                                        List<IOPairProcessedTO> IOPairsSel = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmplSel);
                                        Dictionary<int, string> passTypeAllIOPairsOneSelWU = TotalHourCalc(IOPairsSel, passTypesDictionary);

                                        if (IOPairsSel.Count > 0)
                                        {
                                            CalcWorkOnHoliday(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.Items[index].Value));
                                            CalcNightWork(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.Items[index].Value));
                                            CalcRotaryWork(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.Items[index].Value));
                                        }
                                        realisedSel += CalcRealisedTime(listEmplSel, passTypesList, passTypeAllIOPairsOneSelWU, company);
                                        plannedSel += CalcPlannedTime(listEmplSel, fromDate, toDate);

                                        checkDictionary.Add(ouTOSel.OrgUnitID, passTypeAllIOPairsOneSelWU);
                                    }
                                    bool isValid = false;
                                    foreach (KeyValuePair<int, Dictionary<int, string>> checkpair in checkDictionary)
                                    {

                                        if (checkpair.Value.ContainsKey(int.Parse(lbPassTypes.Items[index].Value)) && checkpair.Value[int.Parse(lbPassTypes.Items[index].Value)] != "0.00")
                                            isValid = true;

                                        else
                                        { isValid = false; }

                                    }
                                    if (isValid || passTypeIOPairsRoot.ContainsKey(int.Parse(lbPassTypes.Items[index].Value)))
                                    {
                                        foreach (KeyValuePair<int, Dictionary<int, string>> checkpair in checkDictionary)
                                        {
                                            if (checkpair.Value.ContainsKey(int.Parse(lbPassTypes.Items[index].Value)))
                                            {
                                                ioPairsForSelPassType.Add(checkpair.Key, checkpair.Value[int.Parse(lbPassTypes.Items[index].Value)]);
                                            }
                                            else
                                            {
                                                ioPairsForSelPassType.Add(checkpair.Key, "0");
                                            }
                                        }
                                        //add data for chart, other ute from company
                                        if (passTypeIOPairsRoot.ContainsKey(int.Parse(lbPassTypes.Items[index].Value)))
                                        {
                                            ioPairsForSelPassType.Add(-1, passTypeIOPairsRoot[int.Parse(lbPassTypes.Items[index].Value)]);
                                        }
                                        else
                                        {
                                            ioPairsForSelPassType.Add(-1, "0");
                                        }
                                        //draw chart

                                        DrawStatisticsMultiple(ioPairsForSelPassType, fromDate, toDate, realisedSel, plannedSel, 2, lbPassTypes.Items[index].Text, null, listSelectedWU, numOfTables);
                                        numOfTables++;
                                    }
                                    else
                                    {
                                        lblError.Text += rm.GetString("noReportData", culture) + ": " + lbPassTypes.Items[index].Text + ". ";
                                    }

                                }
                            }
                            else
                            {
                                lblError.Text = rm.GetString("lblPassTypeSelection", culture);
                                writeLog(DateTime.Now, false);
                                return;
                            }
                        }
                        else if (lbPassTypes.GetSelectedIndices().Length == 1)
                        {
                            string wul = tbOrg.Attributes["id"];
                            int rootWU = company;
                            string selectedwu = "";
                            foreach (int ind in lboxUTE.GetSelectedIndices())
                            {
                                selectedwu += lboxUTE.Items[ind].Value + ",";

                            }
                            if (selectedwu.Length > 0)
                                selectedwu = selectedwu.Remove(selectedwu.LastIndexOf(","));

                            //FIND ALL SEL.ORG UNIT AND THEIR CHILDREN
                            List<OrganizationalUnitTO> listSelectedWU = new List<OrganizationalUnitTO>();

                            foreach (string wu in wulIDs.Split(','))
                            {
                                OrganizationalUnitTO wuTOSel = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(int.Parse(wu));
                                listSelectedWU.Add(wuTOSel);
                            }
                            listSelectedWU = new OrganizationalUnit(Session[Constants.sessionConnection]).FindAllChildren(listSelectedWU);

                            Dictionary<int, OrganizationalUnitTO> dictionarySelected = new Dictionary<int, OrganizationalUnitTO>();
                            foreach (OrganizationalUnitTO selected in listSelectedWU)
                            {
                                if (!dictionarySelected.ContainsKey(selected.OrgUnitID))
                                    dictionarySelected.Add(selected.OrgUnitID, selected);

                            }

                            //FIND FOR ALL COMPANY, OR ROOT WU SELECTED, BUT WITHOUT SELECTED
                            List<OrganizationalUnitTO> listOUWhole = new List<OrganizationalUnitTO>();
                            OrganizationalUnitTO wuTO = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(rootWU);
                            listOUWhole.Add(wuTO);
                            listOUWhole = new OrganizationalUnit(Session[Constants.sessionConnection]).FindAllChildren(listOUWhole);
                            List<OrganizationalUnitTO> listOU = new List<OrganizationalUnitTO>();
                            foreach (OrganizationalUnitTO root in listOUWhole)
                            {
                                if (!dictionarySelected.ContainsKey(root.OrgUnitID))
                                {
                                    listOU.Add(root);
                                }

                            }
                            string wuids = "";
                            foreach (OrganizationalUnitTO wuS in listOU)
                            {

                                if (wul != wuS.OrgUnitID.ToString())
                                    wuids = wuids + wuS.OrgUnitID + ",";

                            }
                            if (wuids.Length > 0)
                                wuids = wuids.Substring(0, wuids.Length - 1);


                            List<EmployeeTO> listEmpl = new Employee(Session[Constants.sessionConnection]).SearchByOU(wuids, -1, null, fromDate, toDate);

                            List<IOPairProcessedTO> IOPairsRoot = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmpl);
                            Dictionary<int, string> passTypeIOPairsRoot = TotalHourCalc(IOPairsRoot, passTypesDictionary);
                            if (IOPairsRoot.Count > 0)
                            {
                                CalcWorkOnHoliday(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.SelectedValue));
                                CalcNightWork(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.SelectedValue));
                                CalcRotaryWork(listEmpl, company, IOPairsRoot, fromDate, toDate, passTypeIOPairsRoot, int.Parse(lbPassTypes.SelectedValue));
                            }
                            Dictionary<int, Dictionary<int, string>> checkDictionary = new Dictionary<int, Dictionary<int, string>>();
                            Dictionary<int, string> ioPairsForSelPassType = new Dictionary<int, string>();

                            double realisedSel = 0;
                            double plannedSel = 0;

                            foreach (string wu in wulIDs.Split(','))
                            {

                                OrganizationalUnitTO ouTOSel = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(int.Parse(wu));

                                List<OrganizationalUnitTO> listOne = new List<OrganizationalUnitTO>();
                                listOne.Add(ouTOSel);

                                listOne = new OrganizationalUnit(Session[Constants.sessionConnection]).FindAllChildren(listOne);
                                wuids = "";
                                foreach (OrganizationalUnitTO wuS in listOne)
                                {
                                    wuids = wuids + wuS.OrgUnitID + ",";
                                }
                                if (wuids.Length > 0)
                                    wuids = wuids.Substring(0, wuids.Length - 1);

                                List<EmployeeTO> listEmplSel = new Employee(Session[Constants.sessionConnection]).SearchByOU(wuids, -1, null, fromDate, toDate);

                                List<IOPairProcessedTO> IOPairsSel = FindIOPairsForEmployeeSingle(fromDate, toDate, company, passTypesDictionary, listEmplSel);
                                Dictionary<int, string> passTypeAllIOPairsOneSelWU = TotalHourCalc(IOPairsSel, passTypesDictionary);

                                if (IOPairsSel.Count > 0)
                                {
                                    CalcWorkOnHoliday(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.SelectedValue));
                                    CalcNightWork(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.SelectedValue));
                                    CalcRotaryWork(listEmplSel, company, IOPairsSel, fromDate, toDate, passTypeAllIOPairsOneSelWU, int.Parse(lbPassTypes.SelectedValue));
                                }
                                realisedSel += CalcRealisedTime(listEmplSel, passTypesList, passTypeAllIOPairsOneSelWU, company);
                                plannedSel += CalcPlannedTime(listEmplSel, fromDate, toDate);

                                checkDictionary.Add(ouTOSel.OrgUnitID, passTypeAllIOPairsOneSelWU);
                            }
                            bool isValid = false;
                            foreach (KeyValuePair<int, Dictionary<int, string>> checkpair in checkDictionary)
                            {
                                if (checkpair.Value.ContainsKey(int.Parse(lbPassTypes.SelectedValue)) && checkpair.Value[int.Parse(lbPassTypes.SelectedValue)] != "0.00")
                                {
                                    isValid = true;
                                }
                                else
                                { isValid = false; }

                            }
                            if (isValid || passTypeIOPairsRoot.ContainsKey(int.Parse(lbPassTypes.SelectedValue)))
                            {

                                foreach (KeyValuePair<int, Dictionary<int, string>> checkpair in checkDictionary)
                                {
                                    if (checkpair.Value.ContainsKey(int.Parse(lbPassTypes.SelectedValue)))
                                    {
                                        ioPairsForSelPassType.Add(checkpair.Key, checkpair.Value[int.Parse(lbPassTypes.SelectedValue)]);
                                    }
                                    else
                                    {
                                        ioPairsForSelPassType.Add(checkpair.Key, "0");
                                    }
                                }
                                //add data for chart, other ute from company
                                if (passTypeIOPairsRoot.ContainsKey(int.Parse(lbPassTypes.SelectedValue)))
                                {
                                    ioPairsForSelPassType.Add(-1, passTypeIOPairsRoot[int.Parse(lbPassTypes.SelectedValue)]);
                                }
                                else
                                {
                                    ioPairsForSelPassType.Add(-1, "0");
                                }
                                //draw chart
                                DrawStatisticsMultiple(ioPairsForSelPassType, fromDate, toDate, realisedSel, plannedSel, 1, lbPassTypes.SelectedItem.Text, null, listSelectedWU, numOfTables);
                                numOfTables++;
                            }
                            else
                                lblError.Text = rm.GetString("noReportData", culture);

                        }
                        else
                        {
                            lblError.Text = rm.GetString("noSelectedPassType", culture);
                            writeLog(DateTime.Now, false);
                            return;
                        }

                    }

                }

                Session["TLWUStatistical.numOfCharts"] = numOfTables;

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLWUStatisticalReportsPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnReport_Click(Object sender, EventArgs e)
        {
            try
            {
                if (rbSingle.Checked)
                    Session["TLWUStatistical.single"] = 1;
                else
                    Session["TLWUStatistical.single"] = 2;

                if (lboxUTE.GetSelectedIndices().Length > 0)
                    Session["TLWUStatistical.selectedUTE"] = lboxUTE.GetSelectedIndices();

                if (lbPassTypes.GetSelectedIndices().Length > 0)
                    Session["TLWUStatistical.selectedPassType"] = lbPassTypes.GetSelectedIndices();

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLClockDataPage).Assembly);
                if (Session["TLWUStatistical.dataTable1"] == null && Session["TLWUStatistical.dataTable2"] == null && Session["TLWUStatistical.dataTable3"] == null && Session["TLWUStatistical.dataTable4"] == null)
                {
                    lblError.Text = rm.GetString("noReportData", culture);
                    writeLog(DateTime.Now, false);
                    return;
                }
                if (Menu1.SelectedItem.Value.Equals("0"))
                {
                    int wuID = -1;
                    if (tbWorkshop.Attributes["id"] != null)
                        if (!int.TryParse(tbWorkshop.Attributes["id"], out wuID))
                            wuID = -1;
                    Session[Constants.sessionWU] = wuID;
                    Session[Constants.sessionOU] = null;
                }
                else
                {
                    int ouID = -1;
                    if (tbOrg.Attributes["id"] != null)
                        if (!int.TryParse(tbOrg.Attributes["id"], out ouID))
                            ouID = -1;
                    Session[Constants.sessionWU] = null;
                    Session[Constants.sessionOU] = ouID;
                }
                lblError.Text = "";
                DateTime fromDate = CommonWeb.Misc.createDate(tbFromDate.Text.Trim());
                DateTime toDate = CommonWeb.Misc.createDate(tbToDate.Text.Trim());
                Session["TLWUStatistical.fromDate"] = fromDate.ToString("dd.MM.yyyy");
                Session["TLWUStatistical.toDate"] = toDate.ToString("dd.MM.yyyy");

                if (rbSingle.Checked)
                    Session["TLWUStatistical.report"] = 1;
                else
                    Session["TLWUStatistical.report"] = 2;
                string reportURL = "";
                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                    reportURL = "/ACTAWeb/ReportsWeb/sr/TLWUStatisticalReport_sr.aspx";
                else
                    reportURL = "/ACTAWeb/ReportsWeb/en/TLWUStatisticalReport_en.aspx";
                Response.Redirect("/ACTAWeb/ReportsWeb/ReportPage.aspx?Back=/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx&Report=" + reportURL.Trim(), false);

                writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLWUStatisticalReportsPage.btnReport_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void DrawStatistics(Dictionary<int, string> pairsStatistics, List<EmployeeTO> listEmpl, DateTime fromDate, DateTime toDate, double realisedTime, double plannedTime, List<PassTypeTO> passTypesAll, int numChart, string title, int numOfTables)
        {
            try
            {
                //string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                //DebugLog log = new DebugLog(logFilePath);
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WUStatisticalReportsPage).Assembly);

                numOfTables++;
                string values = "";
                string texts = "";
                string links = "";
                string sliceDisplacments = "";
                string colors = "";
                DataTable table = new DataTable();
                table.Columns.Add("pass_type", typeof(System.String));
                table.Columns.Add("color", typeof(System.String));
                table.Columns.Add("hours", typeof(System.Double));
                table.Columns.Add("perRealised", typeof(System.String));
                table.Columns.Add("perPlanned", typeof(System.String));
                Dictionary<int, string> colorDictionary = new Dictionary<int, string>();
                Dictionary<int, string> ptDesc = new Dictionary<int, string>();

                foreach (PassTypeTO pt in passTypesAll)
                {
                    if (!ptDesc.ContainsKey(pt.PassTypeID))
                    {
                        if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                        {
                            ptDesc.Add(pt.PassTypeID, pt.DescriptionAndID);
                        }
                        else
                        {
                            ptDesc.Add(pt.PassTypeID, pt.DescriptionAltAndID);
                        }
                    }
                    if (pt.SegmentColor == "")
                    {
                        colorDictionary.Add(pt.PassTypeID, "white");
                    }
                    else
                    {
                        colorDictionary.Add(pt.PassTypeID, pt.SegmentColor);
                    }
                }

                if (pairsStatistics.Count > 0)
                {
                    //sum of all hours
                    double count = 0;
                    foreach (int ptID in pairsStatistics.Keys)
                    {
                        count += (double.Parse(pairsStatistics[ptID]));
                    }
                    //calculating for every pass_type hours and set to datarow
                    foreach (int ptID in pairsStatistics.Keys)
                    {
                        string val = Math.Round(((double.Parse(pairsStatistics[ptID]) * 100) / count), 2).ToString();

                        values += val + ",";
                        texts += ptDesc[ptID].Trim() + "|";

                        links += "http://www.google.com,";
                        sliceDisplacments += "0.05,";
                        colors += colorDictionary[ptID] + ",";
                        DataRow row = table.NewRow();

                        row["pass_type"] = ptDesc[ptID].Trim();

                        row["color"] = "";
                        row["hours"] = pairsStatistics[ptID].ToString();
                        row["perRealised"] = Math.Round(((double.Parse(pairsStatistics[ptID]) * 100) / realisedTime), 2).ToString();
                        //if (row["perRealised"].ToString().Contains('.'))
                        //{
                        //    row["perRealised"] = row["perRealised"].ToString().Remove(row["perRealised"].ToString().IndexOf('.') + 3);
                        //}
                        row["perPlanned"] = Math.Round(((double.Parse(pairsStatistics[ptID]) * 100) / plannedTime), 2).ToString();
                        //if (row["perPlanned"].ToString().Contains('.'))
                        //{
                        //    row["perPlanned"] = row["perPlanned"].ToString().Remove(row["perPlanned"].ToString().IndexOf('.') + 3);
                        //}
                        table.Rows.Add(row);
                        table.AcceptChanges();
                    }

                    foreach (PassTypeTO pt in passTypesAll)
                    {
                        DataRow row = table.NewRow();
                        if (!pairsStatistics.ContainsKey(pt.PassTypeID))
                        {
                            row["pass_type"] = ptDesc[pt.PassTypeID].Trim();
                            row["color"] = "";
                            row["hours"] = "0";
                            row["perRealised"] = "0.00";
                            row["perPlanned"] = "0.00";
                            colors += colorDictionary[pt.PassTypeID] + ",";
                            table.Rows.Add(row);
                            table.AcceptChanges();
                        }
                    }

                    int width = 0;
                    int height = 0;
                    if (numChart == 1)
                    {
                        width = 800;
                        height = 220;
                        Session["TLWUStatistical.dataTable1"] = table;
                        Session["TLWUStatistical.realised1"] = realisedTime;
                        Session["TLWUStatistical.planned1"] = plannedTime;
                        Session["TLWUStatistical.title1"] = title;
                    }
                    else
                    {
                        width = 500;
                        height = 120;
                        Session["TLWUStatistical.dataTable" + numOfTables] = table;
                        Session["TLWUStatistical.realised" + numOfTables] = realisedTime;
                        Session["TLWUStatistical.planned" + numOfTables] = plannedTime;
                        Session["TLWUStatistical.title" + numOfTables] = title;
                    }
                    PieChart3D pieChart = FillChart(values, texts, links, sliceDisplacments, colors, width, height);

                    populateCtrlHolder(realisedTime, plannedTime, table, colors, pieChart, numChart, title);

                }
                else
                    lblError.Text = rm.GetString("noMonthPairs", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool isPersonalHoliday(IOPairProcessedTO pair, WorkTimeIntervalTO pairInterval)
        {
            try
            {
                string holidayType = "";
                DateTime pairDate = pair.IOPairDate.Date;

                // if pair is from second interval of night shift, it belongs to previous day
                //WorkTimeIntervalTO pairInterval = getPairInterval(pair, dayPairs);
                if (pairInterval.TimeSchemaID != -1 && Common.Misc.isThirdShiftEndInterval(pairInterval))
                    pairDate = pairDate.AddDays(-1);

                // check if date is personal holiday, no transfering holidays for personal holidays
                // get employee personal holiday category
                EmployeeAsco4 emplAsco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                emplAsco.EmplAsco4TO.EmployeeID = pair.EmployeeID;
                List<EmployeeAsco4TO> ascoList = emplAsco.Search();

                if (ascoList.Count == 1)
                {
                    holidayType = ascoList[0].NVarcharValue1.Trim();

                    if (!holidayType.Trim().Equals(""))
                    {
                        // if category is IV, find holiday date and check if pair date is holiday
                        if (holidayType.Trim().Equals(Constants.holidayTypeIV.Trim()))
                        {
                            DateTime holDate = ascoList[0].DatetimeValue1.Date;

                            if (holDate.Month == pairDate.Month && holDate.Day == pairDate.Day)
                                return true;
                        }
                        else
                        {
                            // if category is I, II or III, check if pair date is personal holiday
                            HolidaysExtended holExtended = new HolidaysExtended(Session[Constants.sessionConnection]);
                            holExtended.HolTO.Type = Constants.personalHoliday.Trim();
                            holExtended.HolTO.Category = holidayType.Trim();

                            if (holExtended.Search(pairDate, pairDate).Count > 0)
                                return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isNationalHoliday(IOPairProcessedTO pair, string EmplTimeSchema, WorkTimeIntervalTO pairInterval)
        {
            try
            {
                DateTime pairDate = pair.IOPairDate.Date;

                // if pair is from second interval of night shift, it belongs to previous day
                //WorkTimeIntervalTO pairInterval = getPairInterval(pair, dayPairs);
                if (pairInterval.TimeSchemaID != -1 && Common.Misc.isThirdShiftEndInterval(pairInterval))
                    pairDate = pairDate.AddDays(-1);

                // check if date is national holiday, national holidays are transferd from Sunday to first working day
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundaysDays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(pairDate, pairDate, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, Session[Constants.sessionConnection], nationalTransferableHolidays);

                if (EmplTimeSchema.ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()))
                {
                    if (nationalHolidaysDays.Contains(pairDate.Date))
                        return true;
                }
                else if (nationalHolidaysDays.Contains(pairDate.Date) || nationalHolidaysSundays.Contains(pairDate.Date))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DrawStatisticsMultiple(Dictionary<int, string> pairsStatistics, DateTime fromDate, DateTime toDate, double realisedTime, double plannedTime, int numCharts, string title, List<WorkingUnitTO> selWUnits, List<OrganizationalUnitTO> selOUnits, int numOfTables)
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WUStatisticalReportsPage).Assembly);
                numOfTables++;
                string values = "";
                string texts = "";
                string links = "";
                string sliceDisplacments = "";
                string colors = "";
                DataTable table = new DataTable();
                table.Columns.Add("pass_type", typeof(System.String));
                table.Columns.Add("color", typeof(System.String));
                table.Columns.Add("hours", typeof(System.Double));
                table.Columns.Add("perRealised", typeof(System.String));
                table.Columns.Add("perPlanned", typeof(System.String));
                List<string> colorDictionary = new List<string>();
                Dictionary<int, string> ptDesc = new Dictionary<int, string>();

                colorDictionary.AddRange(new string[] { "lime", "SlateBlue", "cyan", "purple", "hotpink", "green", "darkblue", "gray", "purple", "red", "Teal" });
                colorDictionary.AddRange(new string[] { "olive", "Salmon", "blue", "white", "brown", "yellow", "orange", "plum", "lightblue", "khaki", "Tomato" });
                colorDictionary.AddRange(new string[] { "gold", "Turquoise", "DarkSeaGreen", "GreenYellow", "LightSeaGreen", "MidnightBlue", "MistyRose", "Navy", "Peru", "Sienna" });


                if (selWUnits != null)
                {
                    foreach (WorkingUnitTO wuPair in selWUnits)
                    {
                        ptDesc.Add(wuPair.WorkingUnitID, wuPair.Name);
                    }
                }
                else if (selOUnits != null)
                {
                    foreach (OrganizationalUnitTO ouPair in selOUnits)
                    {
                        ptDesc.Add(ouPair.OrgUnitID, ouPair.Name);
                    }
                }
                else
                    return;

                ptDesc.Add(-1, rm.GetString("chrtOthers", culture));

                Dictionary<int, string> color = new Dictionary<int, string>();

                Random rn = new Random();
                int ind = rn.Next(0, 32);
                foreach (KeyValuePair<int, string> ptdescrip in ptDesc)
                {
                    while (color.ContainsValue(colorDictionary[ind]))
                    {
                        Random rand = new Random();
                        ind = rand.Next(0, 32);
                    }
                    color.Add(ptdescrip.Key, colorDictionary[ind]);
                }

                if (pairsStatistics.Count > 0)
                {
                    //sum of all hours
                    double count = 0;
                    foreach (int ptID in pairsStatistics.Keys)
                    {
                        count += (double.Parse(pairsStatistics[ptID]));
                    }
                    //calculating for every pass_type hours and set to datarow
                    foreach (int ptID in pairsStatistics.Keys)
                    {

                        string val = Math.Round(((double.Parse(pairsStatistics[ptID]) * 100) / count), 2).ToString();
                        if (val == "NaN")
                            val = "0.00";


                        values += val + ",";

                        texts += ptDesc[ptID].Trim() + "|";

                        links += "http://www.google.com,";
                        sliceDisplacments += "0.05,";

                        colors += color[ptID] + ",";
                        //  colNum++;
                        DataRow row = table.NewRow();

                        row["pass_type"] = ptDesc[ptID].Trim();

                        row["color"] = "";
                        row["hours"] = pairsStatistics[ptID].ToString();
                        row["perRealised"] = Math.Round(((double.Parse(pairsStatistics[ptID]) * 100) / realisedTime), 2);

                        row["perPlanned"] = Math.Round(((double.Parse(pairsStatistics[ptID]) * 100) / plannedTime), 2);

                        table.Rows.Add(row);
                        table.AcceptChanges();

                    }
                    int width = 0;
                    int height = 0;

                    if (numCharts == 1)
                    {
                        width = 800;
                        height = 220;
                        Session["TLWUStatistical.dataTable1"] = table;
                        Session["TLWUStatistical.realised1"] = realisedTime;
                        Session["TLWUStatistical.planned1"] = plannedTime;
                        Session["TLWUStatistical.title1"] = title;
                    }
                    else
                    {
                        width = 550;
                        height = 120;
                        Session["TLWUStatistical.dataTable" + numOfTables] = table;
                        Session["TLWUStatistical.realised" + numOfTables] = realisedTime;
                        Session["TLWUStatistical.planned" + numOfTables] = plannedTime;
                        Session["TLWUStatistical.title" + numOfTables] = title;
                    }
                    PieChart3D pieChart = FillChart(values, texts, links, sliceDisplacments, colors, width, height);
                    populateCtrlHolder(realisedTime, plannedTime, table, colors, pieChart, numCharts, title);
                }
                else
                    lblError.Text = rm.GetString("noMonthPairs", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double CalcPlannedTime(List<EmployeeTO> listEmpl, DateTime fromDate, DateTime toDate)
        {
            try
            {
                double plannedTime = 0;
                string employees = "";
                List<WorkTimeIntervalTO> intervalsEmpl = new List<WorkTimeIntervalTO>();
                foreach (EmployeeTO empl in listEmpl)
                {
                    employees += empl.EmployeeID + ",";

                    List<EmployeeTimeScheduleTO> timeScheduleList = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedules(empl.EmployeeID.ToString(), fromDate, toDate);
                    string schemaID = "";
                    foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
                    {

                        schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                    }
                    if (!schemaID.Equals(""))
                    {
                        schemaID = schemaID.Substring(0, schemaID.Length - 2);
                    }
                    List<WorkTimeSchemaTO> timeSchema = new TimeSchema(Session[Constants.sessionConnection]).Search(schemaID);

                    for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
                    {
                        if (timeScheduleList.Count > 0)
                        {
                            intervalsEmpl = this.getTimeSchemaInterval(empl.EmployeeID, date, timeScheduleList, timeSchema);//geting time intervals list for specified employee and date

                            foreach (WorkTimeIntervalTO tsInterval in intervalsEmpl)
                            {
                                TimeSpan duration = (new DateTime(tsInterval.EndTime.TimeOfDay.Ticks - tsInterval.StartTime.TimeOfDay.Ticks)).TimeOfDay;
                                plannedTime += (double)duration.Hours;
                            }
                        }
                    }
                }
                return plannedTime;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void CalcNightWork(List<EmployeeTO> listEmpl, int company,
            List<IOPairProcessedTO> IOPairsList, DateTime fromDate1, DateTime toDate1, Dictionary<int, string> passesForIOPairs, int pass_selected)
        {
            try
            {
                string nightWork = "";
                TimeSpan nightSpan = new TimeSpan();
                int pass_type_id = -1;

                Common.Rule rule2 = new Common.Rule(Session[Constants.sessionConnection]);
                rule2.RuleTO.WorkingUnitID = company;

                rule2.RuleTO.RuleType = Constants.RuleNightWork;

                List<RuleTO> rules2 = rule2.Search();

                if (rules2.Count > 0)
                {
                    pass_type_id = rules2[0].RuleValue;
                }
                if (pass_selected == -1 || pass_selected == pass_type_id)
                {
                    foreach (EmployeeTO empl in listEmpl)
                    {
                        Common.Rule rule1 = new Common.Rule(Session[Constants.sessionConnection]);
                        rule1.RuleTO.WorkingUnitID = company;
                        rule1.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                        rule1.RuleTO.RuleType = Constants.RuleNightWork;

                        List<RuleTO> rules1 = rule1.Search();

                        if (rules1.Count == 1)
                        {
                            pass_type_id = rules1[0].RuleValue;

                            DateTime fromDate = rules1[0].RuleDateTime1;
                            DateTime toDate = rules1[0].RuleDateTime2;
                            List<string> rulesList = new List<string>();
                            foreach (string ruleName in Constants.effectiveWorkWageTypes())
                            {
                                Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                                rule.RuleTO.WorkingUnitID = company;
                                rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                                rule.RuleTO.RuleType = ruleName;
                                List<RuleTO> rules = rule.Search();
                                if (rules.Count == 1)
                                {

                                    rulesList.Add(rules[0].RuleValue.ToString());
                                }
                            }
                            Dictionary<int, TimeSpan> typesCounter = new Dictionary<int, TimeSpan>();


                            foreach (IOPairProcessedTO iopair in IOPairsList)
                            {

                                if (iopair.EmployeeID == empl.EmployeeID)
                                {
                                    if (rulesList.Contains(iopair.PassTypeID.ToString()))
                                    {
                                        TimeSpan pairDuration = new TimeSpan();
                                        if (!iopair.StartTime.Date.Equals(new DateTime()) && !iopair.EndTime.Date.Equals(new DateTime()))
                                        {

                                            if (iopair.EndTime.TimeOfDay > fromDate.TimeOfDay)
                                                pairDuration = iopair.EndTime.TimeOfDay - fromDate.TimeOfDay;
                                            else if (iopair.StartTime.TimeOfDay < toDate.TimeOfDay)
                                                pairDuration = toDate.TimeOfDay - iopair.StartTime.TimeOfDay;
                                            else
                                                break;
                                            //if shift is until 23:59 add one minute
                                            if (iopair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));

                                            nightSpan = nightSpan.Add(pairDuration);



                                        }
                                    }
                                }

                            }
                        }
                    }
                  
                    decimal hours = decimal.Parse((nightSpan.TotalHours).ToString());
                    //string minutes = ((((int)nightSpan.TotalMinutes - (int)nightSpan.TotalHours * 60) * 100) / 60).ToString();
                    //if (minutes == "0")
                    //{
                    //    nightWork = hours + "." + "00";
                    //}
                    //else if (minutes.Length == 1)
                    //{
                    //    nightWork = hours + "." + minutes + "0";
                    //}
                    //else
                    //{
                    //    nightWork = hours + "." + minutes;
                    //}
                    if (hours != 0)
                    {
                        nightWork = Math.Round(hours, 2).ToString();
                        if (passesForIOPairs.ContainsKey(pass_type_id))
                            passesForIOPairs[pass_type_id] = nightWork;
                        else
                            passesForIOPairs.Add(pass_type_id, nightWork);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalcRotaryWork(List<EmployeeTO> listEmpl, int company,
        List<IOPairProcessedTO> IOPairsList, DateTime fromDate1, DateTime toDate1, Dictionary<int, string> passesForIOPairs, int pass_selected)
        {
            try
            {
                int pass_type_id = -1;
                //check if selected pass is rotary or is rbsingle.checked
                Common.Rule rule2 = new Common.Rule(Session[Constants.sessionConnection]);
                rule2.RuleTO.WorkingUnitID = company;
                rule2.RuleTO.RuleType = Constants.RuleComanyRotaryShift;

                List<RuleTO> rules2 = rule2.Search();

                if (rules2.Count > 0)
                {
                    pass_type_id = rules2[0].RuleValue;
                }

                if (pass_selected == -1 || pass_selected == pass_type_id)
                {
                    string nightWork = "";
                    TimeSpan nightSpan = new TimeSpan();
                    List<string> rulesList = new List<string>();
                    foreach (EmployeeTO empl in listEmpl)
                    {
                        foreach (string ruleName in Constants.effectiveWorkRotaryWageTypes())
                        {
                            Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                            rule.RuleTO.WorkingUnitID = company;
                            rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                            rule.RuleTO.RuleType = ruleName;
                            List<RuleTO> rules = rule.Search();
                            if (rules.Count == 1)
                            {

                                rulesList.Add(rules[0].RuleValue.ToString());
                            }
                        }

                        Dictionary<int, TimeSpan> typesCounter = new Dictionary<int, TimeSpan>();

                        List<EmployeeTimeScheduleTO> timeScheduleList = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedules(empl.EmployeeID.ToString(), fromDate1.Date, toDate1.Date);
                        string schemaID = "";
                        foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
                        {
                            schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                        }
                        if (!schemaID.Equals(""))
                        {
                            schemaID = schemaID.Substring(0, schemaID.Length - 2);
                        }

                        List<WorkTimeSchemaTO> wSchema = new TimeSchema(Session[Constants.sessionConnection]).Search(schemaID);
                        foreach (IOPairProcessedTO iopair in IOPairsList)
                        {
                            if (iopair.EmployeeID == empl.EmployeeID)
                            {
                                if (rulesList.Contains(iopair.PassTypeID.ToString()))
                                {
                                    List<WorkTimeIntervalTO> timeSchemaIntervalList = getTimeSchemaInterval(empl.EmployeeID, iopair.IOPairDate.Date, timeScheduleList, wSchema);
                                    int industrial = -1;

                                    foreach (WorkTimeSchemaTO wsch in wSchema)
                                    {
                                        if (wsch.TimeSchemaID == timeSchemaIntervalList[0].TimeSchemaID)
                                        {
                                            industrial = wsch.Turnus;
                                        }
                                    }
                                    if (industrial == Constants.yesInt)
                                    {
                                        TimeSpan pairDuration = new TimeSpan();
                                        if (!iopair.StartTime.Date.Equals(new DateTime()) && !iopair.EndTime.Date.Equals(new DateTime()))
                                        {
                                            pairDuration = iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay;
                                            //if shift is until 23:59 add one minute
                                            if (iopair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));

                                            nightSpan = nightSpan.Add(pairDuration);

                                        }

                                    }
                                }
                            }
                        }
                    }
                    decimal hours = decimal.Parse((nightSpan.TotalHours).ToString());
                    //string minutes = ((((int)nightSpan.TotalMinutes - (int)nightSpan.TotalHours * 60) * 100) / 60).ToString();
                    //if (minutes == "0")
                    //{
                    //    nightWork = hours + "." + "00";
                    //}
                    //else if (minutes.Length == 1)
                    //{
                    //    nightWork = hours + "." + minutes + "0";
                    //}
                    //else
                    //{
                    //    nightWork = hours + "." + minutes;
                    //} 
                    nightWork = Math.Round(hours, 2).ToString();
                    if (nightWork != "0.00")
                    {
                        if (passesForIOPairs.ContainsKey(pass_type_id))
                            passesForIOPairs[pass_type_id] = nightWork;
                        else
                            passesForIOPairs.Add(pass_type_id, nightWork);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CalcWorkOnHoliday(List<EmployeeTO> listEmpl, int company,
    List<IOPairProcessedTO> IOPairsList, DateTime fromDate1, DateTime toDate1, Dictionary<int, string> passesForIOPairs, int pass_selected)
        {
            try
            {
                int pass_type_id = -1;
                //check if selected pass is rotary or is rbsingle.checked
                Common.Rule rule2 = new Common.Rule(Session[Constants.sessionConnection]);
                rule2.RuleTO.WorkingUnitID = company;
                rule2.RuleTO.RuleType = Constants.RuleWorkOnHolidayPassType;

                List<RuleTO> rules2 = rule2.Search();

                if (rules2.Count > 0)
                {
                    pass_type_id = rules2[0].RuleValue;
                }

                if (pass_selected == -1 || pass_selected == pass_type_id)
                {
                    string nightWork = "";
                    TimeSpan nightSpan = new TimeSpan();
                    List<string> rulesList = new List<string>();
                    foreach (EmployeeTO empl in listEmpl)
                    {
                        foreach (string ruleName in Constants.effectiveWorkWageTypes())
                        {
                            Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                            rule.RuleTO.WorkingUnitID = company;
                            rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                            rule.RuleTO.RuleType = ruleName;
                            List<RuleTO> rules = rule.Search();
                            if (rules.Count == 1)
                            {
                                rulesList.Add(rules[0].RuleValue.ToString());
                            }
                        }

                        Dictionary<int, TimeSpan> typesCounter = new Dictionary<int, TimeSpan>();

                        List<EmployeeTimeScheduleTO> timeScheduleList = new EmployeesTimeSchedule(Session[Constants.sessionConnection]).SearchEmployeesSchedules(empl.EmployeeID.ToString(), fromDate1.Date, toDate1.Date);
                        string schemaID = "";
                        foreach (EmployeeTimeScheduleTO employeeTimeSchedule in timeScheduleList)
                        {
                            schemaID += employeeTimeSchedule.TimeSchemaID.ToString() + ", ";
                        }
                        if (!schemaID.Equals(""))
                        {
                            schemaID = schemaID.Substring(0, schemaID.Length - 2);
                        }

                        List<WorkTimeSchemaTO> wSchema = new TimeSchema(Session[Constants.sessionConnection]).Search(schemaID);
                        foreach (IOPairProcessedTO iopair in IOPairsList)
                        {
                            if (iopair.EmployeeID == empl.EmployeeID)
                            {
                                if (rulesList.Contains(iopair.PassTypeID.ToString()))
                                {
                                    List<WorkTimeIntervalTO> timeSchemaIntervalList = getTimeSchemaInterval(empl.EmployeeID, iopair.IOPairDate.Date, timeScheduleList, wSchema);
                                    string industrial = "";

                                    foreach (WorkTimeSchemaTO wsch in wSchema)
                                    {
                                        if (wsch.TimeSchemaID == timeSchemaIntervalList[0].TimeSchemaID)
                                        {
                                            industrial = wsch.Type;
                                        }
                                    }
                                    WorkTimeIntervalTO wtime = timeSchemaIntervalList[0];
                                    if (isNationalHoliday(iopair, industrial, wtime) || isPersonalHoliday(iopair, wtime))
                                    {

                                        TimeSpan pairDuration = new TimeSpan();
                                        if (!iopair.StartTime.Date.Equals(new DateTime()) && !iopair.EndTime.Date.Equals(new DateTime()))
                                        {

                                            pairDuration = iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay;

                                            //if shift is until 23:59 add one minute
                                            if (iopair.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                                pairDuration = pairDuration.Add(new TimeSpan(0, 1, 0));

                                            nightSpan = nightSpan.Add(pairDuration);

                                        }
                                    }
                                }
                            }
                        }
                    }
                    decimal hours = decimal.Parse((nightSpan.TotalHours).ToString().Trim());
                    //string minutes = ((((int)nightSpan.TotalMinutes - (int)nightSpan.TotalHours * 60) * 100) / 60).ToString();
                    //if (minutes == "0")
                    //{
                    //    nightWork = hours + "." + "00";
                    //}
                    //else if (minutes.Length == 1)
                    //{
                    //    nightWork = hours + "." + minutes + "0";
                    //}
                    //else
                    //{
                    //    nightWork = hours + "." + minutes;
                    //}
                    nightWork = Math.Round(hours, 2).ToString();
                    if (nightWork != "0.00")
                    {
                        if (passesForIOPairs.ContainsKey(pass_type_id))
                            passesForIOPairs[pass_type_id] = nightWork;
                        else
                            passesForIOPairs.Add(pass_type_id, nightWork);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double CalcRealisedTime(List<EmployeeTO> listEmpl, List<PassTypeTO> passTypesAll, Dictionary<int, string> pairsStatistics, int company)
        {
            try
            {
                string pass_type_presence = "";
                int pass_type_id = -1;
                double realisedTime = 0;
                foreach (EmployeeTO empl in listEmpl)
                {
                    if (company != -1)
                    {
                        Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                        rule.RuleTO.WorkingUnitID = company;
                        rule.RuleTO.EmployeeTypeID = empl.EmployeeTypeID;
                        rule.RuleTO.RuleType = Constants.RuleCompanyRegularWork;

                        List<RuleTO> rules = rule.Search();

                        if (rules.Count == 1)
                        {
                            //  pass_type_presence += rules[0].RuleValue + ",";
                            pass_type_id = rules[0].RuleValue;
                            foreach (PassTypeTO pt in passTypesAll)
                            {
                                if (pt.PassTypeID == pass_type_id)
                                {
                                    if (!pass_type_presence.Contains(pt.PassTypeID.ToString()))
                                    { pass_type_presence += pt.PassTypeID + ","; }
                                }
                            }
                        }
                    }
                }
                foreach (PassTypeTO pt in passTypesAll)
                {
                    if (pt.IsPass == Constants.overtimePassType)
                    {
                        pass_type_presence += pt.PassTypeID + ",";
                    }
                }
                if (pass_type_presence.Length > 0)
                    pass_type_presence = pass_type_presence.Remove(pass_type_presence.LastIndexOf(','));

                string[] passRealised = pass_type_presence.Split(',');
                foreach (string s in passRealised)
                {
                    foreach (int ptID in pairsStatistics.Keys)
                    {

                        if (ptID == int.Parse(s))
                        {
                            realisedTime += double.Parse(pairsStatistics[ptID]);
                            break;
                        }
                    }
                }
                return realisedTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private PieChart3D FillChart(string values, string texts, string links, string sliceDisplacments, string colors, int width, int height)
        {
            try
            {
                values = values.Substring(0, values.Length - 1);
                texts = texts.Substring(0, texts.Length - 1);
                links = links.Substring(0, links.Length - 1);
                sliceDisplacments = sliceDisplacments.Substring(0, sliceDisplacments.Length - 1);
                colors = colors.Substring(0, colors.Length - 1);
                PieChart3D pieChart = new PieChart3D();
                pieChart.ForeColor = "Black";
                pieChart.Width = width;
                pieChart.Height = height;
                pieChart.Values = values;
                pieChart.Texts = texts;
                pieChart.Links = links;
                pieChart.Colors = colors;
                pieChart.SliceDisplacments = sliceDisplacments;
                pieChart.Opacity = 100;
                pieChart.ShadowStyle = System.Drawing.PieChart.ShadowStyle.GradualShadow;
                pieChart.EdgeColorType = System.Drawing.PieChart.EdgeColorType.DarkerThanSurface;

                pieChart.FontFamily = "Times New Roman";
                pieChart.FontSize = 9;
                pieChart.FontStyle = System.Drawing.FontStyle.Regular;
                return pieChart;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateCtrlHolder(double realisedTime, double plannedTime, DataTable table, string colors, PieChart3D pieChart, int numChart, string title)
        {
            //string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            //DebugLog log = new DebugLog(logFilePath);
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WUStatisticalReportsPage).Assembly);
                if (numChart == 1)
                {

                    Panel resultPanel = new Panel();
                    resultPanel.HorizontalAlign = HorizontalAlign.Left;
                    resultPanel.Width = 980;
                    resultPanel.Height = 520;
                    resultPanel.ScrollBars = ScrollBars.Auto;
                    PlaceHolder ctrlHolder = new PlaceHolder();

                    PlaceHolder dataGridHolder = new PlaceHolder();

                    Label realisedLabel = new Label();
                    Label plannedLabel = new Label();
                    realisedLabel.Text = rm.GetString("lblRealised", culture);
                    plannedLabel.Text = rm.GetString("lblPlanned", culture);

                    TextBox realisedTextBox = new TextBox();
                    TextBox plannedTextBox = new TextBox();

                    Label titleLabel = new Label();
                    titleLabel.Text = title;

                    realisedTextBox.Text = realisedTime.ToString();
                    plannedTextBox.Text = plannedTime.ToString();

                    realisedTextBox.ReadOnly = true;
                    plannedTextBox.ReadOnly = true;
                    plannedTextBox.CssClass = "contentTbDisabledRight";
                    realisedTextBox.CssClass = "contentTbDisabledRight";
                    plannedLabel.CssClass = "contentLblLeft";
                    realisedLabel.CssClass = "contentLblLeft";
                    titleLabel.CssClass = "contentLblLeft";
                    Table10.CssClass = "tabTable";

                    resultPanel.Controls.Add(titleLabel);
                    resultPanel.Controls.Add(new LiteralControl("<br /><br />"));
                    resultPanel.Controls.Add(new LiteralControl(" &nbsp;&nbsp; &nbsp;&nbsp &nbsp;&nbsp;&nbsp &nbsp;&nbsp"));
                    resultPanel.Controls.Add(ctrlHolder);
                    resultPanel.Controls.Add(new LiteralControl("<br />"));
                    resultPanel.Controls.Add(realisedLabel);
                    resultPanel.Controls.Add(realisedTextBox);
                    resultPanel.Controls.Add(new LiteralControl("<br /><br />"));
                    resultPanel.Controls.Add(plannedLabel);
                    resultPanel.Controls.Add(new LiteralControl(" &nbsp;&nbsp;"));
                    resultPanel.Controls.Add(plannedTextBox);
                    resultPanel.Controls.Add(new LiteralControl("<br /><br />"));
                    resultPanel.Controls.Add(dataGridHolder);
                    TableCell8.Controls.Add(resultPanel);

                    DataGrid dg = new DataGrid();
                    dg.BackColor = Color.White;
                    dg.AutoGenerateColumns = false;


                    dg.Font.Name = "Verdana";
                    dg.Font.Size = 8;
                    dg.ForeColor = Color.Black;

                    dg.Columns.Add(CreateBoundColumn("pass_type", 0, 300, false, false));
                    dg.Columns.Add(CreateBoundColumn("color", 1, 10, false, false));
                    dg.Columns.Add(CreateBoundColumn("hours", 2, 200, true, false));
                    dg.Columns.Add(CreateBoundColumn("perRealised", 3, 200, false, true));
                    dg.Columns.Add(CreateBoundColumn("perPlanned", 4, 200, false, true));


                    if (rbSingle.Checked)
                        dg.Columns[0].HeaderText = rm.GetString("hdrPassType", culture);
                    else
                        dg.Columns[0].HeaderText = rm.GetString("hdrWorkingUnit", culture);
                    dg.Columns[1].HeaderText = "";
                    dg.Columns[2].HeaderText = rm.GetString("hdrHours", culture);
                    dg.Columns[3].HeaderText = rm.GetString("hdrPercentRealised", culture);
                    dg.Columns[4].HeaderText = rm.GetString("hdrPercentPlanned", culture);
                    dg.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    dg.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    dg.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    dg.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Right;


                    dg.DataSource = table;
                    dg.DataBind();
                    resultPanel.CssClass = "resultPanel";

                    string[] colorsArr = colors.Split(',');
                    int i = 1;

                    foreach (string s in colorsArr)
                    {
                        if (i <= dg.Items.Count)
                        {
                            System.Drawing.Color colk = System.Drawing.ColorTranslator.FromHtml(s);
                            dg.Items[i - 1].Cells[1].BackColor = colk;
                            i++;
                        }
                    }

                    dataGridHolder.Controls.Add(dg);
                    ctrlHolder.Controls.Add(pieChart);


                }
                else
                {

                    Table tablePanel = new Table();
                    tablePanel.CssClass = " tabNoBorderTable";
                    TableRow tableRow = new TableRow();
                    TableCell tableCell = new TableCell();
                    tableCell.Width = 950;
                    tableRow.Width = 950;
                    tablePanel.Width = 950;

                    tableCell.CssClass = "contentCell";
                    tablePanel.HorizontalAlign = HorizontalAlign.Left;
                    Panel resultPanel = new Panel();
                    resultPanel.HorizontalAlign = HorizontalAlign.Left;
                    resultPanel.Width = 960;
                    resultPanel.Height = 250;
                    resultPanel.ScrollBars = ScrollBars.Auto;
                    PlaceHolder ctrlHolder = new PlaceHolder();

                    PlaceHolder dataGridHolder = new PlaceHolder();

                    Label realisedLabel = new Label();
                    Label plannedLabel = new Label();
                    Label titleLabel = new Label();
                    titleLabel.Text = title;
                    realisedLabel.Text = rm.GetString("lblRealised", culture);
                    plannedLabel.Text = rm.GetString("lblPlanned", culture);

                    TextBox realisedTextBox = new TextBox();
                    TextBox plannedTextBox = new TextBox();

                    realisedTextBox.Text = realisedTime.ToString();
                    plannedTextBox.Text = plannedTime.ToString();

                    realisedTextBox.ReadOnly = true;
                    plannedTextBox.ReadOnly = true;

                    plannedTextBox.CssClass = "contentTbDisabledRight";
                    realisedTextBox.CssClass = "contentTbDisabledRight";
                    plannedLabel.CssClass = "contentLblLeft";
                    realisedLabel.CssClass = "contentLblLeft";
                    titleLabel.CssClass = "contentLblLeft";
                    tableCell.CssClass = "tabCellBorder";

                    resultPanel.Controls.Add(titleLabel);
                    resultPanel.Controls.Add(new LiteralControl("<br />"));
                    resultPanel.Controls.Add(new LiteralControl(" &nbsp;&nbsp; &nbsp;&nbsp &nbsp;&nbsp;&nbsp &nbsp;&nbsp"));
                    resultPanel.Controls.Add(ctrlHolder);
                    resultPanel.Controls.Add(new LiteralControl("<br />"));
                    resultPanel.Controls.Add(realisedLabel);
                    resultPanel.Controls.Add(realisedTextBox);
                    resultPanel.Controls.Add(new LiteralControl("<br /><br />"));
                    resultPanel.Controls.Add(plannedLabel);
                    resultPanel.Controls.Add(new LiteralControl(" &nbsp;&nbsp;"));
                    resultPanel.Controls.Add(plannedTextBox);
                    resultPanel.Controls.Add(new LiteralControl("<br /><br />"));
                    resultPanel.Controls.Add(dataGridHolder);
                    tableCell.Controls.Add(resultPanel);
                    tableRow.Controls.Add(tableCell);
                    tablePanel.Controls.Add(tableRow);
                    TableCell8.Controls.Add(tablePanel);

                    DataGrid dg = new DataGrid();
                    dg.BackColor = Color.White;
                    dg.AutoGenerateColumns = false;

                    dg.Font.Name = "Verdana";
                    dg.Font.Size = 8;
                    dg.ForeColor = Color.Black;


                    dg.Columns.Add(CreateBoundColumn("pass_type", 0, 200, false, false));
                    dg.Columns.Add(CreateBoundColumn("color", 1, 10, false, false));
                    dg.Columns.Add(CreateBoundColumn("hours", 2, 150, true, false));
                    dg.Columns.Add(CreateBoundColumn("perRealised", 3, 200, false, true));
                    dg.Columns.Add(CreateBoundColumn("perPlanned", 4, 200, false, true));
                    dg.Columns[0].HeaderText = "";
                    dg.Columns[1].HeaderText = "";
                    dg.Columns[2].HeaderText = rm.GetString("hdrHours", culture);
                    dg.Columns[3].HeaderText = rm.GetString("hdrPercentRealised", culture);
                    dg.Columns[4].HeaderText = rm.GetString("hdrPercentPlanned", culture);
                    dg.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    dg.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    dg.Columns[3].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    dg.Columns[4].ItemStyle.HorizontalAlign = HorizontalAlign.Right;


                    dg.DataSource = table;
                    dg.DataBind();
                    resultPanel.CssClass = "resultPanel";
                    string[] colorsArr = colors.Split(',');
                    int i = 1;

                    foreach (string s in colorsArr)
                    {
                        if (i < dg.Items.Count)
                        {
                            System.Drawing.Color colk = System.Drawing.ColorTranslator.FromHtml(s);
                            dg.Items[i - 1].Cells[1].BackColor = colk;
                            i++;
                        }
                    }

                    dataGridHolder.Controls.Add(dg);
                    ctrlHolder.Controls.Add(pieChart);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<WorkTimeIntervalTO> getTimeSchemaInterval(int employeeID, DateTime date, List<EmployeeTimeScheduleTO> timeScheduleListForEmployee, List<WorkTimeSchemaTO> timeSchema)
        {
            List<WorkTimeIntervalTO> intervalList = new List<WorkTimeIntervalTO>();
            try
            {
                int timeScheduleIndex = -1;

                for (int scheduleIndex = 0; scheduleIndex < timeScheduleListForEmployee.Count; scheduleIndex++)
                {

                    if (date >= timeScheduleListForEmployee[scheduleIndex].Date)
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                if (timeScheduleIndex >= 0)
                {
                    int cycleDuration = 0;
                    int startDay = timeScheduleListForEmployee[timeScheduleIndex].StartCycleDay;
                    int schemaID = timeScheduleListForEmployee[timeScheduleIndex].TimeSchemaID;
                    List<WorkTimeSchemaTO> timeSchemaEmployee = new List<WorkTimeSchemaTO>();
                    foreach (WorkTimeSchemaTO timeSch in timeSchema)
                    {
                        if (timeSch.TimeSchemaID == schemaID)
                        {
                            timeSchemaEmployee.Add(timeSch);
                        }
                    }
                    WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                    if (timeSchemaEmployee.Count > 0)
                    {
                        schema = timeSchemaEmployee[0];
                        cycleDuration = schema.CycleDuration;
                    }

                    TimeSpan days = new TimeSpan(date.Date.Ticks - timeScheduleListForEmployee[timeScheduleIndex].Date.Date.Ticks);

                    Dictionary<int, WorkTimeIntervalTO> table = schema.Days[(startDay + (int)days.TotalDays) % cycleDuration];
                    for (int i = 0; i < table.Count; i++)
                    {
                        intervalList.Add(table[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return intervalList;
        }

        private BoundColumn CreateBoundColumn(string dataFieldValue, int colIndex, double width, bool isSec, bool isPercent)
        {
            try
            {
                BoundColumn column = new BoundColumn();
                column.DataField = dataFieldValue;
                if (isSec)
                    column.DataFormatString = "{0:f2}";
                if (isPercent)
                    column.DataFormatString = "{0:f2}";
                column.ItemStyle.Width = new Unit(width);
                column.ItemStyle.BorderColor = ColorTranslator.FromHtml(Constants.colBorderColor);
                column.ItemStyle.BorderStyle = BorderStyle.Solid;
                column.ItemStyle.BorderWidth = Unit.Parse(Constants.colBorderWidth.ToString().Trim());
                column.Visible = true;
                column.ItemStyle.Wrap = true;
                return column;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<int, string> TotalHourCalc(List<IOPairProcessedTO> IOPairList, Dictionary<int, PassTypeTO> passTypes)
        {
            try
            {
                Dictionary<int, TimeSpan> typesCounter = new Dictionary<int, TimeSpan>();
                Dictionary<int, string> passesForIOPairs = new Dictionary<int, string>();
                foreach (IOPairProcessedTO iopair in IOPairList)
                {
                    if (!iopair.StartTime.Date.Equals(new DateTime()) && !iopair.EndTime.Date.Equals(new DateTime()))
                    {

                        if (passTypes.ContainsKey(iopair.PassTypeID))
                        {
                            TimeSpan duration = iopair.EndTime.TimeOfDay.Subtract(iopair.StartTime.TimeOfDay);
                            if (iopair.EndTime.Hour == 23 && iopair.EndTime.Minute == 59)
                                duration = duration.Add(new TimeSpan(0, 1, 0)); // add one minute if it is last interval from third shift
                            if (typesCounter.ContainsKey(iopair.PassTypeID))
                                typesCounter[iopair.PassTypeID] = typesCounter[iopair.PassTypeID].Add(duration);
                            else
                                typesCounter.Add(iopair.PassTypeID, duration);

                        }

                    }
                }
                foreach (int ptID in typesCounter.Keys)
                {
                    decimal hours = decimal.Parse((typesCounter[ptID].TotalHours).ToString());
                    //int minutes = ((((int)typesCounter[ptID].TotalMinutes - (int)typesCounter[ptID].TotalHours * 60) * 100) / 60);
                    decimal minutes = ((((decimal)typesCounter[ptID].TotalMinutes - (decimal)typesCounter[ptID].TotalHours * 60) * 100) / 60);
                    decimal min = minutes / 100;
                    decimal nesto = hours;
                    passesForIOPairs[ptID] = Math.Round(nesto, 2).ToString();

                }



                return passesForIOPairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<IOPairProcessedTO> FindIOPairsForEmployeeSingle(DateTime fromDate, DateTime toDate, int company, Dictionary<int, PassTypeTO> passTypesComp, List<EmployeeTO> listEmpl)
        {
            try
            {

                string passTypes = "";
                foreach (KeyValuePair<int, PassTypeTO> passPair in passTypesComp)
                {
                    passTypes += passPair.Value.PassTypeID + ",";
                }
                if (!passTypes.Equals(""))
                    passTypes = passTypes.Remove(passTypes.LastIndexOf(','));

                List<IOPairProcessedTO> IOPairList = new List<IOPairProcessedTO>();
                List<IOPairProcessedTO> AllIOPairList = new List<IOPairProcessedTO>();
                List<DateTime> datesList = new List<DateTime>();
                DateTime day = fromDate;
                while (day <= toDate)
                {
                    datesList.Add(day);
                    day = day.AddDays(1);
                }

                foreach (EmployeeTO Empl in listEmpl)
                {
                    IOPairList = new IOPairProcessed(Session[Constants.sessionConnection]).SearchAllPairsForEmpl(Empl.EmployeeID.ToString().Trim(), datesList, passTypes);
                    AllIOPairList.AddRange(IOPairList);
                }
                return AllIOPairList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getRootOrgUnit(int wu, Dictionary<int, OrganizationalUnitTO> OrgUnitDictionary)
        {
            int orgUnitID = wu;
            try
            {
                while (true)
                {
                    if (!OrgUnitDictionary.ContainsKey(orgUnitID))
                        break;
                    else
                    {
                        if (OrgUnitDictionary[orgUnitID].ParentOrgUnitID == OrgUnitDictionary[orgUnitID].ParentOrgUnitID)
                        {
                            orgUnitID = OrgUnitDictionary[orgUnitID].ParentOrgUnitID;
                            break;
                        }
                        else
                        {
                            orgUnitID = OrgUnitDictionary[orgUnitID].ParentOrgUnitID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orgUnitID;
        }

        public bool isEqualWU(WorkingUnitTO wunit, List<WorkingUnitTO> listWu)
        {
            try
            {
                bool iis = false;
                foreach (WorkingUnitTO wu in listWu)
                {
                    if (wu.WorkingUnitID == wunit.WorkingUnitID)
                        iis = true;
                }
                return iis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool isEqualOU(OrganizationalUnitTO ounit, List<OrganizationalUnitTO> listOU)
        {
            try
            {
                bool iis = false;
                foreach (OrganizationalUnitTO ou in listOU)
                {
                    if (ou.OrgUnitID == ounit.OrgUnitID)
                        iis = true;
                }
                return iis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected Dictionary<int, WorkingUnitTO> getSelectedWU(string wuIDs)
        {
            try
            {
                Dictionary<int, WorkingUnitTO> WUnits = getWUnits();
                Dictionary<int, WorkingUnitTO> SelWU = new Dictionary<int, WorkingUnitTO>();
                List<WorkingUnitTO> selWU = new List<WorkingUnitTO>();
                string[] wuList = wuIDs.Split(',');
                foreach (string wu in wuList)
                {
                    WorkingUnitTO wuTo = new WorkingUnitTO();
                    foreach (KeyValuePair<int, WorkingUnitTO> wuPair in WUnits)
                    {

                        if (wu == wuPair.Key.ToString())
                        {
                            wuTo = wuPair.Value;

                            SelWU.Add(int.Parse(wu), wuTo);
                            selWU.Add(wuTo);
                            break;
                        }
                    }

                }
                selWU = new WorkingUnit(Session[Constants.sessionConnection]).FindAllChildren(selWU);
                foreach (WorkingUnitTO wt in selWU)
                {
                    if (!SelWU.ContainsKey(wt.WorkingUnitID))
                        SelWU.Add(wt.WorkingUnitID, wt);
                }

                return SelWU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected Dictionary<int, OrganizationalUnitTO> getSelectedOU(string ouIDs)
        {
            try
            {
                Dictionary<int, OrganizationalUnitTO> OUnits = getOUnits();
                Dictionary<int, OrganizationalUnitTO> SelOU = new Dictionary<int, OrganizationalUnitTO>();
                List<OrganizationalUnitTO> selOU = new List<OrganizationalUnitTO>();
                string[] ouList = ouIDs.Split(',');
                foreach (string wu in ouList)
                {
                    OrganizationalUnitTO ouTo = new OrganizationalUnitTO();
                    foreach (KeyValuePair<int, OrganizationalUnitTO> ouPair in OUnits)
                    {

                        if (wu == ouPair.Key.ToString())
                        {
                            ouTo = ouPair.Value;

                            SelOU.Add(int.Parse(wu), ouTo);
                            selOU.Add(ouTo);
                            break;
                        }
                    }

                }
                selOU = new OrganizationalUnit(Session[Constants.sessionConnection]).FindAllChildren(selOU);
                foreach (OrganizationalUnitTO wt in selOU)
                {
                    if (!SelOU.ContainsKey(wt.OrgUnitID))
                        SelOU.Add(wt.OrgUnitID, wt);
                }

                return SelOU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                lblError.Text = "";
                for (int i = 0; i < Menu1.Items.Count; i++)
                {
                    if (i == int.Parse(e.Item.Value))
                    {
                        Menu1.Items[i].Selected = true;
                        MultiView1.SetActiveView(MultiView1.Views[i]);
                    }

                    CommonWeb.Misc.setMenuImage(Menu1, i, Menu1.Items[i].Selected);
                    CommonWeb.Misc.setMenuSeparator(Menu1, i, Menu1.Items[i].Selected);
                }

                int defaultWUID = -1;
                int defaultOUID = -1;
                if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                {
                    EmployeeAsco4 emplAsco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                    emplAsco.EmplAsco4TO.EmployeeID = ((EmployeeTO)Session[Constants.sessionLogInEmployee]).EmployeeID;

                    List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();

                    if (emplAscoList.Count == 1)
                    {
                        defaultWUID = emplAscoList[0].IntegerValue2;
                        defaultOUID = emplAscoList[0].IntegerValue3;
                    }
                }
                // value 0 - WU tab, value 1 - OU tab
                if (e.Item.Value.Equals("0"))
                {
                    int wuID = -1;
                    lboxUTE.Items.Clear();
                    lbPassTypes.Items.Clear();
                    if (tbWorkshop.Attributes["id"] != null && int.TryParse(tbWorkshop.Attributes["id"].Trim(), out wuID))
                    {
                        populateUTE(wuID.ToString(), true);
                        populatePassTypes(wuID.ToString());
                    }
                    else if (defaultWUID != -1)
                    {
                        wuID = defaultWUID;
                        populateWU(defaultWUID);
                        populateUTE(defaultWUID.ToString(), true);
                        populatePassTypes(defaultWUID.ToString());
                    }
                    else
                        wuID = -1;

                    Session[Constants.sessionWU] = wuID;
                    Session[Constants.sessionOU] = null;
                }
                else
                {
                    int ouID = -1;
                    lboxUTE.Items.Clear();
                    lbPassTypes.Items.Clear();
                    if (tbOrg.Attributes["id"] != null && int.TryParse(tbOrg.Attributes["id"].Trim(), out ouID))
                    {
                        populateUTE(ouID.ToString(), false);
                        lbPassTypes.Items.Clear();

                        List<WorkingUnitXOrganizationalUnitTO> listwUnitxOUnitTO = new List<WorkingUnitXOrganizationalUnitTO>();
                        WorkingUnitXOrganizationalUnitTO wUnitxOUnitTO = new WorkingUnitXOrganizationalUnitTO();
                        wUnitxOUnitTO.OrgUnitID = ouID;
                        WorkingUnitXOrganizationalUnit wUnitxOUnit = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                        wUnitxOUnit.WUXouTO = wUnitxOUnitTO;
                        listwUnitxOUnitTO = wUnitxOUnit.Search();
                        if (listwUnitxOUnitTO.Count > 0)
                        {
                            string oneComp = listwUnitxOUnitTO[0].WorkingUnitID.ToString();
                            if (oneComp != "-1")
                            {
                                populatePassTypes(oneComp);
                            }
                        }
                    }
                    else if (defaultOUID != -1)
                    {
                        ouID = defaultOUID;
                        populateOU(defaultOUID);
                        populateUTE(defaultOUID.ToString(), true);
                        List<WorkingUnitXOrganizationalUnitTO> listwUnitxOUnitTO = new List<WorkingUnitXOrganizationalUnitTO>();
                        WorkingUnitXOrganizationalUnitTO wUnitxOUnitTO = new WorkingUnitXOrganizationalUnitTO();
                        wUnitxOUnitTO.OrgUnitID = ouID;
                        WorkingUnitXOrganizationalUnit wUnitxOUnit = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                        wUnitxOUnit.WUXouTO = wUnitxOUnitTO;
                        listwUnitxOUnitTO = wUnitxOUnit.Search();
                        if (listwUnitxOUnitTO.Count > 0)
                        {
                            string oneComp = listwUnitxOUnitTO[0].WorkingUnitID.ToString();
                            if (oneComp != "-1")
                            {
                                populatePassTypes(oneComp);
                            }
                        }
                    }
                    else
                        ouID = -1;

                    Session[Constants.sessionOU] = ouID;
                    Session[Constants.sessionWU] = null;
                }

                // save selected filter state
                SaveState();

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in TLWUStatisticalReportsPage.Menu1_MenuItemClick(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLWUStatisticalReportsPage.aspx&Header=" + Constants.falseValue.Trim(), false);
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

                Session[Constants.sessionFilterState] = CommonWeb.Misc.SaveState(this, "TLWUStatisticalReportsPage.", filterState);
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
                    CommonWeb.Misc.LoadState(this, "TLDetailedDataPage.", filterState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
