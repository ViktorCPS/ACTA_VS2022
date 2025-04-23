using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Common;
using TransferObjects;
using Util;

namespace Reports.Grundfos
{
    public partial class EmployeePYSumData : UserControl
    {
        private const int lblWidth = 66;
        private const int colWidth = 70;
        private const int colHeight = 40;
        private const int hdrHeight = 60;
        private const int colNum = 44;
        
        DebugLog log;
        ResourceManager rm;
        ApplUserTO logInUser;
        private CultureInfo culture;

        EmployeeTO empl = new EmployeeTO();
        Dictionary<string, EmployeePYDataSumTO> sumDict = new Dictionary<string, EmployeePYDataSumTO>();
        Dictionary<string, RuleTO> emplRules = new Dictionary<string, RuleTO>();
        int rbr = -1;
        List<int> paidLeaves = new List<int>();
        List<int> unpaidLeaves = new List<int>();
        
        bool initialiazing = false;

        public EmployeePYSumData(EmployeeTO empl, Dictionary<string, EmployeePYDataSumTO> sumDict, Dictionary<string, RuleTO> emplRules, List<int> paidLeaves, List<int> unpaidLeaves, 
            int rbr, int xPos, int yPos, bool header)
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);
                logInUser = NotificationController.GetLogInUser();
                rm = new ResourceManager("Reports.ReportResource", typeof(EmployeePYSumData).Assembly);
                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

                this.empl = empl;
                this.sumDict = sumDict;
                this.emplRules = emplRules;
                this.rbr = rbr;
                this.paidLeaves = paidLeaves;
                this.unpaidLeaves = unpaidLeaves;
                
                this.Location = new Point(xPos, yPos);
                this.Width = colWidth * colNum;

