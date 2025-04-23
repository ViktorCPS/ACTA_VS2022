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
using System.Globalization;
using System.Resources;
using System.DirectoryServices.AccountManagement;

namespace ACTAWebUI
{
    public partial class ADAMLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    setLanguage();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void setLanguage()
        {
            try
            {
                lblLogin.Text = "ADAM test";
                lblUserName.Text = "Korisničko ime:";
                lblPassword.Text = "Lozinka:";
               
                btnOK.Text = "OK";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool UsrAuth(string usr, string pwd)
        {
            bool status = false;
            try
            {
                PrincipalContext ctx = new PrincipalContext(
                                          ContextType.ApplicationDirectory,
                                          "vldap.fg.local:636",
                                          "CN=fgadam,DC=fg,DC=local",
                                          "A004904", "RssPX1suJcf9");

                string connectedServer = ctx.ConnectedServer;

                status = ctx.ValidateCredentials(usr, pwd);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }


        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";

                if (UsrAuth(tbUserName.Text.Trim(), tbPassword.Text.Trim()))
                    lblError.Text = "Uspela validacija";
                else
                    lblError.Text = "Neuspela validacija";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
