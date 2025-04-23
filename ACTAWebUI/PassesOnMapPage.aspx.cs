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

namespace ACTAWebUI
{
    public partial class PassesOnMapPage : System.Web.UI.Page
    {

        const string pageName = "PassesOnMapPage";
        string[] centerLoc = new string[2];
        decimal lati = 0;
        decimal longi = 0;
        int noOfMarkers = 0;

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
                // parameter in request query string is visit_id
                if (!IsPostBack)
                {
                    btnClose.Attributes.Add("onclick", "return closeWindow();");

                    //gets ID from TLClockDataPage
                    uint[] passID = getIDs();

                    string markers = GetMarkers(passID);
                    if (!markers.Equals(""))
                    {
                        decimal[] centerLocation = center(lati, longi, noOfMarkers);
                        if (centerLocation[0] != 0 && centerLocation[1] != 0)
                        {
                            centerLoc[0] = centerLocation[0].ToString();
                            centerLoc[1] = centerLocation[1].ToString();
                        }
                        Literal1.Text = @"
                         <script type='text/javascript'>
                             function initialize() {

                                    var mapOptions = {
                                        center: new google.maps.LatLng("
                                                + centerLoc[0] + "," + centerLoc[1] + "), " +
                                            @"zoom: 12, 
                                        mapTypeId: google.maps.MapTypeId.ROADMAP
                                    };

                                    var myMap = new google.maps.Map(document.getElementById('mapArea'),mapOptions);"

                                        + markers +
                                 @"}
                         </script>";

                        setLanguage1();
                    }
                    else
                    {
                        Literal1.Text = @"
                         <script type='text/javascript'>
                             function initialize() {

                                    var mapOptions = {
                                        mapTypeId: google.maps.MapTypeId.ROADMAP
                                    };
                                }
                         </script>";

                        setLanguage2();
                       
                    }
                }

                writeLog(DateTime.Now, false);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in PassesOnMapPage.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private decimal[] center(decimal lati, decimal longi, int no)
        {
            decimal centerLati = lati / no;
            decimal centerLongi = longi / no;

            decimal[] centerLongiLati = new decimal[2];

            centerLongiLati[0] = Math.Round(centerLati, 6);
            centerLongiLati[1] = Math.Round(centerLongi, 6) ;
            return centerLongiLati;
        }

        protected string GetMarkers(uint[] passIDArray)
        {
            string markers = "";

            List<PassesAdditionalInfoTO> listOfPasses = new List<PassesAdditionalInfoTO>();
            foreach (uint uintPass in passIDArray)
            {
                PassesAdditionalInfoTO pass = new PassesAdditionalInfo().Find(uintPass);

                if (pass != null && pass.PassID != 0)
                {
                    listOfPasses.Add(pass);
                }
            }
            int i = 0;
            if (listOfPasses.Count > 0)
            {
                foreach (PassesAdditionalInfoTO p in listOfPasses)
                {
                    string longilati = p.GpsData;
                    string[] separateLongLati = longilati.Split('|');
                    if (i == 0)
                    {
                        centerLoc = separateLongLati;
                    }

                    markers +=
                        @"var marker" + i.ToString() + @" = new google.maps.Marker({
                        position: new google.maps.LatLng(" + separateLongLati[0] + ", " + separateLongLati[1] + ")," +
                        @"map: myMap,
                        title:'" + p.CardholderName + "'});";
                    i++;

                    lati += Decimal.Parse(separateLongLati[0]);
                    longi += Decimal.Parse(separateLongLati[1]);
                    noOfMarkers++;
                }
            }
            return markers;
        }

        private uint[] getIDs()
        {
            string s = Request.QueryString["passID"];

            if (s == null)
            {
                Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in PassesOnMapPage.Page_Load()&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
            }
            string[] array = s.Split('|');
            uint[] arrayOfUint = new uint[array.Count()]; //niz UINT registracija
            int i = 0;
            foreach (string pass in array)
            {
                uint passID = 0;
                if (!uint.TryParse(pass, out passID))
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in PassesOnMapPage.Page_Load()&Back=/ACTAWeb/ACTAWebUI/TLClockDataPage.aspx&Header=" + Constants.falseValue.Trim(), false);
                }

                arrayOfUint[i] = passID;
                i++;
            }
            return arrayOfUint;
            
        }

        private void setLanguage1()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmployeeSchedulesPage).Assembly);

                lblTitle.Text = rm.GetString("lblPassesOnMap", culture);

                btnClose.Text = rm.GetString("btnClose", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setLanguage2()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(EmployeeSchedulesPage).Assembly);

                lblTitle.Text = rm.GetString("lblPassesOnMap", culture);

                btnClose.Text = rm.GetString("btnClose", culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
