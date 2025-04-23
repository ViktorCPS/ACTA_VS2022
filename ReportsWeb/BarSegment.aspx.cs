using System;
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

using TransferObjects;
using Common;
using Util;

namespace ReportsWeb
{
    public partial class BarSegment : System.Web.UI.Page
    {
        private const string outWSBorderColor = "#FFCCCC";
        private const string outWSColor = "#FFE6E6";
        private const string inWSBorderColor = "#CCEBCC";
        private const string inWSColor = "#E6F5E6";

        const string pageName = "BarSegment";

        //private DateTime LoadTime
        //{
        //    get
        //    {
        //        DateTime loadDate = new DateTime();
        //        if (ViewState["loadDate"] != null && ViewState["loadDate"] is DateTime)
        //        {
        //            loadDate = (DateTime)ViewState["loadDate"];
        //        }

        //        return loadDate;
        //    }
        //    set
        //    {
        //        if (value.Equals(new DateTime()))
        //            ViewState["loadDate"] = null;
        //        else
        //            ViewState["loadDate"] = value;
        //    }
        //}

        //private string Message
        //{
        //    get
        //    {
        //        string message = "";
        //        if (ViewState["message"] != null)
        //            message = ViewState["message"].ToString().Trim();

        //        return message;
        //    }
        //    set
        //    {
        //        if (value.Trim().Equals(""))
        //            ViewState["message"] = null;
        //        else
        //            ViewState["message"] = value;
        //    }
        //}

        //private DateTime StartLoadTime
        //{
        //    get
        //    {
        //        DateTime startLoadDate = new DateTime();
        //        if (ViewState["startLoadDate"] != null && ViewState["startLoadDate"] is DateTime)
        //        {
        //            startLoadDate = (DateTime)ViewState["startLoadDate"];
        //        }

        //        return startLoadDate;
        //    }
        //    set
        //    {
        //        if (value.Equals(new DateTime()))
        //            ViewState["startLoadDate"] = null;
        //        else
        //            ViewState["startLoadDate"] = value;
        //    }
        //}

        //protected void Page_PreLoad(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        StartLoadTime = DateTime.Now;
        //        LoadTime = new DateTime();
        //        Message = "";
        //    }
        //    catch { }
        //}

        //protected void Page_Unload(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        writeLog(DateTime.Now, true);
        //    }
        //    catch { }
        //}

        //private void writeLog(DateTime date, bool writeToFile)
        //{
        //    try
        //    {
        //        string writeFile = ConfigurationManager.AppSettings["writeLoadTime"];

        //        if (writeFile != null && writeFile.Trim().ToUpper().Equals(Constants.yes.Trim().ToUpper()))
        //        {
        //            DebugLog log = new DebugLog(Constants.logFilePath + "LoadTime.txt");

        //            if (!writeToFile)
        //            {
        //                string message = pageName;

        //                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO)
        //                    message += "|" + ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).Name.Trim();

        //                message += "|" + date.ToString("dd.MM.yyyy HH:mm:ss");

        //                message += "|" + ((int)date.Subtract(StartLoadTime).TotalMilliseconds).ToString();

        //                Message = message;
        //                LoadTime = date;
        //            }
        //            else if (Message != null && !Message.Trim().Equals(""))
        //            {
        //                Message += "|" + ((int)date.Subtract(LoadTime).TotalMilliseconds).ToString();

        //                log.writeLog(Message);
        //                StartLoadTime = new DateTime();
        //                LoadTime = new DateTime();
        //                Message = "";
        //            }
        //        }
        //    }
        //    catch { }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

