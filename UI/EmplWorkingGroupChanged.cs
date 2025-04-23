using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Resources;
using System.Globalization;

using Common;
using TransferObjects;
using Util;

namespace UI
{
    /// <summary>
	/// Summary description for EmplWorkingGroupChanged.
	/// </summary>
    public class EmplWorkingGroupChanged : Form
    {
        private System.Windows.Forms.Button btnUpdate;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private WorkingGroupTO currentGroup = null;

		private CultureInfo culture;
        ResourceManager rm;

		DebugLog log;
		ApplUserTO logInUser;
        
        private GroupBox gbTimeSchema;
        private GroupBox gbStartDate;
        private RadioButton rbSelectDate;
        private RadioButton rbToday;
        private RadioButton rbNextMonth;
        private DateTimePicker dtpSelectDate;
        private Label lblStartDate;

        // Schema Details List View indexes
        const int DayNum = 0;
        const int IntervalNum = 1;
        const int StartTime = 2;
        const int EndTime = 3;
        const int Tolerance = 4;
        
        private ListViewItemComparer1 _comp1;

        private List<WorkTimeSchemaTO> timeSchemas;
        private TextBox tbTimeSchema;
        private Label lblTimeSchema;
        private ListView lvTimeSchemaDetails;
        private Label lblFormInfo;
        private DateTime minDate;

        private IDbTransaction trans;
        private int emplID;

        private Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
        private Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
        private Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>>();

        public EmplWorkingGroupChanged(int wrkGrpID, int employeeID, Dictionary<int, EmployeeTO> emplDict, Dictionary<int, EmployeeAsco4TO> ascoDict, 
            Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict, IDbTransaction transaction)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				InitializeComponent();
				this.CenterToScreen();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

                this.trans = transaction;
                currentGroup = new WorkingGroupTO();
                currentGroup = new WorkingGroup().Find(wrkGrpID, this.trans);
                emplID = employeeID;
				logInUser = NotificationController.GetLogInUser();
				
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
				rm = new ResourceManager("UI.Resource",typeof(EmplWorkingGroupChanged).Assembly);
				setLanguage();

                rbToday.Checked = true;

                tbTimeSchema.ReadOnly = true;
                dtpSelectDate.Enabled = false;

