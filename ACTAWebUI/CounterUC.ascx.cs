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
using System.Globalization;
using System.Resources;

using TransferObjects;
using Util;
using CommonWeb;

namespace ACTAWebUI
{
    public partial class CounterUC : System.Web.UI.UserControl
    {
        private const int counterWidth = 45;
        private const int counterMinuteWidth = 30;
        private const int separatorWidth = 9;
        private const int lastSeparatorWidth = 180;

        // units
        private const string day = "DAY";
        private const string hour = "HOUR";
        private const string minute = "MINUTE";

        // attributes
        private const string id = "id";        
        private const string unit = "unit";

        private const int maxMinute = 59;

        private bool readOnly = true;
        private Dictionary<int, EmployeeCounterValueTO> counterValues = new Dictionary<int, EmployeeCounterValueTO>();
        private Dictionary<int, string> counterNames = new Dictionary<int, string>();
        private int emplID = -1;
        private bool insertSeparator = true;        
        private int lastSeparatorW;
        private int reservedALDays = -1;
        private bool hideBH = false;
        
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        public bool HideBH
        {
            get { return hideBH; }
            set { hideBH = value; }
        }

        public Dictionary<int, EmployeeCounterValueTO> CounterValues
        {
            get { return counterValues; }
            set { counterValues = value; }
        }

        public Dictionary<int, string> CounterNames
        {
            get { return counterNames; }
            set { counterNames = value; }
        }

        public int EmplID
        {
            get { return emplID; }
            set { emplID = value; }
        }

        public bool InsertSeparator
        {
            get { return insertSeparator; }
            set { insertSeparator = value; }
        }

        public int ReservedALDays
        {
            get { return reservedALDays; }
            set { reservedALDays = value; }
        }

        public int LastSeparatorWidth
        {
            get { return lastSeparatorW; }
            set { lastSeparatorW = value; }
        }
                
