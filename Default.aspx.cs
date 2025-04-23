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
using System.Resources;
using System.Globalization;

//using Common;

namespace ACTAWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        //private void setLanguage()
        //{
        //    try
        //    {
        //        lbtnExit.Text = rm.GetString("lbtnExit", culture);

        //        // set menu items language and create dinamicaly menuItems
        //        MenuItem menuPasses = Menu.FindItem("Passes");                
        //        menuPasses.Text = rm.GetString("lbtnPasses", culture);

        //        // add menuPasses subItems
        //        //MenuItem menuPasses1 = new MenuItem("Prolasci 1", "Passes 1", "", "");
        //        //menuPasses.ChildItems.Add(menuPasses1);

        //        //MenuItem menuPasses11 = new MenuItem("Prolasci 1.1", "Passes 1.1", "", "");
        //        //menuPasses1.ChildItems.Add(menuPasses11);

        //        //MenuItem menuPasses12 = new MenuItem("Prolasci 1.2", "Passes 1.2", "", "");
        //        //menuPasses1.ChildItems.Add(menuPasses12);

        //        //MenuItem menuPasses2 = new MenuItem("Prolasci 2", "Passes 2", "", "");
        //        //menuPasses.ChildItems.Add(menuPasses2);
                                
        //        MenuItem menuPermissions = Menu.FindItem("Permissions");
        //        menuPermissions.Text = rm.GetString("lbtnExitPermissions", culture);

        //        MenuItem menuAbsences = Menu.FindItem("Absences");
        //        menuAbsences.Text = rm.GetString("lbtnAbsences", culture);

        //        //MenuItem menuPresenceReport = Menu.FindItem("PresenceReport");
        //        //menuPresenceReport.Text = rm.GetString("lbtnPresenceReport", culture);

        //        if (Session["LoggedInUser"] != null)
        //            lblLoggedIn.Text = Session["LoggedInUser"].ToString().Trim();
        //        else
        //            lblLoggedIn.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}        
    }
}
