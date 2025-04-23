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
using System.Globalization;
using System.Resources;

using TransferObjects;
using Util;
using CommonWeb;
using Common;

namespace ACTAWebUI
{
    public partial class IOPairDataUC : System.Web.UI.UserControl
    {
        private IOPairProcessedTO pairTO = new IOPairProcessedTO();
        private List<WorkTimeIntervalTO> dayIntervals = new List<WorkTimeIntervalTO>();
        private List<WorkTimeIntervalTO> prevDayIntervals = new List<WorkTimeIntervalTO>();
        private List<WorkTimeIntervalTO> nextDayIntervals = new List<WorkTimeIntervalTO>();
        private Dictionary<string, RuleTO> rules = new Dictionary<string, RuleTO>();
        private bool isWorkAbsenceDay = false;
        private bool allowChange = true;
        private Dictionary<int, LocationTO> locDic = new Dictionary<int, LocationTO>();
        private List<PassTypeTO> passTypesAll = new List<PassTypeTO>();
        private Dictionary<int, PassTypeTO> passTypesAllDic = new Dictionary<int, PassTypeTO>();
        private Dictionary<int, PassTypeLimitTO> passTypeLimits = new Dictionary<int, PassTypeLimitTO>();
        private Dictionary<int, List<int>> confirmationTypes = new Dictionary<int, List<int>>();
        Dictionary<int, WorkTimeSchemaTO> schDict = new Dictionary<int, WorkTimeSchemaTO>();
        private WorkTimeSchemaTO timeSchema = new WorkTimeSchemaTO();
        private EmployeeAsco4TO emplAsco = new EmployeeAsco4TO();
        private EmployeeTO empl = new EmployeeTO();
        private List<int> wholeDayAbsenceByHour = new List<int>();
        private bool isExpatOut = false;
        private bool forVerification = false;
        private bool forConfirmation = false;
        private bool byLocations = false;
        private int confirmType = -1;
        private int verifyType = -1;

        private CultureInfo culture;
        private ResourceManager rm;
        private int prevPairPT = -1;
        private int nextPairPT = -1;
        private DateTime dayBegining = new DateTime();
        private DateTime dayEnd = new DateTime();

        public IOPairProcessedTO PairTO
        {
            get { return pairTO; }
            set { pairTO = value; }
        }

        public List<WorkTimeIntervalTO> DayIntervals
        {
            get { return dayIntervals; }
            set { dayIntervals = value; }
        }

        public List<WorkTimeIntervalTO> PrevDayIntervals
        {
            get { return prevDayIntervals; }
            set { prevDayIntervals = value; }
        }

        public List<WorkTimeIntervalTO> NextDayIntervals
        {
            get { return nextDayIntervals; }
            set { nextDayIntervals = value; }
        }

        public Dictionary<string, RuleTO> Rules
        {
            get { return rules; }
            set { rules = value; }
        }

        public List<PassTypeTO> PassTypesAll
        {
            get { return passTypesAll; }
            set { passTypesAll = value; }
        }

        public Dictionary<int, LocationTO> LocationsDic
        {
            get { return locDic; }
            set { locDic = value; }
        }

        public Dictionary<int, PassTypeTO> PassTypesAllDic
        {
            get { return passTypesAllDic; }
            set { passTypesAllDic = value; }
        }

        public Dictionary<int, PassTypeLimitTO> PassTypeLimits
        {
            get { return passTypeLimits; }
            set { passTypeLimits = value; }
        }

        public Dictionary<int, List<int>> ConfirmationTypes
        {
            get { return confirmationTypes; }
            set { confirmationTypes = value; }
        }

        public Dictionary<int, WorkTimeSchemaTO> SchDict
        {
            get { return schDict; }
            set { schDict = value; }
        }

        public bool IsWorkAbsenceDay
        {
            get { return isWorkAbsenceDay; }
            set { isWorkAbsenceDay = value; }
        }

        public bool IsExpatOut
        {
            get { return isExpatOut; }
            set { isExpatOut = value; }
        }

        public bool AllowChange
        {
            get { return allowChange; }
            set { allowChange = value; }
        }

        public WorkTimeSchemaTO EmplTimeSchema
        {
            get { return timeSchema; }
            set { timeSchema = value; }
        }

        public EmployeeAsco4TO EmplAsco
        {
            get { return emplAsco; }
            set { emplAsco = value; }
        }

        public EmployeeTO Empl
        {
            get { return empl; }
            set { empl = value; }
        }

        public List<int> WholeDayAbsenceByHour
        {
            get { return wholeDayAbsenceByHour; }
            set { wholeDayAbsenceByHour = value; }
        }

        public bool ForConfirmation
        {
            get { return forConfirmation; }
            set { forConfirmation = value; }
        }

        public bool ForVerification
        {
            get { return forVerification; }
            set { forVerification = value; }
        }

        public bool ByLocations
        {
            get { return byLocations; }
            set { byLocations = value; }
        }

        public int ConfirmType
        {
            get { return confirmType; }
            set { confirmType = value; }
        }

