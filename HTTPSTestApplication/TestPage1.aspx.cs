﻿using System;
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

namespace HTTPSTestApplication
{
    public partial class TestPage1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["text"] != null)
                lblTest.Text = Session["text"].ToString();
            else
                lblTest.Text = "Welcome to test page!";
        }
    }
}