                if (header)
                {
                    this.Height = hdrHeight + 5;
                    InitializeHeader();
                }
                else
                {
                    this.Height = colHeight + 5;
                    InitializeControl();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitializeHeader()
        {
            try
            {
                int xLbl = colWidth - lblWidth;
                int xPos = xLbl / 2;
                int yPos = (this.Height - hdrHeight) / 2;
                int index = 0;

                // create header columns
                CreateHeaderColumn(rm.GetString("hdrGrRbr", culture), xPos, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrEmplID", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrFirstName", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrLastName", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrRegWork", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrRegWorkPercent", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrWorkOnHoliday", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrHoliday", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrNightWork", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrOvertime", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrAnnualLeave", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrPaidLeave", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrSickLeave30", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrSickLeaveWorkInjury", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrSickLeaveWorkInjuryNotUsed", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrSickLeaveOver30", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrUnpaid", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrPregnencyLeave", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrInvalid", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrTotal", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrOtherB", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrOtherN", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrOtherBN", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrMeal", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrMealB", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrMealN", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrMealO", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrRegresB", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrRegresN", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrCarsBN", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrAdditionalB", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrAdditionalN", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrAdditionalBN", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrTurnus", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrRemark", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrPYAdd", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrPYPercent", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrPoints", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrHolidayAddIIIPercent", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrHiladayAddIII", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrTurnusIIPercent", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrTurnusII", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrOvertimePercentIII", culture), xPos += colWidth, yPos, index++);
                CreateHeaderColumn(rm.GetString("hdrGrOvertimeIII", culture), xPos += colWidth, yPos, index++);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYSumData.InitializeHeader(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void CreateHeaderColumn(string text, int xPos, int yPos, int index)
        {
            try
            {
                this.Controls.Add(CreateLabel(text.Trim(), true, xPos, yPos, index));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYSumData.CreateHeaderColumn(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private void InitializeControl()
        {
            try
            {
                initialiazing = true;

                int xLbl = colWidth - lblWidth;
                int xPos = xLbl / 2;
                int yPos = (this.Height - colHeight) / 2;
                int index = 0;
                
                // create report labels
                // rbr
                this.Controls.Add(CreateLabel(rbr.ToString().Trim(), false, xPos, yPos, index++));
                // employee ID
                this.Controls.Add(CreateLabel(empl.EmployeeID.ToString().Trim(), false, xPos += colWidth, yPos, index++));
                // first name
                this.Controls.Add(CreateLabel(empl.FirstName.Trim(), false, xPos += colWidth, yPos, index++));
                // last name
                this.Controls.Add(CreateLabel(empl.LastName.Trim(), false, xPos += colWidth, yPos, index++));
                // regular work
                decimal rwHours = 0;
                if (emplRules.ContainsKey(Constants.RuleCompanyRegularWork) && sumDict.ContainsKey(emplRules[Constants.RuleCompanyRegularWork].RuleValue.ToString().Trim()))
                    rwHours = sumDict[emplRules[Constants.RuleCompanyRegularWork].RuleValue.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(rwHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));
                // regular work %
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // work on holiday
                decimal wHolHours = 0;
                if (emplRules.ContainsKey(Constants.RuleWorkOnHolidayPassType) && sumDict.ContainsKey(emplRules[Constants.RuleWorkOnHolidayPassType].RuleValue.ToString().Trim()))
                    wHolHours = sumDict[emplRules[Constants.RuleWorkOnHolidayPassType].RuleValue.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(wHolHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));
                // holiday
                decimal holHours = 0;
                if (emplRules.ContainsKey(Constants.RuleHolidayPassType) && sumDict.ContainsKey(emplRules[Constants.RuleHolidayPassType].RuleValue.ToString().Trim()))
                    holHours = sumDict[emplRules[Constants.RuleHolidayPassType].RuleValue.ToString().Trim()].HrsAmount;                
                this.Controls.Add(CreateLabel(holHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));
                // night work
                decimal nwHours = 0;
                if (emplRules.ContainsKey(Constants.RuleNightWork) && sumDict.ContainsKey(emplRules[Constants.RuleNightWork].RuleValue.ToString().Trim()))
                    nwHours = sumDict[emplRules[Constants.RuleNightWork].RuleValue.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(nwHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));
                // overtime
                decimal ovHours = 0;
                if (emplRules.ContainsKey(Constants.RuleCompanyOvertimePaid) && sumDict.ContainsKey(emplRules[Constants.RuleCompanyOvertimePaid].RuleValue.ToString().Trim()))
                    ovHours = sumDict[emplRules[Constants.RuleCompanyOvertimePaid].RuleValue.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(ovHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));
                // annual leave
                decimal alHours = 0;
                if (emplRules.ContainsKey(Constants.RuleCompanyAnnualLeave) && sumDict.ContainsKey(emplRules[Constants.RuleCompanyAnnualLeave].RuleValue.ToString().Trim()))
                    alHours = sumDict[emplRules[Constants.RuleCompanyAnnualLeave].RuleValue.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(alHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));
                // paid leave
                decimal plHours = 0;
                foreach (int plID in paidLeaves)
                {
                    if (sumDict.ContainsKey(plID.ToString().Trim()))
                        plHours += sumDict[plID.ToString().Trim()].HrsAmount;
                }
                this.Controls.Add(CreateLabel(plHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));
                // sick leave 30d
                decimal sl30Hours = 0;
                if (emplRules.ContainsKey(Constants.RuleCompanySickLeave30Days) && sumDict.ContainsKey(emplRules[Constants.RuleCompanySickLeave30Days].RuleValue.ToString().Trim()))
                    sl30Hours = sumDict[emplRules[Constants.RuleCompanySickLeave30Days].RuleValue.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(sl30Hours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));                
                // sick leave 30d work injury
                decimal slInjHours = 0;
                if (emplRules.ContainsKey(Constants.RuleCompanySickLeaveIndustrialInjury) && sumDict.ContainsKey(emplRules[Constants.RuleCompanySickLeaveIndustrialInjury].RuleValue.ToString().Trim()))
                    slInjHours = sumDict[emplRules[Constants.RuleCompanySickLeaveIndustrialInjury].RuleValue.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(slInjHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));                
                // sick leave over 30d work injury
                decimal slInjCHours = 0;
                if (emplRules.ContainsKey(Constants.RuleCompanySickLeaveIndustrialInjuryContinuation) && sumDict.ContainsKey(emplRules[Constants.RuleCompanySickLeaveIndustrialInjuryContinuation].RuleValue.ToString().Trim()))
                    slInjCHours = sumDict[emplRules[Constants.RuleCompanySickLeaveIndustrialInjuryContinuation].RuleValue.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(slInjCHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));                
                // sick leave over 30d
                decimal slCHours = 0;
                if (emplRules.ContainsKey(Constants.RuleCompanySickLeave30DaysContinuation) && sumDict.ContainsKey(emplRules[Constants.RuleCompanySickLeave30DaysContinuation].RuleValue.ToString().Trim()))
                    slCHours = sumDict[emplRules[Constants.RuleCompanySickLeave30DaysContinuation].RuleValue.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(slCHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));                
                // unpaid
                decimal unpHours = 0;                
                foreach (int uplID in unpaidLeaves)
                {
                    if (sumDict.ContainsKey(uplID.ToString().Trim()))
                        unpHours += sumDict[uplID.ToString().Trim()].HrsAmount;
                }
                this.Controls.Add(CreateLabel(unpHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));                
                // pregnency leave
                decimal prHours = 0;
                // *********** insert new rule for PREGNENCY LEAVE!!!
                this.Controls.Add(CreateLabel(prHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));                
                // invalids
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // total
                decimal total = 0;
                // ***** which hours shoud be in total?
                this.Controls.Add(CreateLabel(total.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));
                // other bruto
                decimal obHours = 0;
                if (sumDict.ContainsKey(Constants.GrundfosAddCodes.OB.ToString().Trim()))
                    obHours = sumDict[Constants.GrundfosAddCodes.OB.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateNumBox(obHours, xPos += colWidth, yPos, index++, Constants.GrundfosAddCodes.OB.ToString().Trim()));
                // other neto
                decimal onHours = 0;
                if (sumDict.ContainsKey(Constants.GrundfosAddCodes.ON.ToString().Trim()))
                    onHours = sumDict[Constants.GrundfosAddCodes.ON.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateNumBox(onHours, xPos += colWidth, yPos, index++, Constants.GrundfosAddCodes.ON.ToString().Trim()));
                // other BN
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // meal
                int meals = 0;
                if (sumDict.ContainsKey(Constants.GrundfosAddCodes.MEAL.ToString().Trim()))
                    meals = (int)sumDict[Constants.GrundfosAddCodes.MEAL.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(meals.ToString().Trim(), false, xPos += colWidth, yPos, index++));
                // meal bruto
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // meal neto
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // meal O
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // regres bruto
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // regres neto
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // cars BN
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // add bruto
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // add neto
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // add BN
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // turnus
                decimal turHours = 0;
                if (emplRules.ContainsKey(Constants.RuleComanyRotaryShift) && sumDict.ContainsKey(emplRules[Constants.RuleComanyRotaryShift].RuleValue.ToString().Trim()))
                    turHours = sumDict[emplRules[Constants.RuleComanyRotaryShift].RuleValue.ToString().Trim()].HrsAmount;
                this.Controls.Add(CreateLabel(turHours.ToString(Constants.doubleFormat), false, xPos += colWidth, yPos, index++));
                // remark
                this.Controls.Add(CreateTextBox("", xPos += colWidth, yPos, index++, Constants.GrundfosAddCodes.REM.ToString().Trim()));
                // add
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // add percent
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // points
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // percent work on holiday III
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // work on holiday III
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // percent turnus II
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // turnus II
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // percent overtime III
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));
                // overtime III
                this.Controls.Add(CreateLabel("", false, xPos += colWidth, yPos, index++));

                initialiazing = false;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYSumData.InitializeHeader(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private Label CreateLabel(string text, bool isHeader, int xPos, int yPos, int index)
        {
            try
            {
                Label lbl = new Label();
                lbl.Name = "lbl_" + empl.EmployeeID.ToString() + index.ToString().Trim();
                lbl.AutoSize = false;
                lbl.Width = lblWidth;
                lbl.Text = text.Trim();
                if (isHeader)
                {
                    lbl.Height = hdrHeight;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.BackColor = Color.Lavender;
                }
                else
                {
                    lbl.Height = colHeight;
                    lbl.TextAlign = ContentAlignment.MiddleLeft;
                    lbl.BackColor = Color.White;
                }
                lbl.Location = new Point(xPos, yPos);

                return lbl;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYSumData.CreateLabel(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private TextBox CreateTextBox(string text, int xPos, int yPos, int index, string code)
        {
            try
            {
                TextBox tb = new TextBox();
                tb.Name = "tb_" + empl.EmployeeID.ToString() + index.ToString().Trim();
                tb.Multiline = true;
                tb.Width = lblWidth;
                tb.Height = colHeight;
                tb.Text = text.Trim();
                tb.TextAlign = HorizontalAlignment.Left;
                tb.BackColor = SystemColors.Info;
                tb.Location = new Point(xPos, yPos);
                tb.Tag = code;
                tb.TextChanged += new EventHandler(tb_TextChanged);

                return tb;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYSumData.CreateTextBox(): " + ex.Message + "\n");
                throw ex;
            }
        }

        private NumericUpDown CreateNumBox(decimal value, int xPos, int yPos, int index, string code)
        {
            try
            {
                NumericUpDown num = new NumericUpDown();
                num.Name = "num_" + empl.EmployeeID.ToString() + index.ToString().Trim();
                num.Width = lblWidth;
                num.Height = colHeight;
                num.Minimum = 0;
                num.Maximum = 1000000000;
                num.DecimalPlaces = 2;
                num.Increment = (decimal)(0.01);
                if (value >= num.Minimum && value <= num.Maximum)
                    num.Value = value;
                num.TextAlign = HorizontalAlignment.Right;
                num.BackColor = SystemColors.Info;
                num.Location = new Point(xPos, yPos);
                num.Tag = code;
                num.ValueChanged += new EventHandler(num_ValueChanged);

                return num;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYSumData.CreateNumBox(): " + ex.Message + "\n");
                throw ex;
            }
        }

        public Dictionary<string, EmployeePYDataSumTO> GetDataValues()
        {
            return sumDict;
        }

        public EmployeeTO GetEmployee()
        {
            return empl;
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYSumData.tb_TextChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void num_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!initialiazing && sender is NumericUpDown && ((NumericUpDown)sender).Tag != null)
                {
                    if (sumDict.ContainsKey(((NumericUpDown)sender).Tag.ToString().Trim()))
                        sumDict[((NumericUpDown)sender).Tag.ToString().Trim()].HrsAmount = ((NumericUpDown)sender).Value;
                    else
                        sumDict.Add(((NumericUpDown)sender).Tag.ToString().Trim(), CreateSumRecord(((NumericUpDown)sender).Tag.ToString().Trim(), ((NumericUpDown)sender).Value));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYSumData.num_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private EmployeePYDataSumTO CreateSumRecord(string pyCode, decimal hours)
        {
            try
            {
                uint calcID = 0;
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();

                foreach (string code in sumDict.Keys)
                {
                    calcID = sumDict[code].PYCalcID;
                    startDate = sumDict[code].DateStart.Date;
                    endDate = sumDict[code].DateEnd.Date;
                    break;
                }

                EmployeePYDataSumTO sumTO = new EmployeePYDataSumTO();
                sumTO.Type = Constants.PYTypeReal;
                sumTO.PYCalcID = calcID;
                sumTO.DateStart = startDate.Date;
                sumTO.DateEnd = endDate.Date;
                sumTO.DateStartSickness = Constants.dateTimeNullValue();
                sumTO.EmployeeID = empl.EmployeeID;
                sumTO.PaymentCode = pyCode.Trim();
                sumTO.HrsAmount = hours;

                return sumTO;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmployeePYSumData.CreateSumRecord(): " + ex.Message + "\n");
                throw ex;
            }
        }
    }
}
