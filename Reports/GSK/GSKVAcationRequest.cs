using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Util;
using Common;
using TransferObjects;

namespace Reports.GSK
{
    public partial class GSKVAcationRequest : Form
    {
        private CultureInfo culture;
        private ResourceManager rm;
        DebugLog log;
        EmployeeTO employee;
        EmployeeVacEvidTO employeeVacation;
        // all Holidays, Key is Date, value is Holiday
        Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();
        // all time shemas
        List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();
        List<EmployeeAbsenceTO> absList;
        List<int> absIds = new List<int>();

        public GSKVAcationRequest(EmployeeVacEvidTO vac, EmployeeTO selEmpl, DateTime from, DateTime to, List<EmployeeAbsenceTO> absenceList)
        {
            InitializeComponent();
            this.CenterToScreen();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
            rm = new ResourceManager("Reports.ReportResource", typeof(GSKVAcationRequest).Assembly);
            setLanguage();
            employee = selEmpl;
            tbEmployee.Text = selEmpl.LastName;
            dateTimePicker1.Value = from;
            dateTimePicker2.Value = to;
            employeeVacation = vac;
            absList = absenceList;
            foreach (EmployeeAbsenceTO abs in absList)
            {
                if (!absIds.Contains(abs.RecID))
                    absIds.Add(abs.RecID);
            }
           // populateEmployeeCombo();
        }

