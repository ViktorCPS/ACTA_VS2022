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
using System.Drawing;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace ReportsWeb
{
    public partial class EmployeeWorkingDayViewUC : System.Web.UI.UserControl
    {
        private DateTime date = new DateTime();
        private string firstData = "";
        private List<IOPairProcessedTO> dayPairList = new List<IOPairProcessedTO>();
        private List<IOPairsProcessedHistTO> dayUnverifiedPairList = new List<IOPairsProcessedHistTO>();
        // key is index in list, value is pair
        //private Dictionary<int, IOPairProcessedTO> DayPairDic = new Dictionary<int, IOPairProcessedTO>();
        private List<WorkTimeIntervalTO> dayIntervalList = new List<WorkTimeIntervalTO>();
        private List<PassTO> dayPasses = new List<PassTO>();
        private Dictionary<int, PassTypeTO> passTypes = new Dictionary<int, PassTypeTO>();
        private Dictionary<int, LocationTO> locations = new Dictionary<int, LocationTO>();
        private Color backColor = Color.White;
        private string secondData = "";
        private bool isFirst = false;
        private bool isAltCtrl = false;
        private bool showOnlyPairs = false;
        private bool allowChange = false;
        private bool allowVerify = false;
        private bool allowUndoVerify = false;
        private bool allowConfirm = false;
        private bool showAlert = false;
        private bool histDay = false;
        private bool isHeader = false;
        private bool showReallocated = false;
        private bool byLocations = false;
        private EmployeeTO empl = new EmployeeTO();
        private Dictionary<int, WorkingUnitTO> wUnits = new Dictionary<int,WorkingUnitTO>();
        private WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
        private string postBackCtrlID = "";

        private const double dateLblWidth = 135;
        private const double totalLblWidth = 70;
        private const double height = 14;
        private const double borderWidth = 1;

        private Dictionary<int, double[]> barStartEndPos = new Dictionary<int, double[]>(); // key is index of pair in list, value is list of start and end position of bar for that pair
        private double startPairsPos = dateLblWidth + totalLblWidth + 4 * borderWidth;
        private double endPairsPos;
        private Dictionary<int, double> hourPos = new Dictionary<int,double>(); // key is hour, value is its line position
                
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string FirstData
        {
            get { return firstData; }
            set { firstData = value; }
        }

        public List<IOPairProcessedTO> DayPairList
        {
            get { return dayPairList; }
            set { dayPairList = value; }
        }

        public List<IOPairsProcessedHistTO> DayUnverifiedPairList
        {
            get { return dayUnverifiedPairList; }
            set { dayUnverifiedPairList = value; }
        }

        public List<WorkTimeIntervalTO> DayIntervalList
        {
            get { return dayIntervalList; }
            set { dayIntervalList = value; }
        }

        public List<PassTO> DayPasses
        {
            get { return dayPasses; }
            set { dayPasses = value; }
        }

        public Dictionary<int, PassTypeTO> PassTypes
        {
            get { return passTypes; }
            set { passTypes = value; }
        }

        public Dictionary<int, LocationTO> Locations
        {
            get { return locations; }
            set { locations = value; }
        }

        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        public string SecondData
        {
            get { return secondData; }
            set { secondData = value; }
        }

        public bool IsFirst
        {
            get { return isFirst; }
            set { isFirst = value; }
        }

        public bool IsAltCtrl
        {
            get { return isAltCtrl; }
            set { isAltCtrl = value; }
        }

        public bool IsHeader
        {
            get { return isHeader; }
            set { isHeader = value; }
        }

        public bool AllowChange
        {
            get { return allowChange; }
            set { allowChange = value; }
        }

        public bool AllowVerify
        {
            get { return allowVerify; }
            set { allowVerify = value; }
        }

        public bool AllowUndoVerify
        {
            get { return allowUndoVerify; }
            set { allowUndoVerify = value; }
        }

        public bool AllowConfirm
        {
            get { return allowConfirm; }
            set { allowConfirm = value; }
        }

        public bool ShowAlert
        {
            get { return showAlert; }
            set { showAlert = value; }
        }

        public bool ShowReallocated
        {
            get { return showReallocated; }
            set { showReallocated = value; }
        }

        public bool HistDay
        {
            get { return histDay; }
            set { histDay = value; }
        }
        
        public bool ShowOnlyPairs
        {
            get { return showOnlyPairs; }
            set { showOnlyPairs = value; }
        }

        public bool ByLocations
        {
            get { return byLocations; }
            set { byLocations = value; }
        }

        public EmployeeTO Empl
        {
            get { return empl; }
            set { empl = value; }
        }

        public Dictionary<int, WorkingUnitTO> WUnits
        {
            get { return wUnits; }
            set { wUnits = value; }
        }

        public WorkTimeSchemaTO EmplTimeSchema
        {
            get { return schema; }
            set { schema = value; }
        }

        public string PostBackCtrlID
        {
            get { return postBackCtrlID; }
            set { postBackCtrlID = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // HR Legal Entity and WCManager can only see data without posibility of changing U HUTCHINSON-U WC MANAGER TREBA DA IMA MOGUCNOST PROMENE
                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRLegalEntity)
                 //MM   || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager
                    || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.BCSelfService))
                    AllowChange = false;

                // do not allow manual changes of night flexy time schema
                if (EmplTimeSchema.Type == Constants.schemaTypeNightFlexi)
                    AllowChange = false;
                
                if (ShowOnlyPairs)
                    startPairsPos = 0;

                // do not draw hour line for 0h
                for (int i = 1; i <= 24; i++)
                {
                    if (!hourPos.ContainsKey(i))
                        hourPos.Add(i, startPairsPos + i * 60 * Constants.minutWidth);
                }
                                
                endPairsPos = hourPos[24]; // end position of day pairs view
                                                
                for (int i = 0; i < DayPairList.Count; i++)
                {
                    if (!DayPairList[i].IOPairDate.Date.Equals(Date.Date))
                        continue;

                    IOPairProcessedTO pair = DayPairList[i];
                    
                    double startPos = 0;
                    double endPos = 0;

                    if (!pair.StartTime.Equals(new DateTime()))
                    {
                        startPos = startPairsPos + (pair.StartTime.Hour * 60 + pair.StartTime.Minute) * Constants.minutWidth;
                        if (!pair.EndTime.Equals(new DateTime()))
                        {
                            endPos = startPairsPos + (pair.EndTime.Hour * 60 + pair.EndTime.Minute) * Constants.minutWidth;
                            if (endPos < startPos)
                                continue; // maybe set alert!!!!!
                        }
                        else
                            endPos = startPos + 4;
                    }
                    else
                    {
                        endPos = startPairsPos + (pair.EndTime.Hour * 60 + pair.EndTime.Minute) * Constants.minutWidth;
                        startPos = endPos - 4;
                        if (startPos < 0)
                            startPos = 0;
                    }

                    if (!barStartEndPos.ContainsKey(i))
                    {
                        barStartEndPos.Add(i, new double[] { startPos, endPos });
                    }
                }

                if (DayPairList.Count <= 0 && barStartEndPos.Count <= 0)
                {
                    for (int i = 0; i < DayIntervalList.Count; i++)
                    {
                        WorkTimeIntervalTO interval = DayIntervalList[i];

                        DateTime startTime = interval.StartTime;
                        DateTime endTime = interval.EndTime;

                        if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                        {
                            startTime = interval.EarliestArrived;
                            endTime = interval.EarliestLeft;
                        }

                        double startPos = 0;
                        double endPos = 0;

                        if (interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes > 0)
                        {
                            startPos = startPairsPos + (startTime.Hour * 60 + startTime.Minute) * Constants.minutWidth;
                            endPos = startPairsPos + (endTime.Hour * 60 + endTime.Minute) * Constants.minutWidth;

                            if (endPos < startPos)
                                continue; // maybe set alert!!!!!

                            if (!barStartEndPos.ContainsKey(i))
                            {
                                barStartEndPos.Add(i, new double[] { startPos, endPos });
                            }
                        }
                    }
                }

                DrawGraphControl();
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in EmployeeWorkingDayViewUC.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
        
        private void DrawGraphControl()
        {
            try
            {
                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ReportsWeb.Resource", typeof(EmployeeWorkingDayViewUC).Assembly);

                foreach (Control ctrl in this.Controls)
                {
                    ctrl.Dispose();
                }

                this.Controls.Clear();
                               
                //find company ID
                int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);
                
                if (!ShowOnlyPairs)
                {
                    bool isWeekend = false;
                    if (Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday)
                        isWeekend = true;

                    Label monthLbl = new Label();
                    monthLbl.ID = "monthLbl";
                    if (IsHeader)
                        monthLbl.Width = new Unit(dateLblWidth + totalLblWidth + 2);
                    else
                        monthLbl.Width = new Unit(dateLblWidth);
                    monthLbl.Height = new Unit(height - 2);
                    monthLbl.BackColor = BackColor;
                    if (isWeekend)
                        monthLbl.ForeColor = Color.Red;
                    if (IsFirst)
                        monthLbl.CssClass = "emplWorkingDayUCFirstLbl";
                    else if (IsAltCtrl)
                        monthLbl.CssClass = "emplWorkingDayUCAltLbl";
                    else
                        monthLbl.CssClass = "emplWorkingDayUCLbl";
                    
                    if (IsHeader)
                    {
                        monthLbl.Font.Bold = true;
                        monthLbl.Font.Italic = true;
                    }
                    else if (DayPairList.Count <= 0 && barStartEndPos.Count <= 0 && AllowChange)
                    {
                        monthLbl.Attributes.Add("onclick", "return pairBarChange('" + Date.ToString(Constants.dateFormat.Trim()) + "', '" + Empl.EmployeeTypeID.ToString().Trim()
                            + "', '" + company.ToString().Trim() + "','" + Empl.EmployeeID.ToString().Trim() + "', '" + PostBackCtrlID + "', 'false', '-1', 'false', '-1', '0');");
                    }
                    monthLbl.Text = FirstData.Trim();
                    monthLbl.ToolTip = FirstData.Trim();                    
                    this.Controls.Add(monthLbl);

                    if (!IsHeader)
                    {
                        Label totalLbl = new Label();
                        totalLbl.ID = "totalLbl";
                        totalLbl.Width = new Unit(totalLblWidth);
                        totalLbl.Height = new Unit(height - 2);
                        totalLbl.BackColor = BackColor;
                        if (isWeekend)
                            totalLbl.ForeColor = Color.Red;
                        if (IsFirst)
                        {
                            if (IsHeader)
                                totalLbl.CssClass = "emplWorkingDayHeaderUCFirstLbl";
                            else
                                totalLbl.CssClass = "emplWorkingDayUCFirstLbl";
                        }
                        else if (IsAltCtrl)
                        {
                            if (IsHeader)
                                totalLbl.CssClass = "emplWorkingDayHeaderUCAltLbl";
                            else
                                totalLbl.CssClass = "emplWorkingDayUCAltLbl";
                        }
                        else
                        {
                            if (IsHeader)
                                totalLbl.CssClass = "emplWorkingDayHeaderUCLbl";
                            else
                                totalLbl.CssClass = "emplWorkingDayUCLbl";
                        }
                        
                        totalLbl.Text = SecondData.Trim();
                        if (ShowReallocated)
                        {
                            int totalMinutes = 0;
                            int nrMinutes = 0;
                            int rMinutes = 0;

                            foreach (IOPairProcessedTO pairTO in DayPairList)
                            {
                                if (PassTypes.ContainsKey(pairTO.PassTypeID) && PassTypes[pairTO.PassTypeID].IsPass == Constants.passOnReader && pairTO.PassTypeID != Constants.absence)
                                    totalMinutes += (int)pairTO.EndTime.Subtract(pairTO.StartTime).TotalMinutes;

                                if (pairTO.EndTime.Hour == 23 && pairTO.EndTime.Minute == 59)
                                    totalMinutes++;
                            }

                            if (totalMinutes > Constants.dayDurationStandardShift * 60)
                            {
                                totalLbl.Text += " R";

                                nrMinutes = Constants.dayDurationStandardShift * 60;
                                rMinutes = totalMinutes - nrMinutes;

                                string tooltip = "";

                                int hours = nrMinutes / 60;
                                int minutes = nrMinutes % 60;

                                tooltip += rm.GetString("notreallocated", culture) + " " + hours.ToString().Trim().PadLeft(2, '0') + "h" + minutes.ToString().Trim().PadLeft(2, '0') + "min";

                                tooltip += Environment.NewLine;

                                hours = rMinutes / 60;
                                minutes = rMinutes % 60;

                                tooltip += rm.GetString("reallocated", culture) + " " + hours.ToString().Trim().PadLeft(2, '0') + "h" + minutes.ToString().Trim().PadLeft(2, '0') + "min";

                                totalLbl.ToolTip = tooltip;
                            }
                        }                                              
                        this.Controls.Add(totalLbl);                        
                    }
                }
                else
                {
                    Label lblSeparator = new Label();
                    lblSeparator.ID = "lblSeparator";
                    lblSeparator.Width = new Unit(780);
                    lblSeparator.Height = new Unit(5);
                    lblSeparator.CssClass = "emplWorkingDayUCSeparator";                    
                    this.Controls.Add(lblSeparator);
                }

                double currentPos = startPairsPos;
                int lblCounter = 0;
                int imgCounter = 0;
                
                foreach (int index in barStartEndPos.Keys)
                {
                    double startPos = barStartEndPos[index][0];
                    double endPos = barStartEndPos[index][1];

                    while (currentPos < startPos)
                    {
                        int totalMinutes = (int)(Math.Round((currentPos - startPairsPos) / Constants.minutWidth));
                        int hour = totalMinutes / 60;
                        int min = totalMinutes % 60;

                        if (hourPos.ContainsValue(currentPos))
                        {
                            Label lineLbl = new Label();
                            lineLbl.ID = "lbl" + lblCounter.ToString();
                            lineLbl.Width = new Unit(Constants.hourLineWidth);
                            lineLbl.Height = new Unit(height);
                            lineLbl.BackColor = Color.LightBlue;
                            lineLbl.CssClass = "emplWorkingDayUCEmptyLbl";
                            this.Controls.Add(lineLbl);
                            lblCounter++;

                            currentPos += Constants.hourLineWidth;
                            continue;
                        }
                        
                        double nextHourPos = hourPos[hour + 1];

                        double width = Math.Min(nextHourPos, startPos) - currentPos;
                        if (width < 0)
                            width = 0;
                        
                        Label hourLbl = new Label();
                        hourLbl.ID = "lbl" + lblCounter.ToString();
                        hourLbl.Width = new Unit(width);
                        hourLbl.Height = new Unit(height);
                        hourLbl.BackColor = BackColor;
                        hourLbl.CssClass = "emplWorkingDayUCEmptyLbl";                        
                        this.Controls.Add(hourLbl);
                        lblCounter++;

                        currentPos += width;          
                    }

                    string tooltip = "";
                    int ptID = -1;
                    uint recID = 0;
                    if (DayPairList.Count > index)
                    {
                        if (!DayPairList[index].IOPairDate.Equals(new DateTime()))
                            tooltip += DayPairList[index].IOPairDate.ToString(Constants.dateFormat) + " ";
                        if (!DayPairList[index].StartTime.Equals(new DateTime()))
                            tooltip += DayPairList[index].StartTime.ToString(Constants.timeFormat.Trim());
                        tooltip += "-";
                        if (!DayPairList[index].EndTime.Equals(new DateTime()))
                            tooltip += DayPairList[index].EndTime.ToString(Constants.timeFormat.Trim());
                        tooltip += Environment.NewLine;
                        if (PassTypes.ContainsKey(DayPairList[index].PassTypeID))
                        {
                            if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                                tooltip += PassTypes[DayPairList[index].PassTypeID].Description.Trim();
                            else
                                tooltip += PassTypes[DayPairList[index].PassTypeID].DescAlt.Trim();
                            tooltip += " (" + PassTypes[DayPairList[index].PassTypeID].PaymentCode.Trim() + ")";
                        }
                        if (ByLocations && Locations.ContainsKey(DayPairList[index].LocationID))
                        {
                            tooltip += Environment.NewLine;
                            tooltip += Locations[DayPairList[index].LocationID].Name.Trim();
                        }
                        if (HistDay)
                        {                            
                            tooltip += Environment.NewLine;
                            tooltip += rm.GetString("modifiedBy", culture) + ": " + DayPairList[index].ModifiedName.Trim();
                            tooltip += Environment.NewLine;
                            tooltip += rm.GetString("modifiedTime", culture) + ": " + DayPairList[index].ModifiedTime.ToString(Constants.dateFormat + " " + Constants.timeFormat);
                        }

                        ptID = DayPairList[index].PassTypeID;
                        recID = DayPairList[index].RecID;
                    }

                    double imgWidth = endPos - currentPos;
                    if (imgWidth < 0)
                        imgWidth = 0;
                    double imgHeight = height;
                    
                    // create image url
                    string url = "/ACTAWeb/ReportsWeb/BarSegment.aspx?width=" + imgWidth.ToString() + "&height=" + imgHeight.ToString();
                    if (DayPairList.Count > index)
                    {
                        url += "&pairRecID=" + DayPairList[index].RecID.ToString() + "&pairIndexInList=" + index.ToString().Trim();

                        url += "&inWS=" + isInWorkSchedule(DayPairList[index]).ToString();

                        if (ByLocations)
                        {
                            if (Locations.ContainsKey(DayPairList[index].LocationID))
                            {
                                url += "&barColor=" + Locations[DayPairList[index].LocationID].SegmentColor.Replace('#', ' ').Trim();
                            }
                        }
                        else if (PassTypes.ContainsKey(DayPairList[index].PassTypeID))
                        {
                            url += "&barColor=" + PassTypes[DayPairList[index].PassTypeID].SegmentColor.Replace('#', ' ').Trim();
                        }

                        if (HistDay)
                            url += "&hist=true";
                    }
                    else
                    {
                        url += "&inWS=true";

                        // calculate startH, startMin, endH, endMin
                        double startMinutes = (startPos - startPairsPos) / Constants.minutWidth;
                        int startH = (int)startMinutes / 60;
                        int startMin = (int)startMinutes % 60;

                        double endMinutes = (endPos - startPairsPos) / Constants.minutWidth;
                        int endH = (int)endMinutes / 60;
                        int endMin = (int)endMinutes % 60;

                        url += "&startH=" + startH.ToString().Trim() + "&startMin=" + startMin.ToString().Trim() + "&endH=" + endH.ToString().Trim() + "&endMin=" + endMin.ToString().Trim();
                    }

                    //if (passesTimeDirection.Length > 0)
                    //    url += "&passes=" + passesTimeDirection.Trim();

                    if (AllowChange)
                    {
                        ImageButton BarImgBtn = new ImageButton();
                        BarImgBtn.ID = "BarImgBtnChange" + imgCounter.ToString();
                        BarImgBtn.Width = new Unit(imgWidth);
                        BarImgBtn.Height = new Unit(imgHeight);
                        BarImgBtn.ToolTip = tooltip;
                        BarImgBtn.ImageUrl = url;
                        BarImgBtn.CssClass = "contentTop";
                        BarImgBtn.Attributes.Add("onclick", "return pairBarChange('" + Date.ToString(Constants.dateFormat.Trim()) + "', '" + Empl.EmployeeTypeID.ToString().Trim()
                            + "', '" + company.ToString().Trim() + "','" + Empl.EmployeeID.ToString().Trim() + "', '" + PostBackCtrlID + "', 'false', '-1', 'false', '-1', '" + recID.ToString().Trim() + "');");
                        BarImgBtn.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                        BarImgBtn.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                        imgCounter++;
                        this.Controls.Add(BarImgBtn);
                    }
                    else
                    {
                        // add click for verification
                        if (AllowVerify && DayPairList.Count > index && DayPairList[index].VerificationFlag == (int)Constants.Verification.NotVerified
                            && Session[Constants.sessionLoginCategoryPassTypes] != null && Session[Constants.sessionLoginCategoryPassTypes] is List<int>
                            && ((List<int>)Session[Constants.sessionLoginCategoryPassTypes]).Contains(DayPairList[index].PassTypeID))
                        {
                            ImageButton BarImgBtn = new ImageButton();
                            BarImgBtn.ID = "BarImgBtnVerify" + imgCounter.ToString();
                            BarImgBtn.Width = new Unit(imgWidth);
                            BarImgBtn.Height = new Unit(imgHeight);
                            BarImgBtn.ToolTip = tooltip;
                            BarImgBtn.ImageUrl = url;
                            BarImgBtn.CssClass = "contentTop";
                            //BarImgBtn.Attributes.Add("onclick", this.Page.ClientScript.GetPostBackEventReference(BarImgBtn, Constants.verificationClientScriptArg
                            //    + DayPairList[index].RecID.ToString().Trim()));
                            BarImgBtn.Attributes.Add("onclick", "return pairBarChange('" + Date.ToString(Constants.dateFormat.Trim()) + "', '" + Empl.EmployeeTypeID.ToString().Trim()
                            + "', '" + company.ToString().Trim() + "','" + Empl.EmployeeID.ToString().Trim() + "', '" + PostBackCtrlID + "', 'false', '-1', 'true', '" 
                            + ptID.ToString().Trim() + "', '" + recID.ToString().Trim() + "');");
                            BarImgBtn.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                            BarImgBtn.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                            imgCounter++;
                            this.Controls.Add(BarImgBtn);
                        }
                        // add click for confirmation
                        else if (AllowConfirm && DayPairList.Count > index && DayPairList[index].ConfirmationFlag == (int)Constants.Confirmation.NotConfirmed
                                && Session[Constants.sessionLoginCategoryPassTypes] != null && Session[Constants.sessionLoginCategoryPassTypes] is List<int>
                                && ((List<int>)Session[Constants.sessionLoginCategoryPassTypes]).Contains(DayPairList[index].PassTypeID))
                        {
                            ImageButton BarImgBtn = new ImageButton();
                            BarImgBtn.ID = "BarImgBtnConfirm" + imgCounter.ToString();
                            BarImgBtn.Width = new Unit(imgWidth);
                            BarImgBtn.Height = new Unit(imgHeight);
                            BarImgBtn.ToolTip = tooltip;
                            BarImgBtn.ImageUrl = url;
                            BarImgBtn.CssClass = "contentTop";
                            //BarImgBtn.Attributes.Add("onclick", this.Page.ClientScript.GetPostBackEventReference(BarImgBtn, Constants.confirmationClientScriptArg
                            //    + DayPairList[index].RecID.ToString().Trim()));
                            BarImgBtn.Attributes.Add("onclick", "return pairBarChange('" + Date.ToString(Constants.dateFormat.Trim()) + "', '" + Empl.EmployeeTypeID.ToString().Trim()
                            + "', '" + company.ToString().Trim() + "','" + Empl.EmployeeID.ToString().Trim() + "', '" + PostBackCtrlID + "', 'true', '" + ptID.ToString().Trim() 
                            + "', 'false', '-1', '" + recID.ToString().Trim() + "');");
                            BarImgBtn.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                            BarImgBtn.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                            imgCounter++;
                            this.Controls.Add(BarImgBtn);
                        }
                        else if (AllowUndoVerify && DayPairList.Count > index && DayUnverifiedPairList.Count > 0)
                        {
                            ImageButton BarImgBtn = new ImageButton();
                            BarImgBtn.ID = "BarImgBtnUndoVerify" + imgCounter.ToString();
                            BarImgBtn.Width = new Unit(imgWidth);
                            BarImgBtn.Height = new Unit(imgHeight);
                            BarImgBtn.ToolTip = tooltip;
                            BarImgBtn.ImageUrl = url;
                            BarImgBtn.CssClass = "contentTop";
                            BarImgBtn.Attributes.Add("onclick", this.Page.ClientScript.GetPostBackEventReference(BarImgBtn, Constants.undoVerificationClientScriptArg
                                + DayPairList[index].EmployeeID.ToString().Trim() + "|" + DayPairList[index].IOPairDate.Date.ToString(Constants.dateFormat) 
                                + "|" + DayUnverifiedPairList[0].ModifiedTime.ToString() + "|" + DayUnverifiedPairList[0].ModifiedBy));
                            BarImgBtn.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                            BarImgBtn.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                            imgCounter++;
                            this.Controls.Add(BarImgBtn);
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image BarImg = new System.Web.UI.WebControls.Image();
                            BarImg.ID = "BarImg" + imgCounter.ToString();
                            BarImg.Width = new Unit(imgWidth);
                            BarImg.Height = new Unit(imgHeight);
                            BarImg.ToolTip = tooltip;
                            BarImg.ImageUrl = url;
                            BarImg.CssClass = "contentTop";
                            imgCounter++;
                            this.Controls.Add(BarImg);
                        }                        
                    }
                    
                    currentPos += imgWidth;
                }

                while (currentPos <= endPairsPos)
                {
                    int totalMinutes = (int)(Math.Round((currentPos - startPairsPos) / Constants.minutWidth));
                    int hour = totalMinutes / 60;
                    int min = totalMinutes % 60;

                    if (hourPos.ContainsValue(currentPos))
                    {
                        Label lineLbl = new Label();
                        lineLbl.ID = "lbl" + lblCounter.ToString();
                        lineLbl.Width = new Unit(Constants.hourLineWidth);
                        lineLbl.Height = new Unit(height);
                        lineLbl.BackColor = Color.LightBlue;
                        lineLbl.CssClass = "emplWorkingDayUCEmptyLbl";
                        this.Controls.Add(lineLbl);
                        lblCounter++;

                        currentPos += Constants.hourLineWidth;
                        continue;
                    }

                    double nextHourPos = hourPos[hour + 1];

                    double width = Math.Min(nextHourPos, endPairsPos) - currentPos;
                    if (width < 0)
                        width = 0;

                    Label hourLbl = new Label();
                    hourLbl.ID = "lbl" + lblCounter.ToString();
                    hourLbl.Width = new Unit(width);
                    hourLbl.Height = new Unit(height);
                    hourLbl.BackColor = BackColor;
                    hourLbl.CssClass = "emplWorkingDayUCEmptyLbl";                    
                    this.Controls.Add(hourLbl);
                    lblCounter++;

                    currentPos += width;
                }

                if (ShowAlert)
                {
                    ImageButton warningBtn = new ImageButton();
                    warningBtn.ID = "warning";
                    warningBtn.Width = new Unit(height);
                    warningBtn.Height = new Unit(height);
                    warningBtn.ImageUrl = "/ACTAWeb/CommonWeb/images/warning.png";
                    warningBtn.CssClass = "contentTop";
                    warningBtn.Attributes.Add("onclick", "return pairBarPreview('" + Date.ToString(Constants.dateFormat.Trim()) + "', '" + Empl.EmployeeTypeID.ToString().Trim()
                            + "', '" + company.ToString().Trim() + "','" + Empl.EmployeeID.ToString().Trim() + "');");
                    warningBtn.Attributes.Add("onmouseover", "return document.body.style.cursor = 'pointer'");
                    warningBtn.Attributes.Add("onmouseout", "return document.body.style.cursor = 'default'");
                    this.Controls.Add(warningBtn);
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isInWorkSchedule(IOPairProcessedTO pair)
        {
            try
            {
                bool isInWS = false;
                foreach (WorkTimeIntervalTO interval in DayIntervalList)
                {
                    if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                    {
                        if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                            && pair.EndTime.Subtract(pair.StartTime).TotalMinutes <= interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes
                            && PassTypes.ContainsKey(pair.PassTypeID) && PassTypes[pair.PassTypeID].IsPass != Constants.overtimePassType)
                        {
                            isInWS = true;
                            break;
                        }
                    }
                    else if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeNightFlexi.Trim().ToUpper()))
                    {
                        if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay                            
                            && PassTypes.ContainsKey(pair.PassTypeID) && PassTypes[pair.PassTypeID].IsPass != Constants.overtimePassType)
                        {
                            isInWS = true;
                            break;
                        }
                    }
                    else
                    {
                        if (pair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                        {
                            isInWS = true;
                            break;
                        }
                    }
                }

                return isInWS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}