        protected void Page_Load(object sender, EventArgs e)
        {
            // set last separator width
            if (ReservedALDays <= 0)
                LastSeparatorWidth = lastSeparatorWidth;
            else
                LastSeparatorWidth = lastSeparatorWidth - counterWidth / 2;

            // add counter names
            int counterIndex = 0;
            int usedLeave = 0;
            int thisYearLeave = 0;
            int prevYearLeave = 0;

            if (CounterValues.ContainsKey((int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter))
                usedLeave = CounterValues[(int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter].Value;
            if (CounterValues.ContainsKey((int)Constants.EmplCounterTypes.AnnualLeaveCounter))
                thisYearLeave = CounterValues[(int)Constants.EmplCounterTypes.AnnualLeaveCounter].Value;
            if (CounterValues.ContainsKey((int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter))
                prevYearLeave = CounterValues[(int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter].Value;

            if (prevYearLeave >= usedLeave)
                prevYearLeave -= usedLeave;
            else
            {
                thisYearLeave -= (usedLeave - prevYearLeave);
                prevYearLeave = 0;
            }
                        
            foreach (int type in CounterValues.Keys)
            {
                int cWidth = counterWidth;
                if (type == (int)Constants.EmplCounterTypes.AnnualLeaveCounter && ReservedALDays >= 0)
                    cWidth += counterWidth / 2;

                int width = cWidth + separatorWidth * 2 + 6;
                if (CounterValues[type].MeasureUnit.Trim().ToUpper().Equals(Constants.emplCounterMesureMinute))
                    width = (cWidth + counterMinuteWidth + separatorWidth * 3 + 12);

                Label lblCounterName = new Label();
                lblCounterName.ID = "lblCounterName" + counterIndex.ToString().Trim();
                lblCounterName.Width = new Unit(width);
                lblCounterName.CssClass = "counterSmallLblLeft";
                if (type == (int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter && ReadOnly)
                    lblCounterName.Text = DateTime.Now.AddYears(-1).Year.ToString().Trim();
                else if (type == (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter && ReadOnly)
                    lblCounterName.Text = DateTime.Now.Year.ToString().Trim();
                else if (CounterNames.ContainsKey(type))
                    lblCounterName.Text = CounterNames[type].Trim();                
                this.Controls.Add(lblCounterName);
                
                counterIndex++;
            }

            // insert separator
            if (InsertSeparator)
            {
                Label lblNameSeparator = new Label();
                lblNameSeparator.ID = "lblNameSeparator" + counterIndex.ToString().Trim();
                lblNameSeparator.Width = new Unit(LastSeparatorWidth);
                lblNameSeparator.CssClass = "counterSmallLblLeft";                              
                this.Controls.Add(lblNameSeparator);
            }

            // add counters
            counterIndex = 0;
                        
            foreach (int type in CounterValues.Keys)
            {
                if (CounterValues[type].MeasureUnit.Trim().ToUpper().Equals(Constants.emplCounterMesureMinute))
                {
                    TextBox tbCounterHour = new TextBox();
                    tbCounterHour.ID = "tbCounterHour" + counterIndex.ToString().Trim();
                    tbCounterHour.Width = new Unit(counterWidth);
                    tbCounterHour.ReadOnly = ReadOnly;
                    if (ReadOnly)
                        tbCounterHour.CssClass = "counterTbReadOnly";
                    else
                        tbCounterHour.CssClass = "counterTb";
                    tbCounterHour.Attributes.Add(id.Trim(), type.ToString().Trim());                    
                    tbCounterHour.Attributes.Add(unit.Trim(), hour.Trim());
                    if (type == (int)Constants.EmplCounterTypes.BankHoursCounter && HideBH)
                        tbCounterHour.Text = "0";
                    else
                        tbCounterHour.Text = (CounterValues[type].Value / 60).ToString().Trim();
                    tbCounterHour.AutoPostBack = true;
                    tbCounterHour.TextChanged += new EventHandler(tbCounter_TextChanged);
                    this.Controls.Add(tbCounterHour);
                    

                    Label lblCounterHourSeparator = new Label();
                    lblCounterHourSeparator.ID = "lblCounterHourSeparator" + counterIndex.ToString().Trim();
                    lblCounterHourSeparator.Text = "h";
                    lblCounterHourSeparator.Width = new Unit(separatorWidth);
                    lblCounterHourSeparator.CssClass = "counterSmallLblLeft";
                    this.Controls.Add(lblCounterHourSeparator);

                    TextBox tbCounterMinute = new TextBox();
                    tbCounterMinute.ID = "tbCounterMinute" + counterIndex.ToString().Trim();
                    tbCounterMinute.Width = new Unit(counterMinuteWidth);
                    tbCounterMinute.ReadOnly = ReadOnly;
                    if (ReadOnly)
                        tbCounterMinute.CssClass = "counterTbReadOnly";
                    else
                        tbCounterMinute.CssClass = "counterTb";
                    tbCounterMinute.Attributes.Add(id.Trim(), type.ToString().Trim());                    
                    tbCounterMinute.Attributes.Add(unit.Trim(), minute.Trim());
                    if (type == (int)Constants.EmplCounterTypes.BankHoursCounter && HideBH)
                        tbCounterMinute.Text = "0";
                    else
                        tbCounterMinute.Text = (CounterValues[type].Value % 60).ToString().Trim();
                    tbCounterMinute.AutoPostBack = true;
                    tbCounterMinute.TextChanged += new EventHandler(tbCounter_TextChanged);
                    this.Controls.Add(tbCounterMinute);

                    Label lblCounterMinuteSeparator = new Label();
                    lblCounterMinuteSeparator.ID = "lblCounterMinuteSeparator" + counterIndex.ToString().Trim();                                        
                    lblCounterMinuteSeparator.Width = new Unit(separatorWidth);
                    lblCounterMinuteSeparator.CssClass = "counterSmallLblLeft";
                    lblCounterMinuteSeparator.Text = "min";
                    this.Controls.Add(lblCounterMinuteSeparator);
                }
                else if (CounterValues[type].MeasureUnit.Trim().ToUpper().Equals(Constants.emplCounterMesureDay))
                {
                    int cWidth = counterWidth;
                    if (type == (int)Constants.EmplCounterTypes.AnnualLeaveCounter && ReservedALDays >= 0)
                        cWidth += counterWidth / 2;

                    TextBox tbCounter = new TextBox();
                    tbCounter.ID = "tbCounter" + counterIndex.ToString().Trim();
                    tbCounter.Width = new Unit(cWidth);
                    
                     //18.12.2017 Miodrag / blokiranje menjanja podataka vezanih za godisnje odmore

                    //BLOKIRANJE
                    /*if (type < 4)
                    {
                        tbCounter.ReadOnly = true; // Samo za GO podaci se mere u danima, a te podatke uzimamo iz NAVIGATOR-a
                    }*/
                        //tbCounter.ReadOnly = ReadOnly;
                    //MM
                    if (type == (int)Constants.EmplCounterTypes.AnnualLeaveCounter && ReadOnly)
                    {
                        tbCounter.Text = (prevYearLeave + thisYearLeave).ToString().Trim();
                    }
                    else if (type == (int)Constants.EmplCounterTypes.PrevAnnualLeaveCounter && ReadOnly)
                        tbCounter.Text = prevYearLeave.ToString().Trim();
                    else if (type == (int)Constants.EmplCounterTypes.UsedAnnualLeaveCounter && ReadOnly)
                        tbCounter.Text = thisYearLeave.ToString().Trim();
                    else
                        tbCounter.Text = CounterValues[type].Value.ToString().Trim();

                    if (type == (int)Constants.EmplCounterTypes.AnnualLeaveCounter && ReservedALDays > 0)
                        tbCounter.Text += "(" + ReservedALDays.ToString().Trim() + ")";

                    if (tbCounter.ReadOnly)
                        tbCounter.CssClass = "counterTbReadOnly";
                    else
                        tbCounter.CssClass = "counterTb";
                    tbCounter.Attributes.Add(id.Trim(), type.ToString().Trim());                    
                    tbCounter.Attributes.Add(unit.Trim(), day.Trim());
                    tbCounter.AutoPostBack = true;
                    tbCounter.TextChanged += new EventHandler(tbCounter_TextChanged);
                    this.Controls.Add(tbCounter);

                    Label lblCounterDaySeparator = new Label();
                    lblCounterDaySeparator.ID = "lblCounterDaySeparator" + counterIndex.ToString().Trim();                    
                    lblCounterDaySeparator.Width = new Unit(separatorWidth);
                    lblCounterDaySeparator.CssClass = "counterSmallLblLeft";
                    lblCounterDaySeparator.Text = "d";
                    this.Controls.Add(lblCounterDaySeparator);
                }

                Label lblCounterSeparator = new Label();
                lblCounterSeparator.ID = "lblCounterSeparator" + counterIndex.ToString().Trim();                
                lblCounterSeparator.Width = new Unit(separatorWidth);
                lblCounterSeparator.CssClass = "counterSmallLblLeft";                
                this.Controls.Add(lblCounterSeparator);

                counterIndex++;
            }

            // insert separator
            if (InsertSeparator)
            {
                Label lblSeparator = new Label();
                lblSeparator.ID = "lblSeparator" + counterIndex.ToString().Trim();
                lblSeparator.Width = new Unit(LastSeparatorWidth);
                lblSeparator.CssClass = "counterSmallLblLeft";
                this.Controls.Add(lblSeparator);
            }
        }

        public event ControlEventHandler BubbleClick;

        protected void OnBubbleClick(ControlEventArgs e)
        {
            if (BubbleClick != null)
                BubbleClick(this, e);
        }

        protected void tbCounter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ControlEventArgs args = new ControlEventArgs();

                CultureInfo culture = CommonWeb.Misc.getCulture(Session[Constants.sessionLogInUser]);
                ResourceManager rm = CommonWeb.Misc.getResourceManager("ACTAWebUI.Resource", typeof(CounterUC).Assembly);

                // get counter data
                int counterType = -1;                
                int value = -1;                
                string counterUnit = "";

                if (((TextBox)sender).Attributes[id] != null && !int.TryParse(((TextBox)sender).Attributes[id].Trim(), out counterType))
                    counterType = -1;
                               
                if (((TextBox)sender).Attributes[unit] != null)
                    counterUnit = ((TextBox)sender).Attributes[unit].Trim();
                                
                if (!int.TryParse(((TextBox)sender).Text.Trim(), out value))
                    value = -1;

                if (counterType != -1 && !counterUnit.Trim().Equals(""))
                {
                    if (value < 0)
                        args.Error = rm.GetString("nonValidCounter", culture);
                    else if (((TextBox)sender).Attributes[unit] != null && ((TextBox)sender).Attributes[unit].Trim().ToUpper().Equals(minute.Trim().ToUpper()) && value > maxMinute)
                        args.Error = rm.GetString("maximalMinute", culture);
                    else
                    {
                        if (Session[Constants.sessionCounters] != null && Session[Constants.sessionCounters] is Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>)
                        {
                            Dictionary<int, Dictionary<int, EmployeeCounterValueTO>> emplCounters = (Dictionary<int, Dictionary<int, EmployeeCounterValueTO>>)Session[Constants.sessionCounters];

                            if (emplCounters.ContainsKey(EmplID) && emplCounters[EmplID].ContainsKey(counterType))
                            {                                
                                if (counterUnit.Trim().ToUpper().Equals(day.Trim().ToUpper()))
                                    emplCounters[EmplID][counterType].Value = value;
                                else if (counterUnit.Trim().ToUpper().Equals(hour.Trim().ToUpper()))
                                {
                                    int minutes = emplCounters[EmplID][counterType].Value % 60;
                                    emplCounters[EmplID][counterType].Value = value * 60 + minutes;
                                }
                                else if (counterUnit.Trim().ToUpper().Equals(minute.Trim().ToUpper()))
                                {
                                    int hours = emplCounters[EmplID][counterType].Value / 60;
                                    emplCounters[EmplID][counterType].Value = hours * 60 + value;
                                }

                                Session[Constants.sessionCounters] = emplCounters;
                            }
                        }
                    }
                }
                else
                    args.Error = rm.GetString("nonValidCounter", culture);

                OnBubbleClick(args);
            }
            catch (Exception ex)
            {
                try
                {
                    Response.Redirect("/ACTAWeb/CommonWeb/MessagesPages/ErrorPage.aspx?Error=Error in CounterUC.tbCounter_TextChanged(): " + ex.Message.Replace('\n', ' ').Trim() + "&Header=" + Constants.falseValue.Trim(), false);
                }
                catch (System.Threading.ThreadAbortException) { }
            }
        }
    }
}