        private void setLanguage()
        {
            try
            {
                // button's text
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnGenerateRequest.Text = rm.GetString("btnGenerateRequest", culture);

                // Form name
                this.Text = rm.GetString("GSKVacationRequest", culture);
              
                // label's text
                lblFrom.Text = rm.GetString("lblFrom", culture);
                lblTo.Text = rm.GetString("lblTo", culture);
                lblFor.Text = rm.GetString("lblVacFor", culture);
                lblReplacement.Text = rm.GetString("lblReplacement", culture);
                lblTelephone.Text = rm.GetString("lblTelephone", culture);
                lblEmployee.Text = rm.GetString("lblEmployee", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GSKVacationRequest.setLanguage(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerateRequest_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // get all holidays
                List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

                foreach (HolidayTO holiday in holidayList)
                {
                    holidays.Add(holiday.HolidayDate, holiday);
                }

                // get all time schemas
                timeSchemas = new TimeSchema().Search();
                //geting employeeAbsences with passType = vacation to calculate used vacations for each employee
                DateTime vacYear = new DateTime();
                DateTime nextYear = new DateTime();
                if (!employeeVacation.VacYear.Equals(new DateTime()))
                {
                    vacYear = employeeVacation.VacYear.AddYears(-1);
                    nextYear = employeeVacation.VacYear.AddYears(1);
                }
                employeeVacation.DaysLeft = employeeVacation.NumOfDays;
                employeeVacation.FromLastYear = 0;
                employeeVacation.UsedDays = 0;

                EmployeeAbsence ea = new EmployeeAbsence();
                ea.EmplAbsTO.PassTypeID = Constants.vacation;
                ea.EmplAbsTO.DateStart = vacYear;
                ea.EmplAbsTO.DateEnd = nextYear;
                List<EmployeeAbsenceTO> absencesList = ea.SearchForVacEvid(employeeVacation.EmployeeID.ToString(), "", new DateTime(), new DateTime());
                log.writeBenchmarkLog("GSKActivationRequest.btnGenerateRequest_Click() searched absences");
                Dictionary<DateTime, List<EmployeeAbsenceTO>> absenceTable = new Dictionary<DateTime,List<EmployeeAbsenceTO>>();
                foreach (EmployeeAbsenceTO absence in absencesList)
                {
                    if (!absIds.Contains(absence.RecID))
                    {
                        if (!absenceTable.ContainsKey(absence.VacationYear))
                        {
                            absenceTable.Add(absence.VacationYear, new List<EmployeeAbsenceTO>());
                        }
                        List<EmployeeAbsenceTO> absList = absenceTable[absence.VacationYear];
                        absList.Add(absence);
                        absenceTable[absence.VacationYear] = absList;
                    }
                }
                log.writeBenchmarkLog("GSKActivationRequest.btnGenerateRequest_Click() Hashtable successfully created no of members: "+absenceTable.Count);
                if (absenceTable.ContainsKey(employeeVacation.VacYear))
                {
                    //from year vacation plan and absences count real used days and total time to use
                    EmployeeVacEvidTO vacTemp = Common.Misc.getVacationWithAbsences(employeeVacation, absenceTable[employeeVacation.VacYear], holidays, timeSchemas);
                    employeeVacation.DaysLeft = vacTemp.DaysLeft;
                    employeeVacation.UsedDays = vacTemp.UsedDays;
                    log.writeBenchmarkLog("GSKActivationRequest.btnGenerateRequest_Click() Employee has vacation this year " );
                }
                //search last year vacations
                List<EmployeeVacEvidTO> vacLastYear = new EmployeeVacEvid().getVacations(employeeVacation.EmployeeID.ToString(), employeeVacation.VacYear.AddYears(-1), employeeVacation.VacYear.AddYears(-1), -1, -1);
                if (vacLastYear.Count > 0)
                {
                    EmployeeVacEvidTO vationLY = new EmployeeVacEvidTO();
                    List<EmployeeAbsenceTO> list = new List<EmployeeAbsenceTO>();
                    if(absenceTable.ContainsKey(employeeVacation.VacYear.AddYears(-1)))
                        list = absenceTable[employeeVacation.VacYear.AddYears(-1)];
                    vationLY = Common.Misc.getVacationWithAbsences(vacLastYear[0], list, holidays, timeSchemas);
                    TimeSpan ts = vationLY.ValidTo.Date.Subtract(DateTime.Now);
                    employeeVacation.FromLastYear = Math.Min(vationLY.DaysLeft, ts.Days);
                    log.writeBenchmarkLog("GSKActivationRequest.btnGenerateRequest_Click() Employee has vacation last year ");
                }

                //total for use is summ of days left from last year and days left from this year
                if (employeeVacation.DaysLeft >= 0 && employeeVacation.FromLastYear >= 0)
                {
                    employeeVacation.TotalForUse = employeeVacation.DaysLeft + employeeVacation.FromLastYear;
                }
                else
                    employeeVacation.TotalForUse = employeeVacation.DaysLeft;
                log.writeBenchmarkLog("GSKActivationRequest.btnGenerateRequest_Click() Total for use counted ");

                List<EmployeeTimeScheduleTO> EmplScheduleList = Common.Misc.GetEmployeeTimeSchedules(employee.EmployeeID, dateTimePicker1.Value.Date, dateTimePicker2.Value.Date);
                int requested = 0;
                foreach (EmployeeAbsenceTO abs in this.absList)
                {
                    requested += Common.Misc.getNumOfWorkingDays(EmplScheduleList, abs.DateStart, abs.DateEnd, holidays, timeSchemas);
                }
                int balance = employeeVacation.TotalForUse - requested;

                log.writeBenchmarkLog("GSKActivationRequest.btnGenerateRequest_Click() requested and balance counted ");

                // Table Definition for Crystal Reports
                DataSet dataSetCR = new DataSet();
                DataTable tableI = new DataTable("images");
                tableI.Columns.Add("imageID", typeof(byte));
                tableI.Columns.Add("image", typeof(System.Byte[]));

                //add logo image just once
                DataRow rowI = tableI.NewRow();
                rowI["image"] = Constants.LogoForReport;
                rowI["imageID"] = 1;
                tableI.Rows.Add(rowI);
                tableI.AcceptChanges();

                dataSetCR.Tables.Add(tableI);

                DateTime returnDate = dateTimePicker2.Value.Date;
                int wrkDay = 0;
                while (wrkDay <= 0)
                {
                    returnDate = returnDate.AddDays(1);
                    EmplScheduleList = Common.Misc.GetEmployeeTimeSchedules(employee.EmployeeID, returnDate, returnDate);
                    wrkDay = Common.Misc.getNumOfWorkingDays(EmplScheduleList, returnDate, returnDate, holidays, timeSchemas);
                }
                log.writeBenchmarkLog("GSKActivationRequest.btnGenerateRequest_Click() first working day counted ");

                GSK_sr.GSKVacationRequestCRView view = new Reports.GSK.GSK_sr.GSKVacationRequestCRView(dataSetCR, employee.LastName, cbEmployeeTo.Text.ToString(),
                    requested.ToString(), dateTimePicker1.Value.Date.ToString("dd.MM.yyyy"), dateTimePicker2.Value.ToString("dd.MM.yyyy"), cbReplacement.Text.ToString(),
                    tbTelephone.Text.ToString(), employeeVacation.FromLastYear.ToString(), employeeVacation.NumOfDays.ToString(), employeeVacation.UsedDays.ToString(), balance.ToString(), returnDate.ToString("dd.MM.yyyy"));
                view.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GSKVacationRequest.btnGenerateRequest_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        private void populateEmployeeCombo()
        {
            try
            {
                List<EmployeeTO> currentEmplArray = new List<EmployeeTO>();
                List<EmployeeTO> currentEmplArray1 = new List<EmployeeTO>();

                Employee empl = new Employee();
                empl.EmplTO.Status = Constants.statusActive;
                currentEmplArray = empl.Search();
                
                foreach (EmployeeTO employee in currentEmplArray)
                {
                    employee.LastName += " " + employee.FirstName;
                    currentEmplArray1.Add(employee);
                }
                
                EmployeeTO emplTO = new EmployeeTO();
                emplTO.LastName = rm.GetString("", culture);
                currentEmplArray.Insert(0, emplTO);

                EmployeeTO empl1 = new EmployeeTO();
                empl1.LastName = rm.GetString("", culture);
                currentEmplArray1.Insert(0, empl1);
                
                cbEmployeeTo.DataSource = currentEmplArray;
                cbEmployeeTo.DisplayMember = "LastName";
                //cbEmployeeTo.ValueMember = "EmployeeID";
                cbEmployeeTo.Invalidate();

                cbReplacement.DataSource = currentEmplArray1;
                cbReplacement.DisplayMember = "LastName";
                //cbReplacement.ValueMember = "EmployeeID";
                cbReplacement.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " VacationEvidence.populateEmployeeCombo(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
        private void GSKVAcationRequest_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                populateEmployeeCombo();
                if (employee.AddressID > 0)
                {
                    Common.Address adress = new Address();
                    ArrayList list = adress.Search(employee.AddressID.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                    if (list.Count > 0)
                    {
                        adress = (Address)list[0];
                        tbTelephone.Text = adress.TelNumber1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " GSKVacationRequest.GSKVAcationRequest_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}