        public int VerifyType
        {
            get { return verifyType; }
            set { verifyType = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(IOPairDataUC).Assembly);

                Label lblSeparator1 = new Label();
                lblSeparator1.ID = "lblSeparator1";
                lblSeparator1.Width = new Unit(750);
                lblSeparator1.Height = new Unit(5);                
                this.Controls.Add(lblSeparator1);

                Label lblType = new Label();
                lblType.ID = "lblType";
                lblType.Width = new Unit(60);
                lblType.CssClass = "contentLbl";
                lblType.Text = rm.GetString("lblType", culture);                
                this.Controls.Add(lblType);

                DropDownList cbType = new DropDownList();
                cbType.ID = "cbType";
                cbType.Width = new Unit(180);
                cbType.AutoPostBack = true;
                cbType.CssClass = "contentDDList";
                populateType(cbType);
                cbType.SelectedValue = PairTO.PassTypeID.ToString().Trim();
                if (cbType.SelectedIndex < 0)
                    cbType.SelectedIndex = 0;
                cbType.SelectedIndexChanged += new EventHandler(cbType_SelectedIndexChanged);
                this.Controls.Add(cbType);

                Label lblStartTime = new Label();
                lblStartTime.ID = "lblStartTime";
                lblStartTime.Width = new Unit(80);
                lblStartTime.CssClass = "contentLbl";
                lblStartTime.Text = rm.GetString("lblStartTime", culture);                
                this.Controls.Add(lblStartTime);

                TextBox tbStartTime = new TextBox();
                tbStartTime.ID = "tbStartTime";
                tbStartTime.Width = new Unit(60);
                tbStartTime.AutoPostBack = true;
                tbStartTime.CssClass = "contentTb";
                if (PairTO.StartTime.Equals(new DateTime()))
                    tbStartTime.Text = "";
                else
                    tbStartTime.Text = PairTO.StartTime.ToString(Constants.timeFormat.Trim());
                tbStartTime.TextChanged += new EventHandler(tbStartTime_TextChanged);
                this.Controls.Add(tbStartTime);

                Label lblEndTime = new Label();
                lblEndTime.ID = "lblEndTime";
                lblEndTime.Width = new Unit(80);
                lblEndTime.CssClass = "contentLbl";
                lblEndTime.Text = rm.GetString("lblEndTime", culture);                
                this.Controls.Add(lblEndTime);

                TextBox tbEndTime = new TextBox();
                tbEndTime.ID = "tbEndTime";
                tbEndTime.Width = new Unit(60);
                tbEndTime.AutoPostBack = true;
                tbEndTime.CssClass = "contentTb";
                if (PairTO.EndTime.Equals(new DateTime()))
                    tbEndTime.Text = "";
                else
                    tbEndTime.Text = PairTO.EndTime.ToString(Constants.timeFormat.Trim());
                tbEndTime.TextChanged += new EventHandler(tbEndTime_TextChanged);
                this.Controls.Add(tbEndTime);

                Label lblDesc = new Label();
                lblDesc.ID = "lblDesc";
                lblDesc.Width = new Unit(80);
                lblDesc.CssClass = "contentLbl";
                lblDesc.Text = rm.GetString("lblDesc", culture);                
                this.Controls.Add(lblDesc);

                TextBox tbDesc = new TextBox();
                tbDesc.ID = "tbDesc";
                tbDesc.Width = new Unit(130);
                tbDesc.Height = new Unit(40);
                tbDesc.TextMode = TextBoxMode.MultiLine;
                tbDesc.AutoPostBack = true;
                tbDesc.Text = PairTO.Desc.Trim();
                tbDesc.TextChanged += new EventHandler(tbDesc_TextChanged);
                tbDesc.CssClass = "contentTb";
                this.Controls.Add(tbDesc);

                DropDownList cbLocation = null;
                if (ByLocations)
                {
                    Label lblSeparatorLoc1 = new Label();
                    lblSeparatorLoc1.ID = "lblSeparatorLoc1";
                    lblSeparatorLoc1.Width = new Unit(750);
                    lblSeparatorLoc1.Height = new Unit(5);                    
                    this.Controls.Add(lblSeparatorLoc1);

                    Label lblLocation = new Label();
                    lblLocation.ID = "lblLocation";
                    lblLocation.Width = new Unit(60);
                    lblLocation.CssClass = "contentLbl";
                    lblLocation.Text = rm.GetString("lblLocation", culture);                    
                    this.Controls.Add(lblLocation);

                    cbLocation = new DropDownList();
                    cbLocation.ID = "cbLocation";
                    cbLocation.Width = new Unit(180);
                    cbLocation.AutoPostBack = true;
                    cbLocation.CssClass = "contentDDList";
                    populateLocations(cbLocation);
                    cbLocation.SelectedValue = PairTO.LocationID.ToString().Trim();
                    if (cbLocation.SelectedIndex < 0)
                        cbLocation.SelectedIndex = 0;
                    cbLocation.SelectedIndexChanged += new EventHandler(cbLocation_SelectedIndexChanged);
                    this.Controls.Add(cbLocation);

                    Label lblSeparatorLoc2 = new Label();
                    lblSeparatorLoc2.ID = "lblSeparatorLoc2";
                    lblSeparatorLoc2.Width = new Unit(510);
                    lblSeparatorLoc2.Height = new Unit(5);                    
                    this.Controls.Add(lblSeparatorLoc2);

                    if (ForVerification || ForConfirmation)
                        cbLocation.Enabled = false;
                    else
                        cbLocation.Enabled = AllowChange;
                }

                Label lblSeparator2 = new Label();
                lblSeparator2.ID = "lblSeparator2";
                lblSeparator2.Width = new Unit(750);
                lblSeparator2.Height = new Unit(5);
                this.Controls.Add(lblSeparator2);

                cbType.Enabled = tbStartTime.Enabled = tbEndTime.Enabled = tbDesc.Enabled = AllowChange;

                if (AllowChange)
                {
                    // absence and justified overtime pairs can not be changed
                    if (!isChangingDurationValid(PairTO))
                        tbStartTime.Enabled = tbEndTime.Enabled = false;

                    // start can not be changed to delay pair
                    if (Rules.ContainsKey(Constants.RuleCompanyDelay) && PairTO.PassTypeID == Rules[Constants.RuleCompanyDelay].RuleValue)
                        tbStartTime.Enabled = false;

                    // pass type could not be change in month before if you aren't HRSSC
                    //if(PairTO.IOPairDate<= )
                    //{
                    //}
                    // pass type could not be changed to automatically created regular work, except for expat outs and where verification flag is 1
                    if (!(Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO 
                        && (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC))
                        && Rules.ContainsKey(Constants.RuleCompanyRegularWork) && PairTO.PassTypeID == Rules[Constants.RuleCompanyRegularWork].RuleValue
                        && PairTO.ManualCreated == (int)Constants.recordCreated.Automaticaly && !IsExpatOut && PairTO.VerificationFlag==0)
                        cbType.Enabled = false;

                    // if it is pair of second night shift and date is first day of month, check cutt off date of login user becouse that pair belongs to last day of previous month
                    if (PairTO.IOPairDate.Date == EmplAsco.DatetimeValue3.Date)
                    {
                        if (PassTypesAllDic.ContainsKey(PairTO.PassTypeID))
                        {
                            List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                            if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                            {
                                foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                                {
                                    dayPairs.Add(new IOPairProcessedTO(pair));
                                }
                            }

                            if (PassTypesAllDic[PairTO.PassTypeID].IsPass == Constants.overtimePassType)
                            {
                                int index = indexOf(dayPairs, PairTO);

                                int i = index - 1;

                                bool overtimeAfterShift = true;
                                while (i >= 0 && i < dayPairs.Count - 1)
                                {
                                    if (!dayPairs[i + 1].StartTime.Equals(dayPairs[i].EndTime))
                                    {
                                        overtimeAfterShift = false;
                                        break;
                                    }
                                    else if (PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID) && PassTypesAllDic[dayPairs[i].PassTypeID].IsPass != Constants.overtimePassType)
                                        break;

                                    i--;
                                }

                                if (overtimeAfterShift)
                                {
                                    if (i >= 0)
                                    {
                                        WorkTimeIntervalTO pairInterval = getPairInterval(dayPairs[i], dayPairs);

                                        if (!pairInterval.StartTime.Equals(new DateTime()) || !pairInterval.EndTime.Equals(new DateTime()))
                                        {
                                            if (!Common.Misc.isThirdShiftEndInterval(pairInterval))
                                            {
                                                cbType.Enabled = tbStartTime.Enabled = tbEndTime.Enabled = tbDesc.Enabled = false;

                                                if (cbLocation != null)
                                                    cbLocation.Enabled = false;
                                            }
                                        }
                                    }
                                    else if (!(dayPairs.Count > 0 && !dayPairs[0].StartTime.Equals(new DateTime()) && !dayPairs[0].EndTime.Equals(new DateTime())
                                            && dayPairs[0].StartTime.Hour == 0 && dayPairs[0].StartTime.Minute == 0))
                                    {
                                        cbType.Enabled = tbStartTime.Enabled = tbEndTime.Enabled = tbDesc.Enabled = false;

                                        if (cbLocation != null)
                                            cbLocation.Enabled = false;
                                    }
                                }
                            }
                            else
                            {
                                WorkTimeIntervalTO pairInterval = getPairInterval(PairTO, dayPairs);

                                if (!pairInterval.StartTime.Equals(new DateTime()) || !pairInterval.EndTime.Equals(new DateTime()))
                                {
                                    if (!Common.Misc.isThirdShiftEndInterval(pairInterval))
                                    {
                                        cbType.Enabled = tbStartTime.Enabled = tbEndTime.Enabled = tbDesc.Enabled = false;

                                        if (cbLocation != null)
                                            cbLocation.Enabled = false;
                                    }
                                }
                            }
                        }
                    }

                    // if it is pair of second night shift and date is first day of month, check cutt off date of login user becouse that pair belongs to last day of previous month
                    if (PairTO.IOPairDate.Day == 1)
                    {
                        if (PassTypesAllDic.ContainsKey(PairTO.PassTypeID))
                        {
                            List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                            if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                            {
                                foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                                {
                                    dayPairs.Add(new IOPairProcessedTO(pair));
                                }
                            }

                            if (PassTypesAllDic[PairTO.PassTypeID].IsPass == Constants.overtimePassType)
                            {
                                int index = indexOf(dayPairs, PairTO);

                                int i = index - 1;

                                bool overtimeAfterShift = true;
                                while (i >= 0 && i < dayPairs.Count - 1)
                                {
                                    if (!dayPairs[i + 1].StartTime.Equals(dayPairs[i].EndTime))
                                    {
                                        overtimeAfterShift = false;
                                        break;
                                    }
                                    else if (PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID) && PassTypesAllDic[dayPairs[i].PassTypeID].IsPass != Constants.overtimePassType)
                                        break;

                                    i--;
                                }

                                if (overtimeAfterShift)
                                {
                                    if (i >= 0)
                                    {
                                        WorkTimeIntervalTO pairInterval = getPairInterval(dayPairs[i], dayPairs);

                                        if (!pairInterval.StartTime.Equals(new DateTime()) || !pairInterval.EndTime.Equals(new DateTime()))
                                        {
                                            if (Common.Misc.isThirdShiftEndInterval(pairInterval) && checkCutOffDate(PairTO.IOPairDate.AddDays(-1).Date))
                                            //|| checkCutOffDate(PairTO.IOPairDate.Date))
                                            {
                                                cbType.Enabled = tbStartTime.Enabled = tbEndTime.Enabled = tbDesc.Enabled = false;

                                                if (cbLocation != null)
                                                    cbLocation.Enabled = false;
                                            }
                                        }
                                    }
                                    else if (dayPairs.Count > 0 && !dayPairs[0].StartTime.Equals(new DateTime()) && !dayPairs[0].EndTime.Equals(new DateTime())
                                            && dayPairs[0].StartTime.Hour == 0 && dayPairs[0].StartTime.Minute == 0 && checkCutOffDate(dayPairs[0].IOPairDate.AddDays(-1).Date))
                                    //|| checkCutOffDate(PairTO.IOPairDate.Date))
                                    {
                                        cbType.Enabled = tbStartTime.Enabled = tbEndTime.Enabled = tbDesc.Enabled = false;

                                        if (cbLocation != null)
                                            cbLocation.Enabled = false;
                                    }
                                }
                                //else if (checkCutOffDate(PairTO.IOPairDate.Date))
                                //    cbType.Enabled = tbStartTime.Enabled = tbEndTime.Enabled = tbDesc.Enabled = false;
                            }
                            else
                            {
                                WorkTimeIntervalTO pairInterval = getPairInterval(PairTO, dayPairs);

                                if (!pairInterval.StartTime.Equals(new DateTime()) || !pairInterval.EndTime.Equals(new DateTime()))
                                {
                                    if (Common.Misc.isThirdShiftEndInterval(pairInterval) && checkCutOffDate(PairTO.IOPairDate.AddDays(-1).Date))
                                    //|| checkCutOffDate(PairTO.IOPairDate.Date))
                                    {
                                        cbType.Enabled = tbStartTime.Enabled = tbEndTime.Enabled = tbDesc.Enabled = false;

                                        if (cbLocation != null)
                                            cbLocation.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    //else if (checkCutOffDate(PairTO.IOPairDate.Date))
                    //    cbType.Enabled = tbStartTime.Enabled = tbEndTime.Enabled = tbDesc.Enabled = false;
                }

                dayBegining = new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, 0, 0, 0);
                dayEnd = new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, 23, 59, 0);
            }
            catch (Exception ex)

            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairDataUC.Page_Load(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        public event ControlEventHandler BubbleClick;

        protected void OnBubbleClick(ControlEventArgs e)
        {
            if (BubbleClick != null)
                BubbleClick(this, e);
        }

        protected void tbStartTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ControlEventArgs args = new ControlEventArgs();

                DateTime newStart = CommonWeb.Misc.createTime(((TextBox)sender).Text.Trim());

                if (!newStart.Equals(new DateTime()))
                {
                    newStart = new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, newStart.Hour, newStart.Minute, 0);
                    // end can not be less than start
                    if (!PairTO.EndTime.Equals(new DateTime()) && newStart >= PairTO.EndTime)
                        args.Error = rm.GetString("invalidPairStart", culture);
                    {
                        // overtime pair
                        if (isOvertime(PairTO))
                        {
                            // change start time if it is different than rounding rule for overtime (WS or out of WS)
                            int minPresenceRounding = 1;
                            if (Rules.ContainsKey(Constants.RulePresenceRounding))
                            {
                                minPresenceRounding = Rules[Constants.RulePresenceRounding].RuleValue;

                                if (newStart.Minute % minPresenceRounding != 0)
                                {
                                    newStart = newStart.AddMinutes(minPresenceRounding - (newStart.Minute % minPresenceRounding));
                                    if (newStart > dayEnd)
                                        newStart = dayEnd;
                                }
                            }

                            if (!PairTO.EndTime.Equals(new DateTime()))
                            {
                                if (!IsWorkAbsenceDay)
                                {
                                    if (Rules.ContainsKey(Constants.RuleOvertimeRounding))
                                        minPresenceRounding = Rules[Constants.RuleOvertimeRounding].RuleValue;
                                }
                                else
                                {
                                    if (Rules.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                                        minPresenceRounding = Rules[Constants.RuleOvertimeRoundingOutWS].RuleValue;
                                }

                                int pairDuration = (int)PairTO.EndTime.Subtract(newStart).TotalMinutes;
                                if (PairTO.EndTime.TimeOfDay == new TimeSpan(23, 59, 0))
                                    pairDuration++;

                                if (pairDuration % minPresenceRounding != 0)
                                {
                                    newStart = newStart.AddMinutes(pairDuration % minPresenceRounding);
                                    if (newStart > dayEnd)
                                        newStart = dayEnd;
                                }
                            }

                            RearangeOvertimeStart(newStart, args);
                        }
                        else
                        {
                            // change start time if it is different than rounding rule
                            // if new start couse changing dalay pair, do dalay rounding
                            int minPresenceRounding = 1;

                            bool dalayChange = false;
                            if (Rules.ContainsKey(Constants.RuleCompanyDelay))
                            {
                                if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                                {
                                    foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                                    {
                                        if (pair.PassTypeID == Rules[Constants.RuleCompanyDelay].RuleValue && ((newStart > pair.StartTime && newStart < pair.EndTime)
                                            || PairTO.StartTime.Equals(pair.EndTime)))
                                        {
                                            dalayChange = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (dalayChange)
                            {
                                if (Rules.ContainsKey(Constants.RuleDelayRounding))
                                {
                                    minPresenceRounding = Rules[Constants.RuleDelayRounding].RuleValue;

                                    if (newStart.Minute % minPresenceRounding != 0)
                                    {
                                        newStart = newStart.AddMinutes(minPresenceRounding - (newStart.Minute % minPresenceRounding));
                                        if (newStart > dayEnd)
                                            newStart = dayEnd;                                        
                                    }
                                }
                            }
                            else if (Rules.ContainsKey(Constants.RulePresenceRounding))
                            {
                                minPresenceRounding = Rules[Constants.RulePresenceRounding].RuleValue;

                                if (newStart.Minute % minPresenceRounding != 0)
                                {
                                    if (Rules.ContainsKey(Constants.RuleCompanyRegularWork) && PairTO.PassTypeID == Rules[Constants.RuleCompanyRegularWork].RuleValue)
                                    {
                                        newStart = newStart.AddMinutes(minPresenceRounding - (newStart.Minute % minPresenceRounding));
                                        if (newStart > dayEnd)
                                            newStart = dayEnd;
                                    }
                                    else
                                    {
                                        newStart = newStart.AddMinutes(-(newStart.Minute % minPresenceRounding));
                                        if (newStart < dayBegining)
                                            newStart = dayBegining;
                                    }
                                }
                            }

                            RearangePairStart(newStart, args);
                        }
                    }
                }
                else
                {
                    args.Error = rm.GetString("invalidPairStart", culture);
                }

                OnBubbleClick(args);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairDataUC.tbStartTime_TextChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void tbEndTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ControlEventArgs args = new ControlEventArgs();

                DateTime newEnd = CommonWeb.Misc.createTime(((TextBox)sender).Text.Trim());

                if (!newEnd.Equals(new DateTime()))
                {
                    newEnd = new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, newEnd.Hour, newEnd.Minute, 0);

                    // end can not be less than start
                    if (!PairTO.StartTime.Equals(new DateTime()) && newEnd <= PairTO.StartTime)
                        args.Error = rm.GetString("invalidPairEnd", culture);
                    else
                    {
                        // overtime pair
                        if (isOvertime(PairTO))
                        {
                            // change end time if it is different than rounding rule for overtime (WS or out of WS)
                            int minPresenceRounding = 1;
                            if (Rules.ContainsKey(Constants.RulePresenceRounding))
                            {
                                minPresenceRounding = Rules[Constants.RulePresenceRounding].RuleValue;

                                if (newEnd.TimeOfDay != new TimeSpan(23, 59, 0) && newEnd.Minute % minPresenceRounding != 0)
                                {
                                    newEnd = newEnd.AddMinutes(-(newEnd.Minute % minPresenceRounding));
                                    if (newEnd < dayBegining)
                                        newEnd = dayBegining;
                                }
                            }

                            if (!PairTO.StartTime.Equals(new DateTime()))
                            {
                                if (!IsWorkAbsenceDay)
                                {
                                    if (Rules.ContainsKey(Constants.RuleOvertimeRounding))
                                        minPresenceRounding = Rules[Constants.RuleOvertimeRounding].RuleValue;
                                }
                                else
                                {
                                    if (Rules.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                                        minPresenceRounding = Rules[Constants.RuleOvertimeRoundingOutWS].RuleValue;
                                }

                                int pairDuration = (int)newEnd.Subtract(PairTO.StartTime).TotalMinutes;
                                
                                if (newEnd.TimeOfDay == new TimeSpan(23, 59, 0))
                                    pairDuration++;

                                if (pairDuration % minPresenceRounding != 0)
                                {
                                    newEnd = newEnd.AddMinutes(-(newEnd.Subtract(PairTO.StartTime).TotalMinutes % minPresenceRounding));
                                    if (newEnd < dayBegining)
                                        newEnd = dayBegining;
                                }
                            }
                            
                            RearangeOvertimeEnd(newEnd, args);
                        }
                        else
                        {
                            // change end time if it is different than rounding rule
                            int minPresenceRounding = 1;
                            if (Rules.ContainsKey(Constants.RulePresenceRounding))
                            {
                                minPresenceRounding = Rules[Constants.RulePresenceRounding].RuleValue;

                                if (newEnd.TimeOfDay != new TimeSpan(23, 59, 0) && newEnd.Minute % minPresenceRounding != 0)
                                {
                                    if (Rules.ContainsKey(Constants.RuleCompanyRegularWork) && PairTO.PassTypeID == Rules[Constants.RuleCompanyRegularWork].RuleValue)
                                    {
                                        newEnd = newEnd.AddMinutes(-(newEnd.Minute % minPresenceRounding));
                                        if (newEnd < dayBegining)
                                            newEnd = dayBegining;
                                    }
                                    else
                                    {
                                        newEnd = newEnd.AddMinutes(minPresenceRounding - (newEnd.Minute % minPresenceRounding));
                                        if (newEnd > dayEnd)
                                            newEnd = dayEnd;
                                    }
                                }
                            }

                            RearangePairEnd(newEnd, args);
                        }
                    }
                }
                else
                {
                    args.Error = rm.GetString("invalidPairEnd", culture);
                }

                OnBubbleClick(args);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairDataUC.tbStartTime_TextChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ControlEventArgs args = new ControlEventArgs();

                if (PairTO.StartTime.Equals(new DateTime()) || PairTO.EndTime.Equals(new DateTime()))
                    args.Error = rm.GetString("setPairTime", culture);
                else if (((DropDownList)sender).SelectedIndex >= 0)
                {
                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                    if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                    {
                        foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                        {
                            dayPairs.Add(new IOPairProcessedTO(pair));
                        }
                    }

                    Dictionary<int, EmployeeCounterValueTO> emplCounters = new Dictionary<int, EmployeeCounterValueTO>();
                    if (Session[Constants.sessionEmplCounters] != null && Session[Constants.sessionEmplCounters] is Dictionary<int, EmployeeCounterValueTO>)
                    {
                        foreach (int counterType in ((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters]).Keys)
                        {
                            emplCounters.Add(counterType, new EmployeeCounterValueTO(((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters])[counterType]));
                        }
                    }

                    int index = indexOf(dayPairs, PairTO);

                    if (index >= 0 && index < dayPairs.Count)
                    {
                        int ptID = -1;
                        if (int.TryParse(((DropDownList)sender).SelectedValue.Trim(), out ptID))
                        {
                            if (ptID == Constants.ptEmptyInterval && Rules.ContainsKey(Constants.RuleCompanyOfficialTrip))
                            {
                                // validate if there is official trip pair and if duration will be less then interval duraton
                                bool foundOfficialTrip = false;
                                IOPairProcessedTO officialTripPair = new IOPairProcessedTO();
                                int pairIndex = index - 1;
                                DateTime pairStart = PairTO.StartTime;
                                int duration = 0;

                                while (pairIndex >= 0 && pairIndex < dayPairs.Count && (dayPairs[pairIndex].EndTime.TimeOfDay.Equals(pairStart.TimeOfDay)
                                    || (pairStart.Hour == 0 && pairStart.Minute == 0 && dayPairs[pairIndex].EndTime.Hour == 23 && dayPairs[pairIndex].EndTime.Minute == 59
                                    && pairStart.Date.Equals(dayPairs[pairIndex].IOPairDate.Date.AddDays(1))) && dayPairs[pairIndex].PassTypeID == Constants.absence) 
                                    && dayPairs[pairIndex].PassTypeID != Constants.ptEmptyInterval && !foundOfficialTrip)
                                {
                                    pairStart = dayPairs[pairIndex].StartTime;

                                    int pairDuration = (int)dayPairs[pairIndex].EndTime.Subtract(dayPairs[pairIndex].StartTime).TotalMinutes;

                                    if (dayPairs[pairIndex].EndTime.Hour == 23 && dayPairs[pairIndex].EndTime.Minute == 59)
                                        pairDuration++;

                                    duration += pairDuration;

                                    if (dayPairs[pairIndex].PassTypeID == Rules[Constants.RuleCompanyOfficialTrip].RuleValue)
                                    {
                                        foundOfficialTrip = true;
                                        officialTripPair = dayPairs[pairIndex];
                                    }

                                    pairIndex--;
                                }

                                if (!foundOfficialTrip)
                                {
                                    pairIndex = index + 1;
                                    DateTime pairEnd = PairTO.EndTime;
                                    duration = 0;

                                    while (pairIndex >= 0 && pairIndex < dayPairs.Count && (dayPairs[pairIndex].StartTime.TimeOfDay.Equals(pairEnd.TimeOfDay)
                                    || (pairEnd.Hour == 23 && pairEnd.Minute == 59 && dayPairs[pairIndex].StartTime.Hour == 0 && dayPairs[pairIndex].StartTime.Minute == 0
                                    && pairEnd.Date.Equals(dayPairs[pairIndex].IOPairDate.Date.AddDays(-1)) && dayPairs[pairIndex].PassTypeID == Constants.absence)) 
                                    && dayPairs[pairIndex].PassTypeID != Constants.ptEmptyInterval && !foundOfficialTrip)
                                    {
                                        pairEnd = dayPairs[pairIndex].EndTime;

                                        int pairDuration = (int)dayPairs[pairIndex].EndTime.Subtract(dayPairs[pairIndex].StartTime).TotalMinutes;

                                        if (dayPairs[pairIndex].EndTime.Hour == 23 && dayPairs[pairIndex].EndTime.Minute == 59)
                                            pairDuration++;

                                        duration += pairDuration;

                                        if (dayPairs[pairIndex].PassTypeID == Rules[Constants.RuleCompanyOfficialTrip].RuleValue)
                                        {
                                            foundOfficialTrip = true;
                                            officialTripPair = dayPairs[pairIndex];
                                        }

                                        pairIndex++;
                                    }
                                }

                                if (foundOfficialTrip)
                                {
                                    // check new duration
                                    WorkTimeIntervalTO interval = getPairInterval(officialTripPair, dayPairs);

                                    int intervalDuration = (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes;

                                    if (Common.Misc.isThirdShiftEndInterval(interval))
                                    {
                                        // get last interval from previous day
                                        foreach (WorkTimeIntervalTO prevInterval in PrevDayIntervals)
                                        {
                                            if (Common.Misc.isThirdShiftBeginningInterval(prevInterval))
                                            {
                                                intervalDuration += (int)prevInterval.EndTime.TimeOfDay.Subtract(prevInterval.StartTime.TimeOfDay).TotalMinutes + 1;
                                                break;
                                            }
                                        }
                                    }

                                    if (Common.Misc.isThirdShiftBeginningInterval(interval))
                                    {
                                        // get first interval from next day
                                        foreach (WorkTimeIntervalTO nextInterval in NextDayIntervals)
                                        {
                                            if (Common.Misc.isThirdShiftEndInterval(nextInterval))
                                            {
                                                intervalDuration += (int)nextInterval.EndTime.TimeOfDay.Subtract(nextInterval.StartTime.TimeOfDay).TotalMinutes + 1;
                                                break;
                                            }
                                        }
                                    }

                                    if (duration < intervalDuration)
                                    {
                                        args.Error = rm.GetString("invalidOfficialTripDuration", culture);
                                    }
                                }
                            }

                            List<IOPairProcessedTO> oldPairs = new List<IOPairProcessedTO>();
                            oldPairs.Add(new IOPairProcessedTO(PairTO));
                            List<IOPairProcessedTO> newPairs = new List<IOPairProcessedTO>();
                            IOPairProcessedTO newPair = new IOPairProcessedTO(PairTO);
                            newPair.PassTypeID = ptID;
                            newPairs.Add(newPair);
                            Dictionary<DateTime, WorkTimeSchemaTO> daySchemas = new Dictionary<DateTime, WorkTimeSchemaTO>();
                            daySchemas.Add(PairTO.IOPairDate.Date, EmplTimeSchema);
                            Dictionary<DateTime, List<WorkTimeIntervalTO>> dayIntervalsList = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();
                            // add previous day and next day intervals becouse of validation of using vacation before bank hours are used
                            dayIntervalsList.Add(PairTO.IOPairDate.Date.AddDays(-1), PrevDayIntervals);                            
                            dayIntervalsList.Add(PairTO.IOPairDate.Date, DayIntervals);
                            dayIntervalsList.Add(PairTO.IOPairDate.Date.AddDays(1), NextDayIntervals);
                            List<DateTime> exceptDates = new List<DateTime>();
                            foreach (IOPairProcessedTO pair in dayPairs)
                            {
                                if (!exceptDates.Contains(pair.IOPairDate.Date))
                                    exceptDates.Add(pair.IOPairDate.Date);
                            }
                            //20.10.2017 Miodrag Izdvajanje logike za supervizore
                            string error = "";
                            int tt = (int)Constants.Categories.TL;
                            int wcdr = (int)Constants.Categories.WCManager;
                            // if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == tt)
                            if (((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == tt || ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == wcdr)
                            {
                                error = Common.Misc.validatePairsPassType(PairTO.EmployeeID, EmplAsco, PairTO.IOPairDate, PairTO.IOPairDate, newPairs, oldPairs, dayPairs, ref emplCounters, Rules,
                                  PassTypesAllDic, PassTypeLimits, SchDict, daySchemas, dayIntervalsList, null, null, null, dayPairs, exceptDates, null, Session[Constants.sessionConnection],
                                  CommonWeb.Misc.checkLimit(Session[Constants.sessionLoginCategory]), true, true, false,true);

                            }
                            else
                            {

                                error = Common.Misc.validatePairsPassType(PairTO.EmployeeID, EmplAsco, PairTO.IOPairDate, PairTO.IOPairDate, newPairs, oldPairs, dayPairs, ref emplCounters, Rules,
                                 PassTypesAllDic, PassTypeLimits, SchDict, daySchemas, dayIntervalsList, null, null, null, dayPairs, exceptDates, null, Session[Constants.sessionConnection],
                                 CommonWeb.Misc.checkLimit(Session[Constants.sessionLoginCategory]), true, true, false);
                                //error = validatePairsPassType(fromDate.Date, toDate.Date, emplPairs[empl.EmployeeID], oldPairsList, ref oldCounters, rules, passTypesAll, ptLimits, daySchemas,
                                //    fromWeekAnnualPairs, toWeekAnnualPairs);
                                //}
                            }//mm
                        
                            //if (validatePairPassType(ref emplCounters, PairTO.PassTypeID, ptID, dayPairs, args))
                            if (error.Trim().Equals(""))
                            {
                                int nightShiftIndex = -1;
                                // if pair is whole day absence from night shift, change corresponding night shifts pairs form previous/next day
                                if ((ptID == Constants.absence || (PassTypesAllDic.ContainsKey(ptID) && PassTypesAllDic[ptID].IsPass == Constants.wholeDayAbsence)
                                    || (PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID) && PassTypesAllDic[dayPairs[index].PassTypeID].IsPass == Constants.wholeDayAbsence))
                                    && isWholeIntervalPair(dayPairs[index], getPairInterval(dayPairs[index], dayPairs)))
                                {
                                    if (dayPairs[index].StartTime.Hour == 0 && dayPairs[index].StartTime.Minute == 0 && index > 0
                                        && dayPairs[index - 1].IOPairDate.Date.Equals(dayPairs[index].IOPairDate.AddDays(-1).Date))
                                        nightShiftIndex = index - 1;
                                    else if (dayPairs[index].EndTime.Hour == 23 && dayPairs[index].EndTime.Minute == 59 && index < dayPairs.Count - 1
                                        && dayPairs[index + 1].IOPairDate.Date.Equals(dayPairs[index].IOPairDate.AddDays(1).Date))
                                        nightShiftIndex = index + 1;
                                }

                                if (ptID == Constants.ptEmptyInterval && Rules.ContainsKey(Constants.RuleCompanyInitialNightOvertime)
                                    && dayPairs[index].PassTypeID == Rules[Constants.RuleCompanyInitialNightOvertime].RuleValue)
                                    dayPairs[index].OldPassTypeID = dayPairs[index].PassTypeID;
                                dayPairs[index].PassTypeID = ptID;
                                dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                                dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                                if (nightShiftIndex != -1)
                                {
                                    // validate counters before changing type to second night shift pair
                                    oldPairs = new List<IOPairProcessedTO>();
                                    oldPairs.Add(new IOPairProcessedTO(dayPairs[nightShiftIndex]));
                                    newPairs = new List<IOPairProcessedTO>();
                                    newPair = new IOPairProcessedTO(dayPairs[nightShiftIndex]);
                                    newPair.PassTypeID = ptID;
                                    newPairs.Add(newPair);
                                    daySchemas = new Dictionary<DateTime, WorkTimeSchemaTO>();
                                    daySchemas.Add(dayPairs[nightShiftIndex].IOPairDate.Date, EmplTimeSchema);
                                    dayIntervalsList = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();
                                    dayIntervalsList.Add(PairTO.IOPairDate.Date, DayIntervals);

                                    error = Common.Misc.validatePairsPassType(dayPairs[nightShiftIndex].EmployeeID, EmplAsco, dayPairs[nightShiftIndex].IOPairDate, dayPairs[nightShiftIndex].IOPairDate, newPairs, oldPairs, dayPairs, ref emplCounters, Rules,
                                        PassTypesAllDic, PassTypeLimits, SchDict, daySchemas, dayIntervalsList, null, null, null, dayPairs, exceptDates, null, Session[Constants.sessionConnection], 
                                        CommonWeb.Misc.checkLimit(Session[Constants.sessionLoginCategory]), false, false, false);

                                    if (error.Trim().Equals(""))
                                    {
                                        dayPairs[nightShiftIndex].PassTypeID = ptID;
                                        dayPairs[nightShiftIndex].ManualCreated = (int)Constants.recordCreated.Manualy;
                                        dayPairs[nightShiftIndex].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                                        if (!CommonWeb.Misc.createDate(dayPairs[index].Desc).Equals(new DateTime()))
                                            dayPairs[nightShiftIndex].Desc = dayPairs[index].Desc; // if description contains sickness starting date
                                    }
                                    else
                                        args.Error = rm.GetString(error, culture);
                                }

                                if (args.Error.Trim().Equals(""))
                                {
                                    if (PassTypesAllDic.ContainsKey(ptID))
                                    {
                                        dayPairs[index].ConfirmationFlag = PassTypesAllDic[ptID].ConfirmFlag;

                                        if (ForConfirmation && dayPairs[index].ConfirmationFlag == (int)Constants.Confirmation.Confirmed)
                                        {
                                            dayPairs[index].ConfirmationTime = DateTime.Now;
                                            dayPairs[index].ConfirmedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                                            if (nightShiftIndex != -1)
                                            {
                                                dayPairs[nightShiftIndex].ConfirmationTime = DateTime.Now;
                                                dayPairs[nightShiftIndex].ConfirmedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                            }
                                        }

                                        if (nightShiftIndex != -1)
                                            dayPairs[nightShiftIndex].ConfirmationFlag = PassTypesAllDic[ptID].ConfirmFlag;

                                        // TL and HRSSC automatically verify pairs for other employees - unjustified is verified by default, other not verified
                                        if (CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], PairTO.EmployeeID))
                                        {
                                            if (PassTypesAllDic[ptID].VerificationFlag == (int)Constants.Verification.NotVerified)
                                            {
                                                dayPairs[index].VerificationFlag = (int)Constants.Verification.Verified;
                                                dayPairs[index].VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                                dayPairs[index].VerifiedTime = DateTime.Now;

                                                if (nightShiftIndex != -1)
                                                {
                                                    dayPairs[nightShiftIndex].VerificationFlag = (int)Constants.Verification.Verified;
                                                    dayPairs[nightShiftIndex].VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                                    dayPairs[nightShiftIndex].VerifiedTime = DateTime.Now;
                                                }
                                            }
                                            else
                                            {
                                                dayPairs[index].VerificationFlag = (int)Constants.Verification.Verified;
                                                dayPairs[index].VerifiedBy = "";
                                                dayPairs[index].VerifiedTime = new DateTime();

                                                if (nightShiftIndex != -1)
                                                {
                                                    dayPairs[nightShiftIndex].VerificationFlag = (int)Constants.Verification.Verified;
                                                    dayPairs[nightShiftIndex].VerifiedBy = "";
                                                    dayPairs[nightShiftIndex].VerifiedTime = new DateTime();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[ptID], ForVerification, ForConfirmation);

                                            if (nightShiftIndex != -1)
                                                dayPairs[nightShiftIndex].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[ptID], ForVerification, ForConfirmation);
                                        }
                                    }
                                    else if (ptID == Constants.ptEmptyInterval)
                                    {
                                        dayPairs[index].ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                                        dayPairs[index].VerificationFlag = (int)Constants.Verification.Verified;
                                    }
                                }
                            }
                            else
                                args.Error = rm.GetString(error, culture);
                        }

                        // check minimal presence for regular work or overtime pairs
                        int minimalPresence = 1;
                        if (Rules.ContainsKey(Constants.RuleMinPresence))
                            minimalPresence = Rules[Constants.RuleMinPresence].RuleValue;

                        int minimalPresenceOvertime = 1;
                        if (Rules.ContainsKey(Constants.RuleOvertimeMinimum))
                            minimalPresenceOvertime = Rules[Constants.RuleOvertimeMinimum].RuleValue;

                        if (Rules.ContainsKey(Constants.RuleCompanyRegularWork) && ptID == Rules[Constants.RuleCompanyRegularWork].RuleValue)
                        {
                            // validate duration due to minimal presence
                            int pairDuration = ((int)dayPairs[index].EndTime.TimeOfDay.TotalMinutes - (int)dayPairs[index].StartTime.TimeOfDay.TotalMinutes);

                            // if it is last pair from first night shift interval, add one minute
                            if (dayPairs[index].EndTime.Hour == 23 && dayPairs[index].EndTime.Minute == 59)
                                pairDuration++;

                            if (dayPairs[index].EndTime > dayPairs[index].StartTime && pairDuration < minimalPresence)
                            {
                                args.Error = rm.GetString("pairLessThenMinimum", culture);
                            }
                        }

                        if (PassTypesAllDic.ContainsKey(ptID) && PassTypesAllDic[ptID].IsPass == Constants.overtimePassType && ptID != Constants.overtimeUnjustified)
                        {                            
                            // validate duration due to minimal overtime presence
                            int pairDuration = ((int)dayPairs[index].EndTime.TimeOfDay.TotalMinutes - (int)dayPairs[index].StartTime.TimeOfDay.TotalMinutes);

                            // if it is last pair from first night shift interval, add one minute
                            if (dayPairs[index].EndTime.Hour == 23 && dayPairs[index].EndTime.Minute == 59)
                                pairDuration++;

                            if (dayPairs[index].EndTime > dayPairs[index].StartTime && pairDuration < minimalPresenceOvertime)
                            {
                                args.Error = rm.GetString("overtimeLessThenMinimum", culture);
                            }
                        }

                        if (args.Error.Trim().Equals(""))
                        {
                            List<IOPairProcessedTO> sessionDayPairs = new List<IOPairProcessedTO>();
                            foreach (IOPairProcessedTO pair in dayPairs)
                            {
                                sessionDayPairs.Add(new IOPairProcessedTO(pair));
                            }

                            Session[Constants.sessionDayPairs] = sessionDayPairs;

                            Dictionary<int, EmployeeCounterValueTO> sessionEmplCounters = new Dictionary<int, EmployeeCounterValueTO>();
                            foreach (int counterType in emplCounters.Keys)
                            {
                                sessionEmplCounters.Add(counterType, emplCounters[counterType]);
                            }

                            Session[Constants.sessionEmplCounters] = sessionEmplCounters;
                        }
                    }
                }
                else
                {
                    args.Error = rm.GetString("noSelectedPassType", culture);
                }
                                
                OnBubbleClick(args);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairDataUC.cbType_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void tbDesc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                {
                    foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                    {
                        dayPairs.Add(new IOPairProcessedTO(pair));
                    }
                }

                int index = indexOf(dayPairs, PairTO);

                if (index >= 0 && index < dayPairs.Count)
                {
                    dayPairs[index].Desc = ((TextBox)sender).Text;
                    dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                    dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                    // if description is sickness date start, change other pair from night shift                    
                    if (!CommonWeb.Misc.createDate(dayPairs[index].Desc).Equals(new DateTime()))
                    {
                        int nightShiftIndex = -1;
                        // if pair is whole day absence from night shift, change corresponding night shifts pairs form previous/next day
                        if (PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID) && PassTypesAllDic[dayPairs[index].PassTypeID].IsPass == Constants.wholeDayAbsence)
                        {
                            if (dayPairs[index].StartTime.Hour == 0 && dayPairs[index].StartTime.Minute == 0 && index > 0
                                && dayPairs[index - 1].IOPairDate.Date.Equals(dayPairs[index].IOPairDate.AddDays(-1).Date))
                                nightShiftIndex = index - 1;
                            else if (dayPairs[index].EndTime.Hour == 23 && dayPairs[index].EndTime.Minute == 59 && index < dayPairs.Count - 1
                                && dayPairs[index + 1].IOPairDate.Date.Equals(dayPairs[index].IOPairDate.AddDays(1).Date))
                                nightShiftIndex = index + 1;
                        }

                        if (nightShiftIndex != -1 && nightShiftIndex >= 0 && nightShiftIndex < dayPairs.Count)
                            dayPairs[nightShiftIndex].Desc = dayPairs[index].Desc;
                    }

                    List<IOPairProcessedTO> sessionDayPairs = new List<IOPairProcessedTO>();
                    foreach (IOPairProcessedTO pair in dayPairs)
                    {
                        sessionDayPairs.Add(new IOPairProcessedTO(pair));
                    }

                    Session[Constants.sessionDayPairs] = sessionDayPairs;
                }

                OnBubbleClick(new ControlEventArgs());
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairDataUC.tbStartTime_TextChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        protected void cbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int locID = Constants.locationOut;
                if (!int.TryParse(((DropDownList)sender).SelectedValue.Trim(), out locID))
                    locID = Constants.locationOut;

                List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                {
                    foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                    {
                        dayPairs.Add(new IOPairProcessedTO(pair));
                    }
                }

                int index = indexOf(dayPairs, PairTO);

                if (index >= 0 && index < dayPairs.Count)
                {
                    dayPairs[index].LocationID = locID;
                    dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                    dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                    
                    List<IOPairProcessedTO> sessionDayPairs = new List<IOPairProcessedTO>();
                    foreach (IOPairProcessedTO pair in dayPairs)
                    {
                        sessionDayPairs.Add(new IOPairProcessedTO(pair));
                    }

                    Session[Constants.sessionDayPairs] = sessionDayPairs;
                }

                OnBubbleClick(new ControlEventArgs());
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in IOPairDataUC.cbLocation_SelectedIndexChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }

        private void populateType(DropDownList cbType)
        {
            try
            {
                List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                {
                    foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                    {
                        dayPairs.Add(new IOPairProcessedTO(pair));
                    }
                }

                List<PassTypeTO> ptArray = new List<PassTypeTO>();
                bool isAbsencePair = isWholeDayAbsence();
                bool isOvertimePair = isOvertime(PairTO);
                bool isPersonalHolidayPair = isPersonalHoliday(PairTO, dayPairs);
                bool isNationalHolidayPair = isNationalHoliday(PairTO, dayPairs);
                bool isOfficialTripPair = false;
                if (!isAbsencePair)
                    isOfficialTripPair = isOfficialTripCandidate(PairTO);

                if (!PassTypesAllDic.ContainsKey(PairTO.PassTypeID) && PairTO.PassTypeID != Constants.ptEmptyInterval)
                    ptArray.Add(PassTypesAllDic[PairTO.PassTypeID]);
                else
                {
                    foreach (PassTypeTO pt in PassTypesAll)
                    {
                        if (!(Session[Constants.sessionLoginCategoryPassTypes] != null && Session[Constants.sessionLoginCategoryPassTypes] is List<int>
                                && ((List<int>)Session[Constants.sessionLoginCategoryPassTypes]).Contains(pt.PassTypeID)) && pt.PassTypeID != PairTO.PassTypeID)
                            continue;

                        if (Rules.ContainsKey(Constants.RuleCompanyInitialNightOvertime) && pt.PassTypeID == Rules[Constants.RuleCompanyInitialNightOvertime].RuleValue
                            && pt.PassTypeID != PairTO.PassTypeID)
                            continue;

                        if (ForConfirmation)
                        {
                            if (ConfirmType == pt.PassTypeID || (ConfirmationTypes.ContainsKey(ConfirmType) && ConfirmationTypes[ConfirmType].Contains(pt.PassTypeID))
                                || pt.PassTypeID == PairTO.PassTypeID)
                                ptArray.Add(pt);
                            else
                                continue;
                        }
                        else if (ForVerification)
                        {
                            if (VerifyType == pt.PassTypeID || (isOvertimePair && Rules.ContainsKey(Constants.RuleCompanyOvertimeRejected) && pt.PassTypeID == Rules[Constants.RuleCompanyOvertimeRejected].RuleValue)
                                || (!isOvertimePair && pt.PassTypeID == Constants.absence) || pt.PassTypeID == PairTO.PassTypeID)
                                ptArray.Add(pt);
                            else
                                continue;
                        }
                        else
                        {
                            // if pair needs confirmation, do not get confirmation type
                            if (PairTO.ConfirmationFlag == (int)Constants.Confirmation.NotConfirmed && ConfirmationTypes.ContainsKey(PairTO.PassTypeID)
                                && ConfirmationTypes[PairTO.PassTypeID].Contains(pt.PassTypeID))
                                continue;

                            // if pair is overtime, get overtime types    
                            if (isOvertimePair)
                            {
                                if (pt.IsPass == Constants.overtimePassType)
                                    ptArray.Add(pt);
                            }
                            else
                            {
                                // if pair is on personal holiday, get personal holiday, do not get annual leave and paid leaves (pass types with limits)
                                if (isPersonalHolidayPair && !IsExpatOut)
                                {
                                    if (Rules.ContainsKey(Constants.RulePersonalHolidayPassType) && pt.PassTypeID == Rules[Constants.RulePersonalHolidayPassType].RuleValue
                                        || Rules.ContainsKey(Constants.RuleWorkOnPersonalHolidayPassType) && pt.PassTypeID == Rules[Constants.RuleWorkOnPersonalHolidayPassType].RuleValue) //8.1.2018 Miodrag /prikaz tipa prolaska "Rad na licni praznik" danima kada je zaposlenom namesten licni praznik
                                        ptArray.Add(pt);

                                    if ((Rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && pt.PassTypeID == Rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                        || (Rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && pt.PassTypeID == Rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue)
                                        || pt.LimitCompositeID != -1
                                        || pt.LimitElementaryID != -1 || pt.LimitOccasionID != -1)
                                        continue;
                                }

                                // if pair is on national holiday, get holiday, do not get annual leave (except for industrial schema) and paid leaves (pass types with limits)
                                if (isNationalHolidayPair && !IsExpatOut)
                                {
                                    if (!EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()) && Rules.ContainsKey(Constants.RuleHolidayPassType)
                                        && pt.PassTypeID == Rules[Constants.RuleHolidayPassType].RuleValue)
                                        ptArray.Add(pt);

                                    if (!EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()) && Rules.ContainsKey(Constants.RuleWorkOnHolidayPassType)
                                       && pt.PassTypeID == Rules[Constants.RuleWorkOnHolidayPassType].RuleValue) // NATALIJA 27.02.2018 prikaz prolaska "rad na drzavni praznik" kada je "drzavni praznik"
                                        ptArray.Add(pt);

                                    if (((Rules.ContainsKey(Constants.RuleCompanyAnnualLeave) && pt.PassTypeID == Rules[Constants.RuleCompanyAnnualLeave].RuleValue)
                                        || (Rules.ContainsKey(Constants.RuleCompanyCollectiveAnnualLeave) && pt.PassTypeID == Rules[Constants.RuleCompanyCollectiveAnnualLeave].RuleValue)
                                        || pt.LimitCompositeID != -1
                                        || pt.LimitElementaryID != -1 || pt.LimitOccasionID != -1)
                                        && !EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()))
                                        continue;
                                }

                                // if employee is expat out get always holiday and work on holiday pass type if HRSSC is logged on
                                if (IsExpatOut)
                                {
                                    if (Rules.ContainsKey(Constants.RuleHolidayPassType) && pt.PassTypeID == Rules[Constants.RuleHolidayPassType].RuleValue)
                                        ptArray.Add(pt);

                                    if (Rules.ContainsKey(Constants.RuleWorkOnHolidayPassType) && pt.PassTypeID == Rules[Constants.RuleWorkOnHolidayPassType].RuleValue)
                                        ptArray.Add(pt);
                                }

                                // if par is whole interval absence, get type regular work, unjustified and all whole day absences                    
                                if (isAbsencePair)
                                {
                                    // if interval is from night shift, get only absence type of previous/next night shift interval and unjustified
                                    //if (prevPairPT != -1 && prevPairPT != Constants.absence && pt.PassTypeID != prevPairPT && pt.PassTypeID != Constants.absence)
                                    //    continue;

                                    //if (nextPairPT != -1 && nextPairPT != Constants.absence && pt.PassTypeID != nextPairPT && pt.PassTypeID != Constants.absence)
                                    //    continue;

                                    // regular interval absence pair
                                    if (pt.PassTypeID == Constants.absence || pt.IsPass == Constants.wholeDayAbsence
                                        || (Rules.ContainsKey(Constants.RuleCompanyRegularWork) && pt.PassTypeID == Rules[Constants.RuleCompanyRegularWork].RuleValue)
                                        || (Rules.ContainsKey(Constants.RuleCompanyStopWorking) && pt.PassTypeID == Rules[Constants.RuleCompanyStopWorking].RuleValue)
                                        || (Rules.ContainsKey(Constants.RuleCompanyBankHourUsed) && pt.PassTypeID == Rules[Constants.RuleCompanyBankHourUsed].RuleValue)
                                        || (Rules.ContainsKey(Constants.RuleCompanyInitialOvertimeUsed) && pt.PassTypeID == Rules[Constants.RuleCompanyInitialOvertimeUsed].RuleValue))
                                        ptArray.Add(pt);
                                }
                                // if pass on reader, get sick leave for confirmation and pay leave 65% if HRSSC logged in
                                else if ((pt.PassTypeID == Constants.absence || pt.IsPass == Constants.passOnReader
                                    || (Rules.ContainsKey(Constants.RuleCompanyDelay) && pt.PassTypeID == Rules[Constants.RuleCompanyDelay].RuleValue && pt.PassTypeID == PairTO.PassTypeID)
                                    || (Rules.ContainsKey(Constants.RuleCompanyOfficialTrip) && pt.PassTypeID == Rules[Constants.RuleCompanyOfficialTrip].RuleValue && isOfficialTripPair)
                                    || (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC
                                    && WholeDayAbsenceByHour.Contains(pt.PassTypeID))
                                    || pt.PassTypeID == PairTO.PassTypeID) && !ptArray.Contains(pt))
                                    ptArray.Add(pt);
                            }
                        }
                    }

                    if (!ForConfirmation && !ForVerification &&
                        ((isOvertimePair &&
                        (PairTO.ManualCreated == (int)Constants.recordCreated.Manualy
                        || (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                            && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)))
                        || PairTO.PassTypeID == Constants.ptEmptyInterval))
                    {
                        PassTypeTO emptyPT = new PassTypeTO(Constants.ptEmptyInterval, rm.GetString("emptyInterval", culture), 0, 0, "");
                        emptyPT.DescAlt = emptyPT.Description;
                        ptArray.Insert(0, emptyPT);
                    }

                    //ptArray.Insert(0, new PassTypeTO(-1, rm.GetString("all", culture), 0, 0, ""));
                }

                cbType.DataSource = ptArray;
                if (CommonWeb.Misc.getLanguage(Session[Constants.sessionLogInUser]).Trim().ToUpper().Equals(Constants.Lang_sr.Trim().ToUpper()))
                    cbType.DataTextField = "DescriptionAndID";
                else
                    cbType.DataTextField = "DescriptionAltAndID";
                cbType.DataValueField = "PassTypeID";

                cbType.DataBind(); // bez ovoga se ne poveze lista objekata sa drop down listom i nista se ne prikazuje

                cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void populateLocations(DropDownList cbLocation)
        {
            try
            {
                List<LocationTO> locArray = new List<LocationTO>();

                foreach (int locID in LocationsDic.Keys)
                {
                    if (LocationsDic[locID].Status == Constants.statusActive || LocationsDic[locID].LocationID == PairTO.LocationID || LocationsDic[locID].LocationID == Constants.locationOut)
                        locArray.Add(LocationsDic[locID]);
                }

                cbLocation.DataSource = locArray;                
                cbLocation.DataTextField = "Name";
                cbLocation.DataValueField = "LocationID";

                cbLocation.DataBind(); // bez ovoga se ne poveze lista objekata sa drop down listom i nista se ne prikazuje

                cbLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // rearenge pairs after pair start is changed
        private void RearangePairStart(DateTime newStart, ControlEventArgs args)
        {
            try
            {
                List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                {
                    foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                    {
                        dayPairs.Add(new IOPairProcessedTO(pair));
                    }
                }

                int minimalPresence = 1;
                if (Rules.ContainsKey(Constants.RuleMinPresence))
                    minimalPresence = Rules[Constants.RuleMinPresence].RuleValue;

                // if entered start is before pair start                
                if (PairTO.StartTime > newStart)
                {
                    //rearange pairs before entered start
                    dayPairs = RearangePreviousPairsStart(dayPairs, ref newStart, minimalPresence, args);

                    if (args.Error.Trim().Equals("") && validatePair(PairTO, newStart, PairTO.EndTime, minimalPresence, args, getPairInterval(PairTO, dayPairs)))
                    {
                        // change current pair start
                        int index = indexOf(dayPairs, PairTO);

                        if (index >= 0 && index < dayPairs.Count)
                        {
                            dayPairs[index].StartTime = newStart;
                            dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[index].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID))
                                dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[index].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                        }
                    }
                }
                else if (PairTO.StartTime < newStart)
                {
                    // if entered start is after pair start, rearange prevous pair and pairs after current pair
                    dayPairs = RearangeNextPairsStart(dayPairs, ref newStart, minimalPresence, args);

                    if (args.Error.Trim().Equals("") && validatePair(PairTO, newStart, PairTO.EndTime, minimalPresence, args, getPairInterval(PairTO, dayPairs)))
                    {
                        //change current pair
                        int index = indexOf(dayPairs, PairTO);

                        if (index >= 0 && index < dayPairs.Count)
                        {
                            dayPairs[index].StartTime = newStart;
                            if (dayPairs[index].EndTime < dayPairs[index].StartTime)
                                dayPairs[index].EndTime = dayPairs[index].StartTime;
                            dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[index].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID))
                                dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[index].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                        }
                    }
                }

                if (args.Error.Trim().Equals(""))
                {
                    List<IOPairProcessedTO> sessionDayPairs = new List<IOPairProcessedTO>();
                    foreach (IOPairProcessedTO pair in dayPairs)
                    {
                        sessionDayPairs.Add(new IOPairProcessedTO(pair));
                    }

                    Session[Constants.sessionDayPairs] = sessionDayPairs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // rearange pairs after pair start is changed
        private void RearangeOvertimeStart(DateTime newStart, ControlEventArgs args)
        {
            try
            {
                List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                {
                    foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                    {
                        dayPairs.Add(new IOPairProcessedTO(pair));
                    }
                }

                if (PairTO.EndTime.Equals(new DateTime()))
                {
                    // change current pair start
                    int index = indexOf(dayPairs, PairTO);

                    if (index >= 0 && index < dayPairs.Count)
                    {
                        dayPairs[index].StartTime = newStart;
                        dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                        if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[index].EmployeeID)
                            && PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID))
                            dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[index].PassTypeID], ForVerification, ForConfirmation);
                        dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                    }
                }
                else
                {
                    int minimalPresence = 1;
                    if (!IsWorkAbsenceDay && Rules.ContainsKey(Constants.RuleOvertimeMinimum))
                        minimalPresence = Rules[Constants.RuleOvertimeMinimum].RuleValue;
                    else if (IsWorkAbsenceDay && Rules.ContainsKey(Constants.RuleMinOvertimeOutWS))
                        minimalPresence = Rules[Constants.RuleMinOvertimeOutWS].RuleValue;

                    // if entered start is before pair start                
                    if (PairTO.StartTime > newStart)
                    {
                        //rearange pairs before entered start
                        dayPairs = RearangePreviousOvertimeStart(dayPairs, ref newStart, minimalPresence, args);

                        if (args.Error.Trim().Equals("") && validateOvertimePair(PairTO, newStart, PairTO.EndTime, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            // change current pair start
                            int index = indexOf(dayPairs, PairTO);

                            if (index >= 0 && index < dayPairs.Count)
                            {
                                dayPairs[index].StartTime = newStart;
                                dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                                if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[index].EmployeeID)
                                    && PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID))
                                    dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[index].PassTypeID], ForVerification, ForConfirmation);
                                dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                            }
                        }
                    }
                    else if (PairTO.StartTime < newStart)
                    {
                        // if entered start is after pair start, rearange prevous pair and pairs after current pair
                        dayPairs = RearangeNextOvertimeStart(dayPairs, ref newStart, minimalPresence, args);

                        if (args.Error.Trim().Equals("") && validateOvertimePair(PairTO, newStart, PairTO.EndTime, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            //change current pair
                            int index = indexOf(dayPairs, PairTO);

                            if (index >= 0 && index < dayPairs.Count)
                            {
                                dayPairs[index].StartTime = newStart;
                                if (dayPairs[index].EndTime < dayPairs[index].StartTime)
                                    dayPairs[index].EndTime = dayPairs[index].StartTime;
                                dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                                if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[index].EmployeeID)
                                    && PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID))
                                    dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[index].PassTypeID], ForVerification, ForConfirmation);
                                
                                dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                            }
                        }
                    }
                }

                if (args.Error.Trim().Equals(""))
                {
                    List<IOPairProcessedTO> sessionDayPairs = new List<IOPairProcessedTO>();
                    foreach (IOPairProcessedTO pair in dayPairs)
                    {
                        sessionDayPairs.Add(new IOPairProcessedTO(pair));
                    }

                    Session[Constants.sessionDayPairs] = sessionDayPairs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // rearange pairs after pair end is changed
        private void RearangePairEnd(DateTime newEnd, ControlEventArgs args)
        {
            try
            {
                List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                {
                    foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                    {
                        dayPairs.Add(new IOPairProcessedTO(pair));
                    }
                }

                int minimalPresence = 1;
                if (Rules.ContainsKey(Constants.RuleMinPresence))
                    minimalPresence = Rules[Constants.RuleMinPresence].RuleValue;

                // if entered end is after pair end
                if (PairTO.EndTime < newEnd)
                {
                    // if entered end is after pair end, rearange pairs after that
                    dayPairs = RearangeNextPairsEnd(dayPairs, ref newEnd, minimalPresence, args);

                    if (args.Error.Trim().Equals("") && validatePair(PairTO, PairTO.StartTime, newEnd, minimalPresence, args, getPairInterval(PairTO, dayPairs)))
                    {
                        // change current pair end
                        int index = indexOf(dayPairs, PairTO);

                        if (index >= 0 && index < dayPairs.Count)
                        {
                            dayPairs[index].EndTime = newEnd;
                            dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[index].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID))
                                dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[index].PassTypeID], ForVerification, ForConfirmation);
                    
                            dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                        }
                    }
                }
                else
                {
                    // if entered end is before pair end, rearange next pair and pairs before current pair
                    dayPairs = RearangePreviousPairsEnd(dayPairs, ref newEnd, minimalPresence, args);

                    if (args.Error.Trim().Equals("") && validatePair(PairTO, PairTO.StartTime, newEnd, minimalPresence, args, getPairInterval(PairTO, dayPairs)))
                    {
                        //change current pair
                        int index = indexOf(dayPairs, PairTO);

                        if (index >= 0 && index < dayPairs.Count)
                        {
                            dayPairs[index].EndTime = newEnd;
                            if (dayPairs[index].StartTime > dayPairs[index].EndTime)
                                dayPairs[index].StartTime = dayPairs[index].EndTime;
                            dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[index].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID))
                                dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[index].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                        }
                    }
                }

                if (args.Error.Trim().Equals(""))
                {
                    List<IOPairProcessedTO> sessionDayPairs = new List<IOPairProcessedTO>();
                    foreach (IOPairProcessedTO pair in dayPairs)
                    {
                        sessionDayPairs.Add(new IOPairProcessedTO(pair));
                    }

                    Session[Constants.sessionDayPairs] = sessionDayPairs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // rearange pairs after pair end is changed
        private void RearangeOvertimeEnd(DateTime newEnd, ControlEventArgs args)
        {
            try
            {
                List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                {
                    foreach (IOPairProcessedTO pair in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                    {
                        dayPairs.Add(new IOPairProcessedTO(pair));
                    }
                }

                if (PairTO.StartTime.Equals(new DateTime()))
                {
                    // change current pair end
                    int index = indexOf(dayPairs, PairTO);

                    if (index >= 0 && index < dayPairs.Count)
                    {
                        dayPairs[index].EndTime = newEnd;
                        dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                        if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[index].EmployeeID)
                            && PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID))
                            dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[index].PassTypeID], ForVerification, ForConfirmation);
                        dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                    }
                }
                else
                {
                    int minimalPresence = 1;
                    if (!IsWorkAbsenceDay && Rules.ContainsKey(Constants.RuleOvertimeMinimum))
                        minimalPresence = Rules[Constants.RuleOvertimeMinimum].RuleValue;
                    else if (IsWorkAbsenceDay && Rules.ContainsKey(Constants.RuleMinOvertimeOutWS))
                        minimalPresence = Rules[Constants.RuleMinOvertimeOutWS].RuleValue;

                    // if entered end is after pair end
                    if (PairTO.EndTime < newEnd)
                    {
                        //rearange pairs after entered end
                        dayPairs = RearangeNextOvertimeEnd(dayPairs, ref newEnd, minimalPresence, args);

                        if (args.Error.Trim().Equals("") && validateOvertimePair(PairTO, PairTO.StartTime, newEnd, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            // change current pair end
                            int index = indexOf(dayPairs, PairTO);

                            if (index >= 0 && index < dayPairs.Count)
                            {
                                dayPairs[index].EndTime = newEnd;
                                dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                                if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[index].EmployeeID)
                                    && PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID))
                                    dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[index].PassTypeID], ForVerification, ForConfirmation);
                                dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                            }
                        }
                    }
                    else if (PairTO.EndTime > newEnd)
                    {
                        // if entered end is before pair end, rearange next pair and pairs before current pair
                        dayPairs = RearangePreviousOvertimeEnd(dayPairs, ref newEnd, minimalPresence, args);

                        if (args.Error.Trim().Equals("") && validateOvertimePair(PairTO, PairTO.StartTime, newEnd, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            //change current pair
                            int index = indexOf(dayPairs, PairTO);

                            if (index >= 0 && index < dayPairs.Count)
                            {
                                dayPairs[index].EndTime = newEnd;
                                if (dayPairs[index].EndTime < dayPairs[index].StartTime)
                                    dayPairs[index].StartTime = dayPairs[index].EndTime;
                                dayPairs[index].ManualCreated = (int)Constants.recordCreated.Manualy;
                                if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[index].EmployeeID)
                                    && PassTypesAllDic.ContainsKey(dayPairs[index].PassTypeID))
                                    dayPairs[index].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[index].PassTypeID], ForVerification, ForConfirmation);
                                dayPairs[index].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                            }
                        }
                    }
                }
                if (args.Error.Trim().Equals(""))
                {
                    List<IOPairProcessedTO> sessionDayPairs = new List<IOPairProcessedTO>();
                    foreach (IOPairProcessedTO pair in dayPairs)
                    {
                        sessionDayPairs.Add(new IOPairProcessedTO(pair));
                    }

                    Session[Constants.sessionDayPairs] = sessionDayPairs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> RearangePreviousPairsStart(List<IOPairProcessedTO> dayPairs, ref DateTime newStart, int minimalPresence, ControlEventArgs args)
        {
            try
            {
                WorkTimeIntervalTO pairInterval = getPairInterval(PairTO, dayPairs);

                DateTime oldIntervalEnd = pairInterval.EndTime;

                if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    double pairDuration = pairInterval.EndTime.TimeOfDay.Subtract(pairInterval.StartTime.TimeOfDay).TotalMinutes;

                    // change interval start and end due to new start time
                    if (newStart.TimeOfDay < pairInterval.EarliestArrived.TimeOfDay)
                        pairInterval.StartTime = pairInterval.EarliestArrived;
                    else if (newStart.TimeOfDay >= pairInterval.EarliestArrived.TimeOfDay && newStart.TimeOfDay <= pairInterval.LatestArrivaed.TimeOfDay)
                        pairInterval.StartTime = new DateTime(pairInterval.StartTime.Year, pairInterval.StartTime.Month, pairInterval.StartTime.Day, newStart.Hour, newStart.Minute, 0);

                    pairInterval.EndTime = pairInterval.StartTime.AddMinutes(pairDuration);
                }

                int pairIndex = indexOf(dayPairs, PairTO);

                // if start is moved out of interval, insert overtime pair and change new start to interval start
                if (newStart.TimeOfDay < pairInterval.StartTime.TimeOfDay)
                {
                    int overtimeMinPresence = 1;
                    int overtimeRounding = 1;
                    if (Rules.ContainsKey(Constants.RuleOvertimeMinimum))
                        overtimeMinPresence = Rules[Constants.RuleOvertimeMinimum].RuleValue;
                    if (Rules.ContainsKey(Constants.RuleOvertimeRounding))
                        overtimeRounding = Rules[Constants.RuleOvertimeRounding].RuleValue;

                    if (pairInterval.StartTime.TimeOfDay.Subtract(newStart.TimeOfDay).TotalMinutes % overtimeRounding != 0)
                    {
                        newStart = newStart.AddMinutes(pairInterval.StartTime.TimeOfDay.Subtract(newStart.TimeOfDay).TotalMinutes % overtimeRounding);
                        if (newStart > dayEnd)
                            newStart = dayEnd;
                    }

                    IOPairProcessedTO overtimePairToInsert = overtimePair(newStart, new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, pairInterval.StartTime.Hour, pairInterval.StartTime.Minute, 0));

                    if (!validateOvertimePairOverlap(dayPairs, overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, args) && validateOvertimePair(overtimePairToInsert,
                        overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, overtimeMinPresence, dayPairs, new DateTime(), pairInterval.StartTime, args))
                    {
                        dayPairs.Insert(indexOfFirstInInterval(dayPairs, pairInterval), overtimePairToInsert);
                        newStart = new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, pairInterval.StartTime.Hour, pairInterval.StartTime.Minute, 0);
                    }
                    else
                        return dayPairs;
                }

                pairIndex = indexOf(dayPairs, PairTO);

                for (int i = 0; i < pairIndex && i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (dayPairs[i].EndTime > newStart)
                    {
                        if (validatePair(dayPairs[i], dayPairs[i].StartTime, newStart, minimalPresence, args, pairInterval))
                        {
                            dayPairs[i].EndTime = newStart;
                            dayPairs[i].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[i].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID))
                                dayPairs[i].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[i].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[i].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                            if (dayPairs[i].EndTime < dayPairs[i].StartTime)
                                dayPairs[i].StartTime = dayPairs[i].EndTime;
                        }
                        else
                            break;
                    }
                }

                if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()) && !pairInterval.EndTime.TimeOfDay.Equals(oldIntervalEnd.TimeOfDay))
                {
                    // if flexy interval is changed - if there is pair that is before and after new pair interval end, split it
                    // from all pairs after make one overtime to justify pair
                    // do not allow changing if there already exist one of selected overtime pairs
                    int endIntervalPairIndex = 0;

                    // find pair to split
                    while (endIntervalPairIndex < dayPairs.Count && !(dayPairs[endIntervalPairIndex].StartTime.TimeOfDay < pairInterval.EndTime.TimeOfDay
                        && dayPairs[endIntervalPairIndex].EndTime.TimeOfDay >= pairInterval.EndTime.TimeOfDay))
                    {
                        endIntervalPairIndex++;
                    }

                    if (endIntervalPairIndex < dayPairs.Count)
                    {
                        DateTime lastPairEnd = dayPairs[endIntervalPairIndex].EndTime;
                        DateTime borderPairOldEnd = dayPairs[endIntervalPairIndex].EndTime;
                        int firstNextIntervalPairIndex = endIntervalPairIndex + 1;
                        for (int i = endIntervalPairIndex + 1; i < dayPairs.Count; i++)
                        {
                            // pairs from previous interval lasting
                            if (dayPairs[i].EndTime.TimeOfDay <= oldIntervalEnd.TimeOfDay)
                            {
                                lastPairEnd = dayPairs[i].EndTime;
                                firstNextIntervalPairIndex = i + 1;
                                continue;
                            }

                            // break before hole in pairs
                            if (!dayPairs[i - 1].EndTime.Equals(dayPairs[i].StartTime) || dayPairs[i].PassTypeID == Constants.ptEmptyInterval)
                            {
                                firstNextIntervalPairIndex = i;
                                break;
                            }

                            // allow changing only overtime to justify pairs
                            if (PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID) && PassTypesAllDic[dayPairs[i].PassTypeID].IsPass == Constants.overtimePassType
                                && dayPairs[i].PassTypeID != Constants.overtimeUnjustified)
                            {
                                args.Error = rm.GetString("flexyOvertimeChange", culture);
                                return dayPairs;
                            }

                            lastPairEnd = dayPairs[i].EndTime;
                            firstNextIntervalPairIndex = i + 1;
                        }

                        // change pair end, if it is start changing pair, change its end too
                        int changingPairIndex = indexOf(dayPairs, PairTO);

                        dayPairs[endIntervalPairIndex].EndTime = new DateTime(dayPairs[endIntervalPairIndex].IOPairDate.Year, dayPairs[endIntervalPairIndex].IOPairDate.Month,
                            dayPairs[endIntervalPairIndex].IOPairDate.Day, pairInterval.EndTime.Hour, pairInterval.EndTime.Minute, 0);
                        
                        if (changingPairIndex == endIntervalPairIndex)
                            PairTO.EndTime = dayPairs[endIntervalPairIndex].EndTime;

                        // create one overtime pair
                        int overtimeMinPresence = 1;
                        int overtimeRounding = 1;
                        if (Rules.ContainsKey(Constants.RuleOvertimeMinimum))
                            overtimeMinPresence = Rules[Constants.RuleOvertimeMinimum].RuleValue;
                        if (Rules.ContainsKey(Constants.RuleOvertimeRounding))
                            overtimeRounding = Rules[Constants.RuleOvertimeRounding].RuleValue;

                        if (lastPairEnd.TimeOfDay.Subtract(pairInterval.EndTime.TimeOfDay).TotalMinutes % overtimeRounding != 0)
                        {
                            lastPairEnd = lastPairEnd.AddMinutes(-(lastPairEnd.TimeOfDay.Subtract(pairInterval.EndTime.TimeOfDay).TotalMinutes % overtimeRounding));
                            if (lastPairEnd < dayBegining)
                                lastPairEnd = dayBegining;
                        }

                        IOPairProcessedTO overtimePairAfterInterval = overtimePair(new DateTime(dayPairs[endIntervalPairIndex].IOPairDate.Year, dayPairs[endIntervalPairIndex].IOPairDate.Month,
                            dayPairs[endIntervalPairIndex].IOPairDate.Day, pairInterval.EndTime.Hour, pairInterval.EndTime.Minute, 0), lastPairEnd);

                        // delete pairs between last interval pair and first from next interval
                        int removeIndex = endIntervalPairIndex + 1;
                        int pairsRemoved = 0;
                        int stopWorkingMinutes = 0;
                        int bhCounterDurationOld = 0;
                        int bhCounterDurationNew = 0;

                        if (!borderPairOldEnd.Equals(dayPairs[endIntervalPairIndex].EndTime))
                        {
                            if (Rules.ContainsKey(Constants.RuleCompanyBankHourUsed) && Rules[Constants.RuleCompanyBankHourUsed].RuleValue == dayPairs[endIntervalPairIndex].PassTypeID)
                            {
                                bhCounterDurationOld = (int)borderPairOldEnd.TimeOfDay.Subtract(dayPairs[endIntervalPairIndex].StartTime.TimeOfDay).TotalMinutes;
                                bhCounterDurationNew = (int)dayPairs[endIntervalPairIndex].EndTime.TimeOfDay.Subtract(dayPairs[endIntervalPairIndex].StartTime.TimeOfDay).TotalMinutes;
                            }

                            if (Rules.ContainsKey(Constants.RuleCompanyStopWorking) && Rules[Constants.RuleCompanyStopWorking].RuleValue == dayPairs[endIntervalPairIndex].PassTypeID)
                                stopWorkingMinutes = (int)borderPairOldEnd.TimeOfDay.Subtract(dayPairs[endIntervalPairIndex].StartTime.TimeOfDay).TotalMinutes;
                        }

                        while (removeIndex < dayPairs.Count && pairsRemoved < firstNextIntervalPairIndex - endIntervalPairIndex - 1)
                        {
                            if (Rules.ContainsKey(Constants.RuleCompanyBankHourUsed) && Rules[Constants.RuleCompanyBankHourUsed].RuleValue == dayPairs[removeIndex].PassTypeID)
                            {
                                bhCounterDurationOld += (int)dayPairs[removeIndex].EndTime.TimeOfDay.Subtract(dayPairs[removeIndex].StartTime.TimeOfDay).TotalMinutes;
                            }

                            if (Rules.ContainsKey(Constants.RuleCompanyStopWorking) && Rules[Constants.RuleCompanyStopWorking].RuleValue == dayPairs[removeIndex].PassTypeID)
                                stopWorkingMinutes += (int)dayPairs[removeIndex].EndTime.TimeOfDay.Subtract(dayPairs[removeIndex].StartTime.TimeOfDay).TotalMinutes;
                            
                            dayPairs.RemoveAt(removeIndex);
                            pairsRemoved++;
                        }

                        // change stop working counter - all other counters are from overtime pairs or whole day absences and will not be changed in this case
                        if (stopWorkingMinutes > 0 && Session[Constants.sessionEmplCounters] != null && Session[Constants.sessionEmplCounters] is Dictionary<int, EmployeeCounterValueTO>                            
                            && ((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters]).ContainsKey((int)Constants.EmplCounterTypes.StopWorkingCounter)
                            && ((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters])[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value > 0)
                        {
                            ((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters])[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value -= stopWorkingMinutes;
                            if (((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters])[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value < 0)
                                ((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters])[(int)Constants.EmplCounterTypes.StopWorkingCounter].Value = 0;
                        }

                        // change bank hour counter if needed                            
                        // calculate value for counter considering bank hour rounding rule for old and new par duration
                        if (Rules.ContainsKey(Constants.RuleBankHoursUsedRounding))
                        {
                            int bhRounding = Rules[Constants.RuleBankHoursUsedRounding].RuleValue;

                            if (bhCounterDurationOld % bhRounding != 0)
                                bhCounterDurationOld += bhRounding - bhCounterDurationOld % bhRounding;

                            if (bhCounterDurationNew % bhRounding != 0)
                                bhCounterDurationNew += bhRounding - bhCounterDurationNew % bhRounding;
                        }

                        if (bhCounterDurationOld != bhCounterDurationNew)
                        {
                            // if old type is bank hour used, increase counter - counter is in minutes
                            if (Session[Constants.sessionEmplCounters] != null && Session[Constants.sessionEmplCounters] is Dictionary<int, EmployeeCounterValueTO>
                                && ((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters]).ContainsKey((int)Constants.EmplCounterTypes.BankHoursCounter))
                                ((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters])[(int)Constants.EmplCounterTypes.BankHoursCounter].Value += bhCounterDurationOld - bhCounterDurationNew;
                        }
                                                
                        if (!validateOvertimePairOverlap(dayPairs, overtimePairAfterInterval.StartTime, overtimePairAfterInterval.EndTime, args) && validateOvertimePair(overtimePairAfterInterval,
                            overtimePairAfterInterval.StartTime, overtimePairAfterInterval.EndTime, overtimeMinPresence, dayPairs, pairInterval.EndTime, new DateTime(), args))
                        {
                            dayPairs.Insert(endIntervalPairIndex + 1, overtimePairAfterInterval);
                        }
                        // in this case, we do not need error, if pair is not valid, just remove it from day pairs
                        else
                            args.Error = "";
                    }
                }

                return dayPairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> RearangeNextOvertimeEnd(List<IOPairProcessedTO> dayPairs, ref DateTime newEnd, int minimalPresence, ControlEventArgs args)
        {
            try
            {
                int pairIndex = indexOf(dayPairs, PairTO);

                for (int i = pairIndex + 1; i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (dayPairs[i].StartTime < newEnd)
                    {
                        if (validateOvertimePair(dayPairs[i], newEnd, dayPairs[i].EndTime, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            dayPairs[i].StartTime = newEnd;
                            dayPairs[i].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[i].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID))
                                dayPairs[i].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[i].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[i].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                            if (dayPairs[i].EndTime < dayPairs[i].StartTime)
                                dayPairs[i].EndTime = dayPairs[i].StartTime;
                        }
                        else
                            break;
                    }
                }

                return dayPairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> RearangePreviousOvertimeStart(List<IOPairProcessedTO> dayPairs, ref DateTime newStart, int minimalPresence, ControlEventArgs args)
        {
            try
            {
                int pairIndex = indexOf(dayPairs, PairTO);

                for (int i = 0; i < pairIndex && i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (dayPairs[i].EndTime > newStart)
                    {
                        if (validateOvertimePair(dayPairs[i], dayPairs[i].StartTime, newStart, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            dayPairs[i].EndTime = newStart;
                            dayPairs[i].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[i].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID))
                                dayPairs[i].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[i].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[i].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                            if (dayPairs[i].EndTime < dayPairs[i].StartTime)
                                dayPairs[i].StartTime = dayPairs[i].EndTime;
                        }
                        else
                            break;
                    }
                }

                return dayPairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> RearangeNextPairsStart(List<IOPairProcessedTO> dayPairs, ref DateTime newStart, int minimalPresence, ControlEventArgs args)
        {
            try
            {
                WorkTimeIntervalTO pairInterval = getPairInterval(PairTO, dayPairs);

                DateTime oldIntervalStart = pairInterval.StartTime;
                DateTime oldIntervalEnd = pairInterval.EndTime;

                if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()) && PairTO.StartTime.TimeOfDay == oldIntervalStart.TimeOfDay)
                {
                    double pairDuration = pairInterval.EndTime.TimeOfDay.Subtract(pairInterval.StartTime.TimeOfDay).TotalMinutes;

                    // change interval start and end due to new start time
                    if (newStart.TimeOfDay > pairInterval.LatestArrivaed.TimeOfDay)
                        pairInterval.StartTime = pairInterval.LatestArrivaed;
                    else if (newStart.TimeOfDay >= pairInterval.EarliestArrived.TimeOfDay && newStart.TimeOfDay <= pairInterval.LatestArrivaed.TimeOfDay)
                        pairInterval.StartTime = new DateTime(pairInterval.StartTime.Year, pairInterval.StartTime.Month, pairInterval.StartTime.Day, newStart.Hour, newStart.Minute, 0);

                    pairInterval.EndTime = pairInterval.StartTime.AddMinutes(pairDuration);
                }

                int pairIndex = indexOf(dayPairs, PairTO);

                // if start is moved out of interval, insert overtime pair and change new start to interval end
                if (newStart.TimeOfDay > pairInterval.EndTime.TimeOfDay)
                {
                    int overtimeMinPresence = 1;
                    int overtimeRounding = 1;
                    if (Rules.ContainsKey(Constants.RuleOvertimeMinimum))
                        overtimeMinPresence = Rules[Constants.RuleOvertimeMinimum].RuleValue;
                    if (Rules.ContainsKey(Constants.RuleOvertimeRounding))
                        overtimeRounding = Rules[Constants.RuleOvertimeRounding].RuleValue;
                    
                    if (newStart.TimeOfDay.Subtract(pairInterval.EndTime.TimeOfDay).TotalMinutes % overtimeRounding != 0)
                    {
                        newStart = newStart.AddMinutes(newStart.TimeOfDay.Subtract(pairInterval.EndTime.TimeOfDay).TotalMinutes % overtimeRounding);
                        if (newStart < dayBegining)
                            newStart = dayBegining;
                    }

                    IOPairProcessedTO overtimePairToInsert = overtimePair(new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, pairInterval.EndTime.Hour, pairInterval.EndTime.Minute, 0), newStart);

                    if (!validateOvertimePairOverlap(dayPairs, overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, args) && validateOvertimePair(overtimePairToInsert,
                        overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, overtimeMinPresence, dayPairs, new DateTime(), new DateTime(), args))
                    {
                        dayPairs.Insert(indexOfLastInInterval(dayPairs, pairInterval) + 1, overtimePairToInsert);
                        newStart = new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, pairInterval.EndTime.Hour, pairInterval.EndTime.Minute, 0);
                    }
                    else
                        return dayPairs;
                }

                // if pair is interval begining, insert delay and unjustified pair
                if (PairTO.StartTime.TimeOfDay == oldIntervalStart.TimeOfDay && newStart.TimeOfDay > pairInterval.StartTime.TimeOfDay)
                {
                    if (Common.Misc.isThirdShiftEndInterval(pairInterval))
                    {
                        // if interval is second interval of night shift just insert unjustified pair
                        IOPairProcessedTO unjustifiedPairToInsert = unjustifiedPair(new DateTime(newStart.Year, newStart.Month, newStart.Day, pairInterval.StartTime.Hour, pairInterval.StartTime.Minute, 0), newStart);
                        if (validatePair(unjustifiedPairToInsert, unjustifiedPairToInsert.StartTime, unjustifiedPairToInsert.EndTime, minimalPresence, args, pairInterval))
                            dayPairs.Insert(pairIndex, unjustifiedPairToInsert);
                        else
                            return dayPairs;
                    }
                    else
                    {
                        int delayMax = 0;
                        int delayRounding = 1;

                        if (Empl.Type.Trim().ToUpper() != Constants.emplExtraOrdinary.Trim().ToUpper())
                        {
                            if (Rules.ContainsKey(Constants.RuleDelayMax))
                                delayMax = Rules[Constants.RuleDelayMax].RuleValue;
                            if (Rules.ContainsKey(Constants.RuleDelayRounding))
                                delayRounding = Rules[Constants.RuleDelayRounding].RuleValue;
                        }

                        DateTime delayEnd = new DateTime();
                        DateTime delayStart = new DateTime(newStart.Year, newStart.Month, newStart.Day, pairInterval.StartTime.Hour, pairInterval.StartTime.Minute, 0);
                        if (newStart.TimeOfDay.Subtract(pairInterval.StartTime.TimeOfDay).TotalMinutes > delayMax)
                        {
                            // insert delay without rounding
                            delayEnd = delayStart.AddMinutes(delayMax);
                            if (delayEnd > dayEnd)
                                delayEnd = dayEnd;

                            if (delayEnd > delayStart)
                            {
                                dayPairs.Insert(pairIndex, delayPair(delayStart, delayEnd));
                                pairIndex++;
                            }

                            // insert unjustified
                            IOPairProcessedTO unjustifiedPairToInsert = unjustifiedPair(delayEnd, newStart);
                            if (validatePair(unjustifiedPairToInsert, unjustifiedPairToInsert.StartTime, unjustifiedPairToInsert.EndTime, minimalPresence, args, pairInterval))
                                dayPairs.Insert(pairIndex, unjustifiedPairToInsert);
                            else
                                return dayPairs;
                        }
                        else
                        {
                            // insert only delay pair with rounding                        
                            delayEnd = new DateTime(newStart.Year, newStart.Month, newStart.Day, newStart.Hour, newStart.Minute, 0);
                            if (delayEnd.Minute % delayRounding != 0)
                            {
                                newStart = delayEnd.AddMinutes(delayRounding - (delayEnd.Minute % delayRounding));
                                if (newStart > dayEnd)
                                    newStart = dayEnd;

                                delayEnd = delayEnd.AddMinutes(delayRounding - (delayEnd.Minute % delayRounding));
                                if (delayEnd > dayEnd)
                                    delayEnd = dayEnd;
                            }

                            if (delayEnd > delayStart)
                                dayPairs.Insert(pairIndex, delayPair(delayStart, delayEnd));
                        }
                    }
                }
                else
                {
                    pairIndex = indexOf(dayPairs, PairTO);

                    if (pairIndex > 0 && pairIndex <= dayPairs.Count && dayPairs[pairIndex - 1].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                    {
                        // prolong previous pair by moving it's end to new start 
                        if (validatePair(dayPairs[pairIndex - 1], dayPairs[pairIndex - 1].StartTime, newStart, minimalPresence, args, pairInterval))
                        {
                            dayPairs[pairIndex - 1].EndTime = newStart;
                            dayPairs[pairIndex - 1].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[pairIndex - 1].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[pairIndex - 1].PassTypeID))
                                dayPairs[pairIndex - 1].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[pairIndex - 1].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[pairIndex - 1].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                        }
                        else
                            return dayPairs;
                    }
                }

                pairIndex = indexOf(dayPairs, PairTO);

                // change duration to next pairs
                for (int i = pairIndex + 1; i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (dayPairs[i].StartTime < newStart)
                    {
                        if (validatePair(dayPairs[i], newStart, dayPairs[i].EndTime, minimalPresence, args, pairInterval))
                        {
                            dayPairs[i].StartTime = newStart;
                            dayPairs[i].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[i].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID))
                                dayPairs[i].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[i].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[i].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                            if (dayPairs[i].EndTime < dayPairs[i].StartTime)
                                dayPairs[i].EndTime = dayPairs[i].StartTime;
                        }
                        else
                            break;
                    }
                }

                if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()) && !pairInterval.EndTime.TimeOfDay.Equals(oldIntervalEnd.TimeOfDay))
                {
                    // if flexy interval is changed - if there is pair that is before and after new pair interval end, change start to new interval start
                    // from all pairs after new end make one overtime to justify pair
                    // do not allow changing if there already exist one of selected overtime pairs
                    // insert unjustified pair to fill hole before new end
                    int endIntervalPairIndex = 0;
                    int splitPairIndex = 0;
                    // find pair to split
                    while (splitPairIndex < dayPairs.Count && !(dayPairs[splitPairIndex].StartTime.TimeOfDay < pairInterval.EndTime.TimeOfDay
                        && dayPairs[splitPairIndex].EndTime.TimeOfDay >= pairInterval.EndTime.TimeOfDay))
                    {
                        if (dayPairs[splitPairIndex].EndTime.TimeOfDay <= oldIntervalEnd.TimeOfDay)
                            endIntervalPairIndex = splitPairIndex;

                        // allow changing only overtime to justify pairs
                        if (PassTypesAllDic.ContainsKey(dayPairs[splitPairIndex].PassTypeID) && PassTypesAllDic[dayPairs[splitPairIndex].PassTypeID].IsPass == Constants.overtimePassType
                            && dayPairs[splitPairIndex].PassTypeID != Constants.overtimeUnjustified)
                        {
                            args.Error = rm.GetString("flexyOvertimeChange", culture);
                            return dayPairs;
                        }

                        splitPairIndex++;
                    }

                    if (splitPairIndex < dayPairs.Count)
                    {
                        if (PassTypesAllDic.ContainsKey(dayPairs[splitPairIndex].PassTypeID) && PassTypesAllDic[dayPairs[splitPairIndex].PassTypeID].IsPass == Constants.overtimePassType
                            && dayPairs[splitPairIndex].PassTypeID != Constants.overtimeUnjustified)
                        {
                            args.Error = rm.GetString("flexyOvertimeChange", culture);
                            return dayPairs;
                        }

                        DateTime lastPairEnd = new DateTime();
                        int firstNextIntervalPairIndex = splitPairIndex + 1;
                        if (dayPairs[splitPairIndex].PassTypeID != Constants.ptEmptyInterval)
                        {
                            lastPairEnd = dayPairs[splitPairIndex].EndTime;

                            for (int i = splitPairIndex + 1; i < dayPairs.Count; i++)
                            {
                                // break before hole in pairs
                                if (!dayPairs[i - 1].EndTime.Equals(dayPairs[i].StartTime) || dayPairs[i].PassTypeID == Constants.ptEmptyInterval)
                                {
                                    firstNextIntervalPairIndex = i;
                                    break;
                                }

                                // allow changing only overtime to justify pairs
                                if (PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID) && PassTypesAllDic[dayPairs[i].PassTypeID].IsPass == Constants.overtimePassType
                                    && dayPairs[i].PassTypeID != Constants.overtimeUnjustified)
                                {
                                    args.Error = rm.GetString("flexyOvertimeChange", culture);
                                    return dayPairs;
                                }

                                lastPairEnd = dayPairs[i].EndTime;
                                firstNextIntervalPairIndex = i + 1;
                            }
                        }
                        else
                            lastPairEnd = new DateTime(dayPairs[splitPairIndex].IOPairDate.Year, dayPairs[splitPairIndex].IOPairDate.Month, dayPairs[splitPairIndex].IOPairDate.Day, pairInterval.EndTime.Hour, pairInterval.EndTime.Minute, 0);

                        // delete pairs between end interval pair index and first from next interval
                        int removeIndex = endIntervalPairIndex + 1;
                        int pairsRemoved = 0;

                        while (removeIndex < dayPairs.Count && pairsRemoved < firstNextIntervalPairIndex - endIntervalPairIndex - 1)
                        {
                            dayPairs.RemoveAt(removeIndex);
                            pairsRemoved++;
                        }

                        if (!lastPairEnd.TimeOfDay.Equals(pairInterval.EndTime.TimeOfDay))
                        {
                            // create one overtime pair
                            int overtimeMinPresence = 1;
                            int overtimeRounding = 1;
                            if (Rules.ContainsKey(Constants.RuleOvertimeMinimum))
                                overtimeMinPresence = Rules[Constants.RuleOvertimeMinimum].RuleValue;
                            if (Rules.ContainsKey(Constants.RuleOvertimeRounding))
                                overtimeRounding = Rules[Constants.RuleOvertimeRounding].RuleValue;

                            if (lastPairEnd.TimeOfDay.Subtract(pairInterval.EndTime.TimeOfDay).TotalMinutes % overtimeRounding != 0)
                            {
                                lastPairEnd = lastPairEnd.AddMinutes(-(lastPairEnd.TimeOfDay.Subtract(pairInterval.EndTime.TimeOfDay).TotalMinutes % overtimeRounding));
                                if (lastPairEnd < dayBegining)
                                    lastPairEnd = dayBegining;
                            }

                            IOPairProcessedTO overtimePairAfterInterval = overtimePair(new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month,
                                PairTO.IOPairDate.Day, pairInterval.EndTime.Hour, pairInterval.EndTime.Minute, 0), lastPairEnd);
                            
                            if (!validateOvertimePairOverlap(dayPairs, overtimePairAfterInterval.StartTime, overtimePairAfterInterval.EndTime, args) && validateOvertimePair(overtimePairAfterInterval,
                                overtimePairAfterInterval.StartTime, overtimePairAfterInterval.EndTime, overtimeMinPresence, dayPairs, pairInterval.EndTime, new DateTime(), args))
                            {
                                dayPairs.Insert(endIntervalPairIndex + 1, overtimePairAfterInterval);
                            }
                            // in this case, we do not need error, if pair is not valid, just remove it from day pairs
                            else
                                args.Error = "";
                        }
                    }

                    // insert unjustified
                    IOPairProcessedTO unjustifiedPairBeforeEnd = unjustifiedPair(new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, oldIntervalEnd.Hour, oldIntervalEnd.Minute, 0),
                        new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, pairInterval.EndTime.Hour, pairInterval.EndTime.Minute, 0));
                    if (validatePair(unjustifiedPairBeforeEnd, unjustifiedPairBeforeEnd.StartTime, unjustifiedPairBeforeEnd.EndTime, minimalPresence, args, pairInterval))
                        dayPairs.Insert(endIntervalPairIndex + 1, unjustifiedPairBeforeEnd);
                    else
                        return dayPairs;
                }

                return dayPairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> RearangeNextOvertimeStart(List<IOPairProcessedTO> dayPairs, ref DateTime newStart, int minimalPresence, ControlEventArgs args)
        {
            try
            {
                int pairIndex = indexOf(dayPairs, PairTO);
                // if changing overtime pair is first, insert new overtime pair if it is valid
                if (pairIndex == 0 || (dayPairs.Count > 0 && pairIndex == 1 && !dayPairs[0].IOPairDate.Date.Equals(PairTO.IOPairDate.Date)))
                {
                    if (!PairTO.StartTime.Equals(new DateTime()))
                    {
                        IOPairProcessedTO overtimePairToInsert = overtimePair(PairTO.StartTime, newStart);
                        if (validateOvertimePair(overtimePairToInsert, overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            dayPairs.Insert(pairIndex, overtimePairToInsert);
                        }
                        else
                            return dayPairs;
                    }
                }

                // check previous pair
                else if (pairIndex > 0 && pairIndex < dayPairs.Count + 1 && dayPairs[pairIndex - 1].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                {
                    // if previous pair is not overtime or is overtime with 'hole' between pairs, insert overtime pair if it is valid
                    if (PassTypesAllDic.ContainsKey(dayPairs[pairIndex - 1].PassTypeID) && (PassTypesAllDic[dayPairs[pairIndex - 1].PassTypeID].IsPass != Constants.overtimePassType
                        || (PassTypesAllDic[dayPairs[pairIndex - 1].PassTypeID].IsPass == Constants.overtimePassType && PairTO.StartTime > dayPairs[pairIndex - 1].EndTime)))
                    {
                        if (!PairTO.StartTime.Equals(new DateTime()))
                        {
                            IOPairProcessedTO overtimePairToInsert = overtimePair(PairTO.StartTime, newStart);
                            if (validateOvertimePair(overtimePairToInsert, overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                            {
                                dayPairs.Insert(pairIndex, overtimePairToInsert);
                            }
                            else
                                return dayPairs;
                        }
                    }
                    // if previous pair is overtime, prolong it by moving it's end to new start 
                    else
                    {
                        if (validateOvertimePair(dayPairs[pairIndex - 1], dayPairs[pairIndex - 1].StartTime, newStart, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            dayPairs[pairIndex - 1].EndTime = newStart;
                            dayPairs[pairIndex - 1].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[pairIndex - 1].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[pairIndex - 1].PassTypeID))
                                dayPairs[pairIndex - 1].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[pairIndex - 1].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[pairIndex - 1].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                        }
                        else
                            return dayPairs;
                    }
                }

                pairIndex = indexOf(dayPairs, PairTO);

                // change duration to next pairs
                for (int i = pairIndex + 1; i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (dayPairs[i].StartTime < newStart)
                    {
                        if (validateOvertimePair(dayPairs[i], newStart, dayPairs[i].EndTime, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            dayPairs[i].StartTime = newStart;
                            dayPairs[i].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[i].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID))
                                dayPairs[i].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[i].PassTypeID], ForVerification, ForConfirmation);
                                
                            dayPairs[i].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                            if (dayPairs[i].EndTime < dayPairs[i].StartTime)
                                dayPairs[i].EndTime = dayPairs[i].StartTime;
                        }
                        else
                            break;
                    }
                }

                return dayPairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> RearangePreviousOvertimeEnd(List<IOPairProcessedTO> dayPairs, ref DateTime newEnd, int minimalPresence, ControlEventArgs args)
        {
            try
            {
                int pairIndex = indexOf(dayPairs, PairTO);
                // if changing overtime pair is last, insert new overtime pair if it is valid
                if (pairIndex >= 0 && (pairIndex == dayPairs.Count - 1
                    || (pairIndex == dayPairs.Count - 2 && dayPairs.Count > 0 && !dayPairs[dayPairs.Count - 1].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))))
                {
                    IOPairProcessedTO overtimePairToInsert = overtimePair(newEnd, PairTO.EndTime);
                    if (validateOvertimePair(overtimePairToInsert, overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                    {
                        dayPairs.Add(overtimePairToInsert);
                    }
                    else
                        return dayPairs;
                }

                // check next pair
                else if (pairIndex >= 0 && pairIndex < dayPairs.Count - 1 && dayPairs[pairIndex + 1].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                {
                    // if next pair is not overtime or is overtime but there is 'hole' between those pairs, insert overtime pair if it is valid
                    if (PassTypesAllDic.ContainsKey(dayPairs[pairIndex + 1].PassTypeID) && (PassTypesAllDic[dayPairs[pairIndex + 1].PassTypeID].IsPass != Constants.overtimePassType
                        || (PassTypesAllDic[dayPairs[pairIndex + 1].PassTypeID].IsPass == Constants.overtimePassType && PairTO.EndTime < dayPairs[pairIndex + 1].StartTime)))
                    {
                        IOPairProcessedTO overtimePairToInsert = overtimePair(newEnd, PairTO.EndTime);
                        if (validateOvertimePair(overtimePairToInsert, overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            dayPairs.Insert(pairIndex + 1, overtimePairToInsert);
                        }
                        else
                            return dayPairs;
                    }
                    // if next pair is overtime and there is no 'hole' between those pairs, prolong it by moving it's start to new end
                    else
                    {
                        if (validateOvertimePair(dayPairs[pairIndex + 1], newEnd, dayPairs[pairIndex + 1].EndTime, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            dayPairs[pairIndex + 1].StartTime = newEnd;
                            dayPairs[pairIndex + 1].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[pairIndex + 1].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[pairIndex + 1].PassTypeID))
                                dayPairs[pairIndex + 1].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[pairIndex + 1].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[pairIndex + 1].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                        }
                        else
                            return dayPairs;
                    }
                }

                pairIndex = indexOf(dayPairs, PairTO);

                // change duration to previous pairs
                for (int i = 0; i < pairIndex && i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (dayPairs[i].EndTime > newEnd)
                    {
                        if (validateOvertimePair(dayPairs[i], dayPairs[i].StartTime, newEnd, minimalPresence, dayPairs, new DateTime(), new DateTime(), args))
                        {
                            dayPairs[i].EndTime = newEnd;
                            dayPairs[i].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[i].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID))
                                dayPairs[i].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[i].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[i].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                            if (dayPairs[i].EndTime < dayPairs[i].StartTime)
                                dayPairs[i].StartTime = dayPairs[i].EndTime;
                        }
                        else
                            break;
                    }
                }

                return dayPairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> RearangeNextPairsEnd(List<IOPairProcessedTO> dayPairs, ref DateTime newEnd, int minimalPresence, ControlEventArgs args)
        {
            try
            {
                WorkTimeIntervalTO pairInterval = getPairInterval(PairTO, dayPairs);
                int pairIndex = indexOf(dayPairs, PairTO);

                // if pair is delay, do delay rounding
                if (Rules.ContainsKey(Constants.RuleCompanyDelay) && PairTO.PassTypeID == Rules[Constants.RuleCompanyDelay].RuleValue)
                {
                    int delayMax = 0;
                    int delayRounding = 1;

                    if (Empl.Type.Trim().ToUpper() != Constants.emplExtraOrdinary.Trim().ToUpper())
                    {
                        if (Rules.ContainsKey(Constants.RuleDelayMax))
                            delayMax = Rules[Constants.RuleDelayMax].RuleValue;
                        if (Rules.ContainsKey(Constants.RuleDelayRounding))
                            delayRounding = Rules[Constants.RuleDelayRounding].RuleValue;
                    }

                    if (newEnd.Minute % delayRounding != 0)
                    {
                        newEnd = newEnd.AddMinutes(delayRounding - (newEnd.Minute % delayRounding));
                        if (newEnd > dayEnd)
                            newEnd = dayEnd;
                    }

                    if (newEnd.TimeOfDay.Subtract(PairTO.StartTime.TimeOfDay).TotalMinutes > delayMax)
                    {
                        args.Error = rm.GetString("delayMaxDurationExceeded", culture);
                        return dayPairs;
                    }
                }

                // if end is moved out of interval, insert overtime pair and change new end to interval end
                if (newEnd.TimeOfDay > pairInterval.EndTime.TimeOfDay)
                {
                    int overtimeMinPresence = 1;
                    int overtimeRounding = 1;
                    if (Rules.ContainsKey(Constants.RuleOvertimeMinimum))
                        overtimeMinPresence = Rules[Constants.RuleOvertimeMinimum].RuleValue;
                    if (Rules.ContainsKey(Constants.RuleOvertimeRounding))
                        overtimeRounding = Rules[Constants.RuleOvertimeRounding].RuleValue;

                    int duration = (int)newEnd.TimeOfDay.Subtract(pairInterval.EndTime.TimeOfDay).TotalMinutes;
                    if (newEnd.TimeOfDay == new TimeSpan(23, 59, 0))
                        duration++;

                    if (duration % overtimeRounding != 0)
                    {
                        newEnd = newEnd.AddMinutes(-(newEnd.TimeOfDay.Subtract(pairInterval.EndTime.TimeOfDay).TotalMinutes % overtimeRounding));
                        if (newEnd < dayBegining)
                            newEnd = dayBegining;
                    }

                    IOPairProcessedTO overtimePairToInsert = overtimePair(new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, pairInterval.EndTime.Hour, pairInterval.EndTime.Minute, 0), newEnd);

                    if (!validateOvertimePairOverlap(dayPairs, overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, args) && validateOvertimePair(overtimePairToInsert,
                        overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, overtimeMinPresence, dayPairs, new DateTime(), new DateTime(), args))
                    {
                        dayPairs.Insert(indexOfLastInInterval(dayPairs, pairInterval) + 1, overtimePairToInsert);
                        newEnd = new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, pairInterval.EndTime.Hour, pairInterval.EndTime.Minute, 0);
                    }
                    else
                        return dayPairs;
                }

                pairIndex = indexOf(dayPairs, PairTO);

                for (int i = pairIndex + 1; i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (dayPairs[i].StartTime < newEnd)
                    {
                        if (validatePair(dayPairs[i], newEnd, dayPairs[i].EndTime, minimalPresence, args, pairInterval))
                        {
                            dayPairs[i].StartTime = newEnd;
                            dayPairs[i].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[i].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID))
                                dayPairs[i].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[i].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[i].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                            if (dayPairs[i].EndTime < dayPairs[i].StartTime)
                                dayPairs[i].EndTime = dayPairs[i].StartTime;
                        }
                        else
                            break;
                    }
                }

                return dayPairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IOPairProcessedTO> RearangePreviousPairsEnd(List<IOPairProcessedTO> dayPairs, ref DateTime newEnd, int minimalPresence, ControlEventArgs args)
        {
            try
            {
                WorkTimeIntervalTO pairInterval = getPairInterval(PairTO, dayPairs);
                int pairIndex = indexOf(dayPairs, PairTO);

                // if end is moved out of interval, insert overtime pair and change new end to interval start
                if (newEnd.TimeOfDay < pairInterval.StartTime.TimeOfDay)
                {
                    int overtimeMinPresence = 1;
                    int overtimeRounding = 1;
                    if (Rules.ContainsKey(Constants.RuleOvertimeMinimum))
                        overtimeMinPresence = Rules[Constants.RuleOvertimeMinimum].RuleValue;
                    if (Rules.ContainsKey(Constants.RuleOvertimeRounding))
                        overtimeRounding = Rules[Constants.RuleOvertimeRounding].RuleValue;
                    
                    if (pairInterval.StartTime.TimeOfDay.Subtract(newEnd.TimeOfDay).TotalMinutes % overtimeRounding != 0)
                    {
                        newEnd = newEnd.AddMinutes(-(pairInterval.StartTime.TimeOfDay.Subtract(newEnd.TimeOfDay).TotalMinutes % overtimeRounding));
                        if (newEnd > dayEnd)
                            newEnd = dayEnd;
                    }

                    IOPairProcessedTO overtimePairToInsert = overtimePair(newEnd, new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, pairInterval.StartTime.Hour, pairInterval.StartTime.Minute, 0));

                    if (!validateOvertimePairOverlap(dayPairs, overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, args) && validateOvertimePair(overtimePairToInsert,
                        overtimePairToInsert.StartTime, overtimePairToInsert.EndTime, overtimeMinPresence, dayPairs, new DateTime(), new DateTime(), args))
                    {
                        dayPairs.Insert(indexOfFirstInInterval(dayPairs, pairInterval), overtimePairToInsert);
                        newEnd = new DateTime(PairTO.IOPairDate.Year, PairTO.IOPairDate.Month, PairTO.IOPairDate.Day, pairInterval.StartTime.Hour, pairInterval.StartTime.Minute, 0);
                    }
                    else
                        return dayPairs;
                }

                // if pair is interval end, insert unjustified pair
                if (PairTO.EndTime.TimeOfDay == pairInterval.EndTime.TimeOfDay)
                {
                    // if pair is delay, do delay rounding
                    if (Rules.ContainsKey(Constants.RuleCompanyDelay) && PairTO.PassTypeID == Rules[Constants.RuleCompanyDelay].RuleValue)
                    {
                        int delayRounding = 1;
                        if (Rules.ContainsKey(Constants.RuleDelayRounding))
                            delayRounding = Rules[Constants.RuleDelayRounding].RuleValue;

                        if (newEnd.Minute % delayRounding != 0)
                        {
                            newEnd = newEnd.AddMinutes(delayRounding - (newEnd.Minute % delayRounding));
                            if (newEnd > dayEnd)
                                newEnd = dayEnd;
                        }
                    }

                    IOPairProcessedTO unjustifiedPairToInsert = unjustifiedPair(newEnd, new DateTime(newEnd.Year, newEnd.Month, newEnd.Day, pairInterval.EndTime.Hour, pairInterval.EndTime.Minute, 0));
                    if (validatePair(unjustifiedPairToInsert, unjustifiedPairToInsert.StartTime, unjustifiedPairToInsert.EndTime, minimalPresence, args, pairInterval))
                        dayPairs.Insert(pairIndex + 1, unjustifiedPairToInsert);
                    else
                        return dayPairs;
                }
                else
                {
                    pairIndex = indexOf(dayPairs, PairTO);

                    if (pairIndex >= 0)
                    {
                        // if pair is delay, do delay rounding
                        if (Rules.ContainsKey(Constants.RuleCompanyDelay) && PairTO.PassTypeID == Rules[Constants.RuleCompanyDelay].RuleValue)
                        {
                            int delayRounding = 1;
                            if (Rules.ContainsKey(Constants.RuleDelayRounding))
                                delayRounding = Rules[Constants.RuleDelayRounding].RuleValue;

                            if (newEnd.Minute % delayRounding != 0)
                            {
                                newEnd = newEnd.AddMinutes(delayRounding - (newEnd.Minute % delayRounding));
                                if (newEnd > dayEnd)
                                    newEnd = dayEnd;
                            }
                        }

                        // prolong next pair by moving it's start to new end 
                        if (pairIndex < dayPairs.Count - 1 && dayPairs[pairIndex + 1].IOPairDate.Date.Equals(PairTO.IOPairDate.Date)
                            && validatePair(dayPairs[pairIndex + 1], newEnd, dayPairs[pairIndex + 1].EndTime, minimalPresence, args, pairInterval))
                        {
                            dayPairs[pairIndex + 1].StartTime = newEnd;
                            dayPairs[pairIndex + 1].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[pairIndex + 1].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[pairIndex + 1].PassTypeID))
                                dayPairs[pairIndex + 1].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[pairIndex + 1].PassTypeID], ForVerification, ForConfirmation);
                            dayPairs[pairIndex + 1].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                        }
                        else
                            return dayPairs;
                    }
                }

                pairIndex = indexOf(dayPairs, PairTO);

                // change duration to previous pairs
                for (int i = 0; i < pairIndex && i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (dayPairs[i].EndTime > newEnd)
                    {
                        if (validatePair(dayPairs[i], dayPairs[i].StartTime, newEnd, minimalPresence, args, pairInterval))
                        {
                            dayPairs[i].EndTime = newEnd;
                            dayPairs[i].ManualCreated = (int)Constants.recordCreated.Manualy;
                            if (!CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], dayPairs[i].EmployeeID)
                                && PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID))
                                dayPairs[i].VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[dayPairs[i].PassTypeID], ForVerification, ForConfirmation);
                            
                            dayPairs[i].CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);

                            if (dayPairs[i].EndTime < dayPairs[i].StartTime)
                                dayPairs[i].StartTime = dayPairs[i].EndTime;
                        }
                        else
                            break;
                    }
                }

                return dayPairs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int indexOf(List<IOPairProcessedTO> dayPairs, IOPairProcessedTO pair)
        {
            try
            {
                int index = -1;

                for (int i = 0; i < dayPairs.Count; i++)
                {
                    if (pair.compare(dayPairs[i]))
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int indexOfFirstInInterval(List<IOPairProcessedTO> dayPairs, WorkTimeIntervalTO interval)
        {
            try
            {
                int index = -1;

                for (int i = 0; i < dayPairs.Count; i++)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                    {
                        if (dayPairs[i].StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && dayPairs[i].EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                            && PassTypesAllDic.ContainsKey(dayPairs[i].PassTypeID) && PassTypesAllDic[dayPairs[i].PassTypeID].IsPass != Constants.overtimePassType
                            && dayPairs[i].EndTime.Subtract(dayPairs[i].StartTime).TotalMinutes <= interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                        {
                            index = i;
                            break;
                        }
                    }
                    else if (dayPairs[i].StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && dayPairs[i].EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int indexOfLastInInterval(List<IOPairProcessedTO> dayPairs, WorkTimeIntervalTO interval)
        {
            try
            {
                int index = -1;

                for (int i = dayPairs.Count - 1; i >= 0; i--)
                {
                    if (!dayPairs[i].IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (dayPairs[i].StartTime.TimeOfDay >= getIntervalStart(interval, dayPairs).TimeOfDay && dayPairs[i].EndTime.TimeOfDay <= getIntervalEnd(interval, dayPairs).TimeOfDay)
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isWholeDayAbsence()
        {
            try
            {
                bool isWholeDayAbsence = false;
                foreach (WorkTimeIntervalTO interval in DayIntervals)
                {
                    if (isWholeIntervalPair(PairTO, interval) && (PairTO.PassTypeID == Constants.absence
                        || (PassTypesAllDic.ContainsKey(PairTO.PassTypeID) && PassTypesAllDic[PairTO.PassTypeID].IsPass == Constants.wholeDayAbsence)))
                    {
                        // second night shift interval absence
                        if (Common.Misc.isThirdShiftEndInterval(interval))
                        {
                            // get last pair from first night shift interval
                            IOPairProcessed pair = new IOPairProcessed(Session[Constants.sessionConnection]);
                            pair.IOPairProcessedTO.EmployeeID = PairTO.EmployeeID;
                            pair.IOPairProcessedTO.EndTime = new DateTime(PairTO.IOPairDate.AddDays(-1).Year, PairTO.IOPairDate.AddDays(-1).Month, PairTO.IOPairDate.AddDays(-1).Day, 23, 59, 0);
                            List<IOPairProcessedTO> prevPairList = pair.Search();

                            if (prevPairList.Count > 0)
                            {
                                foreach (WorkTimeIntervalTO prevInterval in PrevDayIntervals)
                                {
                                    if (isWholeIntervalPair(prevPairList[0], prevInterval) && (prevPairList[0].PassTypeID == Constants.absence
                                        || (PassTypesAllDic.ContainsKey(prevPairList[0].PassTypeID) && PassTypesAllDic[prevPairList[0].PassTypeID].IsPass == Constants.wholeDayAbsence)))
                                    {
                                        isWholeDayAbsence = true;
                                        prevPairPT = prevPairList[0].PassTypeID;
                                        break;
                                    }
                                }
                            }
                            else
                                isWholeDayAbsence = true;
                        }
                        // first night shift interval absence
                        else if (Common.Misc.isThirdShiftBeginningInterval(interval))
                        {
                            // get first pair from second night shift interval
                            IOPairProcessed pair = new IOPairProcessed(Session[Constants.sessionConnection]);
                            pair.IOPairProcessedTO.EmployeeID = PairTO.EmployeeID;
                            pair.IOPairProcessedTO.StartTime = new DateTime(PairTO.IOPairDate.AddDays(1).Year, PairTO.IOPairDate.AddDays(1).Month, PairTO.IOPairDate.AddDays(1).Day, 0, 0, 0);
                            List<IOPairProcessedTO> nextPairList = pair.Search();

                            if (nextPairList.Count > 0)
                            {
                                foreach (WorkTimeIntervalTO nextInterval in NextDayIntervals)
                                {
                                    if (isWholeIntervalPair(nextPairList[0], nextInterval) && (nextPairList[0].PassTypeID == Constants.absence
                                        || (PassTypesAllDic.ContainsKey(nextPairList[0].PassTypeID) && PassTypesAllDic[nextPairList[0].PassTypeID].IsPass == Constants.wholeDayAbsence)))
                                    {
                                        isWholeDayAbsence = true;
                                        nextPairPT = nextPairList[0].PassTypeID;
                                        break;
                                    }
                                }
                            }
                            else
                                isWholeDayAbsence = true;
                        }
                        else
                            isWholeDayAbsence = true;
                        break;
                    }
                }

                return isWholeDayAbsence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isOvertime(IOPairProcessedTO pair)
        {
            try
            {
                return PassTypesAllDic.ContainsKey(pair.PassTypeID) && PassTypesAllDic[PairTO.PassTypeID].IsPass == Constants.overtimePassType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isOfficialTripCandidate(IOPairProcessedTO pair)
        {
            try
            {                
                // official trip candidate is unjustified pair that has overtime pairs following or preceeding and total duration of all those pairs are more than interval duration
                if (pair.PassTypeID != Constants.absence)
                    return false;
                else
                {
                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                    if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                    {
                        foreach (IOPairProcessedTO pairTO in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                        {
                            dayPairs.Add(new IOPairProcessedTO(pairTO));
                        }
                    }

                    WorkTimeIntervalTO interval = getPairInterval(pair, dayPairs);
                    
                    //int intervalDuration = (int)interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes;

                    //if (Common.Misc.isThirdShiftEndInterval(interval))
                    //{
                    //    // get last interval from previous day
                    //    foreach (WorkTimeIntervalTO prevInterval in PrevDayIntervals)
                    //    {
                    //        if (Common.Misc.isThirdShiftBeginningInterval(prevInterval))
                    //        {
                    //            intervalDuration += (int)prevInterval.EndTime.TimeOfDay.Subtract(prevInterval.StartTime.TimeOfDay).TotalMinutes + 1;
                    //            break;
                    //        }
                    //    }
                    //}

                    //if (Common.Misc.isThirdShiftBeginningInterval(interval))
                    //{
                    //    // get first interval from next day
                    //    foreach (WorkTimeIntervalTO nextInterval in NextDayIntervals)
                    //    {
                    //        if (Common.Misc.isThirdShiftEndInterval(nextInterval))
                    //        {
                    //            intervalDuration += (int)nextInterval.EndTime.TimeOfDay.Subtract(nextInterval.StartTime.TimeOfDay).TotalMinutes + 1;
                    //            break;
                    //        }
                    //    }
                    //}

                    int intervalDuration = 0;
                    if (Rules.ContainsKey(Constants.RuleOfficialTripDuration))
                        intervalDuration = Rules[Constants.RuleOfficialTripDuration].RuleValue * 60; // rule is in hours

                    if (intervalDuration <= 0)
                        return false;
                    else 
                    {
                        int pairIndex = indexOf(dayPairs, pair);

                        if (pair.StartTime.TimeOfDay.Equals(interval.StartTime.TimeOfDay) || pair.EndTime.TimeOfDay.Equals(interval.EndTime.TimeOfDay))
                        {                            
                            int index = pairIndex - 1;                            
                            int duration = (int)pair.EndTime.Subtract(pair.StartTime).TotalMinutes;

                            if (pair.EndTime.Hour == 23 && pair.EndTime.Minute == 59)
                                duration++;

                            if (pair.StartTime.TimeOfDay.Equals(interval.StartTime.TimeOfDay))
                            {
                                // get overtime pairs preceeding pair
                                DateTime pairStart = pair.StartTime;
                                while (index >= 0 && index < dayPairs.Count && (dayPairs[index].EndTime.TimeOfDay.Equals(pairStart.TimeOfDay)
                                    || (pairStart.Hour == 0 && pairStart.Minute == 0 && dayPairs[index].EndTime.Hour == 23 && dayPairs[index].EndTime.Minute == 59
                                    && pairStart.Date.Equals(dayPairs[index].IOPairDate.Date.AddDays(1))) && dayPairs[index].PassTypeID == Constants.absence) 
                                    && dayPairs[index].PassTypeID != Constants.ptEmptyInterval)
                                {
                                    pairStart = dayPairs[index].StartTime;

                                    int pairDuration = (int)dayPairs[index].EndTime.Subtract(dayPairs[index].StartTime).TotalMinutes;

                                    if (dayPairs[index].EndTime.Hour == 23 && dayPairs[index].EndTime.Minute == 59)
                                        pairDuration++;

                                    duration += pairDuration;
                                    index--;
                                }
                            }

                            if (pair.EndTime.TimeOfDay.Equals(interval.EndTime.TimeOfDay))
                            {
                                // get overtime pairs following pair
                                DateTime pairEnd = pair.EndTime;
                                index = pairIndex + 1;
                                while (index >= 0 && index < dayPairs.Count && (dayPairs[index].StartTime.TimeOfDay.Equals(pairEnd.TimeOfDay)
                                    || (pairEnd.Hour == 23 && pairEnd.Minute == 59 && dayPairs[index].StartTime.Hour == 0 && dayPairs[index].StartTime.Minute == 0
                                    && pairEnd.Date.Equals(dayPairs[index].IOPairDate.Date.AddDays(-1))) && dayPairs[index].PassTypeID == Constants.absence) 
                                    && dayPairs[index].PassTypeID != Constants.ptEmptyInterval)
                                {
                                    pairEnd = dayPairs[index].EndTime;

                                    int pairDuration = (int)dayPairs[index].EndTime.Subtract(dayPairs[index].StartTime).TotalMinutes;

                                    if (dayPairs[index].EndTime.Hour == 23 && dayPairs[index].EndTime.Minute == 59)
                                        pairDuration++;

                                    duration += pairDuration;
                                    index++;
                                }
                            }

                            if (duration < intervalDuration)
                                return false;
                            else
                                return true;
                        }
                        else
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isPersonalHoliday(IOPairProcessedTO pair, List<IOPairProcessedTO> dayPairs)
        {
            try
            {
                // expat out does not have standard holidays
                if (isExpatOut)
                    return false;

                string holidayType = "";
                DateTime pairDate = pair.IOPairDate.Date;

                // if pair is from second interval of night shift, it belongs to previous day
                WorkTimeIntervalTO pairInterval = getPairInterval(pair, dayPairs);
                if (pairInterval.TimeSchemaID != -1 && Common.Misc.isThirdShiftEndInterval(pairInterval))
                    pairDate = pairDate.AddDays(-1);

                // check if date is personal holiday, no transfering holidays for personal holidays
                // get employee personal holiday category
                EmployeeAsco4 emplAsco = new EmployeeAsco4(Session[Constants.sessionConnection]);
                emplAsco.EmplAsco4TO.EmployeeID = pair.EmployeeID;
                List<EmployeeAsco4TO> ascoList = emplAsco.Search();

                if (ascoList.Count == 1)
                {
                    holidayType = ascoList[0].NVarcharValue1.Trim();

                    if (!holidayType.Trim().Equals(""))
                    {
                        // if category is IV, find holiday date and check if pair date is holiday
                        if (holidayType.Trim().Equals(Constants.holidayTypeIV.Trim()))
                        {
                            DateTime holDate = ascoList[0].DatetimeValue1.Date;

                            if (holDate.Month == pairDate.Month && holDate.Day == pairDate.Day)
                                return true;
                        }
                        else
                        {
                            // if category is I, II or III, check if pair date is personal holiday
                            HolidaysExtended holExtended = new HolidaysExtended(Session[Constants.sessionConnection]);
                            holExtended.HolTO.Type = Constants.personalHoliday.Trim();
                            holExtended.HolTO.Category = holidayType.Trim();

                            if (holExtended.Search(pairDate, pairDate).Count > 0)
                                return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isNationalHoliday(IOPairProcessedTO pair, List<IOPairProcessedTO> dayPairs)
        {
            try
            {
                // expat out does not have standard holidays
                if (isExpatOut)
                    return false;

                DateTime pairDate = pair.IOPairDate.Date;

                // if pair is from second interval of night shift, it belongs to previous day
                WorkTimeIntervalTO pairInterval = getPairInterval(pair, dayPairs);
                if (pairInterval.TimeSchemaID != -1 && Common.Misc.isThirdShiftEndInterval(pairInterval))
                    pairDate = pairDate.AddDays(-1);

                // check if date is national holiday, national holidays are transferd from Sunday to first working day
                List<DateTime> nationalHolidaysDays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundays = new List<DateTime>();
                List<DateTime> nationalHolidaysSundaysDays = new List<DateTime>();
                Dictionary<string, List<DateTime>> personalHolidayDays = new Dictionary<string, List<DateTime>>();
                List<HolidaysExtendedTO> nationalTransferableHolidays = new List<HolidaysExtendedTO>();

                Common.Misc.getHolidays(pairDate, pairDate, personalHolidayDays, nationalHolidaysDays, nationalHolidaysSundays, Session[Constants.sessionConnection], nationalTransferableHolidays);

                if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeIndustrial.Trim().ToUpper()))
                {
                    if (nationalHolidaysDays.Contains(pairDate.Date))
                        return true;
                }
                else if (nationalHolidaysDays.Contains(pairDate.Date) || nationalHolidaysSundays.Contains(pairDate.Date))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool validateOvertimePairOverlap(List<IOPairProcessedTO> dayPairs, DateTime start, DateTime end, ControlEventArgs args)
        {
            try
            {
                bool isOverlaping = false;
                foreach (IOPairProcessedTO pair in dayPairs)
                {
                    if (!pair.IOPairDate.Date.Equals(PairTO.IOPairDate.Date))
                        continue;

                    if (pair.StartTime.Equals(pair.EndTime) && (pair.StartTime.Equals(start) || pair.StartTime.Equals(end)))
                        continue;

                    if ((start.TimeOfDay <= pair.StartTime.TimeOfDay && end.TimeOfDay > pair.StartTime.TimeOfDay)
                        || (pair.StartTime.TimeOfDay <= start.TimeOfDay && pair.EndTime.TimeOfDay >= end.TimeOfDay)
                        || (start.TimeOfDay < pair.EndTime.TimeOfDay && end.TimeOfDay >= pair.EndTime.TimeOfDay))
                    {
                        isOverlaping = true;
                        args.Error = rm.GetString("overtimePairOverlap", culture);
                        break;
                    }
                }

                return isOverlaping;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private WorkTimeIntervalTO getPairInterval(IOPairProcessedTO pair, List<IOPairProcessedTO> dayPairs)
        {
            try
            {
                WorkTimeIntervalTO pairInterval = new WorkTimeIntervalTO();
                foreach (WorkTimeIntervalTO interval in DayIntervals)
                {
                    if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                    {
                        if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay 
                            && PassTypesAllDic.ContainsKey(pair.PassTypeID) && PassTypesAllDic[pair.PassTypeID].IsPass != Constants.overtimePassType
                            && pair.EndTime.Subtract(pair.StartTime).TotalMinutes <= interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                        {
                            pairInterval = interval.Clone();
                            pairInterval.StartTime = getIntervalStart(interval, dayPairs);
                            pairInterval.EndTime = getIntervalEnd(interval, dayPairs);
                            break;
                        }
                    }
                    else if (pair.StartTime.TimeOfDay >= interval.StartTime.TimeOfDay && pair.EndTime.TimeOfDay <= interval.EndTime.TimeOfDay)
                    {
                        pairInterval = interval;
                        break;
                    }
                }

                return pairInterval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private WorkTimeIntervalTO getIntervalBeforePair(IOPairProcessedTO pair, List<IOPairProcessedTO> dayPairs)
        {
            try
            {
                WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                for (int i = DayIntervals.Count - 1; i >= 0; i--)
                {
                    if (pair.StartTime.TimeOfDay >= getIntervalEnd(DayIntervals[i], dayPairs).TimeOfDay)
                    {
                        interval = DayIntervals[i].Clone();
                        interval.StartTime = getIntervalStart(DayIntervals[i], dayPairs);
                        interval.EndTime = getIntervalEnd(DayIntervals[i], dayPairs);
                        break;
                    }
                }

                return interval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private WorkTimeIntervalTO getIntervalAfterPair(IOPairProcessedTO pair, List<IOPairProcessedTO> dayPairs)
        {
            try
            {
                WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                for (int i = 0; i < DayIntervals.Count; i++)
                {
                    if (pair.EndTime.TimeOfDay <= getIntervalStart(DayIntervals[i], dayPairs).TimeOfDay)
                    {
                        interval = DayIntervals[i].Clone();
                        interval.StartTime = getIntervalStart(DayIntervals[i], dayPairs);
                        interval.EndTime = getIntervalEnd(DayIntervals[i], dayPairs);
                        break;
                    }
                }

                return interval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isChangingDurationValid(IOPairProcessedTO pair)
        {
            try
            {
                // duration can not be changed to overtime that are not to jusify type and whole day absences
                if ((isOvertime(pair) && pair.PassTypeID != Constants.overtimeUnjustified)
                    || (PassTypesAllDic.ContainsKey(pair.PassTypeID) && PassTypesAllDic[pair.PassTypeID].IsPass == Constants.wholeDayAbsence) || ForConfirmation || ForVerification) 
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool validateOvertimePair(IOPairProcessedTO pair, DateTime start, DateTime end, int minimalPresence, List<IOPairProcessedTO> dayPairs, DateTime intervalBeforeEnd, 
            DateTime intervalAfterStart, ControlEventArgs args)
        {
            try
            {
                // validate duration due to minimal presence
                int pairDuration = ((int)end.TimeOfDay.TotalMinutes - (int)start.TimeOfDay.TotalMinutes);

                // if it is last pair from first night shift interval, add one minute
                if (end.Hour == 23 && end.Minute == 59)
                    pairDuration++;

                if (end > start && pair.PassTypeID != Constants.overtimeUnjustified && pairDuration < minimalPresence)
                {
                    args.Error = rm.GetString("overtimeLessThenMinimum", culture);
                    return false;
                }

                // validate duration due to overtime rounding rule
                int minPresenceRounding = 1;
                if (!IsWorkAbsenceDay)
                {
                    if (Rules.ContainsKey(Constants.RuleOvertimeRounding))
                        minPresenceRounding = Rules[Constants.RuleOvertimeRounding].RuleValue;
                }
                else
                {
                    if (Rules.ContainsKey(Constants.RuleOvertimeRoundingOutWS))
                        minPresenceRounding = Rules[Constants.RuleOvertimeRoundingOutWS].RuleValue;
                }

                if (pairDuration % minPresenceRounding != 0)
                {
                    args.Error = rm.GetString("overtimeNotValidRoundingRule", culture);
                    return false;
                }

                // validate if shift start/end overtime Rules are satisfied
                int shiftStart = 1;
                int shiftEnd = 1;
                if (Rules.ContainsKey(Constants.RuleOvertimeShiftStart))
                    shiftStart = Rules[Constants.RuleOvertimeShiftStart].RuleValue;
                if (Rules.ContainsKey(Constants.RuleOvertimeShiftEnd))
                    shiftEnd = Rules[Constants.RuleOvertimeShiftEnd].RuleValue;
                IOPairProcessedTO pairToValidate = new IOPairProcessedTO(pair);
                pairToValidate.StartTime = start;
                pairToValidate.EndTime = end;
                WorkTimeIntervalTO intervalBefore = getIntervalBeforePair(pairToValidate, dayPairs);

                if (!intervalBeforeEnd.Equals(new DateTime()))
                    intervalBefore.EndTime = intervalBeforeEnd;

                double intervalBeforeDuration = intervalBefore.EndTime.TimeOfDay.Subtract(intervalBefore.StartTime.TimeOfDay).TotalMinutes;

                WorkTimeIntervalTO intervalAfter = getIntervalAfterPair(pairToValidate, dayPairs);

                if (!intervalAfterStart.Equals(new DateTime()))
                    intervalAfter.StartTime = intervalAfterStart;

                double intervalAfterDuration = intervalAfter.EndTime.TimeOfDay.Subtract(intervalAfter.StartTime.TimeOfDay).TotalMinutes;

                if (!intervalBefore.EndTime.Equals(new DateTime()) && intervalBeforeDuration > 0 && pairToValidate.EndTime.TimeOfDay.Subtract(intervalBefore.EndTime.TimeOfDay).TotalMinutes < shiftEnd)
                {
                    args.Error = rm.GetString("overtimeBeforeShiftEndRule", culture);
                    return false;
                }

                if (!intervalAfter.StartTime.Equals(new DateTime()) && intervalAfterDuration > 0 && intervalAfter.StartTime.TimeOfDay.Subtract(pairToValidate.StartTime.TimeOfDay).TotalMinutes < shiftStart)
                {
                    args.Error = rm.GetString("overtimeBeforeShiftStartRule", culture);
                    return false;
                }

                // validate if overtime pair can be changed
                if (pair.PassTypeID != Constants.overtimeUnjustified)
                {
                    args.Error = rm.GetString("overtimeJustifiedChange", culture);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool validatePair(IOPairProcessedTO pair, DateTime start, DateTime end, int minimalPresence, ControlEventArgs args, WorkTimeIntervalTO interval)
        {
            try
            {
                if (!isChangingDurationValid(pair))
                {
                    args.Error = rm.GetString("changingNotAllowedPair", culture);
                    return false;
                }
                if (Rules.ContainsKey(Constants.RuleCompanyRegularWork) && pair.PassTypeID == Rules[Constants.RuleCompanyRegularWork].RuleValue)
                {
                    // validate duration due to minimal presence
                    int pairDuration = ((int)end.TimeOfDay.TotalMinutes - (int)start.TimeOfDay.TotalMinutes);

                    // if it is last pair from first night shift interval, add one minute
                    if (end.Hour == 23 && end.Minute == 59)
                        pairDuration++;

                    if (end > start && pairDuration < minimalPresence)
                    {
                        args.Error = rm.GetString("pairLessThenMinimum", culture);
                        return false;
                    }
                }

                if (Rules.ContainsKey(Constants.RuleCompanyDelay) && pair.PassTypeID == Rules[Constants.RuleCompanyDelay].RuleValue)
                {
                    // validate duration due to minimal presence
                    int pairDuration = ((int)end.TimeOfDay.TotalMinutes - (int)start.TimeOfDay.TotalMinutes);

                    // if it is last pair from first night shift interval, add one minute
                    if (end.Hour == 23 && end.Minute == 59)
                        pairDuration++;

                    int delayMax = pairDuration;
                    if (Rules.ContainsKey(Constants.RuleDelayMax))
                        delayMax = Rules[Constants.RuleDelayMax].RuleValue;
                    
                    if (pairDuration > delayMax)
                    {
                        args.Error = rm.GetString("delayMaxDurationExceeded", culture);
                        return false;
                    }
                }

                // validate if pair is from same time schema interval
                if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    if (!(pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                        && pair.EndTime.Subtract(pair.StartTime).TotalMinutes <= interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes))
                    {
                        args.Error = rm.GetString("pairNotSameInterval", culture);
                        return false;
                    }
                }
                else if (pair.StartTime.TimeOfDay < interval.StartTime.TimeOfDay || pair.EndTime.TimeOfDay > interval.EndTime.TimeOfDay)
                {
                    args.Error = rm.GetString("pairNotSameInterval", culture);
                    return false;
                }
                                
                // change counter for bank hour used type or stop working
                if ((Rules.ContainsKey(Constants.RuleBankHoursUsedRounding) && Rules.ContainsKey(Constants.RuleCompanyBankHourUsed)
                    && pair.PassTypeID == Rules[Constants.RuleCompanyBankHourUsed].RuleValue)
                    || (Rules.ContainsKey(Constants.RuleCompanyStopWorking) && pair.PassTypeID == Rules[Constants.RuleCompanyStopWorking].RuleValue)
                    || (Rules.ContainsKey(Constants.RuleInitialOvertimeUsedRounding) && Rules.ContainsKey(Constants.RuleCompanyInitialOvertimeUsed)
                    && pair.PassTypeID == Rules[Constants.RuleCompanyInitialOvertimeUsed].RuleValue))
                {
                    // if start is after end, move start to end of pair, becouse of recalculation of counters - this pair will be 0 duration pair anyway
                    if (start > end)
                        start = end;

                    Dictionary<int, EmployeeCounterValueTO> emplCounters = new Dictionary<int, EmployeeCounterValueTO>();
                    if (Session[Constants.sessionEmplCounters] != null && Session[Constants.sessionEmplCounters] is Dictionary<int, EmployeeCounterValueTO>)
                    {
                        foreach (int counterType in ((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters]).Keys)
                        {
                            emplCounters.Add(counterType, new EmployeeCounterValueTO(((Dictionary<int, EmployeeCounterValueTO>)Session[Constants.sessionEmplCounters])[counterType]));
                        }
                    }

                    List<IOPairProcessedTO> dayPairs = new List<IOPairProcessedTO>();
                    if (Session[Constants.sessionDayPairs] != null && Session[Constants.sessionDayPairs] is List<IOPairProcessedTO>)
                    {
                        foreach (IOPairProcessedTO pairTO in (List<IOPairProcessedTO>)Session[Constants.sessionDayPairs])
                        {
                            dayPairs.Add(new IOPairProcessedTO(pairTO));
                        }
                    }

                    List<IOPairProcessedTO> oldPairs = new List<IOPairProcessedTO>();
                    oldPairs.Add(new IOPairProcessedTO(pair));
                    List<IOPairProcessedTO> newPairs = new List<IOPairProcessedTO>();
                    IOPairProcessedTO newPair = new IOPairProcessedTO(pair);
                    newPair.StartTime = start;
                    newPair.EndTime = end;
                    newPairs.Add(newPair);
                    Dictionary<DateTime, WorkTimeSchemaTO> daySchemas = new Dictionary<DateTime, WorkTimeSchemaTO>();
                    daySchemas.Add(PairTO.IOPairDate.Date, EmplTimeSchema);
                    Dictionary<DateTime, List<WorkTimeIntervalTO>> dayIntervalsList = new Dictionary<DateTime, List<WorkTimeIntervalTO>>();
                    dayIntervalsList.Add(PairTO.IOPairDate.Date, DayIntervals);

                    List<DateTime> exceptDates = new List<DateTime>();
                    foreach (IOPairProcessedTO dayPair in dayPairs)
                    {
                        if (!exceptDates.Contains(dayPair.IOPairDate.Date))
                            exceptDates.Add(dayPair.IOPairDate.Date);
                    }

                    string error = Common.Misc.validatePairsPassType(PairTO.EmployeeID, EmplAsco, PairTO.IOPairDate, PairTO.IOPairDate, newPairs, oldPairs, dayPairs, ref emplCounters, Rules,
                        PassTypesAllDic, PassTypeLimits, SchDict, daySchemas, dayIntervalsList, null, null, null, dayPairs, exceptDates, null, Session[Constants.sessionConnection], 
                        CommonWeb.Misc.checkLimit(Session[Constants.sessionLoginCategory]), true, true, false);

                    if (!error.Trim().Equals(""))
                    {
                        args.Error = rm.GetString(error, culture);
                        return false;
                    }

                    Dictionary<int, EmployeeCounterValueTO> sessionEmplCounters = new Dictionary<int, EmployeeCounterValueTO>();
                    foreach (int counterType in emplCounters.Keys)
                    {
                        sessionEmplCounters.Add(counterType, emplCounters[counterType]);
                    }

                    Session[Constants.sessionEmplCounters] = sessionEmplCounters;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        private bool isWholeIntervalPair(IOPairProcessedTO pair, WorkTimeIntervalTO interval)
        {
            try
            {
                bool wholeIntervalPair = false;

                if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    if (pair.StartTime.TimeOfDay >= interval.EarliestArrived.TimeOfDay && pair.EndTime.TimeOfDay <= interval.LatestLeft.TimeOfDay
                        && pair.EndTime.Subtract(pair.StartTime).TotalMinutes == interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes)
                    {
                        wholeIntervalPair = true;
                    }
                }
                else if (interval.EndTime.TimeOfDay == pair.EndTime.TimeOfDay && interval.StartTime.TimeOfDay == pair.StartTime.TimeOfDay)
                    wholeIntervalPair = true;

                return wholeIntervalPair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DateTime getIntervalStart(WorkTimeIntervalTO interval, List<IOPairProcessedTO> dayPairs)
        {
            try
            {
                DateTime intervalStart = interval.StartTime;

                if (EmplTimeSchema.Type.Trim().ToUpper().Equals(Constants.schemaTypeFlexi.Trim().ToUpper()))
                {
                    int index = indexOfFirstInInterval(dayPairs, interval);

                    if (index >= 0 && index < dayPairs.Count)
                        intervalStart = dayPairs[index].StartTime;
                }

                return intervalStart;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DateTime getIntervalEnd(WorkTimeIntervalTO interval, List<IOPairProcessedTO> dayPairs)
        {
            try
            {
                DateTime intervalEnd = getIntervalStart(interval, dayPairs).AddMinutes((int)(interval.EndTime.TimeOfDay.Subtract(interval.StartTime.TimeOfDay).TotalMinutes));

                return intervalEnd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
               
        // return true if cut off date passed
        private bool checkCutOffDate(DateTime date)
        {
            try
            {
                int cutOffDate = -1;
                Dictionary<int, WorkingUnitTO> WUnits = new WorkingUnit(Session[Constants.sessionConnection]).getWUDictionary();
                if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.HRSSC)
                {
                    if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    {
                        EmployeeTO Empl = (EmployeeTO)Session[Constants.sessionLogInEmployee];

                        int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);

                        if (company != -1)
                        {
                            Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                            rule.RuleTO.WorkingUnitID = company;
                            rule.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                            rule.RuleTO.RuleType = Constants.RuleHRSSCCutOffDate;

                            List<RuleTO> rules = rule.Search();

                            if (rules.Count == 1)
                            {
                                cutOffDate = rules[0].RuleValue;
                            }
                        }
                    }
                }
                else if (Session[Constants.sessionLoginCategory] != null && Session[Constants.sessionLoginCategory] is ApplUserCategoryTO
                    && ((ApplUserCategoryTO)Session[Constants.sessionLoginCategory]).CategoryID == (int)Constants.Categories.WCManager)
                {
                    if (Session[Constants.sessionLogInEmployee] != null && Session[Constants.sessionLogInEmployee] is EmployeeTO)
                    {
                        EmployeeTO Empl = (EmployeeTO)Session[Constants.sessionLogInEmployee];
                        int company = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, WUnits);

                        if (company != -1)
                        {
                            Common.Rule rule = new Common.Rule(Session[Constants.sessionConnection]);
                            rule.RuleTO.WorkingUnitID = company;
                            rule.RuleTO.EmployeeTypeID = Empl.EmployeeTypeID;
                            rule.RuleTO.RuleType = Constants.RuleWCDRCutOffDate;

                            List<RuleTO> rules = rule.Search();

                            if (rules.Count == 1)
                            {
                                cutOffDate = rules[0].RuleValue;
                            }
                        }
                    }
                }
                else if (Rules.ContainsKey(Constants.RuleCutOffDate))
                {
                    cutOffDate = Rules[Constants.RuleCutOffDate].RuleValue;
                }

                if (date.Date >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                    return false;
                else if (cutOffDate != -1 && Common.Misc.countWorkingDays(DateTime.Now.Date, Session[Constants.sessionConnection]) <= cutOffDate 
                    && date.Date >= new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1, 0, 0, 0))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IOPairProcessedTO unjustifiedPair(DateTime start, DateTime end)
        {
            try
            {
                IOPairProcessedTO unjustifiedPair = new IOPairProcessedTO();
                unjustifiedPair.EmployeeID = PairTO.EmployeeID;
                unjustifiedPair.IOPairDate = PairTO.IOPairDate;
                unjustifiedPair.StartTime = start;
                unjustifiedPair.EndTime = end;
                unjustifiedPair.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                unjustifiedPair.LocationID = Constants.locationOut;
                unjustifiedPair.Alert = Constants.alertStatusNoAlert.ToString();
                unjustifiedPair.ManualCreated = (int)Constants.recordCreated.Manualy;
                unjustifiedPair.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                unjustifiedPair.PassTypeID = Constants.absence;
                if (PassTypesAllDic.ContainsKey(Constants.absence))
                {
                    unjustifiedPair.ConfirmationFlag = PassTypesAllDic[Constants.absence].ConfirmFlag;

                    // TL and HRSSC automatically verify pairs for other employees - unjustified is verified by default, other not verified
                    if (CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], PairTO.EmployeeID))
                    {
                        if (PassTypesAllDic[Constants.absence].VerificationFlag == (int)Constants.Verification.NotVerified)
                        {
                            unjustifiedPair.VerificationFlag = (int)Constants.Verification.Verified;
                            unjustifiedPair.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                            unjustifiedPair.VerifiedTime = DateTime.Now;
                        }
                        else
                            unjustifiedPair.VerificationFlag = (int)Constants.Verification.Verified;
                    }
                    else
                        unjustifiedPair.VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[Constants.absence], ForVerification, ForConfirmation);
                }
                else
                {
                    unjustifiedPair.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                    unjustifiedPair.VerificationFlag = (int)Constants.Verification.Verified;
                }

                unjustifiedPair.IOPairID = Constants.unjustifiedIOPairID;

                return unjustifiedPair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IOPairProcessedTO overtimePair(DateTime start, DateTime end)
        {
            try
            {
                IOPairProcessedTO overtimePair = new IOPairProcessedTO();
                overtimePair.EmployeeID = PairTO.EmployeeID;
                overtimePair.IOPairDate = PairTO.IOPairDate;
                overtimePair.StartTime = start;
                overtimePair.EndTime = end;
                overtimePair.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                overtimePair.LocationID = Constants.locationOut;
                overtimePair.Alert = Constants.alertStatusNoAlert.ToString();
                overtimePair.ManualCreated = (int)Constants.recordCreated.Manualy;
                overtimePair.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                overtimePair.PassTypeID = Constants.overtimeUnjustified;
                if (PassTypesAllDic.ContainsKey(Constants.overtimeUnjustified))
                {
                    overtimePair.ConfirmationFlag = PassTypesAllDic[Constants.overtimeUnjustified].ConfirmFlag;

                    // TL and HRSSC automatically verify pairs for other employees - unjustified is verified by default, other not verified
                    if (CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], PairTO.EmployeeID))
                    {
                        if (PassTypesAllDic[Constants.overtimeUnjustified].VerificationFlag == (int)Constants.Verification.NotVerified)
                        {
                            overtimePair.VerificationFlag = (int)Constants.Verification.Verified;
                            overtimePair.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                            overtimePair.VerifiedTime = DateTime.Now;
                        }
                        else
                            overtimePair.VerificationFlag = (int)Constants.Verification.Verified;                        
                    }
                    else
                        overtimePair.VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[Constants.overtimeUnjustified], ForVerification, ForConfirmation);                    
                }
                else
                {
                    overtimePair.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                    overtimePair.VerificationFlag = (int)Constants.Verification.Verified;
                }

                overtimePair.IOPairID = Constants.unjustifiedIOPairID;

                return overtimePair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IOPairProcessedTO delayPair(DateTime start, DateTime end)
        {
            try
            {
                IOPairProcessedTO delayPair = new IOPairProcessedTO();
                delayPair.EmployeeID = PairTO.EmployeeID;
                delayPair.IOPairDate = PairTO.IOPairDate;
                delayPair.StartTime = start;
                delayPair.EndTime = end;
                delayPair.IsWrkHrsCounter = (int)Constants.IsWrkCount.IsCounter;
                delayPair.LocationID = Constants.locationOut;
                delayPair.Alert = Constants.alertStatusNoAlert.ToString();
                delayPair.ManualCreated = (int)Constants.recordCreated.Manualy;
                delayPair.CreatedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                if (Rules.ContainsKey(Constants.RuleCompanyDelay))
                {
                    delayPair.PassTypeID = Rules[Constants.RuleCompanyDelay].RuleValue;
                    if (PassTypesAllDic.ContainsKey(Rules[Constants.RuleCompanyDelay].RuleValue))
                    {
                        delayPair.ConfirmationFlag = PassTypesAllDic[Rules[Constants.RuleCompanyDelay].RuleValue].ConfirmFlag;
                        // TL and HRSSC automatically verify pairs for other employees - unjustified is verified by default, other not verified
                        if (CommonWeb.Misc.automaticallyVerified(Session[Constants.sessionLoginCategory], Session[Constants.sessionLogInEmployee], PairTO.EmployeeID))
                        {
                            if (PassTypesAllDic[Rules[Constants.RuleCompanyDelay].RuleValue].VerificationFlag == (int)Constants.Verification.NotVerified)
                            {
                                delayPair.VerificationFlag = (int)Constants.Verification.Verified;
                                delayPair.VerifiedBy = CommonWeb.Misc.getLoginUser(Session[Constants.sessionLogInUser]);
                                delayPair.VerifiedTime = DateTime.Now;
                            }
                            else
                                delayPair.VerificationFlag = (int)Constants.Verification.Verified;
                        }
                        else
                            delayPair.VerificationFlag = CommonWeb.Misc.verificationFlag(PassTypesAllDic[Rules[Constants.RuleCompanyDelay].RuleValue], ForVerification, ForConfirmation);
                    }
                    else
                    {
                        delayPair.ConfirmationFlag = (int)Constants.Confirmation.Confirmed;
                        delayPair.VerificationFlag = (int)Constants.Verification.Verified;
                    }
                }

                delayPair.IOPairID = Constants.unjustifiedIOPairID;

                return delayPair;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}