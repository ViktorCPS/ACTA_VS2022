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

using Util;
using TransferObjects;

namespace CommonWeb.Pickers
{
    public partial class DatePicker : System.Web.UI.Page
    {
        //private Hashtable Getholiday()
        //{
        //    Hashtable holiday = new Hashtable();
        //    holiday["1/1/2009"] = "New Year";
        //    holiday["1/5/2009"] = "Guru Govind Singh Jayanti";
        //    holiday["1/8/2009"] = "Muharam (Al Hijra)";
        //    holiday["1/14/2009"] = "Pongal";
        //    holiday["1/26/2009"] = "Republic Day";
        //    holiday["2/23/2009"] = "Maha Shivaratri";
        //    holiday["3/10/2009"] = "Milad un Nabi (Birthday of the Prophet";
        //    holiday["3/21/2009"] = "Holi";
        //    holiday["3/21/2009"] = "Telugu New Year";
        //    holiday["4/3/2009"] = "Ram Navmi";
        //    holiday["4/7/2009"] = "Mahavir Jayanti";
        //    holiday["4/10/2009"] = "Good Friday";
        //    holiday["4/12/2009"] = "Easter";
        //    holiday["4/14/2009"] = "Tamil New Year and Dr Ambedkar Birth Day";
        //    holiday["5/1/2009"] = "May Day";
        //    holiday["5/9/2009"] = "Buddha Jayanti and Buddha Purnima";
        //    holiday["6/24/2009"] = "Rath yatra";
        //    holiday["8/13/2009"] = "Krishna Jayanthi";
        //    holiday["8/14/2009"] = "Janmashtami";
        //    holiday["8/15/2009"] = "Independence Day";
        //    holiday["8/19/2009"] = "Parsi New Year";
        //    holiday["8/23/2009"] = "Vinayaka Chaturthi";
        //    holiday["9/2/2009"] = "Onam";
        //    holiday["9/5/2009"] = "Teachers Day";
        //    holiday["9/21/2009"] = "Ramzan";
        //    holiday["9/27/2009"] = "Ayutha Pooja";
        //    holiday["9/28/2009"] = "Vijaya Dasami (Dusherra)";
        //    holiday["10/2/2009"] = "Gandhi Jayanti";
        //    holiday["10/17/2009"] = "Diwali & Govardhan Puja";
        //    holiday["10/19/2009"] = "Bhaidooj";
        //    holiday["11/2/2009"] = "Guru Nanak Jayanti";
        //    holiday["11/14/2009"] = "Children's Day";
        //    holiday["11/28/2009"] = "Bakrid";
        //    holiday["12/25/2009"] = "Christmas";
        //    holiday["12/28/2009"] = "Muharram";
        //    return holiday;
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //HolidayList = Getholiday();
                //calendar.Caption = "Calender - Author: Puran Singh Mehra";
                //calendar.FirstDayOfWeek = FirstDayOfWeek.Sunday;
                //calendar.NextPrevFormat = NextPrevFormat.ShortMonth;
                //calendar.TitleFormat = TitleFormat.Month;
                //calendar.ShowGridLines = true;
                //calendar.DayStyle.Height = new Unit(50);
                //calendar.DayStyle.Width = new Unit(150);
                //calendar.DayStyle.HorizontalAlign = HorizontalAlign.Center;
                //calendar.DayStyle.VerticalAlign = VerticalAlign.Middle;
                //calendar.OtherMonthDayStyle.BackColor = System.Drawing.Color.AliceBlue;
                
                
                
                
                
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            }
            catch { }
        }
        public DateTime currDate= DateTime.Now.Date;
        Hashtable HolidayList;
        //protected void Calendar_DayRender(object sender, DayRenderEventArgs e)
        //{
        //    try
        //    {
        //        //if (HolidayList[e.Day.Date.ToShortDateString()] != null)
        //        //{
        //        //    Literal literal1 = new Literal();
        //        //    literal1.Text = "<br/>";
        //        //    e.Cell.Controls.Add(literal1);
        //        //    Label label1 = new Label();
        //        //    label1.Text = (string)HolidayList[e.Day.Date.ToShortDateString()];
        //        //    label1.Font.Size = new FontUnit(FontSize.Small);
        //        //    e.Cell.Controls.Add(label1);
        //        //}




        //        // Clear the link from this day
        //        //e.Cell.Controls.Clear();

        //       // HtmlGenericControl Link = new HtmlGenericControl();
        //        //Link.TagName = "a";
        //        //Link.InnerText = e.Day.DayNumberText;
        //        //string jsString = "JavaScript:var openerWindow = window.dialogArguments;"; // openerWindow.document.form1.{0}.value = \'{1}\'; 
        //        //if (Request.QueryString["doPostBack"] != null && Request.QueryString["doPostBack"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
        //        //    jsString += "openerWindow.pagePostBack(\'{0}\'); ";
        //        //jsString += "window.close(); ";
        //       // Link.Attributes.Add("href", String.Format(jsString, Request.QueryString["field"], e.Day.Date.ToString(Constants.dateFormat.Trim())));
        //        //Link.Attributes.Add("href", "window.close()");
        //        //currDate = e.Day.Date;
        //        // Now add our custom link to the page
        //       // e.Cell.Controls.Add(Link);
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in DatePicker.Calendar_DayRender(): " + ex.Message.Replace('\n', ' ').Trim() + "&Back=/ACTAWeb/CommonWeb/Pickers/DatePicker.aspx", false);
        //        }
        //        catch (System.Threading.ThreadAbortException) { }
        //    }
        //}
        public void DateChange(object sender, EventArgs e) 
        {
            //if (calendar != null)
            //{
            //    DateTime currDate = calendar.SelectedDate;
            //    ScriptManager.RegisterStartupScript(this, GetType(), "close", "window.opener.location=window.opener.location; window.close();", true);
            //    return currDate;
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "close", "window.opener.location=window.opener.location; window.close();", true);
            //    return DateTime.Now.Date;
            //}

            ScriptManager.RegisterStartupScript(this, GetType(), "close", "window.opener.location=window.opener.location; window.close();", true);
            currDate = calendar.SelectedDate;
            
        }
        public DateTime DateChangeGet() {
            if (calendar != null)
            {
                return calendar.SelectedDate;
            }
            else
            {
                return currDate;
            }
        }
    }
}
