using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for ExtraHoursCalculation.
	/// </summary>
	public class ExtraHoursCalculation : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox gbCalculateExtraHours;
		private System.Windows.Forms.Label lblWorkingUnit;
		private System.Windows.Forms.TextBox tbWorkingUnit;
		private System.Windows.Forms.Label lblEmployee;
		private System.Windows.Forms.TextBox tbEmployee;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.DateTimePicker dtpFrom;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.DateTimePicker dtpTo;
		private System.Windows.Forms.Button btnCalculate;
		private System.Windows.Forms.Label lblReaders;
		private System.Windows.Forms.ListView lvReaders;
		private System.Windows.Forms.Label lblLastReaderTime;
		private System.Windows.Forms.Button btnClose;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		ExtraHourTO currentExtraHour = null;
		EmployeeTO currentEmployee = null;

		ApplUserTO logInUser;
		ResourceManager rm;				
		private CultureInfo culture;
		DebugLog log;

		private ListViewItemComparer _comp;

		// List View indexes
		const int ReaderIDIndex = 0;
		const int DescIndex = 1;
		const int DateIndex = 2;

        DateTime allReadersDate = new DateTime();

		public ExtraHoursCalculation(int employeeID)
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				currentExtraHour = new ExtraHourTO();
				currentEmployee = new EmployeeTO();
                Employee empl = new Employee();
                empl.EmplTO.EmployeeID = employeeID;
				List<EmployeeTO> employeeList = empl.SearchWithStatuses(new List<string>(), "");
				if (employeeList.Count > 0)
				{
					currentEmployee = employeeList[0];
				}
				logInUser = NotificationController.GetLogInUser();

				this.CenterToScreen();
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				rm = new ResourceManager("UI.Resource",typeof(ExtraHoursCalculation).Assembly);
				setLanguage();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHoursCalculation.ExtraHoursCalculation(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
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
            this.lvReaders = new System.Windows.Forms.ListView();
            this.lblLastReaderTime = new System.Windows.Forms.Label();
            this.gbCalculateExtraHours = new System.Windows.Forms.GroupBox();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.tbEmployee = new System.Windows.Forms.TextBox();
            this.tbWorkingUnit = new System.Windows.Forms.TextBox();
            this.lblWorkingUnit = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblReaders = new System.Windows.Forms.Label();
            this.gbCalculateExtraHours.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvReaders
            // 
            this.lvReaders.BackColor = System.Drawing.SystemColors.Control;
            this.lvReaders.FullRowSelect = true;
            this.lvReaders.GridLines = true;
            this.lvReaders.HideSelection = false;
            this.lvReaders.Location = new System.Drawing.Point(16, 208);
            this.lvReaders.MultiSelect = false;
            this.lvReaders.Name = "lvReaders";
            this.lvReaders.Size = new System.Drawing.Size(584, 232);
            this.lvReaders.TabIndex = 11;
            this.lvReaders.UseCompatibleStateImageBehavior = false;
            this.lvReaders.View = System.Windows.Forms.View.Details;
            this.lvReaders.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvReaders_ColumnClick);
            // 
            // lblLastReaderTime
            // 
            this.lblLastReaderTime.ForeColor = System.Drawing.Color.Red;
            this.lblLastReaderTime.Location = new System.Drawing.Point(16, 456);
            this.lblLastReaderTime.Name = "lblLastReaderTime";
            this.lblLastReaderTime.Size = new System.Drawing.Size(384, 23);
            this.lblLastReaderTime.TabIndex = 12;
            this.lblLastReaderTime.Text = "Last reading time for all readers:";
            this.lblLastReaderTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLastReaderTime.Visible = false;
            // 
            // gbCalculateExtraHours
            // 
            this.gbCalculateExtraHours.Controls.Add(this.btnCalculate);
            this.gbCalculateExtraHours.Controls.Add(this.dtpFrom);
            this.gbCalculateExtraHours.Controls.Add(this.lblFrom);
            this.gbCalculateExtraHours.Controls.Add(this.dtpTo);
            this.gbCalculateExtraHours.Controls.Add(this.lblTo);
            this.gbCalculateExtraHours.Controls.Add(this.tbEmployee);
            this.gbCalculateExtraHours.Controls.Add(this.tbWorkingUnit);
            this.gbCalculateExtraHours.Controls.Add(this.lblWorkingUnit);
            this.gbCalculateExtraHours.Controls.Add(this.lblEmployee);
            this.gbCalculateExtraHours.Location = new System.Drawing.Point(16, 8);
            this.gbCalculateExtraHours.Name = "gbCalculateExtraHours";
            this.gbCalculateExtraHours.Size = new System.Drawing.Size(584, 152);
            this.gbCalculateExtraHours.TabIndex = 0;
            this.gbCalculateExtraHours.TabStop = false;
            this.gbCalculateExtraHours.Text = "Calculate extra hours";
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(496, 120);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 23);
            this.btnCalculate.TabIndex = 9;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd.MM.yyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(136, 88);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(100, 20);
            this.dtpFrom.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(16, 88);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(112, 23);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "From:";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd.MM.yyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(136, 120);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(100, 20);
            this.dtpTo.TabIndex = 8;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(16, 120);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(112, 23);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "To:";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbEmployee
            // 
            this.tbEmployee.Enabled = false;
            this.tbEmployee.Location = new System.Drawing.Point(136, 56);
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(432, 20);
            this.tbEmployee.TabIndex = 4;
            // 
            // tbWorkingUnit
            // 
            this.tbWorkingUnit.Enabled = false;
            this.tbWorkingUnit.Location = new System.Drawing.Point(136, 25);
            this.tbWorkingUnit.Name = "tbWorkingUnit";
            this.tbWorkingUnit.Size = new System.Drawing.Size(432, 20);
            this.tbWorkingUnit.TabIndex = 2;
            // 
            // lblWorkingUnit
            // 
            this.lblWorkingUnit.Location = new System.Drawing.Point(16, 25);
            this.lblWorkingUnit.Name = "lblWorkingUnit";
            this.lblWorkingUnit.Size = new System.Drawing.Size(112, 23);
            this.lblWorkingUnit.TabIndex = 1;
            this.lblWorkingUnit.Text = "Working unit:";
            this.lblWorkingUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEmployee
            // 
            this.lblEmployee.Location = new System.Drawing.Point(16, 56);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(112, 23);
            this.lblEmployee.TabIndex = 3;
            this.lblEmployee.Text = "Employee:";
            this.lblEmployee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(528, 456);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblReaders
            // 
            this.lblReaders.Location = new System.Drawing.Point(16, 176);
            this.lblReaders.Name = "lblReaders";
            this.lblReaders.Size = new System.Drawing.Size(344, 23);
            this.lblReaders.TabIndex = 10;
            this.lblReaders.Text = "Readers and their last reading time:";
            this.lblReaders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ExtraHoursCalculation
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(616, 484);
            this.ControlBox = false;
            this.Controls.Add(this.lblReaders);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbCalculateExtraHours);
            this.Controls.Add(this.lblLastReaderTime);
            this.Controls.Add(this.lvReaders);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(624, 518);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(624, 518);
            this.Name = "ExtraHoursCalculation";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Calculate extra hours";
            this.Load += new System.EventHandler(this.ExtraHoursCalculation_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExtraHoursCalculation_KeyUp);
            this.gbCalculateExtraHours.ResumeLayout(false);
            this.gbCalculateExtraHours.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region Inner Class for sorting items in View List

		/*
		 *  Class used for sorting items in the List View 
		 */
		private class ListViewItemComparer : IComparer
		{
			private ListView _listView;

			public ListViewItemComparer(ListView lv)
			{
				_listView = lv;
			}
			public ListView ListView
			{
				get{return _listView;}
			}

			private int _sortColumn = 0;

			public int SortColumn
			{
				get { return _sortColumn; }
				set { _sortColumn = value; }
			}

			public int Compare(object a, object b)
			{
				ListViewItem item1 = (ListViewItem) a;
				ListViewItem item2 = (ListViewItem) b;

				if (ListView.Sorting == SortOrder.Descending)
				{
					ListViewItem temp = item1;
					item1 = item2;
					item2 = temp;
				}
				// Handle non Detail Cases
				return CompareItems(item1, item2);
			}

			public int CompareItems(ListViewItem item1, ListViewItem item2)
			{
				// Subitem instances
				ListViewItem.ListViewSubItem sub1 = item1.SubItems[SortColumn];
				ListViewItem.ListViewSubItem sub2 = item2.SubItems[SortColumn];

				// Return value based on sort column	
				switch (SortColumn)
				{
					case ExtraHoursCalculation.ReaderIDIndex:
					{
						int firstID = -1;
						int secondID = -1;

						if (!sub1.Text.Trim().Equals("")) 
						{
							firstID = Int32.Parse(sub1.Text.Trim());
						}

						if (!sub2.Text.Trim().Equals(""))
						{
							secondID = Int32.Parse(sub2.Text.Trim());
						}
						
						return CaseInsensitiveComparer.Default.Compare(firstID, secondID);
					}
					case ExtraHoursCalculation.DescIndex:
					{
						return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
					}
					case ExtraHoursCalculation.DateIndex:
					{
						DateTime dt1 = new DateTime(1,1,1,0,0,0);
						DateTime dt2 = new DateTime(1,1,1,0,0,0);

						if (!sub1.Text.Trim().Equals("")) 
						{
                            dt1 = DateTime.ParseExact(sub1.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
						}

						if (!sub2.Text.Trim().Equals(""))
						{
                            dt2 = DateTime.ParseExact(sub2.Text.Trim(), "dd.MM.yyyy   HH:mm", null);
						}
						
						return CaseInsensitiveComparer.Default.Compare(dt1, dt2);
					}
					default:
						throw new IndexOutOfRangeException("Unrecognized column name extension");																
				}
			}
		}

		#endregion

		/// <summary>
		/// Set proper language and initialize List View
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("extraHoursCalculationForm", culture);

				// group box text
				gbCalculateExtraHours.Text = rm.GetString("extraHoursCalculationForm", culture);

				// button's text
				btnCalculate.Text = rm.GetString("btnCalculate", culture);
				btnClose.Text     = rm.GetString("btnClose", culture);

				// label's text				
				lblWorkingUnit.Text      = rm.GetString("lblWorkingUnit", culture);
				lblEmployee.Text         = rm.GetString("lblEmployee", culture);
				lblFrom.Text             = rm.GetString("lblFrom", culture);
				lblTo.Text               = rm.GetString("lblTo", culture);
				lblReaders.Text          = rm.GetString("lblReaders", culture);
				lblLastReaderTime.Text   = rm.GetString("lblLastReaderTime", culture);
												
				// list view
				lvReaders.BeginUpdate();
				lvReaders.Columns.Add(rm.GetString("hdrReaderID", culture), (lvReaders.Width) / 6, HorizontalAlignment.Left);
				lvReaders.Columns.Add(rm.GetString("hdrDescripton", culture), 3 * (lvReaders.Width) / 6, HorizontalAlignment.Left);
				lvReaders.Columns.Add(rm.GetString("hdrLastReaderTime", culture), 2 * (lvReaders.Width) / 6 - 5, HorizontalAlignment.Left);
				lvReaders.EndUpdate();		
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHoursCalculation.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		/// <summary>
		/// Populate List View with earned hours found
		/// </summary>
		/// <param name="accessGroupsList"></param>
		private void populateListView()
		{
			try
			{				
				List<ReaderTO> readerList = new Reader().SearchLastReadTime();
				//CultureInfo ci = CultureInfo.InvariantCulture;

				lvReaders.BeginUpdate();
				lvReaders.Items.Clear();

				//DateTime allReadersDate = new DateTime();

				if (readerList.Count > 0)
				{
					foreach(ReaderTO reader in readerList)
					{
						ListViewItem item = new ListViewItem();
						item.Text = reader.ReaderID.ToString();
						item.SubItems.Add(reader.Description);
						if (!reader.LastReadTime.Date.Equals(new DateTime(1,1,1,0,0,0)))
						{
							//item.SubItems.Add(reader.DateRead.ToString("dd/MM/yyyy  HH:mm", ci));
							item.SubItems.Add(reader.LastReadTime.ToString("dd.MM.yyyy  HH:mm"));
							if ((allReadersDate == new DateTime()) ||
								(reader.LastReadTime < allReadersDate))
								allReadersDate = reader.LastReadTime;
						}
						else
						{								
							item.SubItems.Add("");
						}

						lvReaders.Items.Add(item);
					}
				}

				lvReaders.EndUpdate();
				lvReaders.Invalidate();

				if (allReadersDate != new DateTime())
				{
					lblLastReaderTime.Text = lblLastReaderTime.Text + " " + allReadersDate.ToString("dd.MM.yyyy  HH:mm");
					//dtpFrom.Value = new DateTime(allReadersDate.Year, allReadersDate.Month, 1);
					//dtpTo.Value = allReadersDate;
                    dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dtpTo.Value = DateTime.Now;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHoursCalculation.populateListView(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void ExtraHoursCalculation_Load(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				// Initialize comparer object
				_comp = new ListViewItemComparer(lvReaders);
				lvReaders.ListViewItemSorter = _comp;
				lvReaders.Sorting = SortOrder.Ascending;
	
				tbWorkingUnit.Text = currentEmployee.WorkingUnitName;
				tbEmployee.Text = currentEmployee.EmployeeID + " - " + currentEmployee.FirstName + " " + currentEmployee.LastName;
			
				populateListView();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHoursCalculation.ExtraHoursCalculation_Load(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnCalculate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<int> selectedEmployees = new List<int>();

                // list of IOpairs for selected Time Interval
                List<IOPairTO> ioPairList = new List<IOPairTO>();

                // list of Time Schemas for selected Employee and selected Time Interval
                List<EmployeeTimeScheduleTO> timeScheduleList = new List<EmployeeTimeScheduleTO>();

                DateTime fromDay = dtpFrom.Value;
                DateTime toDay = dtpTo.Value;

                selectedEmployees.Add(currentEmployee.EmployeeID);

                if (fromDay <= toDay)
                {
                    if (allReadersDate.Date < toDay.Date)
                    {
                        DialogResult result = MessageBox.Show(rm.GetString("lblLastReaderTime", culture)
                            + " " + allReadersDate.ToString("dd.MM.yyyy  HH:mm")
                            + ".\n" + rm.GetString("toContinue", culture), "", MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                        {
                            //this.Close();
                            return;
                        }
                    }

                    //get IO Pairs. Take them for one day more, because off night shift
                    ioPairList = new IOPair().SearchForExtraHours(fromDay, toDay.AddDays(1), selectedEmployees);
                    if (ioPairList.Count > 0)
                    {
                        //Check if there are some open IO Pairs in selected Time Interval
                        List<DateTime> openPairs = new IOPair().SearchDatesWithOpenPairs(fromDay, toDay.AddDays(1), currentEmployee.EmployeeID);
                        if (openPairs.Count > 0)
                        {
                            DialogResult result = MessageBox.Show(rm.GetString("intervalHasOpenPairs", culture), "", MessageBoxButtons.YesNo);
                            if (result == DialogResult.No)
                            {
                                //this.Close();
                                return;
                            }
                        }

                        // all time shemas
                        List<WorkTimeSchemaTO> timeSchemas = new TimeSchema().Search();

                        //get time schemas for selected Employee, for selected Time Interval
                        timeScheduleList = new EmployeesTimeSchedule().SearchEmployeesSchedules(currentEmployee.EmployeeID.ToString(), fromDay, toDay);

                        //Get all holidays
                        List<HolidayTO> holidays = new Holiday().Search(new DateTime(), new DateTime());

                        bool inserted = true;
                        for (DateTime day = fromDay; day <= toDay; day = day.AddDays(1))
                        {
                            bool is2DaysShift = false;
                            bool is2DaysShiftPrevious = false;
                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                            //get time schema intervals for specific day, for Employee. Check if specific day or previous day 
                            //are night shift days. If day is night shift day, also take first interval of next day
                            Dictionary<int, WorkTimeIntervalTO> dayIntervals = Common.Misc.getDayTimeSchemaIntervals(timeScheduleList, day, ref is2DaysShift,
                                ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);

                            if (dayIntervals == null)
                                continue;

                            //Find schema for using date, to see the type
                            WorkTimeSchemaTO dateUsedSchema = Common.Misc.getTimeSchemaForDay(timeScheduleList, day, timeSchemas);

                            //Take only IO Pairs for specific day, considering night shifts
                            List<IOPairTO> dayIOPairList = Common.Misc.getEmployeeDayPairs(ioPairList, day, is2DaysShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);

                            //Check if day is holiday
                            HolidayTO currentDay = new HolidayTO();
                            foreach (HolidayTO holiday in holidays)
                            {
                                if (holiday.HolidayDate.Date.Equals(day.Date))
                                {
                                    currentDay = holiday;
                                    break;
                                }
                            }

                            //calculate how many hours employee suppose to work on specific day
                            TimeSpan schedule = new TimeSpan(0);
                            //If current day is Holiday, it is not a working day, and how many hours
                            //suppose to work is zero
                            //It is checked only for non industrial schemas
                            if ((dateUsedSchema.Type.Trim() == Constants.schemaTypeIndustrial)
                                || (currentDay.HolidayDate == (new DateTime(0))))
                            {
                                //not a holiday

                                //Take only intervals for specific day, considering night shifts
                                List<WorkTimeIntervalTO> dayIntervalsList = Common.Misc.getEmployeeDayIntervals(is2DaysShift, is2DaysShiftPrevious, firstIntervalNextDay, dayIntervals);

                                // Trim pairs to the working time interval boundaries considering early/latency rules
                                // but only if type is not Flexi
                                if (dateUsedSchema.Type.Trim() != Constants.schemaTypeFlexi)
                                    dayIOPairList = Common.Misc.trimDayPairs(dayIOPairList, dayIntervalsList);

                                schedule = Common.Misc.getEmployeeScheduleHourDay(dayIntervalsList);
                            }

                            //calculate how many hours employee did work on specific day
                            TimeSpan job = Common.Misc.getEmployeeJobHourDay(dayIOPairList);

                            TimeSpan overtime = job - schedule;
                            if (overtime.TotalMinutes > 0)
                            {
                                ExtraHourTO extraHourExist = new ExtraHour().Find(currentEmployee.EmployeeID, day);
                                if (extraHourExist.EmployeeID == -1)
                                {
                                    int insertedCount = new ExtraHour().Save(currentEmployee.EmployeeID, day, (int)overtime.TotalMinutes);
                                    inserted = ((insertedCount > 0) ? true : false) && inserted;
                                }
                                else
                                {
                                    inserted = new ExtraHour().Update(currentEmployee.EmployeeID, day, (int)overtime.TotalMinutes) && inserted;
                                }
                            }
                            else // (overtime.TotalMinutes <= 0)
                            {
                                ExtraHourTO extraHourExist = new ExtraHour().Find(currentEmployee.EmployeeID, day);
                                if ((extraHourExist.EmployeeID != -1) && (extraHourExist.ExtraTimeAmt >= 0))
                                {
                                    inserted = new ExtraHour().Delete(currentEmployee.EmployeeID, day) && inserted;
                                }
                            }
                        } //for (DateTime day = fromDay; day <= toDay; day = day.AddDays(1))

                        if (inserted)
                        {
                            MessageBox.Show(rm.GetString("extraHoursSuccessfullyCalculated", culture));
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("extraHoursNotCalculated", culture));
                        }
                    } //if (ioPairList.Count > 0)
                    else
                    {
                        MessageBox.Show(rm.GetString("noIOPairsEmployeeInterval", culture));
                    }
                } //if (fromDay <= toDay)
                else
                {
                    MessageBox.Show(rm.GetString("fromDateGreaterToDate", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHoursCalculation.btnCalculate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHoursCalculation.btnClose_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvReaders_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				SortOrder prevOrder = lvReaders.Sorting;
				lvReaders.Sorting = SortOrder.None;

				if (e.Column == _comp.SortColumn)
				{
					if (prevOrder == SortOrder.Ascending)
					{
						lvReaders.Sorting = SortOrder.Descending;
					}
					else
					{
						lvReaders.Sorting = SortOrder.Ascending;
					}
				}
				else
				{
					// New Sort Order
					_comp.SortColumn = e.Column;
					lvReaders.Sorting = SortOrder.Ascending;
				}
                lvReaders.ListViewItemSorter = _comp;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExtraHoursCalculation.lvReaders_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void ExtraHoursCalculation_KeyUp(object sender, KeyEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExtraHoursCalculation.ExtraHoursCalculation_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