                // parameters in query string should be pairRecID, pairIndexInList (if recID = 0), height, width, inWS, barColor
                if (!IsPostBack)
                {
                    IOPairProcessedTO iopair = new IOPairProcessedTO();
                    uint recID = 0;
                    int index = -1;
                    if (Request.QueryString["pairRecID"] != null)
                    {
                        if (!uint.TryParse(Request.QueryString["pairRecID"].Trim(), out recID))
                            recID = 0;                        
                    }
                    if (Request.QueryString["pairIndexInList"] != null)
                    {
                        if (!int.TryParse(Request.QueryString["pairIndexInList"].Trim(), out index))
                            index = -1;
                    }

                    if (Request.QueryString["legend"] != null)
                    {
                        iopair.IOPairDate = DateTime.Now.Date;
                        iopair.StartTime = new DateTime(iopair.IOPairDate.Year, iopair.IOPairDate.Month, iopair.IOPairDate.Day, 8, 0, 0);
                        iopair.EndTime = new DateTime(iopair.IOPairDate.Year, iopair.IOPairDate.Month, iopair.IOPairDate.Day, 9, 0, 0);

                        if (Request.QueryString["verify"] != null && Request.QueryString["verify"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                            iopair.VerificationFlag = (int)Constants.Verification.NotVerified;

                        if (Request.QueryString["confirm"] != null && Request.QueryString["confirm"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))
                            iopair.ConfirmationFlag = (int)Constants.Confirmation.NotConfirmed;
                    }
                    else if (recID != 0)
                    {
                        if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                        {
                            foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                            {
                                if (pair.RecID == recID)
                                {
                                    iopair = new IOPairProcessedTO(pair);
                                    break;
                                }
                            }
                        }
                        else if (Request.QueryString["hist"] != null && Request.QueryString["hist"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper()))                        
                            iopair = new IOPairProcessedTO(new IOPairsProcessedHist(Session[Constants.sessionConnection]).Find(recID));                        
                        else
                            iopair = new IOPairProcessed(Session[Constants.sessionConnection]).Find(recID);
                    }
                    else if (index >= 0)
                    {
                        if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO> && ((List<IOPairProcessedTO>)Session[Constants.sessionDayPairs]).Count > index)
                        {
                            iopair = new IOPairProcessedTO(((List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])[index]);
                        }
                    }

                    Response.ContentType = "image/png";

                    // Read chart setup information
                    double height = 10;
                    double width = 30;
                    bool inWS = false;
                    string barColor = "#FFFFFF";                    

                    if (Request.QueryString["height"] != null)
                    {
                        if (!double.TryParse(Request.QueryString["height"].Trim(), out height))
                            height = 10;
                    }
                    if (Request.QueryString["width"] != null)
                    {
                        if (!double.TryParse(Request.QueryString["width"].Trim(), out width))
                            width = 100;
                    }
                    if (Request.QueryString["inWS"] != null)
                    {
                        inWS = Request.QueryString["inWS"].Trim().ToUpper().Equals(Constants.trueValue.Trim().ToUpper());
                    }
                    if (Request.QueryString["barColor"] != null)
                    {
                        barColor = "#" + Request.QueryString["barColor"].Trim();                            
                    }

                    //List<PassTO> passes = new List<PassTO>();
                    //if (Request.QueryString["passes"] != null)
                    //{
                    //    string[] pString = Request.QueryString["passes"].Trim().Split('|');

                    //    foreach (string p in pString)
                    //    {
                    //        int hour = -1;
                    //        int min = -1;
                    //        string dir = "";

                    //        int hIndex = p.IndexOf(":");
                    //        int minIndex = p.IndexOf(";");
                    //        if (hIndex >= 0 && int.TryParse(p.Substring(0, hIndex).Trim(), out hour))
                    //        {
                    //            if (minIndex >= 0 && minIndex > hIndex && int.TryParse(p.Substring(hIndex + 1, minIndex - hIndex -1).Trim(), out min))                                
                    //                dir = p.Substring(minIndex + 1);                                
                    //            else
                    //                min = -1;
                    //        }
                    //        else
                    //            hour = -1;

                    //        if (hour != -1 && min != -1 && !dir.Trim().Equals(""))
                    //        {
                    //            DateTime passTime = new DateTime(iopair.IOPairDate.Year, iopair.IOPairDate.Month, iopair.IOPairDate.Day, hour, min, 0);

                    //            if (iopair.StartTime.TimeOfDay <= passTime.TimeOfDay && iopair.EndTime.TimeOfDay >= passTime.TimeOfDay)
                    //            {
                    //                PassTO pass = new PassTO();
                    //                pass.Direction = dir;
                    //                pass.EmployeeID = iopair.EmployeeID;
                    //                pass.EventTime = passTime;                                    
                    //                passes.Add(pass);
                    //            }
                    //        }
                    //    }
                    //}

                    DateTime startTime = iopair.StartTime;
                    DateTime endTime = iopair.EndTime;

                    if (startTime.Equals(new DateTime()) && endTime.Equals(new DateTime()))
                    {
                        int startHour = -1;
                        int startMinute = -1;
                        int endHour = -1;
                        int endMinute = -1;

                        if (Request.QueryString["startH"] != null)
                        {
                            if (!int.TryParse(Request.QueryString["startH"].Trim(), out startHour))
                                startHour = -1;
                        }
                        if (Request.QueryString["startMin"] != null)
                        {
                            if (!int.TryParse(Request.QueryString["startMin"].Trim(), out startMinute))
                                startMinute = -1;
                        }
                        if (Request.QueryString["endH"] != null)
                        {
                            if (!int.TryParse(Request.QueryString["endH"].Trim(), out endHour))
                                endHour = -1;
                        }
                        if (Request.QueryString["endMin"] != null)
                        {
                            if (!int.TryParse(Request.QueryString["endMin"].Trim(), out endMinute))
                                endMinute = -1;
                        }

                        if (startHour != -1 && startMinute != -1)
                            startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, startHour, startMinute, 0);

                        if (endHour != -1 && endMinute != -1)
                            endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, endHour, endMinute, 0);
                    }

