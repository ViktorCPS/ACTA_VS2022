using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Globalization;
using System.IO;

using Common;
using TransferObjects;
using Util;

namespace Reports
{
	/// <summary>
	/// Summary description for WorkingUnitsReports.
	/// </summary>
	public class PaymentReports : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbWorkingUnit;
		private System.Windows.Forms.Label lblWorkingUnitName;
		private System.Windows.Forms.ComboBox cbWorkingUnit;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox gbTimeInterval;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.CheckBox chbHierarhicly;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		CultureInfo culture;
		ResourceManager rm;
		ApplUserTO logInUser;

		DebugLog debug;

		// list of pairs for report
		List<IOPairTO> ioPairList = new List<IOPairTO>();

		// list of Time Schema for selected Employee and selected Time Interval
        List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

		// list of Time Schedule for one month
        List<EmployeeTimeScheduleTO> timeSchedule = new List<EmployeeTimeScheduleTO>();

		// Key is Pass Type Id, Value is Pass Type
		Dictionary<int, PassTypeTO> passTypes = new Dictionary<int,PassTypeTO>();

		// all records for report
		ArrayList rowList = new ArrayList();

		// all Holidays, Key is Date, value is Holiday
		Dictionary<DateTime, HolidayTO> holidays = new Dictionary<DateTime,HolidayTO>();

		// all employee time schedules
        Dictionary<int, List<EmployeeTimeScheduleTO>> emplTimeSchedules = new Dictionary<int,List<EmployeeTimeScheduleTO>>();

		// all time shemas
		List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

		// Date of previous pair
		DateTime oldDate = new DateTime(0);
		// Date of currant pair
		DateTime currentDate = new DateTime(0);
		private System.Windows.Forms.Label lblCount;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblEmployee;
		private System.Windows.Forms.ProgressBar prbEmployee;

		// Counters processed employees 
        int empCount;
        private GroupBox gbFilter;
        private Button btnSaveCriteria;
        private ComboBox cbFilter;

		// Working Units that logInUser is granted to
		//ArrayList wUnits;
        private Button btnEmplStatuses;

        Dictionary<int, EmployeeTO> employeesDic = new Dictionary<int, EmployeeTO>();
        private GroupBox gbPaymentReport;
        private CheckBox chbEmplStatusReport;
        private TextBox tbReportError;

        Filter filter;