                this.emplDict = emplDict;
                this.ascoDict = ascoDict;
                this.rulesDict = rulesDict;
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Arrow;
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.gbTimeSchema = new System.Windows.Forms.GroupBox();
            this.tbTimeSchema = new System.Windows.Forms.TextBox();
            this.lblTimeSchema = new System.Windows.Forms.Label();
            this.lvTimeSchemaDetails = new System.Windows.Forms.ListView();
            this.gbStartDate = new System.Windows.Forms.GroupBox();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpSelectDate = new System.Windows.Forms.DateTimePicker();
            this.rbSelectDate = new System.Windows.Forms.RadioButton();
            this.rbToday = new System.Windows.Forms.RadioButton();
            this.rbNextMonth = new System.Windows.Forms.RadioButton();
            this.lblFormInfo = new System.Windows.Forms.Label();
            this.gbTimeSchema.SuspendLayout();
            this.gbStartDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(227, 456);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 14;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // gbTimeSchema
            // 
            this.gbTimeSchema.Controls.Add(this.tbTimeSchema);
            this.gbTimeSchema.Controls.Add(this.lblTimeSchema);
            this.gbTimeSchema.Controls.Add(this.lvTimeSchemaDetails);
            this.gbTimeSchema.Location = new System.Drawing.Point(33, 78);
            this.gbTimeSchema.Name = "gbTimeSchema";
            this.gbTimeSchema.Size = new System.Drawing.Size(445, 194);
            this.gbTimeSchema.TabIndex = 3;
            this.gbTimeSchema.TabStop = false;
            this.gbTimeSchema.Text = "Current group time schema";
            // 
            // tbTimeSchema
            // 
            this.tbTimeSchema.Location = new System.Drawing.Point(123, 22);
            this.tbTimeSchema.MaxLength = 64;
            this.tbTimeSchema.Name = "tbTimeSchema";
            this.tbTimeSchema.Size = new System.Drawing.Size(299, 20);
            this.tbTimeSchema.TabIndex = 2;
            // 
            // lblTimeSchema
            // 
            this.lblTimeSchema.Location = new System.Drawing.Point(22, 20);
            this.lblTimeSchema.Name = "lblTimeSchema";
            this.lblTimeSchema.Size = new System.Drawing.Size(88, 23);
            this.lblTimeSchema.TabIndex = 1;
            this.lblTimeSchema.Text = "Time Schema:";
            this.lblTimeSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvTimeSchemaDetails
            // 
            this.lvTimeSchemaDetails.BackColor = System.Drawing.SystemColors.Control;
            this.lvTimeSchemaDetails.FullRowSelect = true;
            this.lvTimeSchemaDetails.GridLines = true;
            this.lvTimeSchemaDetails.HideSelection = false;
            this.lvTimeSchemaDetails.Location = new System.Drawing.Point(22, 47);
            this.lvTimeSchemaDetails.MultiSelect = false;
            this.lvTimeSchemaDetails.Name = "lvTimeSchemaDetails";
            this.lvTimeSchemaDetails.Size = new System.Drawing.Size(400, 136);
            this.lvTimeSchemaDetails.TabIndex = 3;
            this.lvTimeSchemaDetails.UseCompatibleStateImageBehavior = false;
            this.lvTimeSchemaDetails.View = System.Windows.Forms.View.Details;
            this.lvTimeSchemaDetails.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvTimeSchemaDetails_ColumnClick);
            // 
            // gbStartDate
            // 
            this.gbStartDate.Controls.Add(this.lblStartDate);
            this.gbStartDate.Controls.Add(this.dtpSelectDate);
            this.gbStartDate.Controls.Add(this.rbSelectDate);
            this.gbStartDate.Controls.Add(this.rbToday);
            this.gbStartDate.Controls.Add(this.rbNextMonth);
            this.gbStartDate.Location = new System.Drawing.Point(60, 296);
            this.gbStartDate.Name = "gbStartDate";
            this.gbStartDate.Size = new System.Drawing.Size(395, 145);
            this.gbStartDate.TabIndex = 12;
            this.gbStartDate.TabStop = false;
            this.gbStartDate.Text = "Using date";
            // 
            // lblStartDate
            // 
            this.lblStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.Location = new System.Drawing.Point(16, 99);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(374, 43);
            this.lblStartDate.TabIndex = 9;
            this.lblStartDate.Text = "* Look into schema definition list to see time schedule for selected using date";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpSelectDate
            // 
            this.dtpSelectDate.CustomFormat = "dd/MM/yyyy";
            this.dtpSelectDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSelectDate.Location = new System.Drawing.Point(180, 70);
            this.dtpSelectDate.Name = "dtpSelectDate";
            this.dtpSelectDate.ShowUpDown = true;
            this.dtpSelectDate.Size = new System.Drawing.Size(160, 20);
            this.dtpSelectDate.TabIndex = 4;
            this.dtpSelectDate.ValueChanged += new System.EventHandler(this.dtpSelectDate_ValueChanged);
            // 
            // rbSelectDate
            // 
            this.rbSelectDate.Location = new System.Drawing.Point(16, 70);
            this.rbSelectDate.Name = "rbSelectDate";
            this.rbSelectDate.Size = new System.Drawing.Size(160, 24);
            this.rbSelectDate.TabIndex = 3;
            this.rbSelectDate.Text = "Select date";
            this.rbSelectDate.CheckedChanged += new System.EventHandler(this.rbSelectDate_CheckedChanged);
            // 
            // rbToday
            // 
            this.rbToday.Checked = true;
            this.rbToday.Location = new System.Drawing.Point(16, 20);
            this.rbToday.Name = "rbToday";
            this.rbToday.Size = new System.Drawing.Size(160, 24);
            this.rbToday.TabIndex = 1;
            this.rbToday.TabStop = true;
            this.rbToday.Text = "Today";
            this.rbToday.CheckedChanged += new System.EventHandler(this.rbToday_CheckedChanged);
            // 
            // rbNextMonth
            // 
            this.rbNextMonth.Location = new System.Drawing.Point(16, 45);
            this.rbNextMonth.Name = "rbNextMonth";
            this.rbNextMonth.Size = new System.Drawing.Size(160, 24);
            this.rbNextMonth.TabIndex = 2;
            this.rbNextMonth.Text = "First day next month";
            this.rbNextMonth.CheckedChanged += new System.EventHandler(this.rbNextMonth_CheckedChanged);
            // 
            // lblFormInfo
            // 
            this.lblFormInfo.AutoSize = true;
            this.lblFormInfo.Location = new System.Drawing.Point(30, 28);
            this.lblFormInfo.Name = "lblFormInfo";
            this.lblFormInfo.Size = new System.Drawing.Size(459, 13);
            this.lblFormInfo.TabIndex = 15;
            this.lblFormInfo.Text = "You have changed employee\'s working group. You have to assign group schedule to e" +
                "mployee.";
            // 
            // EmplWorkingGroupChanged
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(511, 491);
            this.ControlBox = false;
            this.Controls.Add(this.lblFormInfo);
            this.Controls.Add(this.gbStartDate);
            this.Controls.Add(this.gbTimeSchema);
            this.Controls.Add(this.btnUpdate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(519, 525);
            this.MinimumSize = new System.Drawing.Size(519, 525);
            this.Name = "EmplWorkingGroupChanged";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "WTGroupsAdd";
            this.Load += new System.EventHandler(this.WTGroupsAdd_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EmplWorkingGroupChanged_KeyUp);
            this.gbTimeSchema.ResumeLayout(false);
            this.gbTimeSchema.PerformLayout();
            this.gbStartDate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        #region Inner Class for sorting items in Schema Details View List

        /*
		 *  Class used for sorting items in the List View 
		*/
        private class ListViewItemComparer1 : IComparer
        {
            private ListView _listView;

            public ListViewItemComparer1(ListView lv)
            {
                _listView = lv;
            }
            public ListView ListView
            {
                get { return _listView; }
            }

            private int _sortColumn = 0;

            public int SortColumn
            {
                get { return _sortColumn; }
                set { _sortColumn = value; }
            }

            public int Compare(object a, object b)
            {
                ListViewItem item1 = (ListViewItem)a;
                ListViewItem item2 = (ListViewItem)b;

                if (ListView.Sorting == System.Windows.Forms.SortOrder.Descending)
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
                    case EmplWorkingGroupChanged.DayNum:
                    case EmplWorkingGroupChanged.IntervalNum:
                    case EmplWorkingGroupChanged.Tolerance:
                        {
                            int prvi = -1;
                            int drugi = -1;
                            if (!sub1.Text.Trim().Equals(""))
                            {
                                prvi = Int32.Parse(sub1.Text.Trim());
                            }
                            if (!sub2.Text.Trim().Equals(""))
                            {
                                drugi = Int32.Parse(sub2.Text.Trim());
                            }
                            if ((SortColumn == 0) && (prvi == drugi))
                            {
                                if (!item1.SubItems[1].Text.Trim().Equals(""))
                                {
                                    prvi = Int32.Parse(item1.SubItems[1].Text.Trim());
                                }
                                if (!item2.SubItems[1].Text.Trim().Equals(""))
                                {
                                    drugi = Int32.Parse(item2.SubItems[1].Text.Trim());
                                }
                            }
                            return CaseInsensitiveComparer.Default.Compare(prvi, drugi);
                        }
                    case EmplWorkingGroupChanged.StartTime:
                    case EmplWorkingGroupChanged.EndTime:
                        {
                            return CaseInsensitiveComparer.Default.Compare(sub1.Text, sub2.Text);
                        }
                    default:
                        {
                            return 0;
                        }
                }
            }
        }

