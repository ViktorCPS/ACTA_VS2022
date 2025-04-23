using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Resources;
using System.Configuration;
using Microsoft.Reporting.WebForms;

using Common;
using TransferObjects;
using Util;

namespace ACTAWebUI
{
    public partial class WCFormsPage : System.Web.UI.Page
    {
        const string pageName = "WCFormsPage";

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
            EmployeeTO Empl = new EmployeeTO();
            try
            {

                if (!IsPostBack)
                {
                    btnShow.Attributes.Add("onclick", "return document.body.style.cursor = 'wait'");
                    setLanguage();

                    Empl = getEmployee();

                    InitializeData(Empl);
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCFormsPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?emplID=" + Empl.EmployeeID + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
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

        private string emplTypes(EmployeeTO employee)
        {
            try
            {
                string empl = "";
                //// get selected company
                int company = -1;

                int wuID = employee.WorkingUnitID;
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                company = Common.Misc.getRootWorkingUnit(wuID, WUnits);
                if (company == -1)
                {
                    WorkingUnitXOrganizationalUnit wuXou = new WorkingUnitXOrganizationalUnit(Session[Constants.sessionConnection]);
                    wuXou.WUXouTO.OrgUnitID = employee.OrgUnitID;
                    List<WorkingUnitXOrganizationalUnitTO> list = wuXou.Search();
                    if (list.Count > 0)
                    {
                        WorkingUnitXOrganizationalUnitTO wuXouTO = list[0];
                        company = Common.Misc.getRootWorkingUnit(wuXouTO.WorkingUnitID, WUnits);
                    }
                    else
                        company = -1;
                }
                List<EmployeeTypeTO> listEmplTypes = new List<EmployeeTypeTO>();
                listEmplTypes = new EmployeeType(Session[Constants.sessionConnection]).Search();
                foreach (EmployeeTypeTO emplType in listEmplTypes)
                {
                    if (emplType.EmployeeTypeID == employee.EmployeeTypeID && emplType.WorkingUnitID == company)
                    {
                        empl = emplType.EmployeeTypeName;
                        break;
                    }
                }

                return empl;
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCFormsPage).Assembly);

                if (Empl.EmployeeID != -1)
                {
                    tbFirstName.Text = Empl.FirstName.Trim();
                    tbLastName.Text = Empl.LastName.Trim();
                    tbPayroll.Text = Empl.EmployeeID.ToString().Trim();
                    // get additional employee data
                    EmployeeAsco4 asco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                    asco.EmplAsco4TO.EmployeeID = Empl.EmployeeID;
                    List<EmployeeAsco4TO> ascoList = asco.Search();

                    EmployeeAsco4TO ascoTO = new EmployeeAsco4TO();
                    if (ascoList.Count > 0)
                        ascoTO = ascoList[0];

                    tbStringone.Text = emplTypes(Empl);

                    // tbStringone.Text = ascoList[0].NVarcharValue2.Trim();
                    tbBranch.Text = ascoTO.NVarcharValue6.Trim();

                    // get employee working unit - UTE
                    WorkingUnit wu = new WorkingUnit(Session[Constants.sessionConnection]);
                    WorkingUnitTO tempWU = wu.FindWU(Empl.WorkingUnitID);
                    tbUTE.Text = tempWU.Code.Trim();
                    tbWUnit.Text = tempWU.Name.Trim();
                    tbWUnit.ToolTip = tempWU.Description.Trim() + "(" + tempWU.Code.Trim() + ")";

                    // get workshop (parent of UTE)
                    wu.WUTO = tempWU;
                    tempWU = wu.getParentWorkingUnit();
                    tbWorkgroup.Text = tempWU.Code.Trim();

                    // get cost centar
                    wu.WUTO = tempWU;
                    tempWU = wu.getParentWorkingUnit();
                    tbCostCentar.Text = tempWU.Code.Trim();

                    // get plant
                    wu.WUTO = tempWU;
                    tempWU = wu.getParentWorkingUnit();
                    tbPlant.Text = tempWU.Code.Trim();

                    // get organizational unit
                    OrganizationalUnitTO ou = new OrganizationalUnit(Session[Constants.sessionConnection]).Find(Empl.OrgUnitID);
                    tbOUnit.Text = ou.Code.Trim();
                    tbOUnit.ToolTip = ou.Name.Trim();

                    // get FS responsible persons
                    List<EmployeeTO> emplWUResList = new Employee(Session[Constants.sessionConnection]).SearchEmployeesWUResponsible(Empl.WorkingUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);
                    string responsibility = rm.GetString("FSResPerson", culture) + Environment.NewLine;
                    foreach (EmployeeTO empl in emplWUResList)
                    {
                        responsibility += empl.FirstAndLastName.Trim() + Environment.NewLine;
                    }

                    // get OU responsible persons
                    List<EmployeeTO> emplOUResList = new Employee(Session[Constants.sessionConnection]).SearchEmployeesOUResponsible(Empl.OrgUnitID.ToString().Trim(), null, DateTime.Now.Date, DateTime.Now.Date);
                    responsibility += rm.GetString("OUResPerson", culture) + Environment.NewLine;
                    foreach (EmployeeTO empl in emplOUResList)
                    {
                        responsibility += empl.FirstAndLastName.Trim() + Environment.NewLine;
                    }

                    tbStringone.ToolTip = responsibility.Trim();

                    string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                    int cost = 0;
                    bool costum = int.TryParse(costumer, out cost);
                    bool isFiat = (cost == (int)Constants.Customers.FIAT);

                    rowBranch.Visible = rowBranchtlbl.Visible = rowPlant.Visible = rowPlantlbl.Visible = rowCC.Visible = rowCClbl.Visible = rowWG.Visible = rowWGlbl.Visible = rowUTE.Visible = rowUTElbl.Visible = isFiat;
                    rowWU.Visible = rowWUlbl.Visible = !isFiat;

                    // create dinamically asco data
                    Dictionary<string, string> ascoMetadata = new EmployeeAsco4Metadata(Session[Constants.sessionConnection]).GetMetadataWebValues(CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]));

                    int counter = 0;
                    foreach (string col in ascoMetadata.Keys)
                    {
                        if (ascoMetadata[col].Trim() == "")
                            continue;

                        // make row separator label
                        Label separatorLbl = new Label();
                        separatorLbl.ID = "separatorLbl" + counter.ToString().Trim();
                        separatorLbl.Width = new Unit(105);
                        separatorLbl.Height = new Unit(5);
                        separatorLbl.CssClass = "contentLblLeft";
                        ascoCtrlHolder.Controls.Add(separatorLbl);

                        // make asco name label
                        Label ascoNameLbl = new Label();
                        ascoNameLbl.ID = "ascoNameLbl" + counter.ToString().Trim();
                        ascoNameLbl.Width = new Unit(105);
                        ascoNameLbl.Text = ascoMetadata[col].Trim() + ":";
                        ascoNameLbl.CssClass = "contentLblLeft";
                        ascoCtrlHolder.Controls.Add(ascoNameLbl);

                        // make pass type counter label
                        TextBox ascoTb = new TextBox();
                        ascoTb.ID = "ascoLbl" + counter.ToString().Trim();
                        ascoTb.Width = new Unit(105);
                        ascoTb.Text = Common.Misc.getAscoValue(ascoTO, col);
                        ascoTb.ReadOnly = true;
                        ascoTb.CssClass = "contentTbDisabled";
                        ascoCtrlHolder.Controls.Add(ascoTb);

                        counter++;
                    }
                }
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
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCFormsPage).Assembly);

                lblEmplData.Text = rm.GetString("lblEmplData", culture);
                lblFirstName.Text = rm.GetString("lblFirstName", culture);
                lblLastName.Text = rm.GetString("lblLastName", culture);
                lblStringone.Text = rm.GetString("lblEmployeeType", culture);
                lblPlant.Text = rm.GetString("lblPlant", culture);
                lblCostCentar.Text = rm.GetString("lblCostCentar", culture);
                lblWorkgroup.Text = rm.GetString("lblWorkgroup", culture);
                lblUTE.Text = rm.GetString("lblUTE", culture);
                lblBranch.Text = rm.GetString("lblBranch", culture);
                lblOUnit.Text = rm.GetString("lblOUnit", culture);
                lblWUnit.Text = rm.GetString("lblWUnit", culture);
                btnShow.Text = rm.GetString("btnShow", culture);
                rbEmail.Text = rm.GetString("rbTraining", culture);
                rbFillIn.Text = rm.GetString("rbTravelReq", culture);
                rbFuel.Text = rm.GetString("rbFuel", culture);
                lblForms.Text = rm.GetString("lblForms", culture);
                lblPayroll.Text = rm.GetString("lblPayrollID", culture);
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
        protected void btnShow_Click(Object sender, EventArgs e)
        {
            EmployeeTO Empl = new EmployeeTO();
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(WCFormsPage).Assembly);
                string id = "";
                string email = "";
                string cost_centre = "";
                Empl = getEmployee();

                EmployeeAsco4 emplAsco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                emplAsco.EmplAsco4TO.EmployeeID = Empl.EmployeeID;

                List<EmployeeAsco4TO> emplAscoList = emplAsco.Search();

                if (emplAscoList.Count == 1)
                {
                    id = emplAscoList[0].NVarcharValue4;
                    email = emplAscoList[0].NVarcharValue3;
                }

                // get employee working unit - UTE
                WorkingUnit wu = new WorkingUnit(Session[Constants.sessionConnection]);
                WorkingUnitTO tempWU = wu.FindWU(Empl.WorkingUnitID);
                // get workshop (parent of UTE)
                wu.WUTO = tempWU;
                tempWU = wu.getParentWorkingUnit();
                //   tbWorkgroup.Text = tempWU.Code.Trim();

                // get cost centar
                wu.WUTO = tempWU;
                tempWU = wu.getParentWorkingUnit();
                //  tbCostCentar.Text = tempWU.Code.Trim();
                cost_centre = tempWU.Description.Trim();

                int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, getWUnits());
                WorkingUnitTO working = new WorkingUnit(Session[Constants.sessionConnection]).FindWU(company);
                tempWU = wu.FindWU(Empl.WorkingUnitID);
                DataSet ds = new DataSet();
                DataTable table = new DataTable("Forms");
                table.Columns.Add("name", typeof(System.String));
                table.Columns.Add("id", typeof(System.String));
                table.Columns.Add("department", typeof(System.String));
                table.Columns.Add("costcentre", typeof(System.String));
                table.Columns.Add("email", typeof(System.String));
                table.Columns.Add("jmbg", typeof(System.String));
                table.Columns.Add("company", typeof(System.String));
                ds.Tables.Add(table);
                DataRow row = table.NewRow();
                row["name"] = Empl.FirstAndLastName;
                row["jmbg"] = id;
                row["department"] = tempWU.Name;
                row["costcentre"] = cost_centre;
                row["email"] = email;
                row["id"] = Empl.EmployeeID;
                row["company"] = working.Description;
                table.Rows.Add(row);
                table.AcceptChanges();

                TableCell13.Controls.Clear();
                TableCell13.HorizontalAlign = HorizontalAlign.Center;
                TableCell13.VerticalAlign = VerticalAlign.Middle;
                ReportViewer ReportViewer1 = new ReportViewer();
                ReportViewer1.Height = 500;
                ReportViewer1.Width = 500;
                ReportViewer1.ShowRefreshButton = false;
                ReportViewer1.ShowPageNavigationControls = false;

                ReportViewer1.ShowFindControls = false;
                string costumer = Common.Misc.getCustomer(Session[Constants.sessionConnection]);
                int cost = 0;
                bool costum = int.TryParse(costumer, out cost);
                bool isFiat = (cost == (int)Constants.Customers.FIAT);



                TableCell13.Controls.Add(ReportViewer1);
                //  CustomizeRV(ReportViewer1);
                if (rbEmail.Checked)
                {
                    string name = "";
                    string emailReport = "";
                    if (isFiat)
                    {
                        name = "Name/Fas ID:";
                        emailReport = "email to jovana.andjelic@fiat.com latest one week after the training";
                    }
                    else
                    {
                        name = "Name/ID:";
                    }
                    ReportParameter param1 = new ReportParameter("id", name);
                    ReportParameter param2 = new ReportParameter("email", emailReport);
                    ReportViewer1.LocalReport.ReportPath = "ReportsWeb\\en\\WCFormsReport_en.rdlc";
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { param1, param2});
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], ds.Tables[0]));
                }
                else if (rbFillIn.Checked)
                {
                    string signatureDirector = "";
                    string fasCar = "";
                    if (isFiat) 
                    {
                        signatureDirector = "Signature of FAS Director                                                Potpis FAS Direktora";
                        fasCar = "Service car / Službeni auto    ";
                    }
                    else 
                    {
                        signatureDirector = "Signature of Director                                                    Potpis Direktora";
                        fasCar = "Service car / Službeni auto    ";
                    }
                    string ID = "";
                    if (isFiat)
                    {
                        ID = "Payroll ID / Matični broj";
                    }
                    else
                    {
                        ID = "ID";
                    }
                    ReportParameter param1 = new ReportParameter("signatureDirector", signatureDirector);
                    ReportParameter param2 = new ReportParameter("fasCar", fasCar);
                    ReportParameter param3 = new ReportParameter("id", ID);
                    ReportViewer1.LocalReport.ReportPath = "ReportsWeb\\WCTravelRequisitionReport.rdlc";
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { param1, param2,param3});
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], ds.Tables[0]));
                }
                else if (rbFuel.Checked)
                {
                    string ID = "";
                    if (isFiat)
                    {
                        ID = "Payroll ID / Matični broj u FAS";
                    }
                    else
                    {
                        ID = "ID";
                    }
                    ReportParameter param1 = new ReportParameter("id", ID);
                    ReportViewer1.LocalReport.ReportPath = "ReportsWeb\\FuelReimbursementReport.rdlc";
                    DataTable tableFuel = new DataTable("Forms");
                    tableFuel.Columns.Add("name", typeof(System.String));
                    tableFuel.Columns.Add("ID", typeof(System.String));
                    tableFuel.Columns.Add("lastName", typeof(System.String));
                    tableFuel.Columns.Add("costcentre", typeof(System.String));
                    tableFuel.Columns.Add("company", typeof(System.String));

                    DataRow rowFuel = tableFuel.NewRow();
                    rowFuel["name"] = Empl.FirstName;
                    rowFuel["lastName"] = Empl.LastName;
                    rowFuel["costcentre"] = cost_centre;
                    rowFuel["id"] = Empl.EmployeeID;
                    rowFuel["company"] = working.Description;
                    tableFuel.Rows.Add(rowFuel);
                    tableFuel.AcceptChanges();
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { param1 });
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportViewer1.LocalReport.GetDataSourceNames()[0], tableFuel));
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCFormsPage.btnShow_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCFormsPage.aspx?emplID=" + Empl.EmployeeID, false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void CustomizeRV(System.Web.UI.Control reportControl)
        {
            foreach (System.Web.UI.Control childControl in reportControl.Controls)
            {
                if (childControl.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                {
                    System.Web.UI.WebControls.DropDownList ddList = (System.Web.UI.WebControls.DropDownList)childControl;
                    ddList.PreRender += new EventHandler(ddList_PreRender);
                }
                if (childControl.Controls.Count > 0)
                {
                    CustomizeRV(childControl);
                }
            }
        }

        void ddList_PreRender(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
            {
                System.Web.UI.WebControls.DropDownList ddList = (System.Web.UI.WebControls.DropDownList)sender;
                System.Web.UI.WebControls.ListItemCollection listItems = ddList.Items;

                if ((listItems != null) && (listItems.Count > 0) && (listItems.FindByText("Acrobat (PDF) file") != null))
                {
                    foreach (System.Web.UI.WebControls.ListItem list in listItems)
                    {
                        if (list.Text.Equals("Acrobat (PDF) file"))
                        {
                            list.Enabled = false;
                        }
                    }
                }
            }
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            EmployeeTO Empl = new EmployeeTO();
            try
            {
                TableCell13.Controls.Clear();
                ReportViewer ReportViewer1 = new ReportViewer();
                ReportViewer1.Height = 535;
                ReportViewer1.Width = 900;

                TableCell13.Controls.Add(ReportViewer1);
                Empl = getEmployee();
                RadioButton rb = sender as RadioButton;
                if (rb == rbEmail)
                {
                    rbFillIn.Checked = rbFuel.Checked = !rbEmail.Checked;
                }

                else if (rb == rbFillIn)
                {
                    rbEmail.Checked = rbFuel.Checked = !rbFillIn.Checked;
                }
                else if (rb == rbFuel)
                {
                    rbEmail.Checked = rbFillIn.Checked = !rbFuel.Checked;

                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in WCFormsPage.rb_CheckedChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/WCReportsPage.aspx?emplID=" + Empl.EmployeeID + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