		public PaymentReports()
		{
			InitializeComponent();

			// Init debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			debug = new DebugLog(logFilePath);

			// Language tool
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("Reports.ReportResource", typeof(WorkingUnitsReports).Assembly);
			setLanguage();
			logInUser = NotificationController.GetLogInUser();
			populateWorkigUnitCombo();
			
			DateTime date = DateTime.Now.Date;
			this.CenterToScreen();

			dtpFromDate.Value = new DateTime(date.Year, date.Month, 1);
			dtpToDate.Value = date;

			// get all Pass Types
			List<PassTypeTO> passTypesAll = new PassType().Search();
			foreach (PassTypeTO pt in passTypesAll)
			{
				passTypes.Add(pt.PassTypeID, pt);
			}

			// get all holidays
			List<HolidayTO> holidayList = new Holiday().Search(new DateTime(), new DateTime());

			foreach (HolidayTO holiday in holidayList)
			{
				holidays.Add(holiday.HolidayDate, holiday);
			}

			// get all time schemas
			timeSchemas = new TimeSchema().Search();

            btnEmplStatuses.Enabled = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gbWorkingUnit = new System.Windows.Forms.GroupBox();
            this.chbHierarhicly = new System.Windows.Forms.CheckBox();
            this.cbWorkingUnit = new System.Windows.Forms.ComboBox();
            this.lblWorkingUnitName = new System.Windows.Forms.Label();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.prbEmployee = new System.Windows.Forms.ProgressBar();
            this.lblCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSaveCriteria = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.btnEmplStatuses = new System.Windows.Forms.Button();
            this.gbPaymentReport = new System.Windows.Forms.GroupBox();
            this.chbEmplStatusReport = new System.Windows.Forms.CheckBox();
            this.tbReportError = new System.Windows.Forms.TextBox();
            this.gbWorkingUnit.SuspendLayout();
            this.gbTimeInterval.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.gbPaymentReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWorkingUnit
            // 
            this.gbWorkingUnit.Controls.Add(this.chbHierarhicly);
            this.gbWorkingUnit.Controls.Add(this.cbWorkingUnit);
            this.gbWorkingUnit.Controls.Add(this.lblWorkingUnitName);
            this.gbWorkingUnit.Location = new System.Drawing.Point(22, 20);
            this.gbWorkingUnit.Name = "gbWorkingUnit";
            this.gbWorkingUnit.Size = new System.Drawing.Size(340, 103);
            this.gbWorkingUnit.TabIndex = 0;
            this.gbWorkingUnit.TabStop = false;
            this.gbWorkingUnit.Tag = "FILTERABLE";
            this.gbWorkingUnit.Text = "Working Units";
            // 
            // chbHierarhicly
            // 
            this.chbHierarhicly.Location = new System.Drawing.Point(80, 60);
            this.chbHierarhicly.Name = "chbHierarhicly";
            this.chbHierarhicly.Size = new System.Drawing.Size(104, 24);
            this.chbHierarhicly.TabIndex = 2;
            this.chbHierarhicly.Text = "Hierarchy ";
            // 
            // cbWorkingUnit
            // 
            this.cbWorkingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkingUnit.Location = new System.Drawing.Point(80, 24);
            this.cbWorkingUnit.Name = "cbWorkingUnit";
            this.cbWorkingUnit.Size = new System.Drawing.Size(248, 21);
            this.cbWorkingUnit.TabIndex = 1;
            // 
            // lblWorkingUnitName
            // 
            this.lblWorkingUnitName.Location = new System.Drawing.Point(16, 24);
            this.lblWorkingUnitName.Name = "lblWorkingUnitName";
            this.lblWorkingUnitName.Size = new System.Drawing.Size(48, 23);
            this.lblWorkingUnitName.TabIndex = 0;
            this.lblWorkingUnitName.Text = "Name:";
            this.lblWorkingUnitName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.dtpToDate);
            this.gbTimeInterval.Controls.Add(this.lblTo);
            this.gbTimeInterval.Controls.Add(this.dtpFromDate);
            this.gbTimeInterval.Controls.Add(this.lblFrom);
            this.gbTimeInterval.Location = new System.Drawing.Point(21, 131);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(483, 64);
            this.gbTimeInterval.TabIndex = 2;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Tag = "FILTERABLE";
            this.gbTimeInterval.Text = "Date Interval";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd.MM.yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(352, 24);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(126, 20);
            this.dtpToDate.TabIndex = 3;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(316, 24);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(24, 23);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd.MM.yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(80, 24);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(124, 20);
            this.dtpFromDate.TabIndex = 1;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(16, 24);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(40, 23);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(22, 274);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(136, 23);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(417, 365);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // prbEmployee
            // 
            this.prbEmployee.Location = new System.Drawing.Point(134, 235);
            this.prbEmployee.Name = "prbEmployee";
            this.prbEmployee.Size = new System.Drawing.Size(371, 16);
            this.prbEmployee.Step = 1;
            this.prbEmployee.TabIndex = 3;
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(31, 229);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(32, 16);
            this.lblCount.TabIndex = 15;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(71, 229);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 16);
            this.label2.TabIndex = 16;
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(80, 229);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(32, 16);
            this.lblEmployee.TabIndex = 17;
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSaveCriteria);
            this.gbFilter.Controls.Add(this.cbFilter);
            this.gbFilter.Location = new System.Drawing.Point(368, 20);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(137, 103);
            this.gbFilter.TabIndex = 1;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Work with filter";
            // 
            // btnSaveCriteria
            // 
            this.btnSaveCriteria.Location = new System.Drawing.Point(32, 56);
            this.btnSaveCriteria.Name = "btnSaveCriteria";
            this.btnSaveCriteria.Size = new System.Drawing.Size(82, 23);
            this.btnSaveCriteria.TabIndex = 1;
            this.btnSaveCriteria.Text = "Save criteria";
            this.btnSaveCriteria.UseVisualStyleBackColor = true;
            this.btnSaveCriteria.Click += new System.EventHandler(this.btnSaveCriteria_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.Location = new System.Drawing.Point(6, 24);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(126, 21);
            this.cbFilter.TabIndex = 0;
            this.cbFilter.Tag = "NOTFILTERABLE";
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // btnEmplStatuses
            // 
            this.btnEmplStatuses.Location = new System.Drawing.Point(230, 365);
            this.btnEmplStatuses.Name = "btnEmplStatuses";
            this.btnEmplStatuses.Size = new System.Drawing.Size(136, 23);
            this.btnEmplStatuses.TabIndex = 2;
            this.btnEmplStatuses.Text = "Employees statuses";
            this.btnEmplStatuses.Click += new System.EventHandler(this.btnEmplStatuses_Click);
            // 
            // gbPaymentReport
            // 
            this.gbPaymentReport.Controls.Add(this.btnGenerate);
            this.gbPaymentReport.Controls.Add(this.gbWorkingUnit);
            this.gbPaymentReport.Controls.Add(this.gbFilter);
            this.gbPaymentReport.Controls.Add(this.gbTimeInterval);
            this.gbPaymentReport.Controls.Add(this.prbEmployee);
            this.gbPaymentReport.Location = new System.Drawing.Point(16, 14);
            this.gbPaymentReport.Name = "gbPaymentReport";
            this.gbPaymentReport.Size = new System.Drawing.Size(521, 315);
            this.gbPaymentReport.TabIndex = 0;
            this.gbPaymentReport.TabStop = false;
            this.gbPaymentReport.Text = "Payment report";
            // 
            // chbEmplStatusReport
            // 
            this.chbEmplStatusReport.AutoSize = true;
            this.chbEmplStatusReport.Location = new System.Drawing.Point(37, 369);
            this.chbEmplStatusReport.Name = "chbEmplStatusReport";
            this.chbEmplStatusReport.Size = new System.Drawing.Size(133, 17);
            this.chbEmplStatusReport.TabIndex = 1;
            this.chbEmplStatusReport.Text = "Employee status report";
            this.chbEmplStatusReport.UseVisualStyleBackColor = true;
            this.chbEmplStatusReport.CheckedChanged += new System.EventHandler(this.chbEmplStatusReport_CheckedChanged);
            // 
            // tbReportError
            // 
            this.tbReportError.BackColor = System.Drawing.SystemColors.Info;
            this.tbReportError.Location = new System.Drawing.Point(12, 403);
            this.tbReportError.Multiline = true;
            this.tbReportError.Name = "tbReportError";
            this.tbReportError.ReadOnly = true;
            this.tbReportError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbReportError.Size = new System.Drawing.Size(525, 133);
            this.tbReportError.TabIndex = 18;
            // 
            // PaymentReports
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(555, 558);
            this.ControlBox = false;
            this.Controls.Add(this.tbReportError);
            this.Controls.Add(this.chbEmplStatusReport);
            this.Controls.Add(this.gbPaymentReport);
            this.Controls.Add(this.btnEmplStatuses);
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaymentReports";
            this.ShowInTaskbar = false;
            this.Text = "PaymentReports";
            this.Load += new System.EventHandler(this.PaymentReports_Load);
            this.gbWorkingUnit.ResumeLayout(false);
            this.gbTimeInterval.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbPaymentReport.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btnGenerateReport_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
                tbReportError.Text = "";
                rowList = new ArrayList();
				List<EmployeeTO> employees = new List<EmployeeTO>();
				List<int> employeesID = new List<int>();
				IOPair ioPair = new IOPair();
				int wuID = -1;
				string wUnits = "";
				List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();
                			
				if (this.dtpFromDate.Value <= this.dtpToDate.Value)
				{
					if (cbWorkingUnit.SelectedIndex > 0)
					{
						wuID = (int) cbWorkingUnit.SelectedValue;

						if (this.chbHierarhicly.Checked)
						{
							WorkingUnit wu = new WorkingUnit();
                            wu.WUTO.WorkingUnitID = wuID;
							
							wuArray = wu.Search();
							
							wuArray = wu.FindAllChildren(wuArray);
							wuID = -1;
						}

						foreach (WorkingUnitTO wu in wuArray)
						{
							wUnits += wu.WorkingUnitID.ToString().Trim() + ","; 
						}
					
						if (wUnits.Length > 0)
						{
							wUnits = wUnits.Substring(0, wUnits.Length - 1);
						}
						else
						{
							wUnits = wuID.ToString().Trim();
						}

						// find Employees for selected Working Unit
						List<EmployeeTO> allEmployees = new Employee().SearchByWU(wUnits.Trim());

                        foreach (EmployeeTO empl in allEmployees)
                        {
                            if (empl.Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper()) || empl.Status.Trim().ToUpper().Equals(Constants.statusBlocked.Trim().ToUpper()))
                                employees.Add(empl);
                        }
					}
					else
					{
						// find all Employees
						List<EmployeeTO> allEmployees = new Employee().Search();
                        foreach (EmployeeTO empl in allEmployees)
                        {
                            if (empl.Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper()) || empl.Status.Trim().ToUpper().Equals(Constants.statusBlocked.Trim().ToUpper()))
                                employees.Add(empl);
                        }
                        WorkingUnit wu = new WorkingUnit();
                        wu.WUTO.Status = Constants.DefaultStateActive;
                        List<WorkingUnitTO> wunit = wu.Search();
						
                        foreach (WorkingUnitTO wuTO in wunit)
						{
							wUnits += wuTO.WorkingUnitID.ToString().Trim() + ",";
						}
						if (wUnits.Length > 0)
						{
							wUnits = wUnits.Substring(0, wUnits.Length - 1);
						}
					}

					foreach (EmployeeTO empl in employees)
					{
						// Employee IDs
						employeesID.Add(empl.EmployeeID);

                        if (!employeesDic.ContainsKey(empl.EmployeeID))
                            employeesDic.Add(empl.EmployeeID, empl);
					}

					// get all valid IO Pairs for selected Working Unit and time interval
					// get iopairs for one day more, becouse if employee start night shift in last day,
					// hours of night shift from next are calculated together with hours from last day
					// io pairs are sorted by wu_name, empl_last_name, empl_first_name, io_pair_date ascending
					ioPairList = ioPair.SearchForWU(wuID, wUnits, dtpFromDate.Value, dtpToDate.Value.AddDays(1));

					// Key is Employee ID, value is ArrayList of valid IO Pairs for that Employee
					Dictionary<int, List<IOPairTO>> emplPairs = new Dictionary<int,List<IOPairTO>>();

					foreach (int emplID in employeesID)
					{
						emplPairs.Add(emplID, new List<IOPairTO>());
					}

					// io pairs for particular employee will be sorted by io_pair_date ascending
					for (int i = 0; i < ioPairList.Count; i++)
					{
                        try
                        {
                            emplPairs[ioPairList[i].EmployeeID].Add(ioPairList[i]);
                        }
                        catch (Exception ex)
                        {
                            debug.writeLog(DateTime.Now + " PaymentReports.btnGenerate_Click() populate emplPairs: emplID = " + ioPairList[i].EmployeeID + " Error: " + ex.Message + "\n" + ex.StackTrace);
                            string errorText = rm.GetString("paymentReportRetiredEmplError", culture) + " " + ioPairList[i].EmployeeID.ToString() + " ";
                            
                            EmployeeTO empl = new Employee().Find(ioPairList[i].EmployeeID.ToString());
                            
                            if (empl.EmployeeID != -1)
                            {
                                errorText += " " + empl.FirstName + " " + empl.LastName + " " + rm.GetString("wu", culture) + " " + empl.WorkingUnitID.ToString();
                            }
                            errorText += ". " + rm.GetString("retiredEmplError", culture);
                            errorText += Environment.NewLine;
                            tbReportError.Text += errorText;
                            tbReportError.Refresh();
                        }
					}

					// get all time schedules for all employees for the given period of time
					foreach (int emplID in employeesID)
					{
                        if (!emplTimeSchedules.ContainsKey(emplID))
                            emplTimeSchedules.Add(emplID, GetEmployeeTimeSchedules(emplID, dtpFromDate.Value, dtpToDate.Value.AddDays(1)));
					}

					this.label2.Text = "/";
					this.label2.Refresh();
                    prbEmployee.Value = 0;
					prbEmployee.Maximum = employeesID.Count;
					this.lblEmployee.Text = employeesID.Count.ToString();
					this.lblEmployee.Refresh();
					this.lblCount.Text = "0";
					this.lblCount.Refresh();

					int employeeIDindex = -1;
					foreach(int employeeID in employeesID)
					{
						employeeIDindex++;

						// add rows with employee ID, and hours for every payment code
						// generateData(employeeID, (ArrayList) emplPairs[employeeID], dtpFromDate.Value, dtpToDate.Value.AddDays(1));

						Hashtable paymentCodesHours = new Hashtable();
						for (DateTime day = dtpFromDate.Value; day <= dtpToDate.Value; day = day.AddDays(1))
						{
							bool isRegularSchema = true;
                            Dictionary<int, WorkTimeIntervalTO> edi = GetDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day, ref isRegularSchema);
							if (edi == null) continue;
                            Dictionary<int, WorkTimeIntervalTO> employeeDayIntervals = new Dictionary<int,WorkTimeIntervalTO>();
							IDictionaryEnumerator ediEnum = edi.GetEnumerator();
							while(ediEnum.MoveNext())
							{
                                employeeDayIntervals.Add((int)ediEnum.Key, ((WorkTimeIntervalTO)ediEnum.Value).Clone());
							}

							List<IOPairTO> edp = GetEmployeeDayPairs(emplPairs[employeeID],isRegularSchema,day);
							List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
							foreach(IOPairTO ioPairTO in edp)
							{
								employeeDayPairs.Add(new IOPairTO(ioPairTO));
							}

							Hashtable dayPaymentCodesHours = null;
							if (isRegularSchema)
							{
								dayPaymentCodesHours = CalculatePaymentPerRegularEmployeeDay(employeeID,employeeDayPairs,employeeDayIntervals,day);
							}
							else
							{
								dayPaymentCodesHours = CalculatePaymentPerBrigadeEmployeeDay(employeeID,employeeDayPairs,employeeDayIntervals,day);
							}

							IDictionaryEnumerator dayPaymentCodesHoursEnum = dayPaymentCodesHours.GetEnumerator();
							while(dayPaymentCodesHoursEnum.MoveNext())
							{
								if(!paymentCodesHours.ContainsKey(dayPaymentCodesHoursEnum.Key))
								{
									paymentCodesHours.Add(dayPaymentCodesHoursEnum.Key,dayPaymentCodesHoursEnum.Value);
								}
								else
								{
									paymentCodesHours[dayPaymentCodesHoursEnum.Key] = 
										((TimeSpan)paymentCodesHours[dayPaymentCodesHoursEnum.Key]).Add((TimeSpan)dayPaymentCodesHoursEnum.Value);
								}
							}
						}

						FillReportRowList(employeeID,employees[employeeIDindex], paymentCodesHours);

						empCount = empCount + 1;
						prbEmployee.Value++;
						this.lblCount.Text = empCount.ToString();
						this.lblCount.Refresh();
					}
				}
				else
				{
					MessageBox.Show (rm.GetString("wrongDatePickUp", culture));
					return;
				}
				this.label2.Text = "";
				// report
				//generatePDFReport();
				//generateCSVReport();
				generateTXTReport();
				
				//this.Close();
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " PaymentReports.btnGenerate_Click(): " + ex.Message + "\n" + ex.StackTrace);
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
			}
		}


		/// <summary>
		/// Gets all the employee's io pairs for the given working day
		/// </summary>
		/// <param name="emplPairs"></param>
		/// <param name="day"></param>
		/// <returns></returns>
		private List<IOPairTO> GetEmployeeDayPairs(List<IOPairTO> emplPairs, bool isRegularSchema, DateTime day)
		{
            List<IOPairTO> employeeDayPairs = new List<IOPairTO>();
			foreach(IOPairTO iopair in emplPairs)
			{
				if (iopair.IOPairDate == day)
				{
					employeeDayPairs.Add(iopair);
				}

				// pairs that belong to the tomorrow's part of the night shift (00:00-07:00)
				if (!isRegularSchema && (iopair.IOPairDate == day.AddDays(1) &&
					iopair.StartTime.TimeOfDay < new TimeSpan(7,0,0)))
				{
					employeeDayPairs.Add(iopair);
				}
			}
			return employeeDayPairs;
		}

		/// <summary>
		/// gets list of employee's time schedules for the given period of time
		/// </summary>
		/// <param name="employeeID"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
        private List<EmployeeTimeScheduleTO> GetEmployeeTimeSchedules(int employeeID, DateTime from, DateTime to)
		{
            List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

			DateTime date = from.Date;
			while ((date <= to.Date) || (date.Month == to.Month))
			{
                List<EmployeeTimeScheduleTO> timeSchedule = new EmployeesTimeSchedule().SearchMonthSchedule(employeeID, date);

				foreach (EmployeeTimeScheduleTO ets in timeSchedule)
				{
					timeScheduleList.Add(ets);
				}

				date = date.AddMonths(1);
			}

			return timeScheduleList;
		}

		/// <summary>
		/// gets employee's working intervals for the given day
		/// </summary>
		/// <param name="employeeTimeScheduleList"></param>
		/// <param name="day"></param>
		/// <returns></returns>
        private Dictionary<int, WorkTimeIntervalTO> GetDayTimeSchemaIntervals(List<EmployeeTimeScheduleTO> employeeTimeScheduleList, DateTime day, ref bool isRegularSchema)
		{
			// find actual time schedule for the day
			int timeScheduleIndex = -1;
			for (int scheduleIndex = 0; scheduleIndex < employeeTimeScheduleList.Count; scheduleIndex++)
			{
                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
				if (day >= employeeTimeScheduleList[scheduleIndex].Date)
					//&& (day.Month == ((EmployeesTimeSchedule) (employeeTimeScheduleList[scheduleIndex])).Date.Month))
				{
					timeScheduleIndex = scheduleIndex;
				}
			}
			if (timeScheduleIndex == -1) return null;

			EmployeeTimeScheduleTO employeeTimeSchedule = employeeTimeScheduleList[timeScheduleIndex];

			// find actual time schema for the day
			WorkTimeSchemaTO actualTimeSchema = null;
			foreach (WorkTimeSchemaTO timeSchema in timeSchemas)
			{
				if (timeSchema.TimeSchemaID == employeeTimeSchedule.TimeSchemaID)
				{
					actualTimeSchema = timeSchema;
					break;
				}
			}
			if (actualTimeSchema == null) return null;

            /* 2008-03-14
             * From now one, take the last existing time schedule, don't expect that every month has 
             * time schedule*/
			//int dayNum = (employeeTimeSchedule.StartCycleDay + day.Day - employeeTimeSchedule.Date.Day) % actualTimeSchema.CycleDuration;
            TimeSpan ts = new TimeSpan(day.Date.Ticks - employeeTimeSchedule.Date.Date.Ticks);
            int dayNum = (employeeTimeSchedule.StartCycleDay + (int)ts.TotalDays) % actualTimeSchema.CycleDuration;

            Dictionary<int, WorkTimeIntervalTO> intervals = actualTimeSchema.Days[dayNum];

			isRegularSchema = isRegularTimeSchema(actualTimeSchema);

			return intervals;
		}

		bool isHoliday(DateTime day)
		{
			return (holidays.ContainsKey(day));
		}

		bool isWeekend(DateTime day)
		{
			return ((day.DayOfWeek == DayOfWeek.Saturday) || (day.DayOfWeek == DayOfWeek.Sunday));
		}

		bool isRegularTimeSchema(WorkTimeSchemaTO schema)
		{
			// hard coded - no attribute - it'll be done according to the proper attribute in the table
			return ((schema.TimeSchemaID == 15) || (schema.TimeSchemaID == 16) ||(schema.TimeSchemaID == 17) ||
					(schema.TimeSchemaID == 18) || (schema.TimeSchemaID == 19) || (schema.TimeSchemaID == 21));
		}

        bool is8hoursShift(Dictionary<int, WorkTimeIntervalTO> intervals)
		{
			TimeSpan ts = new TimeSpan(0,0,0);
			for (int i = 0; i < intervals.Count; i++)
			{
				ts = ts.Add(intervals[i].EndTime - intervals[i].StartTime);
			}
			return (!(ts > new TimeSpan(8,0,0)));
		}

		bool isWorkingDay(Dictionary<int, WorkTimeIntervalTO> intervals)
		{
			TimeSpan ts = new TimeSpan(0,0,0);
			for (int i = 0; i < intervals.Count; i++)
			{
				ts = ts.Add(intervals[i].EndTime - intervals[i].StartTime);
			}
			return (ts > new TimeSpan(0,0,0));
		}

        bool isNightShiftDay(Dictionary<int, WorkTimeIntervalTO> dayIntervals)
		{
			// see if the day is a night shift day (contains intervals starting with 00:00 and/or finishing with 23:59)
			IDictionaryEnumerator dayIntervalsEnum = dayIntervals.GetEnumerator(); 
			while(dayIntervalsEnum.MoveNext())
			{
                WorkTimeIntervalTO dayInterval = (WorkTimeIntervalTO)dayIntervalsEnum.Value;
				if (((dayInterval.StartTime.TimeOfDay == new TimeSpan(0,0,0)) ||
					(dayInterval.EndTime.TimeOfDay == new TimeSpan(23,59,0))) &&
					(dayInterval.EndTime > dayInterval.StartTime))
				{
					return true;
				}
			}
			return false;
		}

		private void populateWorkigUnitCombo()
		{
			try
			{
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

				if (logInUser != null)
				{
					wuArray = new ApplUsersXWU().FindWUForUser(logInUser.UserID.Trim(), Constants.ReportPurpose);
                    //wUnits = (ArrayList)wuArray.Clone();
				}

				wuArray.Insert(0, new WorkingUnitTO(-1, 0, rm.GetString("all", culture), rm.GetString("all", culture), "", 0));

				cbWorkingUnit.DataSource = wuArray;
				cbWorkingUnit.DisplayMember = "Name";
				cbWorkingUnit.ValueMember = "WorkingUnitID";
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " PaymentReports.populateWorkigUnitCombo(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void setLanguage()
		{
			try
			{
				this.Text = rm.GetString("PaymentReports", culture);

				gbWorkingUnit.Text = rm.GetString("workingUnits", culture);
                gbPaymentReport.Text = rm.GetString("gbPaymentReport", culture);
                gbFilter.Text = rm.GetString("gbFilter", culture);
				lblWorkingUnitName.Text = rm.GetString("lblName", culture);
				chbHierarhicly.Text = rm.GetString("hierarchically", culture);
                chbEmplStatusReport.Text = rm.GetString("chbEmplStatusReport", culture);
				lblFrom.Text = rm.GetString("lblFrom", culture);
				gbTimeInterval.Text = rm.GetString("timeInterval", culture);
				lblTo.Text = rm.GetString("lblTo", culture);
				btnGenerate.Text = rm.GetString("btnGenerateReport", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
                btnEmplStatuses.Text = rm.GetString("btnEmplStatuses", culture);
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " PaymentReports.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void generateTXTReport()
		{
			try
			{
				//write data to TXT file
				string wu = "";
				if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
				{
					wu = rm.GetString("repAll", culture);
				}
				else
				{
					wu = cbWorkingUnit.Text.Trim();
				}

				string fileName = Constants.txtDocPath
					+ "Izvestaj za plate-" + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";

				FileStream stream = new FileStream(fileName, FileMode.Create);
				stream.Close();

				StreamWriter writer = File.AppendText(fileName);
				foreach (string row in rowList)
				{
					writer.WriteLine(row);
				}
				writer.Close();
				System.Diagnostics.Process.Start("notepad.exe",fileName);
			}
			catch (System.Threading.ThreadAbortException)
			{
			}
			catch(Exception ex)
			{
				debug.writeLog(DateTime.Now + " EmployeeAnaliticReport.generateTXTReport(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// calculate hours per each payment code for one regular time schema employee day
		/// </summary>
		/// <param name="employeeID"></param>
		/// <param name="ioPairs"></param>
		/// <param name="dayIntervals"></param>
		/// <param name="day"></param>
        private Hashtable CalculatePaymentPerRegularEmployeeDay(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
        {
            Hashtable paymentCodesHours = new Hashtable();

            // if weekend day do nothing
            if (isWeekend(day)) return paymentCodesHours;

            // Sanja 28.09.2011. if employee working unit is invalid or wait working unit, calculate hours they should work regarding to time schema
            if (employeesDic.ContainsKey(employeeID) && (employeesDic[employeeID].WorkingUnitID == Constants.invalidWU || employeesDic[employeeID].WorkingUnitID == Constants.waitWU))
            {
                string paymentCode = "";
                if (employeesDic[employeeID].WorkingUnitID == Constants.invalidWU)
                    paymentCode = passTypes[Constants.invalidPT].PaymentCode;
                else
                    paymentCode = passTypes[Constants.waitPT].PaymentCode;

                TimeSpan duration = new TimeSpan(0, 0, 0);

                foreach (int intNum in dayIntervals.Keys)
                {
                    duration = duration.Add(dayIntervals[intNum].EndTime.TimeOfDay - dayIntervals[intNum].StartTime.TimeOfDay);
                }

                if (duration.Hours == 8)
                    duration = duration.Add(new TimeSpan(0, -30, 0));
                else if (duration.Hours == 12)
                    duration = duration.Add(new TimeSpan(0, -45, 0));

                if (paymentCodesHours.ContainsKey(paymentCode))
                    ((TimeSpan)paymentCodesHours[paymentCode]).Add(duration);
                else
                    paymentCodesHours.Add(paymentCode, duration);

                return paymentCodesHours;
            }

            // if holiday day add 7.5 hours to the payment code 0550 (praznik) and close the day
            if (isHoliday(day))
            {
                paymentCodesHours.Add(passTypes[Constants.holiday].PaymentCode, new TimeSpan(7, 30, 0));
                return paymentCodesHours;
            }

            // if whole day absences found add 7.5 hours to its payment code (various) and close the day
            foreach (IOPairTO iopair in ioPairs)
            {
                PassTypeTO passType = passTypes[iopair.PassTypeID];
                if (passType.IsPass == Constants.wholeDayAbsence)
                {
                    paymentCodesHours.Add(passType.PaymentCode, new TimeSpan(7, 30, 0));
                    return paymentCodesHours;
                }
            }

            // remove unneeded pairs and trim first and last pair to the working time interval boundaries
            // considering latency rules
            CleanUpDayPairs(ioPairs, dayIntervals, day);

            // if no valid pairs set latency for the whole day (payment code 4010) and close the day
            if (ioPairs.Count <= 0)
            {
                paymentCodesHours.Add(passTypes[Constants.late].PaymentCode, new TimeSpan(7, 30, 0));
                return paymentCodesHours;
            }

            // sum duration of all pairs (they're all with pass type 1 - prolasci na citacu)
            TimeSpan totalDuration = new TimeSpan(0, 0, 0);
            foreach (IOPairTO iopair in ioPairs)
            {
                totalDuration = totalDuration.Add(iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
            }

            // calculate latency as difference between expected hours and actual duration (already
            // calculated considering start and end latencies, round it up to full hour and set
            // latency (payment code 4010)
            TimeSpan totalLatency = new TimeSpan(8, 0, 0) - totalDuration;
            if (totalLatency.Minutes != 0)
            {
                totalLatency = totalLatency.Add(new TimeSpan(1, -totalLatency.Minutes, -totalLatency.Seconds));
            }

            // calculate sum of ordinary private exits (bez preraspodele), round it up to full hour
            // and set value for the payment code 4000
            TimeSpan totalPrivateExits = new TimeSpan(0, 0, 0);
            foreach (IOPairTO iopair in ioPairs)
            {
                if (iopair.PassTypeID == Constants.privateOut)
                {
                    totalPrivateExits = totalPrivateExits.Add(iopair.EndTime.TimeOfDay - iopair.StartTime.TimeOfDay);
                }
            }
            if (totalPrivateExits.Minutes != 0)
            {
                totalPrivateExits = totalPrivateExits.Add(new TimeSpan(1, -totalPrivateExits.Minutes, -totalPrivateExits.Seconds));
            }

            // calculate payed hours (payment code 0031) considering break time, and meal tickets (payment code 3000)
            TimeSpan totalPayedHours = new TimeSpan(8, 0, 0) - totalLatency - totalPrivateExits;

            if (totalPayedHours > new TimeSpan(4, 0, 0))	// add a meal ticket and subtract break time from working hours
            {
                paymentCodesHours.Add(passTypes[Constants.tickets].PaymentCode, new TimeSpan(1, 0, 0));
                totalPayedHours = totalPayedHours.Add(new TimeSpan(0, -30, 0));
            }
            else	// subtract break time from greater of total latency time and total privat exits time
            {
                if ((totalLatency > totalPrivateExits) && (totalLatency >= new TimeSpan(0, -30, 0)))
                {
                    totalLatency = totalLatency.Add(new TimeSpan(0, -30, 0));
                }
                else if (totalPrivateExits >= new TimeSpan(0, -30, 0))
                {
                    totalPrivateExits = totalPrivateExits.Add(new TimeSpan(0, -30, 0));
                }
            }

            if (totalPayedHours.Hours > 0)
            {
                paymentCodesHours.Add(passTypes[Constants.regularWork].PaymentCode, totalPayedHours);
            }

            if (totalLatency.Hours > 0)
            {
                paymentCodesHours.Add(passTypes[Constants.late].PaymentCode, totalLatency);
            }

            if (totalPrivateExits.Hours > 0)
            {
                paymentCodesHours.Add(passTypes[Constants.privateOut].PaymentCode, totalPrivateExits);
            }

            // calculate sum of night hours (the ones after 22:00), round it down it to full hour
            // and set payment code 0400
            TimeSpan totalNightHours = new TimeSpan(0, 0, 0);
            foreach (IOPairTO iopair in ioPairs)
            {
                if ((iopair.PassTypeID != Constants.privateOut) && (iopair.EndTime.TimeOfDay > new TimeSpan(22, 0, 0)))
                {
                    TimeSpan startNightWork = new TimeSpan(22, 0, 0);
                    if (iopair.StartTime.TimeOfDay > startNightWork) startNightWork = iopair.StartTime.TimeOfDay;
                    totalNightHours = totalNightHours.Add(iopair.EndTime.TimeOfDay - startNightWork);
                }
            }
            if (totalNightHours.Minutes != 0)
            {
                totalNightHours = totalNightHours.Add(new TimeSpan(0, -totalNightHours.Minutes, -totalNightHours.Seconds));
            }
            if (totalNightHours.Hours > 0)
            {
                paymentCodesHours.Add(passTypes[Constants.nightWork].PaymentCode, totalNightHours);
            }

            return paymentCodesHours;
        }

        private Hashtable CalculatePaymentPerBrigadeEmployeeDay(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
		{
			Hashtable paymentCodesHours = new Hashtable();

            // Sanja 28.09.2011. if employee working unit is invalid or wait working unit, calculate hours they should work regarding to time schema
            if (employeesDic.ContainsKey(employeeID) && (employeesDic[employeeID].WorkingUnitID == Constants.invalidWU || employeesDic[employeeID].WorkingUnitID == Constants.waitWU))
            {
                string paymentCode = "";
                if (employeesDic[employeeID].WorkingUnitID == Constants.invalidWU)
                    paymentCode = (passTypes[Constants.invalidPT]).PaymentCode;
                else
                    paymentCode = (passTypes[Constants.waitPT]).PaymentCode;

                if (isNightShiftDay(dayIntervals))
                {
                    // night shift day can have 1 or 2 intervals
                    if (dayIntervals.Count == 1)
                    {
                        // if night shift day has only 00:00- interval it's already calculated as yesterday
                        // and should be dropped
                        if (dayIntervals[0].StartTime.TimeOfDay == new TimeSpan(0, 0, 0))
                        {
                            dayIntervals.Clear();
                        }
                        // if night shift day has only -23:59 interval make it normal night shift day adding
                        // tomorrow's first interval
                        else
                        {                            
                            bool dummy = true;
                            dayIntervals.Add(1, dayIntervals[0]);
                            Dictionary<int, WorkTimeIntervalTO> tomorrowIntervals = GetDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day.AddDays(1), ref dummy);
                            if (tomorrowIntervals != null)
                            {
                                dayIntervals[0] = tomorrowIntervals[0];
                            }
                            else
                            {
                                dayIntervals[0] = new WorkTimeIntervalTO();
                            }
                        }
                    }
                }
                
                TimeSpan duration = new TimeSpan(0, 0, 0);

                foreach (int intNum in dayIntervals.Keys)
                {
                    duration = duration.Add(dayIntervals[intNum].EndTime.TimeOfDay - dayIntervals[intNum].StartTime.TimeOfDay);
                }

                // add missing minute to raound to whole hour
                if (duration.Minutes == 59)
                    duration = duration.Add(new TimeSpan(0, 1, 0));

                if (duration.Hours == 8)
                    duration = duration.Add(new TimeSpan(0, -30, 0));
                else if (duration.Hours == 12)
                    duration = duration.Add(new TimeSpan(0, -45, 0));

                if (paymentCodesHours.ContainsKey(paymentCode))
                    paymentCodesHours[paymentCode] = ((TimeSpan)paymentCodesHours[paymentCode]).Add(duration);
                else
                    paymentCodesHours.Add(paymentCode, duration);

                return paymentCodesHours;
            }

			// remove unneeded pairs and trim first and last pair to the working time interval boundaries
			// considering latency rules, differently for regular and night shift days
			if(!isNightShiftDay(dayIntervals))
			{
				CleanUpDayPairs(ioPairs,dayIntervals,day);
			}
			else
			{
				CleanUpNightPairs(employeeID,ioPairs,dayIntervals,day);
			}

			// if not working day do nothing
			if (!isWorkingDay(dayIntervals)) return paymentCodesHours;

			TimeSpan expectedHours = is8hoursShift(dayIntervals) ? new TimeSpan(8,0,0) : new TimeSpan(12,0,0);
			TimeSpan expectedBreak = (expectedHours == new TimeSpan(8,0,0)) ? new TimeSpan(0,30,0) : new TimeSpan(0,45,0);
			TimeSpan expectedMealTickets = (expectedHours == new TimeSpan(8,0,0)) ? new TimeSpan(1,0,0) : new TimeSpan(1,30,0);

			// if no valid pairs set latency for the whole day (payment code 4010) and close the day
			if (ioPairs.Count <= 0)
			{
				paymentCodesHours.Add(passTypes[Constants.late].PaymentCode,expectedHours.Subtract(expectedBreak));
				return paymentCodesHours;
			}

			// if whole day absences found add 7.5 hours or 11.25 h to its payment code (various) and close the day
			foreach(IOPairTO iopair in ioPairs)
			{
				PassTypeTO passType = passTypes[iopair.PassTypeID];
				if (passType.IsPass == Constants.wholeDayAbsence)
				{
					paymentCodesHours.Add(passType.PaymentCode,expectedHours.Subtract(expectedBreak));
					return paymentCodesHours;
				}
			}

			// sum duration of all pairs (they're all with pass type 1 - prolasci na citacu)
			TimeSpan totalDuration = new TimeSpan(0,0,0);
			foreach(IOPairTO iopair in ioPairs)
			{
				totalDuration = totalDuration.Add(iopair.EndTime.TimeOfDay-iopair.StartTime.TimeOfDay);
				if ((iopair.EndTime.TimeOfDay.Hours == 23) && (iopair.EndTime.TimeOfDay.Minutes == 59)) totalDuration = totalDuration.Add(new TimeSpan(0,1,0));			}

			// calculate latency as difference between expected hours and actual duration (already
			// calculated considering start and end latencies, round it up to full hour and set
			// latency (payment code 4010)
			TimeSpan totalLatency = expectedHours - totalDuration;
			if (totalLatency.Minutes != 0)
			{
				totalLatency = totalLatency.Add(new TimeSpan(1,-totalLatency.Minutes,-totalLatency.Seconds));
			}

			// calculate sum of ordinary private exits (4000 - bez preraspodele) and round it up to full hour
			TimeSpan totalPrivateExits = new TimeSpan(0,0,0);
			foreach(IOPairTO iopair in ioPairs)
			{
				if (iopair.PassTypeID == Constants.privateOut)
				{
					totalPrivateExits = totalPrivateExits.Add(iopair.EndTime.TimeOfDay-iopair.StartTime.TimeOfDay);
					if ((iopair.EndTime.TimeOfDay.Hours == 23) && (iopair.EndTime.TimeOfDay.Minutes == 59)) totalPrivateExits = totalPrivateExits.Add(new TimeSpan(0,1,0));
				}
			}		
			if (totalPrivateExits.Minutes != 0)
			{
				totalPrivateExits = totalPrivateExits.Add(new TimeSpan(1,-totalPrivateExits.Minutes,-totalPrivateExits.Seconds));
			}

			// calculate sum of night hours (0560 - the ones after 22:00), round it down it to full hour
			TimeSpan totalNightHours = new TimeSpan(0,0,0);
			foreach(IOPairTO iopair in ioPairs)
			{
				if ((iopair.PassTypeID != Constants.privateOut) && (iopair.EndTime.TimeOfDay > new TimeSpan(22,0,0)))
				{
					TimeSpan startNightWork = new TimeSpan(22,0,0);
					if (iopair.StartTime.TimeOfDay > startNightWork) startNightWork = iopair.StartTime.TimeOfDay;
					totalNightHours = totalNightHours.Add(iopair.EndTime.TimeOfDay-startNightWork);
				}
				if ((iopair.PassTypeID != Constants.privateOut) && (iopair.StartTime.TimeOfDay < new TimeSpan(6,0,0)))
				{
					TimeSpan endNightWork = new TimeSpan(6,0,0);
					if (iopair.EndTime.TimeOfDay < endNightWork) endNightWork = iopair.EndTime.TimeOfDay;
					totalNightHours = totalNightHours.Add(endNightWork - iopair.StartTime.TimeOfDay);
				}
				if ((iopair.EndTime.TimeOfDay.Hours == 23) && (iopair.EndTime.TimeOfDay.Minutes == 59)) totalNightHours = totalNightHours.Add(new TimeSpan(0,1,0));
			}		
			if (totalNightHours.Minutes != 0)
			{
				totalNightHours = totalNightHours.Add(new TimeSpan(0,-totalNightHours.Minutes,-totalNightHours.Seconds));
			}

			// calculate payed hours (payment code 0031) considering break time, and meal tickets (payment code 3000)
			TimeSpan totalPayedHours = expectedHours - totalLatency - totalPrivateExits;

			if (totalPayedHours > new TimeSpan(expectedHours.Hours/2,0,0))	// add a meal ticket and subtract break time from working hours
			{
				paymentCodesHours.Add(passTypes[Constants.tickets].PaymentCode,expectedMealTickets);
				totalPayedHours = totalPayedHours.Subtract(expectedBreak);
				//if (totalNightHours > totalPayedHours-totalNightHours) totalNightHours = totalNightHours.Subtract(expectedBreak);

                // if working time is 12h, break time is splitted, 15min is calculated in day hours and 30min are calculated in night hours
                // if working time is 8h, break time is 30min and is calculated in night hours
                if (totalNightHours > totalPayedHours - totalNightHours) totalNightHours = totalNightHours.Subtract(new TimeSpan(0, 30, 0));
			}
			else	// subtract break time from greater of total latency time and total private exits time
			{
				if ((totalLatency > totalPrivateExits) && (totalLatency >= expectedBreak))
				{
					totalLatency = totalLatency.Subtract(expectedBreak);
				}
				else if (totalPrivateExits >= expectedBreak)
				{
					totalPrivateExits = totalPrivateExits.Subtract(expectedBreak);
				}
			}

			// set value for the payment code 0031 and 0560 (regular work and work on holiday)
			if (totalPayedHours.Hours > 0) 
			{
				paymentCodesHours.Add(passTypes[Constants.regularWork].PaymentCode,totalPayedHours);
				if (isHoliday(day))
				{
					paymentCodesHours.Add(passTypes[Constants.workOnHoliday].PaymentCode,totalPayedHours);
				}
			}

			// set value for the payment code 4010 (latency)
			if (totalLatency.Hours > 0) 
			{
				paymentCodesHours.Add(passTypes[Constants.late].PaymentCode,totalLatency);
			}

			// set value for the payment code 0400 (night work)
			if (totalNightHours.Hours > 0) 
			{
				paymentCodesHours.Add(passTypes[Constants.nightWork].PaymentCode,totalNightHours);
			}

			// set value for the payment code 4000 (private exits)
			if (totalPrivateExits.Hours > 0) 
			{
				paymentCodesHours.Add(passTypes[Constants.privateOut].PaymentCode,totalPrivateExits);
			}

			return paymentCodesHours;
		}

		/// <summary>
		/// removes unneeded pairs and trim first and last pair to the working time interval boundaries
		/// considering latency rules
		/// </summary>
		/// <param name="ioPairs"></param>
		/// <param name="dayIntervals"></param>
        void CleanUpDayPairs(List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
		{
			// remove invalid pairs and pairs outside of the interval
			IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
			while(ioPairsEnum.MoveNext())
			{
				IOPairTO iopair = (IOPairTO) ioPairsEnum.Current;
				if((iopair.IOPairDate != day) || (iopair.StartTime > iopair.EndTime) || 
					(iopair.StartTime.TimeOfDay > dayIntervals[0].EndTime.TimeOfDay) ||
					(iopair.EndTime.TimeOfDay   < dayIntervals[0].StartTime.TimeOfDay))
				{
					ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
					ioPairsEnum = ioPairs.GetEnumerator();
				}
			}
			if (ioPairs.Count <= 0) return;

			// find first and last pair
			IOPairTO firstPair = (IOPairTO)ioPairs[0];
			IOPairTO lastPair = firstPair;
			foreach(IOPairTO iopair in ioPairs)
			{
				if (iopair.StartTime < firstPair.StartTime) firstPair = iopair;
				if (iopair.EndTime > lastPair.EndTime) lastPair = iopair;
			}
			
			// round up first pair start time to full hour, considering start tolerance
			if (firstPair.StartTime.TimeOfDay <= dayIntervals[0].LatestArrivaed.TimeOfDay)
			{
				firstPair.StartTime = firstPair.StartTime.Add(firstPair.StartTime.TimeOfDay.Negate() + dayIntervals[0].StartTime.TimeOfDay);
			}
			else
			{
				if (firstPair.StartTime.Minute != 0)
				{
					firstPair.StartTime = firstPair.StartTime.Add(new TimeSpan(1,-firstPair.StartTime.Minute,-firstPair.StartTime.Second));
				}
			}
			if (firstPair.StartTime > firstPair.EndTime) firstPair.EndTime = firstPair.StartTime;

			// round down last pair end time to full hour, considering end tolerance
			if (lastPair.EndTime.TimeOfDay >= dayIntervals[0].EarliestLeft.TimeOfDay)
			{
				lastPair.EndTime = lastPair.EndTime.Add(lastPair.EndTime.TimeOfDay.Negate() + dayIntervals[0].EndTime.TimeOfDay);
			}
			else
			{
				if (lastPair.EndTime.Minute != 0)
				{
					lastPair.EndTime = lastPair.EndTime.Add(new TimeSpan(0,-lastPair.EndTime.Minute,-lastPair.EndTime.Second));
				}
			}
			if (lastPair.StartTime > lastPair.EndTime) lastPair.StartTime = lastPair.EndTime;
		}

		/// <summary>
		/// removes unneeded pairs belonging to the night shift and trim first and last pair
		/// to the working time interval boundaries considering latency rules
		/// </summary>
		/// <param name="ioPairs"></param>
		/// <param name="dayIntervals"></param>
        void CleanUpNightPairs(int employeeID, List<IOPairTO> ioPairs, Dictionary<int, WorkTimeIntervalTO> dayIntervals, DateTime day)
		{
			// night shift day can have 1 or 2 intervals
			if (dayIntervals.Count == 1)
			{
				// if night shift day has only 00:00- interval it's already calculated as yesterday
				// and should be dropped
				if (dayIntervals[0].StartTime.TimeOfDay == new TimeSpan(0,0,0))
				{
					ioPairs.Clear();
					dayIntervals.Clear();
					return;
				}
					// if night shift day has only -23:59 interval make it normal night shift day adding
					// tomorrow's first interval
				else
				{
					dayIntervals.Add(1, dayIntervals[0]);
					bool dummy = true;
                    Dictionary<int, WorkTimeIntervalTO> tomorrowIntervals = GetDayTimeSchemaIntervals(emplTimeSchedules[employeeID], day.AddDays(1), ref dummy);
					if (tomorrowIntervals != null)	dayIntervals[0] = tomorrowIntervals[0];
					else
					{
						debug.writeLog(DateTime.Now + " PaymentReports.CleanUpNightPairs(): Tomorrow intervals cannot be found! Employee: " + employeeID.ToString() + " Day: " + day.ToShortDateString() + "\n");
						return;
					}
				}
			}

			// remove invalid pairs and pairs outside of the interval
			IEnumerator ioPairsEnum = ioPairs.GetEnumerator();
			while(ioPairsEnum.MoveNext())
			{
				IOPairTO iopair = (IOPairTO) ioPairsEnum.Current;
				if (
					(iopair.StartTime > iopair.EndTime) ||
					(
					(iopair.StartTime.TimeOfDay > dayIntervals[0].EndTime.TimeOfDay)   &&
					(iopair.StartTime.TimeOfDay < dayIntervals[1].StartTime.TimeOfDay) &&
					(iopair.EndTime.TimeOfDay   > dayIntervals[0].EndTime.TimeOfDay)   &&
					(iopair.EndTime.TimeOfDay   < dayIntervals[1].StartTime.TimeOfDay)
					)
					)
				{
                    ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
					ioPairsEnum = ioPairs.GetEnumerator();
				}
			}
			if (ioPairs.Count <= 0) return;

			// remove morning pairs belonging to the previous day
			ioPairsEnum = ioPairs.GetEnumerator();
			while(ioPairsEnum.MoveNext())
			{
				IOPairTO iopair = (IOPairTO) ioPairsEnum.Current;
				if((iopair.IOPairDate == day) && (iopair.StartTime.TimeOfDay < dayIntervals[0].EndTime.TimeOfDay))
				{
                    ioPairs.Remove((IOPairTO)ioPairsEnum.Current);
					ioPairsEnum = ioPairs.GetEnumerator();
				}
			}
			if (ioPairs.Count <= 0) return;

			// find first and last pair
			IOPairTO firstPair = (IOPairTO)ioPairs[0];
			IOPairTO lastPair = firstPair;
			foreach(IOPairTO iopair in ioPairs)
			{
				if (iopair.StartTime < firstPair.StartTime) firstPair = iopair;
				if (iopair.EndTime > lastPair.EndTime) lastPair = iopair;
			}

			// find the proper interval for the first and last pair considering the day
			int firstPairInterval = (firstPair.IOPairDate == day) ? 1 : 0;
			int lastPairInterval  = (lastPair.IOPairDate  == day) ? 1 : 0;

			// round up first pair start time to full hour, considering start tolerance
			if (firstPair.StartTime.TimeOfDay <= dayIntervals[firstPairInterval].LatestArrivaed.TimeOfDay)
			{
				firstPair.StartTime = firstPair.StartTime.Add(firstPair.StartTime.TimeOfDay.Negate() + dayIntervals[firstPairInterval].StartTime.TimeOfDay);
			}
			else
			{
				if (firstPair.StartTime.Minute != 0)
				{
					firstPair.StartTime = firstPair.StartTime.Add(new TimeSpan(1,-firstPair.StartTime.Minute,-firstPair.StartTime.Second));
				}
			}
			if (firstPair.StartTime > firstPair.EndTime) firstPair.EndTime = firstPair.StartTime;	
		
			// round down last pair end time to full hour, considering end tolerance
			if (lastPair.EndTime.TimeOfDay >= dayIntervals[lastPairInterval].EarliestLeft.TimeOfDay)
			{
				lastPair.EndTime = lastPair.EndTime.Add(lastPair.EndTime.TimeOfDay.Negate() + dayIntervals[lastPairInterval].EndTime.TimeOfDay);
			}
			else
			{
				if (lastPair.EndTime.Minute != 0)
				{
					lastPair.EndTime = lastPair.EndTime.Add(new TimeSpan(0,-lastPair.EndTime.Minute,-lastPair.EndTime.Second));
				}
			}
			if (lastPair.StartTime > lastPair.EndTime) lastPair.StartTime = lastPair.EndTime;
		}

		void FillReportRowList(int employeeID, EmployeeTO emplData, Hashtable payTotal)
		{            
			// generate row of report
			TimeSpan time;
			string total = "";
			string wu = "";
			string wugroup = "";
			string lfname = "";
		
			foreach (string code in payTotal.Keys)
			{
				time = (TimeSpan) payTotal[code];

				// round up meal tickets for 12 hours shift
				if (code == passTypes[Constants.tickets].PaymentCode)
				{
					if (time.Minutes == 30)
					{
						time = time.Add(new TimeSpan(1,-30,0));
					}
				}

				total = ((int) (time.TotalHours * 100)).ToString();
				int decimalHour = Int32.Parse(total) % 100;
				int hour = Int32.Parse(total) / 100;
				if (decimalHour < 25)
				{
					decimalHour = 0;
				}
				else if (decimalHour < 50)
				{
					decimalHour = 25;
				}
				else if (decimalHour < 75)
				{
					decimalHour = 50;
				}
				else
				{
					decimalHour = 75;
				}
						
				total = (hour * 100 + decimalHour).ToString();
						
				wu = emplData.WorkingUnitID.ToString();
				wugroup = emplData.WorkingGroupID.ToString();
				lfname = emplData.LastName + " " + emplData.FirstName;

				string row = employeeID.ToString().PadLeft(6, '0');
				row += code.PadLeft(4, '0');
				row += total.PadLeft(8, '0');
				row += wu.PadLeft(4, '0');
				row += wugroup.PadLeft(2, '0');
				row += lfname.PadRight(35, ' ' );
				rowList.Add(row);
			}
		}

		private bool isRegularSchema (WorkTimeSchemaTO schema)
		{
			bool isRegular = true;

			try
			{
				if (schema == null)
				{
					return false;
				}

				// Time Schema is regular if cycle duration is 7 and interval duration of last two days are 0
				// is this condition enough to determine regular Time Schema?!!!
				if (schema.CycleDuration != 7)
				{
					isRegular = false;
				}
				else
				{
                    Dictionary<int, WorkTimeIntervalTO> intervals = schema.Days[5];
					TimeSpan hours = new TimeSpan();
					foreach (int intNum in intervals.Keys)
					{
						hours = hours.Add(intervals[intNum].EndTime.Subtract(intervals[intNum].StartTime));
					}

					if (hours.TotalSeconds > 0)
					{
						isRegular = false;
					}
					else
					{
						intervals = schema.Days[6];
						hours = new TimeSpan();
						foreach (int intNum in intervals.Keys)
						{
							hours = hours.Add(intervals[intNum].EndTime.Subtract(intervals[intNum].StartTime));
						}

						if (hours.TotalSeconds > 0)
						{
							isRegular = false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				debug.writeLog(DateTime.Now + " PaymentReports.isRegularSchema(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isRegular;
		}

        private void PaymentReports_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                filter = new Filter();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PaymentReports.PaymentReports_Load(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbFilter.SelectedIndex == 0)
                {
                    this.btnSaveCriteria.Text = rm.GetString("SaveFilter", culture);
                }
                else
                {
                    this.btnSaveCriteria.Text = rm.GetString("UpdateFilter", culture);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                UIFeatures.FilterAdd filterAdd = new UIFeatures.FilterAdd(this, filter);
                filterAdd.ShowDialog();
                filter.LoadFilters(cbFilter, this, rm.GetString("newFilter", culture));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.WaitCursor;
            }
        }

        private void btnEmplStatuses_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<string> employeesData = new List<string>();
                Dictionary<int, string> groupNames = new Dictionary<int, string>();

                List<WorkingGroupTO> groupList = new WorkingGroup().Search();
                foreach (WorkingGroupTO group in groupList)
                {
                    groupNames.Add(group.EmployeeGroupID, group.GroupName);
                }

                List<EmployeeTO> employees = new List<EmployeeTO>();
                int wuID = -1;
                string wUnits = "";
                List<WorkingUnitTO> wuArray = new List<WorkingUnitTO>();

                if (cbWorkingUnit.SelectedIndex > 0)
                {
                    wuID = (int)cbWorkingUnit.SelectedValue;

                    if (this.chbHierarhicly.Checked)
                    {
                        WorkingUnit wu = new WorkingUnit();
                        wu.WUTO.WorkingUnitID = wuID;

                        wuArray = wu.Search();

                        wuArray = wu.FindAllChildren(wuArray);
                        wuID = -1;
                    }

                    foreach (WorkingUnitTO wu in wuArray)
                    {
                        wUnits += wu.WorkingUnitID.ToString().Trim() + ",";
                    }

                    if (wUnits.Length > 0)
                    {
                        wUnits = wUnits.Substring(0, wUnits.Length - 1);
                    }
                    else
                    {
                        wUnits = wuID.ToString().Trim();
                    }

                    // find Employees for selected Working Unit
                    List<EmployeeTO> allEmployees = new Employee().SearchByWU(wUnits.Trim());

                    foreach (EmployeeTO empl in allEmployees)
                    {
                        if (empl.Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper()) || empl.Status.Trim().ToUpper().Equals(Constants.statusBlocked.Trim().ToUpper()))
                            employees.Add(empl);
                    }
                }
                else
                {
                    // find all Employees
                    List<EmployeeTO> allEmployees = new Employee().Search();
                    foreach (EmployeeTO empl in allEmployees)
                    {
                        if (empl.Status.Trim().ToUpper().Equals(Constants.statusActive.Trim().ToUpper()) || empl.Status.Trim().ToUpper().Equals(Constants.statusBlocked.Trim().ToUpper()))
                            employees.Add(empl);
                    }

                    WorkingUnit wu = new WorkingUnit();
                    wu.WUTO.Status = Constants.DefaultStateActive;
                    List<WorkingUnitTO> wunit = wu.Search();
                    
                    foreach (WorkingUnitTO wuTO in wunit)
                    {
                        wUnits += wuTO.WorkingUnitID.ToString().Trim() + ",";
                    }

                    if (wUnits.Length > 0)
                    {
                        wUnits = wUnits.Substring(0, wUnits.Length - 1);
                    }
                }

                foreach (EmployeeTO empl in employees)
                {
                    // data string format: employee_id, working_unit_id, working_unit_name, last_name, first_name, cycle_day, time_schema_id, time_schema_name, employee_group_id, group_name
                    string emplData = empl.EmployeeID.ToString() + "|" + empl.WorkingUnitID.ToString() + "|" + empl.WorkingUnitName.Trim() + "|"
                        + empl.LastName.Trim() + "|" + empl.FirstName.Trim() + "|";

                    // get time schema details
                    DateTime month = dtpFromDate.Value.Date;
                    List<EmployeeTimeScheduleTO> timeSchedule = new EmployeesTimeSchedule().SearchMonthSchedule(empl.EmployeeID, month);

                    if (timeSchedule.Count > 0)
                    {
                        //for (int scheduleIndex = 0; scheduleIndex < timeSchedule.Count; scheduleIndex++)
                        //{
                        int scheduleIndex = timeSchedule.Count - 1;
                        int cycleDuration = 0;
                        int day = timeSchedule[scheduleIndex].StartCycleDay + 1;
                        int schemaID = timeSchedule[scheduleIndex].TimeSchemaID;
                        TimeSchema sch = new TimeSchema();
                        sch.TimeSchemaTO.TimeSchemaID = schemaID;
                        List<WorkTimeSchemaTO> timeSchema = sch.Search();
                        WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                        if (timeSchema.Count > 0)
                        {
                            schema = timeSchema[0];
                            cycleDuration = schema.CycleDuration;
                        }

                        /* 2008-03-14
                         * From now one, take the last existing time schedule, don't expect that every month has 
                         * time schedule*/
                        int i = timeSchedule[scheduleIndex].Date.Day - 1;
                        if (
                            (timeSchedule[scheduleIndex].Date.Year < month.Year)
                            ||
                            (
                                (timeSchedule[scheduleIndex].Date.Year == month.Year)
                                && (timeSchedule[scheduleIndex].Date.Month < month.Month)
                            )
                           )
                        {
                            i = 0;

                            TimeSpan ts = new TimeSpan((new DateTime(month.Year, month.Month, 1)).Date.Ticks - timeSchedule[scheduleIndex].Date.Date.Ticks);
                            day = ((timeSchedule[scheduleIndex].StartCycleDay + (int)ts.TotalDays) % cycleDuration) + 1;
                        }

                        int cycleDay = ((day + month.Day - 1) % cycleDuration);
                        if (cycleDay == 0)
                            cycleDay = cycleDuration;
                        emplData += cycleDay.ToString() + "|" + schema.TimeSchemaID.ToString() + "|" + schema.Name.Trim() + "|";
                    }
                    else
                    {
                        emplData += " | | |";
                    }

                    emplData += empl.WorkingGroupID.ToString() + "|";

                    if (groupNames.ContainsKey(empl.WorkingGroupID))
                        emplData += groupNames[empl.WorkingGroupID].Trim();
                    else
                        emplData += " ";


                    employeesData.Add(emplData);
                }

                generateTXTEmplDataReport(employeesData);
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PaymentReports.btnEmplStatuses_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void generateTXTEmplDataReport(List<string> emplData)
        {
            try
            {
                //write data to TXT file
                string wu = "";
                if (cbWorkingUnit.Text.Trim().Equals(rm.GetString("all", culture)))
                {
                    wu = rm.GetString("repAll", culture);
                }
                else
                {
                    wu = cbWorkingUnit.Text.Trim();
                }

                string fileName = Constants.txtDocPath
                    + "Podaci o zaposlenima-" + wu + "-" + DateTime.Now.ToString("dd_MM_yyy HH_mm") + ".txt";

                FileStream stream = new FileStream(fileName, FileMode.Create);
                stream.Close();

                StreamWriter writer = File.AppendText(fileName);
                foreach (string row in emplData)
                {
                    writer.WriteLine(row);
                }
                writer.Close();
                System.Diagnostics.Process.Start("notepad.exe", fileName);
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PaymentReports.generateTXTEmplDataReport(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

        private void chbEmplStatusReport_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (chbEmplStatusReport.Checked)
                {
                    tbReportError.Text = "";
                    btnEmplStatuses.Enabled = true;
                    gbFilter.Enabled = false;
                    lblTo.Enabled = false;
                    dtpToDate.Enabled = false;
                    btnGenerate.Enabled = false;
                    tbReportError.Enabled = false;
                }
                else
                {
                    btnEmplStatuses.Enabled = false;
                    gbFilter.Enabled = true;
                    lblTo.Enabled = true;
                    dtpToDate.Enabled = true;
                    btnGenerate.Enabled = true;
                    tbReportError.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                debug.writeLog(DateTime.Now + " PaymentReports.chbEmplStatusReport_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}


