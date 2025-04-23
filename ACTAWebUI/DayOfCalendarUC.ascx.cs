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
using System.Drawing;
using Util;
using System.Globalization;
using System.Resources;

namespace ACTAWebUI
{
    public partial class DayOfCalendarUC : System.Web.UI.UserControl
    {
        private DateTime date = new DateTime();
        private int schemaID = -1;
        private int cycleDay = -1;
        private bool transprent = true;
        private bool selected = false;
        private string color = "";
            
        private const double firstLblWidth = 29;
        private const double secondLblWidth = 69;
        private const int firstLblHeight = 50;
        private const int secundLblHeight = 50;
        private string description = "";

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        public bool Transprent
        {
            get { return transprent; }
            set { transprent = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public int SchemaID
        {
            get { return schemaID; }
            set { schemaID = value; }
        }

        public int CycleDay
        {
            get { return cycleDay; }
            set { cycleDay = value; }
        }

     
       

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DrawDay();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in DayOfCalendarUC.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void DrawDay()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(TLClockDataPage).Assembly);

                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Dispose();
                }

                this.Controls.Clear();

              //first label
                    Label dateLbl = new Label();
                    dateLbl.ID = "dateLbl";
                    if (selected)
                    {
                        dateLbl.Width = new Unit(firstLblWidth);
                        dateLbl.Height = new Unit(firstLblHeight);
                    }
                    else
                    {
                        dateLbl.Width = new Unit(firstLblWidth+1);
                        dateLbl.Height = new Unit(firstLblHeight+1);
                    }
                    if (!transprent)
                    {
                        
                        dateLbl.BackColor = ColorTranslator.FromHtml(Color);
                        if (!selected)
                            dateLbl.CssClass = "contentBoldBlueLbl";
                        else
                        {
                            switch (Color)
                            {
                                case Constants.calendarWeekEndDayColor:
                                        dateLbl.CssClass = "contentBoldBlueBorderWeekendLbl";
                                    break;
                                case Constants.calendarDayColor:
                                        dateLbl.CssClass = "contentBoldBlueBorderLbl";
                                    break;
                                case Constants.calendarNationalHolidayDayColor:
                                        dateLbl.CssClass = "contentBoldBlueBorderNationalHolidayLbl";
                                    break;
                                case Constants.calendarPersonalHolidayDayColor:
                                        dateLbl.CssClass = "contentBoldBlueBorderPersonalHolidayLbl";
                                    break;

                            }
                        }
                        dateLbl.Text = date.Day.ToString();
                    }
                   
                    this.Controls.Add(dateLbl);

                //secund label
                    Label secondLbl = new Label();
                    secondLbl.ID = "secondLbl";
                    if (selected)
                    {
                        secondLbl.Width = new Unit(secondLblWidth);
                        secondLbl.Height = new Unit(firstLblHeight);
                    }
                    else
                    {
                        secondLbl.Width = new Unit(secondLblWidth+1);
                        secondLbl.Height = new Unit(firstLblHeight+1);
                    }
                    if (!transprent)
                    {
                        secondLbl.BackColor = ColorTranslator.FromHtml(Color);
                        if (!selected)
                            secondLbl.CssClass = "contentLblLeftDay";
                        else
                        {
                            switch (Color)
                            {
                                case Constants.calendarWeekEndDayColor:
                                    if (selected)
                                        secondLbl.CssClass = "contentLblLeftBorderWeekendDay";
                                    break;
                                case Constants.calendarDayColor:
                                    if (selected)
                                        secondLbl.CssClass = "contentLblLeftBorderDay";
                                    break;
                                case Constants.calendarNationalHolidayDayColor:
                                    if (selected)
                                        secondLbl.CssClass = "contentLblLeftBorderNationalHolidayDay";
                                    break;
                                case Constants.calendarPersonalHolidayDayColor:
                                    if (selected)
                                        secondLbl.CssClass = "contentLblLeftBorderPersonalHolidayDay";
                                    break;
                            }
                        }

                        if (schemaID != -1 && cycleDay != -1)
                        {
                            secondLbl.Text = rm.GetString("lblTimeSchema", culture) + schemaID.ToString();
                            if (secondLbl.Text.Length > 11)
                            {
                                secondLbl.Text += "<br>";
                                secondLbl.Text += Description;
                                secondLbl.Text += "<br>";
                            }
                            else
                            {
                                secondLbl.Text += "<br><br>";
                                secondLbl.Text += Description;
                                secondLbl.Text += "<br>";
                            }
                            secondLbl.Text += rm.GetString("lblCycleDay", culture) + cycleDay.ToString();
                        }
                    }
                  
                    this.Controls.Add(secondLbl);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}