        #endregion

		/// <summary>
		/// Set proper language.
		/// </summary>
		public void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("emplGroupChangedForm", culture);

                // group box text
                gbTimeSchema.Text = rm.GetString("gbTimeSchema", culture);
                gbStartDate.Text = rm.GetString("gbStartDate", culture);

                // radio button's text
                rbToday.Text = rm.GetString("rbToday", culture);
                rbNextMonth.Text = rm.GetString("rbNextMonth", culture);
                rbSelectDate.Text = rm.GetString("rbSelectDate", culture);

				// button's text
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				
				// label's text
                lblTimeSchema.Text = rm.GetString("lblSchema", culture);
                lblStartDate.Text = rm.GetString("lblStartDateDef", culture);
                lblFormInfo.Text = rm.GetString("lblFormInfo", culture);

				// list view
				lvTimeSchemaDetails.BeginUpdate();
                lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrDayNum", culture), lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
                lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrIntervalNumber", culture), lvTimeSchemaDetails.Width / 5 - 5, HorizontalAlignment.Center);
                lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrSartTime", culture), lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
                lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrEndTime", culture), lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
                lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrTolerance", culture), lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
                lvTimeSchemaDetails.EndUpdate();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.setLanguage(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;                
            
                bool successfull = true;

                List<EmployeeGroupsTimeScheduleTO> groupSchedules = new EmployeeGroupsTimeSchedule().SearchFromSchedules(
                    currentGroup.EmployeeGroupID, minDate, this.trans);

                if (!minDate.Equals(new DateTime()))
                {
                    DateTime startingDate = new DateTime(minDate.Year, minDate.Month, minDate.Day, 0, 0, 0);

                    EmployeesTimeSchedule ets = new EmployeesTimeSchedule();
                    ets.SetTransaction(this.trans);
                    successfull = ets.DeleteFromToSchedule(emplID, startingDate.Date, new DateTime(), "", false) && successfull;

                    if (successfull)
                    {
                        int timeScheduleIndex = -1;
                        for (int scheduleIndex = 0; scheduleIndex < groupSchedules.Count; scheduleIndex++)
                        {
                            if (startingDate.Date >= groupSchedules[scheduleIndex].Date)
                            {
                                timeScheduleIndex = scheduleIndex;
                            }
                        }
                        if (timeScheduleIndex >= 0)
                        {
                            EmployeeGroupsTimeScheduleTO egts = groupSchedules[timeScheduleIndex];
                            int startDay = egts.StartCycleDay;
                            int schemaID = egts.TimeSchemaID;

                            WorkTimeSchemaTO actualTimeSchema = null;
                            foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemas)
                            {
                                if (currentTimeSchema.TimeSchemaID == schemaID)
                                {
                                    actualTimeSchema = currentTimeSchema;
                                    break;
                                }
                            }
                            if (actualTimeSchema != null)
                            {
                                int cycleDuration = actualTimeSchema.CycleDuration;

                                TimeSpan ts = new TimeSpan(startingDate.Date.Ticks - egts.Date.Date.Ticks);
                                int dayNum = (startDay + (int)ts.TotalDays) % cycleDuration;

                                int insert = ets.Save(emplID, startingDate.Date, schemaID, dayNum, "", false);
                                successfull = (insert > 0) && successfull;

                                if (successfull)
                                {
                                    for (int scheduleIndex = timeScheduleIndex + 1; scheduleIndex < groupSchedules.Count; scheduleIndex++)
                                    {
                                        egts = groupSchedules[scheduleIndex];

                                        insert = ets.Save(emplID, egts.Date,
                                            egts.TimeSchemaID, egts.StartCycleDay, "", false);
                                        successfull = (insert > 0) && successfull;

                                        if (!successfull)
                                            break;
                                    }
                                }
                            }
                        }

                        // validate new employee schedule
                        if (successfull)
                        {
                            bool validFundHrs = true;
                            DateTime invalidDate = Common.Misc.isValidTimeSchedule(emplDict, ascoDict, rulesDict, emplID.ToString().Trim(), startingDate.Date, startingDate.Date.AddMonths(1).Date, this.trans, null, false, ref validFundHrs, true);

                            if (!invalidDate.Equals(new DateTime()))
                            {
                                successfull = false;
                                if (validFundHrs)
                                    MessageBox.Show(rm.GetString("notValidScheduleAssigned", culture)
                                        + " " + invalidDate.Date.AddDays(-1).ToString(Constants.dateFormat) + "/" + invalidDate.Date.ToString(Constants.dateFormat));
                                else
                                    MessageBox.Show(rm.GetString("notValidFundHrs", culture), " " + invalidDate.Date.ToString(Constants.dateFormat) + "-"
                                        + invalidDate.AddDays(6).Date.ToString(Constants.dateFormat));
                            }
                        }

                        if (successfull)
                        {
                            // delete absences pairs and update absences to unused
                            deleteIOPUpdateEA(emplID, startingDate.Date, this.trans);

                            //recalculate pauses
                            if (startingDate.Date <= DateTime.Now.Date)
                            {
                                IOPair ioPair = new IOPair();
                                ioPair.recalculatePause(emplID.ToString().Trim(), startingDate.Date, DateTime.Now.Date, this.trans);
                            }
                        } //if (successful)
                    } //if (successful)
                } //if (!minDate.Equals(new DateTime()))

