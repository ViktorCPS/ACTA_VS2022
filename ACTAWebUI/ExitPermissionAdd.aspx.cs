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
    public partial class ExitPermissionAdd : System.Web.UI.Page
    {
        private static CultureInfo culture;
        private static ResourceManager rm;
        private static bool addForm = true;
        private static ExitPermissionTO permTO;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                    rm = new ResourceManager("ACTAWebUI.Resource", typeof(ExitPermissionAdd).Assembly);

                    lbtnExit.Attributes.Add("onclick", "return LogoutByButton();");
                    btnDate.Attributes.Add("onclick", "return calendarPicker('tbDate');");
                    
                    setLanguage();
                    populateTypes();

                    lblError.Text = "";
                    if (Session["selectedKeys"] != null && ((List<string>)Session["selectedKeys"]).Count > 0)
                    {
                        // update form
                        addForm = false;
                        int permID = int.Parse(((List<string>)Session["selectedKeys"])[0]);
                        permTO = new ExitPermission().Find(permID);

                        cbType.SelectedValue = permTO.PassTypeID.ToString().Trim();
                        tbDate.Text = permTO.StartTime.ToString(Constants.dateFormat);
                        tbTime.Text = permTO.StartTime.ToString(Constants.timeFormat);
                        tbOffset.Text = permTO.Offset.ToString().Trim();
                        tbDescription.Text = permTO.Description.Trim();
                        cbType.Focus();

                        if (permTO.Used == (int)Constants.Used.Yes)
                        {
                            cbType.Enabled = false;
                            tbDate.Enabled = false;
                            tbTime.Enabled = false;
                            tbOffset.Enabled = false;
                            tbDescription.Focus();
                        }
                    }
                    else
                    {
                        // add form
                        addForm = true;
                        permTO = new ExitPermissionTO();
                        tbDate.Text = DateTime.Now.ToString(Constants.dateFormat.Trim());
                        tbTime.Text = DateTime.Now.ToString(Constants.timeFormat.Trim());
                        tbOffset.Text = Constants.offset.ToString().Trim();
                        cbType.Focus();
                    }

                    if (Request.QueryString["showNavigation"] != null && Request.QueryString["showNavigation"].Trim().ToUpper().Equals(Constants.falseValue.Trim().ToUpper()))
                    {
                        lbtnMenu.Visible = lbtnExit.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionAdd.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionAdd.aspx", false);
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
                lbtnBack.Text = rm.GetString("lbtnBack", culture);
                
                lblDate.Text = rm.GetString("lblDate", culture) + "*";
                lblTime.Text = rm.GetString("lblTime", culture) + "*";
                lblDateFormat.Text = rm.GetString("lblDateFormat", culture);
                lblDateExample.Text = rm.GetString("lblDateExample", culture) + " " + rm.GetString("lblTimeExample", culture);
                lblType.Text = rm.GetString("lblType", culture) + "*";
                lblOffset.Text = rm.GetString("lblOffset", culture) + "*";
                lblDesc.Text = rm.GetString("lblDesc", culture);
                if (Session["selectedKeys"] != null && ((List<string>)Session["selectedKeys"]).Count > 0)
                {
                    lblExitPermissionAddUpd.Text = rm.GetString("lblExitPermissionUpd", culture);
                    btnSave.Text = rm.GetString("btnUpdate", culture);
                }
                else
                {
                    lblExitPermissionAddUpd.Text = rm.GetString("lblExitPermissionAdd", culture);
                    btnSave.Text = rm.GetString("btnSave", culture);
                }
                if (Session["LoggedInUser"] != null)
                    lblLoggedInUser.Text = Session["LoggedInUser"].ToString().Trim();
                else
                    lblLoggedInUser.Text = "";
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
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionAdd.lbtnMenu_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionAdd.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void lbtnBack_Click(Object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["Back"] != null)
                    Response.Redirect(Request.QueryString["Back"], false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionAdd.lbtnBack_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionAdd.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void btnSave_Click(Object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";

                // validation
                int passTypeID = -1;

                if (cbType.SelectedIndex <= 0)
                {
                    lblError.Text = rm.GetString("noType", culture);
                    cbType.Focus();
                    return;
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
                }

                if (tbDate.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noDate", culture);
                    tbDate.Focus();
                    return;
                }

                if (tbTime.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noTime", culture);
                    tbTime.Focus();
                    return;
                }

                DateTime date = CommonWeb.Misc.createDate(tbDate.Text.Trim());
                DateTime time = CommonWeb.Misc.createTime(tbTime.Text.Trim());

                if (!tbDate.Text.Trim().Equals("") && date.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidDate", culture);
                    tbDate.Focus();
                    date = new DateTime();
                    time = new DateTime();
                    return;
                }

                if (!tbTime.Text.Trim().Equals("") && time.Equals(new DateTime()))
                {
                    lblError.Text = rm.GetString("invalidTime", culture);
                    tbTime.Focus();
                    date = new DateTime();
                    time = new DateTime();
                    return;
                }

                date = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);

                if (date < DateTime.Now)
                {
                    lblError.Text = rm.GetString("passDateTime", culture);
                    tbDate.Focus();
                    date = new DateTime();
                    time = new DateTime();
                    return;
                }

                if (tbOffset.Text.Trim().Equals(""))
                {
                    lblError.Text = rm.GetString("noOffset", culture);
                    tbOffset.Focus();
                    return;
                }

                int offset = -1;

                if (!tbOffset.Text.Trim().Equals(""))
                {
                    if (!int.TryParse(tbOffset.Text.Trim(), out offset) || offset <= 0)
                    {
                        lblError.Text = rm.GetString("invalidOffset", culture);
                        tbOffset.Focus();
                        offset = -1;
                        return;
                    }
                }
                               
                // create exit permission to add/update                
                permTO.PassTypeID = passTypeID;
                permTO.StartTime = date;
                permTO.Offset = offset;
                permTO.Description = tbDescription.Text.Trim();
                                
                permTO.IssuedBy = Session["LoggedInUser"].ToString().Trim();
                permTO.EmployeeID = int.Parse(Session["UserID"].ToString().Trim());

                if (addForm)
                {
                    // add form - set used and verified by of new permission
                    permTO.Used = (int)Constants.Used.No;
                    permTO.VerifiedBy = Session["LoggedInUser"].ToString().Trim();

                    if (new ExitPermission().Save(permTO) > 0)
                    {
                        lbtnBack_Click(this, new EventArgs());
                    }
                    else
                    {
                        lblError.Text = rm.GetString("permissionNotSaved", culture);
                    }
                }
                else
                {
                    // update form - set permissionID of permission to update
                    permTO.PermissionID = int.Parse(((List<string>)Session["selectedKeys"])[0]);

                    if (new ExitPermission().Update(permTO))
                    {
                        lbtnBack_Click(this, new EventArgs());
                    }
                    else
                    {
                        lblError.Text = rm.GetString("permissionNotUpdated", culture);
                    }
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in ExitPermissionAdd.btnSave_Click(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/ExitPermissionAdd.aspx", false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}