                    Bitmap barBmp = drawBar((int)width, (int)height, iopair, startTime, endTime, inWS, barColor);
                    MemoryStream memStream = new MemoryStream();                    
                    
                    // Render BitMap Stream Back To Client
                    barBmp.Save(memStream, ImageFormat.Png);
                    memStream.WriteTo(Response.OutputStream);
                }

                //writeLog(DateTime.Now, false);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in BarSegment.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private Bitmap drawBar(int width, int height, IOPairProcessedTO ioPair, DateTime startTime, DateTime endTime, bool inWS, string barColor)
        {
            Bitmap barBmp = new Bitmap(width, height);

            try
            {
                Graphics gr = Graphics.FromImage(barBmp);                
                Color IOPairColor = ColorTranslator.FromHtml(barColor);

                // draw background
                Rectangle backRect = new Rectangle(1, 1, width - 2, height - 2);
                
                SolidBrush backBrush;
                Pen backBorder;

                if (inWS)
                {
                    backBrush = new SolidBrush(ColorTranslator.FromHtml(inWSColor));
                    backBorder = new Pen(ColorTranslator.FromHtml(inWSBorderColor));
                }
                else
                {
                    backBrush = new SolidBrush(ColorTranslator.FromHtml(outWSColor));
                    backBorder = new Pen(ColorTranslator.FromHtml(outWSBorderColor));
                }

                if (ioPair.ManualCreated == (int)Constants.recordCreated.Manualy)
                    backBorder = new Pen(Color.Black);
                                
                gr.FillRectangle(backBrush, backRect);
                gr.DrawRectangle(backBorder, backRect);

                // first hour line to draw
                int hour = startTime.Hour;
                if (startTime.Minute > 0)
                    hour++;

                // draw hour lines
                while (startTime.Hour == hour || endTime.Hour >= hour)
                {
                    Pen hourPen = new Pen(Color.LightBlue);
                    float hourX = (float)((hour * 60 - startTime.Hour * 60 - startTime.Minute) * Constants.minutWidth);

                    gr.DrawLine(hourPen, hourX, 2, hourX, height - 2);
                    hour++;
                }

                try
                {
                    if (!ioPair.StartTime.Equals(new DateTime()) || !ioPair.EndTime.Equals(new DateTime()))
                    {
                        if (ioPair.StartTime.Date.Equals(new DateTime())) //If there is no StartTime draw an arrow
                        {
                            // Create points for polygon
                            PointF p1 = new PointF(width, 0);
                            PointF p2 = new PointF(width, height);
                            PointF p3 = new PointF(0, height / 2);
                            PointF[] ptsArray =
                        {
                            p1, p2,p3
                        };
                            LinearGradientBrush brush5 = new LinearGradientBrush(p1, p3, IOPairColor, Color.White);
                            brush5.SetSigmaBellShape(0.5f, 0.6f);
                            gr.SmoothingMode = SmoothingMode.HighQuality;
                            // Draw open pair
                            gr.FillPolygon(brush5, ptsArray);
                        }
                        else if (ioPair.EndTime.Date.Equals(new DateTime())) //If there is no EndTime draw an arrow
                        {
                            // Create points for polygon
                            PointF p1 = new PointF(0, 0);
                            PointF p2 = new PointF(0, height);
                            PointF p3 = new PointF(width, height / 2);
                            PointF[] ptsArray =
                        {
                            p1, p2,p3
                        };
                            LinearGradientBrush brush5 = new LinearGradientBrush(p1, p3, IOPairColor, Color.White);
                            brush5.SetSigmaBellShape(0.5f, 0.6f);
                            gr.SmoothingMode = SmoothingMode.HighQuality;
                            // Draw open pair
                            gr.FillPolygon(brush5, ptsArray);
                        }
                        else
                        {
                            if (width > 8) //If bar isn't to short draw 3D effect
                            {
                                RectangleF barRect3D = new RectangleF(2, height / 3, width - 4, height / 3);
                                LinearGradientBrush barBrush3D = new LinearGradientBrush(barRect3D, IOPairColor, Color.White, 90);
                                barBrush3D.SetSigmaBellShape(0.8f, 0.8f);

                                RectangleF rectElipse = new RectangleF(width - 4, height / 3, 4, height / 3);
                                LinearGradientBrush brushElipse = new LinearGradientBrush(rectElipse, IOPairColor, Color.White, 90);
                                brushElipse.SetSigmaBellShape(0.2f, 0.8f);
                                gr.SmoothingMode = SmoothingMode.HighQuality;

                                gr.FillRectangle(barBrush3D, barRect3D);
                                gr.FillEllipse(brushElipse, rectElipse);

                                gr.DrawEllipse(new Pen(IOPairColor), new RectangleF(width - 4, height / 3, 4, height / 3));
                                gr.FillEllipse(new SolidBrush(IOPairColor), new RectangleF(0, height / 3, 4, height / 3));
                            }
                            else //If bar is to short draw just rectangle
                            {
                                RectangleF barRect = new RectangleF(0, height / 3, width, height / 3);
                                LinearGradientBrush barBrush = new LinearGradientBrush(barRect, IOPairColor, Color.White, 90);
                                barBrush.SetSigmaBellShape(0.8f, 0.8f);
                                gr.FillRectangle(barBrush, barRect);
                            }
                        }

                        Pen linePen = new Pen(Color.Gray);
                        // draw confirmation and verification marks if needed
                        if (ioPair.ConfirmationFlag == (int)Constants.Confirmation.NotConfirmed)
                        {
                            int startX = 0;
                            int startY = height / 3;

                            while (startX + 5 < width)
                            {
                                gr.DrawLine(linePen, startX, startY * 2, startX + 5, startY);
                                startX += 5;
                            }
                        }

                        if (ioPair.VerificationFlag == (int)Constants.Verification.NotVerified)
                        {
                            int startX = 0;
                            int startY = height / 3;

                            while (startX + 5 < width)
                            {
                                gr.DrawLine(linePen, startX, startY, startX + 5, startY * 2);
                                startX += 5;
                            }
                        }
                    }
                }
                catch (System.Threading.ThreadAbortException)
                {
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    // Release resources
                    // Dispose of objects
                    gr.Dispose();
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return barBmp;
        }
    }
}