                ((EmployeeAdd)this.Owner).setTimeSchemaChanged(successfull);

                this.Close();
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 2627)
                {
                    log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.btnUpdate_Click(): " + rm.GetString("groupExist", culture) + "\n");
                    MessageBox.Show(rm.GetString("groupExist", culture));
                }
                else
                {
                    log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.btnUpdate_Click(): " + sqlex.Message + "\n");
                    MessageBox.Show(sqlex.Message);
                }
                this.Close();
            }
            catch (MySqlException mysqlex)
            {
                if (mysqlex.Number == 1062)
                {
                    log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.btnUpdate_Click(): " + rm.GetString("groupExist", culture) + "\n");
                    MessageBox.Show(rm.GetString("groupExist", culture));
                }
                else
                {
                    log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.btnUpdate_Click(): " + mysqlex.Message + "\n");
                    MessageBox.Show(mysqlex.Message);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                this.Close();
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        // delete absences pairs and update absences to unused
        private void deleteIOPUpdateEA(int employeeID, DateTime startingDate, IDbTransaction trans)
        {
            try
            {

                #region Reprocess Dates
                if (startingDate.Date <= DateTime.Now.Date)
                {
                    List<DateTime> datesList = new List<DateTime>();

                    DateTime endDate = new IOPairProcessed().getMaxDateOfPair(employeeID.ToString(),trans);

                    if (endDate.Date < DateTime.Now.Date)
                        endDate = DateTime.Now.Date;

                    for (DateTime dt = startingDate.Date; dt <= endDate; dt = dt.AddDays(1))
                    {
                        datesList.Add(dt);
                    }

                    //list od datetime for each employee
                    Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                    emplDateWholeDayList.Add(employeeID, datesList);
                    if(datesList.Count >0)
                    Common.Misc.ReprocessPairsAndRecalculateCounters(employeeID.ToString(), startingDate.Date, DateTime.Now.Date, trans, emplDateWholeDayList, null, "");
                }
                #endregion

                DateTime end = new DateTime(0);
                EmployeeAbsence ea = new EmployeeAbsence();
                ea.EmplAbsTO.EmployeeID = employeeID;
                ea.EmplAbsTO.DateStart = startingDate.Date;
                ea.EmplAbsTO.DateEnd = end;
                List<EmployeeAbsenceTO> emplAbsences = ea.Search("", trans);

                foreach (EmployeeAbsenceTO abs in emplAbsences)
                {
                    new EmployeeAbsence().UpdateEADeleteIOP(abs.RecID, abs.EmployeeID, abs.PassTypeID,
                        abs.PassTypeID, abs.DateStart, abs.DateEnd, abs.DateStart, abs.DateEnd, (int)Constants.Used.No, trans);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.deleteIOPUpdateEA(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }

      

        private void populateTimeSchemaDetailsListView(DateTime date)
        {
            try
            {
                List<EmployeeGroupsTimeScheduleTO> timeScheduleList = new List<EmployeeGroupsTimeScheduleTO>();
                timeScheduleList = new EmployeeGroupsTimeSchedule().SearchMonthSchedule(currentGroup.EmployeeGroupID, date, this.trans);

                int timeScheduleIndex = -1;
                for (int scheduleIndex = 0; scheduleIndex < timeScheduleList.Count; scheduleIndex++)
                {
                    if (date >= timeScheduleList[scheduleIndex].Date)
                    {
                        timeScheduleIndex = scheduleIndex;
                    }
                }

                Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> days = new Dictionary<int,Dictionary<int,WorkTimeIntervalTO>>();
                int dayNum = -1;
                if (timeScheduleIndex >= 0)
                {
                    EmployeeGroupsTimeScheduleTO egts = timeScheduleList[timeScheduleIndex];
                    int startDay = egts.StartCycleDay;
                    int schemaID = egts.TimeSchemaID;

                    WorkTimeSchemaTO actualTimeSchema = null;
                    foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemas)
                    {
                        if (currentTimeSchema.TimeSchemaID == schemaID)
                        {
                            actualTimeSchema = currentTimeSchema;
                            break;
                        }
                    }
                    if (actualTimeSchema != null)
                    {
                        tbTimeSchema.Text = actualTimeSchema.TimeSchemaID + " - " + actualTimeSchema.Name;

                        days = actualTimeSchema.Days;
                        int cycleDuration = actualTimeSchema.CycleDuration;

                        TimeSpan ts = new TimeSpan(date.Date.Ticks - egts.Date.Date.Ticks);
                        dayNum = (startDay + (int)ts.TotalDays) % cycleDuration;
                    }
                    else
                        tbTimeSchema.Text = "";
                }
                else
                    tbTimeSchema.Text = "";

                lvTimeSchemaDetails.BeginUpdate();
                lvTimeSchemaDetails.Items.Clear();

                WorkTimeIntervalTO interval = new WorkTimeIntervalTO();
                Dictionary<int, WorkTimeIntervalTO> intervals = new Dictionary<int,WorkTimeIntervalTO>();
                foreach (int dayKey in days.Keys)
                {
                    bool select = false;
                    if (dayKey == dayNum)
                        select = true;

                    intervals = days[dayKey];

                    foreach (int intKey in intervals.Keys)
                    {
                        interval = intervals[intKey];

                        ListViewItem item = new ListViewItem();
                        item.Text = (interval.DayNum + 1).ToString();

                        item.SubItems.Add((interval.IntervalNum + 1).ToString());
                        item.SubItems.Add(interval.StartTime.ToString("HH:mm"));
                        item.SubItems.Add(interval.EndTime.ToString("HH:mm"));

                        // Check tolerance
                        TimeSpan ts0 = new TimeSpan();
                        ts0 = interval.StartTime.Subtract(interval.EarliestArrived);
                        int tm0;
                        if (ts0.Minutes < 0)
                        {
                            tm0 = 60 + ts0.Minutes;
                        }
                        else
                        {
                            tm0 = ts0.Minutes;
                        }

                        TimeSpan ts1 = new TimeSpan();
                        ts1 = interval.LatestArrivaed.Subtract(interval.StartTime);
                        int tm1;
                        if (ts1.Minutes < 0)
                        {
                            tm1 = 60 + ts1.Minutes;
                        }
                        else
                        {
                            tm1 = ts1.Minutes;
                        }

                        TimeSpan ts2 = new TimeSpan();
                        ts2 = interval.EndTime.Subtract(interval.EarliestLeft);
                        int tm2;
                        if (ts2.Minutes < 0)
                        {
                            tm2 = 60 + ts2.Minutes;
                        }
                        else
                        {
                            tm2 = ts2.Minutes;
                        }

                        TimeSpan ts3 = new TimeSpan();
                        ts3 = interval.LatestLeft.Subtract(interval.EndTime);
                        int tm3;
                        if (ts3.Minutes < 0)
                        {
                            tm3 = 60 + ts3.Minutes;
                        }
                        else
                        {
                            tm3 = ts3.Minutes;
                        }

                        if ((tm0 == tm1) && (tm0 == tm2) && (tm0 == tm3) && (tm1 == tm2) && (tm1 == tm3) && (tm2 == tm3)
                            && (tm0 > 0) && (tm1 > 0) && (tm2 > 0) && (tm3 > 0))
                        {
                            item.SubItems.Add(Convert.ToString(ts0.Minutes));
                        }
                        else
                        {
                            item.SubItems.Add("");
                        }

                        item.Tag = interval;

                        if (select)
                            item.Selected = true;

                        lvTimeSchemaDetails.Items.Add(item);
                    }
                }

                lvTimeSchemaDetails.EndUpdate();
                lvTimeSchemaDetails.Invalidate();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.populateTimeSchemaDetailsListView(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		private void WTGroupsAdd_Load(object sender, System.EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Initialize comparer objects
                _comp1 = new ListViewItemComparer1(lvTimeSchemaDetails);
                lvTimeSchemaDetails.ListViewItemSorter = _comp1;
                lvTimeSchemaDetails.Sorting = System.Windows.Forms.SortOrder.Ascending;

                timeSchemas = new TimeSchema().Search(this.trans);
                minDate = DateTime.Now;

                if (currentGroup.EmployeeGroupID != -1)
                    populateTimeSchemaDetailsListView(DateTime.Now.Date);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.WTGroupsAdd_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void lvTimeSchemaDetails_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.Windows.Forms.SortOrder prevOrder = lvTimeSchemaDetails.Sorting;

                if (e.Column == _comp1.SortColumn)
                {
                    if (prevOrder == System.Windows.Forms.SortOrder.Ascending)
                    {
                        lvTimeSchemaDetails.Sorting = System.Windows.Forms.SortOrder.Descending;
                    }
                    else
                    {
                        lvTimeSchemaDetails.Sorting = System.Windows.Forms.SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp1.SortColumn = e.Column;
                    lvTimeSchemaDetails.Sorting = System.Windows.Forms.SortOrder.Ascending;
                }
                lvTimeSchemaDetails.Sort();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.lvTimeSchemaDetails_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbToday_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rbToday.Checked)
                {
                    populateTimeSchemaDetailsListView(DateTime.Now.Date);
                    dtpSelectDate.Enabled = false;
                    minDate = DateTime.Now.Date;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.rbToday_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbNextMonth_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rbNextMonth.Checked)
                {
                    DateTime nextMonth = DateTime.Now.Date.AddMonths(1);
                    DateTime firstNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                    populateTimeSchemaDetailsListView(firstNextMonth.Date);
                    dtpSelectDate.Enabled = false;
                    minDate = firstNextMonth;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.rbNextMonth_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void rbSelectDate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                if (rbSelectDate.Checked)
                {
                    dtpSelectDate.Enabled = true;
                    populateTimeSchemaDetailsListView(dtpSelectDate.Value.Date);
                    minDate = dtpSelectDate.Value.Date;
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.rbSelectDate_CheckedChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void dtpSelectDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DateTime lastMonth = DateTime.Now.Date.AddMonths(-1);
                DateTime firstLastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                if (dtpSelectDate.Value.Date < firstLastMonth.Date)
                {
                    dtpSelectDate.Value = DateTime.Now.Date;
                    MessageBox.Show(rm.GetString("minSelectDate", culture));
                }
                else
                {
                    // get dictionary of all rules, key is company and value are rules by employee type id
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule().SearchWUEmplTypeDictionary(trans);
                    Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit().getWUDictionary(trans);

                    EmployeeTO empl = (new Employee()).Find(emplID.ToString(),trans);

                    int cutOffDate = -1;
                    int emplCompany = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wUnits);

                    if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(empl.EmployeeTypeID) && emplRules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleHRSSCCutOffDate))
                        cutOffDate = emplRules[emplCompany][empl.EmployeeTypeID][Constants.RuleHRSSCCutOffDate].RuleValue;

                    if (cutOffDate != -1 && Common.Misc.countWorkingDays(DateTime.Now.Date, null) > cutOffDate && dtpSelectDate.Value.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                    {
                        dtpSelectDate.Value = DateTime.Now.Date;
                        MessageBox.Show(rm.GetString("cutOffDayPessed", culture));
                    }
                }
                
                populateTimeSchemaDetailsListView(dtpSelectDate.Value.Date);
                minDate = dtpSelectDate.Value.Date;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.dtpSelectDate_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void EmplWorkingGroupChanged_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " EmplWorkingGroupChanged.EmplWorkingGroupChanged_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}