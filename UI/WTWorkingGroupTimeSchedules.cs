using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Data;

using System.Text;
using Common;
using Util;
using TransferObjects;

using System.Resources;
using System.Globalization;

namespace UI
{
	/// <summary>
	/// Summary description for WTEmployeesTimeSchedule.
	/// </summary>
	public class WTWorkingGroupTimeSchedules : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblMonth;
		private System.Windows.Forms.DateTimePicker dtpMonth;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox tbMon;
		private System.Windows.Forms.TextBox tbTue;
		private System.Windows.Forms.TextBox tbWed;
		private System.Windows.Forms.TextBox tbThr;
		private System.Windows.Forms.TextBox tbFri;
		private System.Windows.Forms.TextBox tbSat;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnAssign;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private CultureInfo culture;
		ResourceManager rm;
		
		// Controller instance
		public NotificationController Controller;
		
		// Observer client instance
		public NotificationObserverClient observerClient;

		private System.Windows.Forms.Label lblTimeSchema;
		private System.Windows.Forms.ListView lvTimeSchemaDetails;
		private System.Windows.Forms.ComboBox cbTimeSchema;
		private System.Windows.Forms.TextBox tbSun;
        private System.Windows.Forms.Label lblGroupID;
        private System.Windows.Forms.TextBox tbGroupID;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Button btnClear;
        		
		DebugLog log;
		ApplUserTO logInUser;

        private DateTime selectedDay = new DateTime();
        private DateTime minSelected;
        private DateTime maxSelected;
        private Dictionary<DateTime, DayOfCalendar> calendarDays;
        private List<DateTime> selectedDays;

		private bool shiftStatus = false;

		const int DayNum = 0;
		const int IntervalNum = 1;
		const int StartTime = 2;
		const int EndTime = 3;
		const int Tolerance = 4;
		
		private ListViewItemComparer _comp;
        List<DateTime> datesList = new List<DateTime>();
        Dictionary<int, WorkTimeSchemaTO> schemas = new Dictionary<int, WorkTimeSchemaTO>();

        WorkingGroupTO Group = new WorkingGroupTO();

		public WTWorkingGroupTimeSchedules(WorkingGroupTO grp)
		{
			try
			{
				InitializeComponent();
				IntitObserverClient();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				logInUser = NotificationController.GetLogInUser();

                calendarDays = new Dictionary<DateTime, DayOfCalendar>();
                selectedDays = new List<DateTime>();

				this.CenterToScreen();

				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
				rm = new ResourceManager("UI.Resource",typeof(WTEmployeesTimeSchedule).Assembly);
				setLanguage();

                this.Group = grp;

				this.tbGroupID.Text = grp.EmployeeGroupID.ToString().Trim();
				this.tbName.Text = grp.GroupName.Trim();
				this.tbDescription.Text = grp.Description.Trim();                                
			}
			catch(Exception ex)
			{                
				throw new Exception(ex.Message);
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
			this.lblGroupID = new System.Windows.Forms.Label();
			this.tbGroupID = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.lblMonth = new System.Windows.Forms.Label();
			this.dtpMonth = new System.Windows.Forms.DateTimePicker();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tbSun = new System.Windows.Forms.TextBox();
			this.tbSat = new System.Windows.Forms.TextBox();
			this.tbFri = new System.Windows.Forms.TextBox();
			this.tbThr = new System.Windows.Forms.TextBox();
			this.tbWed = new System.Windows.Forms.TextBox();
			this.tbTue = new System.Windows.Forms.TextBox();
			this.tbMon = new System.Windows.Forms.TextBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblTimeSchema = new System.Windows.Forms.Label();
			this.cbTimeSchema = new System.Windows.Forms.ComboBox();
			this.btnAssign = new System.Windows.Forms.Button();
			this.lvTimeSchemaDetails = new System.Windows.Forms.ListView();
			this.btnClear = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblGroupID
			// 
			this.lblGroupID.Location = new System.Drawing.Point(8, 64);
			this.lblGroupID.Name = "lblGroupID";
			this.lblGroupID.Size = new System.Drawing.Size(96, 23);
			this.lblGroupID.TabIndex = 0;
			this.lblGroupID.Text = "Group ID:";
			this.lblGroupID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbGroupID
			// 
			this.tbGroupID.Enabled = false;
			this.tbGroupID.Location = new System.Drawing.Point(120, 64);
			this.tbGroupID.Name = "tbGroupID";
			this.tbGroupID.ReadOnly = true;
			this.tbGroupID.Size = new System.Drawing.Size(152, 20);
			this.tbGroupID.TabIndex = 1;
			this.tbGroupID.Text = "";
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(8, 96);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(96, 23);
			this.lblName.TabIndex = 2;
			this.lblName.Text = "Name:";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbName
			// 
			this.tbName.Enabled = false;
			this.tbName.Location = new System.Drawing.Point(120, 96);
			this.tbName.Name = "tbName";
			this.tbName.ReadOnly = true;
			this.tbName.Size = new System.Drawing.Size(152, 20);
			this.tbName.TabIndex = 3;
			this.tbName.Text = "";
			// 
			// lblDescription
			// 
			this.lblDescription.Location = new System.Drawing.Point(8, 128);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(96, 23);
			this.lblDescription.TabIndex = 4;
			this.lblDescription.Text = "Description:";
			this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbDescription
			// 
			this.tbDescription.Enabled = false;
			this.tbDescription.Location = new System.Drawing.Point(120, 128);
			this.tbDescription.Name = "tbDescription";
			this.tbDescription.ReadOnly = true;
			this.tbDescription.Size = new System.Drawing.Size(152, 20);
			this.tbDescription.TabIndex = 5;
			this.tbDescription.Text = "";
			// 
			// lblMonth
			// 
			this.lblMonth.Location = new System.Drawing.Point(32, 240);
			this.lblMonth.Name = "lblMonth";
			this.lblMonth.Size = new System.Drawing.Size(72, 23);
			this.lblMonth.TabIndex = 10;
			this.lblMonth.Text = "Month:";
			this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dtpMonth
			// 
			this.dtpMonth.CustomFormat = "MMM, yyyy";
			this.dtpMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpMonth.Location = new System.Drawing.Point(120, 240);
			this.dtpMonth.Name = "dtpMonth";
			this.dtpMonth.ShowUpDown = true;
			this.dtpMonth.Size = new System.Drawing.Size(152, 20);
			this.dtpMonth.TabIndex = 11;
			this.dtpMonth.ValueChanged += new System.EventHandler(this.dtpMonth_ValueChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tbSun);
			this.panel1.Controls.Add(this.tbSat);
			this.panel1.Controls.Add(this.tbFri);
			this.panel1.Controls.Add(this.tbThr);
			this.panel1.Controls.Add(this.tbWed);
			this.panel1.Controls.Add(this.tbTue);
			this.panel1.Controls.Add(this.tbMon);
			this.panel1.Location = new System.Drawing.Point(40, 280);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(640, 295);
			this.panel1.TabIndex = 12;
			// 
			// tbSun
			// 
			this.tbSun.BackColor = System.Drawing.Color.Lavender;
			this.tbSun.Location = new System.Drawing.Point(545, 0);
			this.tbSun.Name = "tbSun";
			this.tbSun.ReadOnly = true;
			this.tbSun.Size = new System.Drawing.Size(90, 20);
			this.tbSun.TabIndex = 6;
			this.tbSun.Text = "Sun";
			this.tbSun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbSat
			// 
			this.tbSat.BackColor = System.Drawing.Color.Lavender;
			this.tbSat.Location = new System.Drawing.Point(455, 0);
			this.tbSat.Name = "tbSat";
			this.tbSat.ReadOnly = true;
			this.tbSat.Size = new System.Drawing.Size(90, 20);
			this.tbSat.TabIndex = 5;
			this.tbSat.Text = "Sat";
			this.tbSat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbFri
			// 
			this.tbFri.BackColor = System.Drawing.Color.Lavender;
			this.tbFri.Location = new System.Drawing.Point(365, 0);
			this.tbFri.Name = "tbFri";
			this.tbFri.ReadOnly = true;
			this.tbFri.Size = new System.Drawing.Size(90, 20);
			this.tbFri.TabIndex = 4;
			this.tbFri.Text = "Fri";
			this.tbFri.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbThr
			// 
			this.tbThr.BackColor = System.Drawing.Color.Lavender;
			this.tbThr.Location = new System.Drawing.Point(275, 0);
			this.tbThr.Name = "tbThr";
			this.tbThr.ReadOnly = true;
			this.tbThr.Size = new System.Drawing.Size(90, 20);
			this.tbThr.TabIndex = 3;
			this.tbThr.Text = "Thr";
			this.tbThr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbWed
			// 
			this.tbWed.BackColor = System.Drawing.Color.Lavender;
			this.tbWed.Location = new System.Drawing.Point(185, 0);
			this.tbWed.Name = "tbWed";
			this.tbWed.ReadOnly = true;
			this.tbWed.Size = new System.Drawing.Size(90, 20);
			this.tbWed.TabIndex = 2;
			this.tbWed.Text = "Wed";
			this.tbWed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbTue
			// 
			this.tbTue.BackColor = System.Drawing.Color.Lavender;
			this.tbTue.Location = new System.Drawing.Point(95, 0);
			this.tbTue.Name = "tbTue";
			this.tbTue.ReadOnly = true;
			this.tbTue.Size = new System.Drawing.Size(90, 20);
			this.tbTue.TabIndex = 1;
			this.tbTue.Text = "Tue";
			this.tbTue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbMon
			// 
			this.tbMon.BackColor = System.Drawing.Color.Lavender;
			this.tbMon.Location = new System.Drawing.Point(5, 0);
			this.tbMon.Name = "tbMon";
			this.tbMon.ReadOnly = true;
			this.tbMon.Size = new System.Drawing.Size(90, 20);
			this.tbMon.TabIndex = 0;
			this.tbMon.Text = "Mon";
			this.tbMon.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(40, 592);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 13;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(608, 592);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblTimeSchema
			// 
			this.lblTimeSchema.Location = new System.Drawing.Point(288, 16);
			this.lblTimeSchema.Name = "lblTimeSchema";
			this.lblTimeSchema.Size = new System.Drawing.Size(88, 23);
			this.lblTimeSchema.TabIndex = 6;
			this.lblTimeSchema.Text = "Time Schema:";
			this.lblTimeSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbTimeSchema
			// 
			this.cbTimeSchema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTimeSchema.Location = new System.Drawing.Point(392, 16);
			this.cbTimeSchema.Name = "cbTimeSchema";
			this.cbTimeSchema.Size = new System.Drawing.Size(184, 21);
			this.cbTimeSchema.TabIndex = 7;
			this.cbTimeSchema.SelectedIndexChanged += new System.EventHandler(this.cbTimeSchema_SelectedIndexChanged);
			// 
			// btnAssign
			// 
			this.btnAssign.Location = new System.Drawing.Point(304, 208);
			this.btnAssign.Name = "btnAssign";
			this.btnAssign.TabIndex = 9;
			this.btnAssign.Text = "Assign";
			this.btnAssign.Click += new System.EventHandler(this.btnAssign_Click);
			// 
			// lvTimeSchemaDetails
			// 
			this.lvTimeSchemaDetails.FullRowSelect = true;
			this.lvTimeSchemaDetails.GridLines = true;
			this.lvTimeSchemaDetails.HideSelection = false;
			this.lvTimeSchemaDetails.Location = new System.Drawing.Point(304, 56);
			this.lvTimeSchemaDetails.MultiSelect = false;
			this.lvTimeSchemaDetails.Name = "lvTimeSchemaDetails";
			this.lvTimeSchemaDetails.Size = new System.Drawing.Size(392, 136);
			this.lvTimeSchemaDetails.TabIndex = 8;
			this.lvTimeSchemaDetails.View = System.Windows.Forms.View.Details;
			this.lvTimeSchemaDetails.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvTimeSchemaDetails_ColumnClick);
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(144, 592);
			this.btnClear.Name = "btnClear";
			this.btnClear.TabIndex = 14;
			this.btnClear.Text = "Clear";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// WTWorkingGroupTimeSchedules
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 628);
			this.ControlBox = false;
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.lvTimeSchemaDetails);
			this.Controls.Add(this.btnAssign);
			this.Controls.Add(this.cbTimeSchema);
			this.Controls.Add(this.lblTimeSchema);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.dtpMonth);
			this.Controls.Add(this.lblMonth);
			this.Controls.Add(this.tbDescription);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.tbGroupID);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.lblGroupID);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(720, 655);
			this.MinimumSize = new System.Drawing.Size(720, 655);
			this.Name = "WTWorkingGroupTimeSchedules";
			this.ShowInTaskbar = false;
            this.KeyPreview = true;
			this.Text = "WTEmployeesTimeSchedule";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WTEmployeesTimeSchedule_KeyDown);
			this.Load += new System.EventHandler(this.WTEmployeesTimeSchedule_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WTEmployeesTimeSchedule_KeyUp);
			this.Closed += new System.EventHandler(this.WTEmployeesTimeSchedule_Closed);
			this.panel1.ResumeLayout(false);
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
					case WTWorkingGroupTimeSchedules.DayNum:
					case WTWorkingGroupTimeSchedules.IntervalNum:
					case WTWorkingGroupTimeSchedules.Tolerance:
					{
						return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text.Trim()),Int32.Parse(sub2.Text.Trim()));
					}
					case WTWorkingGroupTimeSchedules.StartTime:
					case WTWorkingGroupTimeSchedules.EndTime:
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
		/// Init Controller and Observer Client
		/// </summary>
		private void IntitObserverClient()
		{
			Controller = NotificationController.GetInstance();
			observerClient = new NotificationObserverClient(this.ToString());
			Controller.AttachToNotifier(observerClient);
			this.observerClient.Notification += new NotificationEventHandler(this.DaySelected);
		}

		/// <summary>
		/// Set proper language and initialize list view of Time Schedule Details.
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("employeesTimeSchedule", culture);

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnAssign.Text = rm.GetString("btnAssign", culture);
				btnClear.Text = rm.GetString("btnClear", culture);

				// label's text
				lblGroupID.Text = rm.GetString("lblGroupID", culture);
				lblName.Text = rm.GetString("lblName", culture);
				lblDescription.Text = rm.GetString("lblDescription", culture);
				lblMonth.Text = rm.GetString("lblMonth", culture);
				lblTimeSchema.Text = rm.GetString("lblSchema", culture);

				// text box text
				tbMon.Text = rm.GetString("Mon", culture);
				tbTue.Text = rm.GetString("Tue", culture);
				tbWed.Text = rm.GetString("Wed", culture);
				tbThr.Text = rm.GetString("Thr", culture);
				tbFri.Text = rm.GetString("Fri", culture);
				tbSat.Text = rm.GetString("Sat", culture);
				tbSun.Text = rm.GetString("Sun", culture);

				// list view
				lvTimeSchemaDetails.BeginUpdate();

				lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrDayNum", culture),  lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
				lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrIntervalNumber", culture),  lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
				lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrSartTime", culture),  lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
				lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrEndTime", culture),  lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);
				lvTimeSchemaDetails.Columns.Add(rm.GetString("hdrTolerance", culture),  lvTimeSchemaDetails.Width / 5, HorizontalAlignment.Center);

				lvTimeSchemaDetails.EndUpdate();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.setLanguage(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Populate Time Schema combo box.
		/// </summary>
		private void populateTimeSchema()
		{
			try
			{
                List<WorkTimeSchemaTO> tsArray = new List<WorkTimeSchemaTO>();

                foreach (int id in schemas.Keys)
                {
                    if (!schemas[id].Status.Equals(Constants.statusRetired))
                    {

                        WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                        sch.Name = schemas[id].TimeSchemaID + " - " + schemas[id].Name;
                        sch.TimeSchemaID = schemas[id].TimeSchemaID;

                        tsArray.Add(sch);
                    }
                }

                WorkTimeSchemaTO ts = new WorkTimeSchemaTO();
                ts.Name = rm.GetString("all", culture);
                tsArray.Insert(0, ts);

                cbTimeSchema.DataSource = tsArray;
                cbTimeSchema.DisplayMember = "Name";
                cbTimeSchema.ValueMember = "TimeSchemaID";
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.populateTimeSchema(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Draw calendar for the selected month and year.
		/// </summary>
		/// <param name="month"></param>
		private void setCalendar(DateTime month)
		{
			try
			{
				int currentMonth = month.Month;
				int width = this.tbMon.Width - Constants.calendarHSpace;
				int height = this.tbMon.Height * 2;
                int startXPos = Constants.calendarLeft;
                int startYPos = this.tbMon.Height + Constants.calendarTop;
				int xPos;
				int yPos;
				int row = 0;

                DateTime date = new DateTime(month.Year, month.Month, 1);
                DateTime endDate = Common.Misc.getWeekEnding(new DateTime(month.Year, month.Month, 1).AddMonths(1).AddDays(-1));
				
				// set to default values
                this.panel1.Controls.Clear();
				this.calendarDays.Clear();
				this.selectedDays.Clear();
                this.selectedDay = new DateTime();
                this.minSelected = date.AddMonths(2);
                this.maxSelected = date.AddMonths(-1);

				addCalendarHeader();

                while (date.Date <= endDate.Date)
                {
                    xPos = startXPos + getDayOfWeek(date) * (width + Constants.calendarHSpace);
                    yPos = startYPos + row * (height + Constants.calendarVSpace);

                    DayOfCalendar dayOfCalendar = new DayOfCalendar(xPos, yPos, -1, "", date.Day.ToString(), -1, "");
                    dayOfCalendar.Date = new DateTime(date.Year, date.Month, date.Day);
                    dayOfCalendar.Schema = new WorkTimeSchemaTO();
                    dayOfCalendar.StartDay = -1;

                    this.panel1.Controls.Add(dayOfCalendar);
                    this.calendarDays.Add(dayOfCalendar.Date.Date, dayOfCalendar);

                    // if next day is Monday, drow in next row
                    if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        row++;
                    }

                    date = date.AddDays(1);
                }

                populateTimeSchedule(Group.EmployeeGroupID, dtpMonth.Value);
				setNotWorkingDays(dtpMonth.Value);

				this.panel1.Invalidate();
				this.Invalidate();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.setCalendar(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Returns day of week.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		private int getDayOfWeek(DateTime date)
		{
			try
			{
				switch (date.DayOfWeek)
				{
					case DayOfWeek.Monday:
						return 0;
						//break;
					case DayOfWeek.Tuesday:
						return 1;
						//break;
					case DayOfWeek.Wednesday:
						return 2;
						//break;
					case DayOfWeek.Thursday:
						return 3;
						//break;
					case DayOfWeek.Friday:
						return 4;
						//break;
					case DayOfWeek.Saturday:
						return 5;
						//break;
					case DayOfWeek.Sunday:
						return 6;
						//break;
					default:
						return 0;
						//break;
				}
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.getDayOfWeek(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Add header to the calendar.
		/// </summary>
		private void addCalendarHeader()
		{
			this.panel1.Controls.Add(tbMon);
			this.panel1.Controls.Add(tbTue);
			this.panel1.Controls.Add(tbWed);
			this.panel1.Controls.Add(tbThr);
			this.panel1.Controls.Add(tbFri);
			this.panel1.Controls.Add(tbSat);
			this.panel1.Controls.Add(tbSun);
		}

		/// <summary>
		/// When calendar value is changed, calendar for selected month is drawn.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dtpMonth_ValueChanged(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DateTime lastMonth = DateTime.Now.Date.AddMonths(-1);
                DateTime firstLastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                
                if (this.dtpMonth.Value < firstLastMonth.Date)
                {
                    this.dtpMonth.Value = DateTime.Now.Date;
                    MessageBox.Show(rm.GetString("minSelectDate", culture));
                }
                else
                {
                    Employee emplObj = new Employee();                    
                    emplObj.EmplTO.WorkingGroupID = Group.EmployeeGroupID;
                    List<EmployeeTO> employeeList = emplObj.Search();

                    // get dictionary of all rules, key is company and value are rules by employee type id
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule().SearchWUEmplTypeDictionary();
                    Dictionary<int, WorkingUnitTO> wUnits = new WorkingUnit().getWUDictionary();

                    foreach (EmployeeTO empl in employeeList)
                    {
                        int cutOffDate = -1;
                        int emplCompany = Common.Misc.getRootWorkingUnit(empl.WorkingUnitID, wUnits);

                        if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(empl.EmployeeTypeID) && emplRules[emplCompany][empl.EmployeeTypeID].ContainsKey(Constants.RuleHRSSCCutOffDate))
                            cutOffDate = emplRules[emplCompany][empl.EmployeeTypeID][Constants.RuleHRSSCCutOffDate].RuleValue;

                        if (cutOffDate != -1 && Common.Misc.countWorkingDays(DateTime.Now.Date, null) > cutOffDate && this.dtpMonth.Value < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                        {
                            this.dtpMonth.Value = DateTime.Now.Date;
                            MessageBox.Show(rm.GetString("cutOffDayPessed", culture));
                            break;
                        }
                    }
                }

                datesList = new List<DateTime>();
                setCalendar(this.dtpMonth.Value);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.dtpMonth_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;

            }
		}

		/// <summary>
		/// When selected Time Schema is changed, List view is populated with details of selected schema.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbTimeSchema_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
            {
                if (cbTimeSchema.SelectedIndex > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                    if (schemas.ContainsKey((int)cbTimeSchema.SelectedValue))
                        sch = schemas[(int)cbTimeSchema.SelectedValue];

                    populateListView(sch.Days);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.cbTimeSchema_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void populateListView (Dictionary<int, Dictionary<int, WorkTimeIntervalTO>> days)
		{
			try
			{
				lvTimeSchemaDetails.BeginUpdate();
				lvTimeSchemaDetails.Items.Clear();

                WorkTimeIntervalTO interval = new WorkTimeIntervalTO();

                Dictionary<int, WorkTimeIntervalTO> intervals = new Dictionary<int,WorkTimeIntervalTO>();

				foreach(int dayKey in days.Keys)
				{
					intervals = days[dayKey];

					foreach(int intKey in intervals.Keys)
					{
						interval = intervals[intKey];

						ListViewItem item = new ListViewItem();
						item.Text = (interval.DayNum + 1).ToString();

						item.SubItems.Add((interval.IntervalNum + 1).ToString());
						item.SubItems.Add(interval.StartTime.ToString("HH:mm"));
						item.SubItems.Add(interval.EndTime.ToString("HH:mm"));

						// Check tolerance
						TimeSpan ts0  = new TimeSpan();
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

						lvTimeSchemaDetails.Items.Add(item);
					}
				}

                _comp = new ListViewItemComparer(lvTimeSchemaDetails);
                lvTimeSchemaDetails.ListViewItemSorter = _comp;
                lvTimeSchemaDetails.Sorting = SortOrder.Ascending;

				lvTimeSchemaDetails.EndUpdate();
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.populateListView(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Assigning selected Schema and appropriate days of schema to selected days of calendar.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAssign_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (cbTimeSchema.SelectedIndex <= 0)
                {
                    MessageBox.Show(rm.GetString("selectSchema", culture));
                }
                else if (lvTimeSchemaDetails.SelectedItems.Count != 1)
                {
                    MessageBox.Show(rm.GetString("selectOneDay", culture));
                }
                else
                {
                    int day = ((WorkTimeIntervalTO)lvTimeSchemaDetails.SelectedItems[0].Tag).DayNum + 1;
                    if (selectedDays.Count == 0)
                    {
                        MessageBox.Show(rm.GetString("selectOneDayCalendar", culture));
                    }
                    else
                    {
                        DateTime startDate = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);

                        for (int i = 0; i < selectedDays.Count; i++)
                        {
                            if (calendarDays.ContainsKey(selectedDays[i].Date))
                            {
                                datesList.Add(calendarDays[selectedDays[i].Date].Date);
                                WorkTimeSchemaTO sch = new WorkTimeSchemaTO();
                                if (schemas.ContainsKey((int)cbTimeSchema.SelectedValue))
                                    sch = schemas[(int)cbTimeSchema.SelectedValue];
                                calendarDays[selectedDays[i].Date].Schema = sch;
                                calendarDays[selectedDays[i].Date].StartDay = day - 1;
                                calendarDays[selectedDays[i].Date].setSchemaText(rm.GetString("Schema", culture) + ": " + calendarDays[selectedDays[i].Date].Schema.TimeSchemaID.ToString());
                                calendarDays[selectedDays[i].Date].setCycleDayText(rm.GetString("SchemaDay", culture) + ": " + (calendarDays[selectedDays[i].Date].StartDay + 1).ToString());
                                day = (day == calendarDays[selectedDays[i].Date].Schema.CycleDuration ? 1 : day + 1);
                                calendarDays[selectedDays[i].Date].Invalidate();
                            }
                        }

                        //for (int i = (int)selectedDays[0]; i <= (int)selectedDays[selectedDays.Count - 1]; i++)
                        //{
                        //    datesList.Add(calendarDays[i - 1].Date);
                        //    TimeSchema sch = new TimeSchema();
                        //    sch.TimeSchemaTO.TimeSchemaID = (int)cbTimeSchema.SelectedValue;
                        //    calendarDays[i - 1].Schema = sch.Search()[0];
                        //    calendarDays[i - 1].StartDay = day - 1;
                        //    calendarDays[i - 1].setSchemaText(rm.GetString("Schema", culture) + ": " + calendarDays[i - 1].Schema.TimeSchemaID.ToString());
                        //    calendarDays[i - 1].setCycleDayText(rm.GetString("SchemaDay", culture) + ": " + (calendarDays[i - 1].StartDay + 1).ToString());
                        //    day = (day == calendarDays[i - 1].Schema.CycleDuration ? 1 : day + 1);
                        //    calendarDays[i - 1].Invalidate();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.btnAssign_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void WTEmployeesTimeSchedule_Closed(object sender, System.EventArgs e)
		{
			Controller.DettachFromNotifier(this.observerClient);
		}

		/// <summary>
		/// Method that implements selecting of event which will be rised.
		/// </summary>
		public void DaySelected(object sender, NotificationEventArgs e)
		{
			try
			{
                if (!e.daySelected.Equals(new DateTime()))
                {
                    selectedDays.Clear();
                    if (!shiftStatus)
                    {
                        selectedDay = new DateTime();
                        minSelected = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1, 0, 0, 0).AddMonths(2);
                        maxSelected = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1, 0, 0, 0).AddMonths(-1);
                        UnselectDays(e.daySelected);
                    }

                    selectedDay = e.daySelected;
                    if (selectedDay.Date < minSelected.Date)
                    {
                        minSelected = selectedDay.Date;
                    }
                    if (selectedDay.Date > maxSelected.Date)
                    {
                        maxSelected = selectedDay.Date;
                    }

                    if (this.shiftStatus)
                    {
                        DateTime currDate = minSelected.Date;
                        while (currDate.Date <= maxSelected.Date)
                        {
                            selectedDays.Add(currDate.Date);
                            if (calendarDays.ContainsKey(currDate.Date))
                                calendarDays[currDate.Date].SelectDay();
                            currDate = currDate.AddDays(1);
                        }
                    }
                    else
                    {
                        selectedDays.Add(selectedDay);
                    }
                }
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.DaySelected(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		public void UnselectDays(DateTime selectedDay)
		{
            foreach (DateTime day in this.calendarDays.Keys)
            {
                if (!selectedDays.Contains(day.Date) && !day.Date.Equals(selectedDay))
                {
                    calendarDays[day].UnselectDay();
                }
            }
		}

		private void WTEmployeesTimeSchedule_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// if Shift is pressed
			if (e.KeyValue == 16)
			{
				this.shiftStatus = true;
			}
		}

		private void WTEmployeesTimeSchedule_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // if Shift is released
                if (e.KeyValue == 16)
                {
                    this.shiftStatus = false;
                }

                if (e.KeyCode.Equals(Keys.F1))
                {
                    Util.Misc.helpManualHtml(this.Name);
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.WTEmployeesTimeSchedule_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void WTEmployeesTimeSchedule_Load(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                _comp = new ListViewItemComparer(lvTimeSchemaDetails);
                lvTimeSchemaDetails.ListViewItemSorter = _comp;
                lvTimeSchemaDetails.Sorting = SortOrder.Ascending;

                schemas = new TimeSchema().getDictionary();
                populateTimeSchema();
                setCalendar(this.dtpMonth.Value);

                DateTime date = DateTime.Today;
                this.dtpMonth.Value = new DateTime(date.Year, date.Month, 1);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.WTEmployeesTimeSchedule_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void lvTimeSchemaDetails_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SortOrder prevOrder = lvTimeSchemaDetails.Sorting;
                lvTimeSchemaDetails.Sorting = SortOrder.None;

                if (e.Column == _comp.SortColumn)
                {
                    if (prevOrder == SortOrder.Ascending)
                    {
                        lvTimeSchemaDetails.Sorting = SortOrder.Descending;
                    }
                    else
                    {
                        lvTimeSchemaDetails.Sorting = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New Sort Order
                    _comp.SortColumn = e.Column;
                    lvTimeSchemaDetails.Sorting = SortOrder.Ascending;
                } 
                lvTimeSchemaDetails.ListViewItemSorter = _comp;
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.lvTimeSchemaDetails_ColumnClick(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            EmployeeGroupsTimeSchedule emplGroupsTimeSchedule = new EmployeeGroupsTimeSchedule();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool assignStatus = true;                
                DateTime minDate = new DateTime();
                DateTime maxDate = new DateTime();
                foreach (DateTime calDate in calendarDays.Keys)
                {
                    if (calendarDays[calDate].Schema.TimeSchemaID == -1)
                    {
                        assignStatus = false;
                        MessageBox.Show(rm.GetString("assignEachDay", culture));
                        break;
                    }
                    else
                    {
                        if (minDate.Equals(new DateTime()))
                            minDate = calDate.Date;
                        else if (calDate.Date < minDate.Date)
                            minDate = calDate.Date;
                        if (calDate.Date > maxDate.Date)
                            maxDate = calDate.Date;
                    }
                }
                                
                if (assignStatus)
                {
                    DateTime month = dtpMonth.Value;
                    DateTime startDate = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
                    DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));
                    //DateTime grpToDate = (new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1)).AddDays(-1);
                    List<EmployeeGroupsTimeScheduleTO> nextGrpSchedule = emplGroupsTimeSchedule.SearchGroupsNextSchedule(Group.EmployeeGroupID.ToString().Trim(), endDate.Date);

                    if (nextGrpSchedule.Count > 0)                    
                        endDate = nextGrpSchedule[0].Date.AddDays(-1).Date;                    
                    else                    
                        endDate = new DateTime();                    

                    StringBuilder message = new StringBuilder();
                    message.Append(rm.GetString("grpScheduleAdd1", culture) + " " +
                        month.ToString("dd.MM.yyyy"));
                    if (endDate != new DateTime(0))
                        message.Append(" " + rm.GetString("grpScheduleAdd2", culture) + " " +
                            endDate.Date.ToString("dd.MM.yyyy") + ". \n");
                    else
                        message.Append(" " + rm.GetString("grpScheduleAdd3", culture) + "\n");

                    if ((endDate == new DateTime(0)) ||
                        ((month.Year < endDate.Year) ||
                        ((endDate.Year == endDate.Year) && (month.Month < endDate.Month))))
                        message.Append(rm.GetString("grpScheduleAdd4", culture) + "\n");
                    message.Append(rm.GetString("grpScheduleAdd5", culture) + "\n");
                    message.Append(rm.GetString("grpScheduleAdd6", culture) + "\n");

                    DialogResult result = MessageBox.Show(message.ToString(), "", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        return;
                    }

                    /* 2008-03-14
                     * Now, there is employee_groups_time_schedule table, so, we need to enter values in it.*/

                    Employee empl = new Employee();
                    empl.EmplTO.WorkingGroupID = Group.EmployeeGroupID;
                    List<EmployeeTO> employeeList = empl.Search();

                    EmployeesTimeSchedule emplTimeSchedule = new EmployeesTimeSchedule();

                    Dictionary<int, EmployeeTO> emplDict = new Dictionary<int, EmployeeTO>();
                    Dictionary<int, EmployeeAsco4TO> ascoDict = new Dictionary<int, EmployeeAsco4TO>();
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Common.Rule().SearchWUEmplTypeDictionary();

                    string pauseEmployees = "";
                    foreach (EmployeeTO employee in employeeList)
                    {
                        pauseEmployees += employee.EmployeeID.ToString().Trim() + ",";
                    }
                    if (pauseEmployees.Length > 0)
                    {
                        pauseEmployees = pauseEmployees.Substring(0, pauseEmployees.Length - 1);
                        emplDict = new Employee().SearchDictionary(pauseEmployees);
                        ascoDict = new EmployeeAsco4().SearchDictionary(pauseEmployees);
                    }

                    bool trans = emplGroupsTimeSchedule.BeginTransaction();
                    if (trans)
                    {
                        bool successful = true;
                        //successful = emplGroupsTimeSchedule.DeleteMonthSchedule(Int32.Parse(this.tbGroupID.Text), dtpMonth.Value, false) && successful;
                        successful = emplGroupsTimeSchedule.DeleteSchedule(Group.EmployeeGroupID, startDate.Date, endDate.Date, false) && successful;

                        int groupPrevSchemaID = -1;
                        int groupExpectedStartDay = -1;                        

                        if (successful)
                        {
                            foreach (DateTime day in calendarDays.Keys)
                            {
                                // if schema is different, or if schema is the same the same but cycle day is not the expected one
                                if ((calendarDays[day].Schema.TimeSchemaID != groupPrevSchemaID) ||
                                    ((calendarDays[day].Schema.TimeSchemaID == groupPrevSchemaID) && (calendarDays[day].StartDay != groupExpectedStartDay)))
                                {
                                    successful = (emplGroupsTimeSchedule.Save(Group.EmployeeGroupID, calendarDays[day].Date, calendarDays[day].Schema.TimeSchemaID, calendarDays[day].StartDay, false) > 0 ? true : false) && successful;

                                    groupPrevSchemaID = calendarDays[day].Schema.TimeSchemaID;
                                }

                                groupExpectedStartDay = (calendarDays[day].StartDay == calendarDays[day].Schema.CycleDuration - 1 ? 0 : calendarDays[day].StartDay + 1);
                            }

                            /* 2008-03-14
                             * update absences and pauses for retired also */
                            //string[] statuses = {Constants.statusActive, Constants.statusBlocked}; 
                            //ArrayList employeeList = new Employee().SearchWithStatuses("", "", "", "", statuses, "", "", "", tbGroupID.Text.Trim(), "");
                            //Employee empl = new Employee();
                            //empl.EmplTO.WorkingGroupID = Group.EmployeeGroupID;
                            //List<EmployeeTO> employeeList = empl.Search(emplGroupsTimeSchedule.GetTransaction());

                            //EmployeesTimeSchedule emplTimeSchedule = new EmployeesTimeSchedule();
                                                        
                            //foreach (EmployeeTO employee in employeeList)
                            //{
                            //    pauseEmployees += employee.EmployeeID.ToString().Trim() + ",";
                            //}
                            //if (pauseEmployees.Length > 0)
                            //{
                            //    pauseEmployees = pauseEmployees.Substring(0, pauseEmployees.Length - 1);
                            //}
                            /*ArrayList nextSchedule = new ArrayList();
                            DateTime toDate = (new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1)).AddDays(-1);
                            if (!pauseEmployees.Equals(""))
                            {
                                nextSchedule = emplTimeSchedule.SearchEmployeesNextSchedule(pauseEmployees, toDate);
                            }

                            Hashtable employeesSchedules = new Hashtable();
                            foreach (EmployeesTimeSchedule employeesTimeSchedule in nextSchedule)
                            {
                                employeesSchedules.Add(employeesTimeSchedule.EmployeeID, employeesTimeSchedule.Date);
                            }*/

                            DateTime maxToDate = new DateTime();
                            if (endDate == new DateTime(0))
                                maxToDate = DateTime.Now.Date;
                            else
                            {
                                maxToDate = endDate;
                                if (maxToDate.Date > DateTime.Now.Date)
                                    maxToDate = DateTime.Now.Date;
                            }

                            foreach (EmployeeTO employee in employeeList)
                            {
                                //emplTimeSchedule.DeleteMonthSchedule(employee.EmployeeID, dtpMonth.Value);
                                emplTimeSchedule.SetTransaction(emplGroupsTimeSchedule.GetTransaction());
                                successful = emplTimeSchedule.DeleteFromToSchedule(employee.EmployeeID, startDate.Date, endDate.Date, "", false) && successful;

                                if (!successful)
                                    break;

                                int prevSchemaID = -1;
                                int expectedStartDay = -1;

                                foreach (DateTime day in calendarDays.Keys)
                                {
                                    // if schema is different, or if schema is the same the same but cycle day is not the expected one
                                    if ((calendarDays[day].Schema.TimeSchemaID != prevSchemaID) ||
                                        ((calendarDays[day].Schema.TimeSchemaID == prevSchemaID) && (calendarDays[day].StartDay != expectedStartDay)))
                                    {
                                        successful = (emplTimeSchedule.Save(employee.EmployeeID, calendarDays[day].Date, calendarDays[day].Schema.TimeSchemaID, calendarDays[day].StartDay, "", false) > 0 ? true : false) && successful;

                                        prevSchemaID = calendarDays[day].Schema.TimeSchemaID;
                                    }

                                    expectedStartDay = (calendarDays[day].StartDay == calendarDays[day].Schema.CycleDuration - 1 ? 0 : calendarDays[day].StartDay + 1);

                                    if (!successful)
                                        break;
                                }

                                if (!successful)
                                    break;

                                /* 2008-03-14
                                 * From now one, take the last existing time schedule, don't expect that every month has 
                                 * time schedule
                                 * Get max date for pause recalculation according to that.*/
                                /*DateTime end = new DateTime(0);
                                if (employeesSchedules.ContainsKey(employee.EmployeeID))
                                {
                                    toDate = ((DateTime)employeesSchedules[employee.EmployeeID]).Date.AddDays(-1).Date;
                                    if (toDate.Date > DateTime.Now.Date)
                                        toDate = DateTime.Now.Date;

                                    end = ((DateTime)employeesSchedules[employee.EmployeeID]).Date.AddDays(-1).Date;
                                }
                                else
                                {
                                    toDate = DateTime.Now.Date;

                                    end = new DateTime(0);
                                }
                                if ((maxToDate == new DateTime()) || (maxToDate.Date < toDate.Date))
                                {
                                    maxToDate = toDate.Date;
                                }*/

                                /* 2008-03-14 */
                                // delete absences pairs for that month and update absences to unused
                                //deleteIOPUpdateEA(employee.EmployeeID, end);
                                deleteIOPUpdateEA(employee.EmployeeID, endDate.Date, emplGroupsTimeSchedule.GetTransaction());
                            }

                            if (successful)
                            {
                                /* 2008-03-14 */
                                // delete absences pairs for that month and update absences to unused
                                //deleteIOPUpdateEA();

                                if (!pauseEmployees.Equals(""))
                                {
                                    IOPair ioPair = new IOPair();

                                    if ((month.Year < DateTime.Now.Year) ||
                                        ((month.Year == DateTime.Now.Year) && (month.Month <= DateTime.Now.Month)))
                                    {
                                        //ioPair.recalculatePause(pauseEmployees, new DateTime(month.Year, month.Month, 1), (new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1)).AddDays(-1));
                                        ioPair.recalculatePause(pauseEmployees, new DateTime(month.Year, month.Month, 1), maxToDate, emplGroupsTimeSchedule.GetTransaction());
                                    }
                                }
                            }
                        }

                        if (successful)
                        {
                            // validate new employee schedule
                            bool validFundHrs = true;
                            DateTime invalidDate = Common.Misc.isValidTimeSchedule(emplDict, ascoDict, rulesDict, pauseEmployees.Trim(), minDate.Date, 
                                maxDate.Date, emplGroupsTimeSchedule.GetTransaction(), null, false, ref validFundHrs, true);
                            if (invalidDate.Equals(new DateTime()))
                            {
                                emplGroupsTimeSchedule.CommitTransaction();
                                MessageBox.Show(rm.GetString("grpScheduleSaved", culture));
                                this.Close();
                            }
                            else
                            {
                                emplGroupsTimeSchedule.RollbackTransaction();
                                if (validFundHrs)
                                    MessageBox.Show(rm.GetString("notValidScheduleAssigned", culture)
                                        + " " + invalidDate.Date.AddDays(-1).ToString(Constants.dateFormat) + "/" + invalidDate.Date.ToString(Constants.dateFormat));
                                else
                                    MessageBox.Show(rm.GetString("notValidFundHrs", culture), " " + invalidDate.Date.ToString(Constants.dateFormat) + "-"
                                        + invalidDate.AddDays(6).Date.ToString(Constants.dateFormat));
                            }
                        }
                        else
                        {
                            emplGroupsTimeSchedule.RollbackTransaction();
                            MessageBox.Show(rm.GetString("grpScheduleNotSaved", culture));
                        }
                    } // if (trans)                    
                } //if(assignStatus)
            }
            catch (Exception ex)
            {
                if (emplGroupsTimeSchedule.GetTransaction() != null)
                {
                    emplGroupsTimeSchedule.RollbackTransaction();
                }
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        /* 2008-03-14
         * Now, there is employee_groups_time_schedule table, so, we have initial values from it and 
         * time schedule can be populated for each month*/
        private void populateTimeSchedule(int groupID, DateTime month)
        {
            try
            {
                DateTime startDate = new DateTime(month.Year, month.Month, 1);
                DateTime endDate = Common.Misc.getWeekEnding(new DateTime(month.Year, month.Month, 1).AddMonths(1).AddDays(-1));

                List<EmployeeGroupsTimeScheduleTO> timeSchedule = new EmployeeGroupsTimeSchedule().SearchGroupsSchedules(groupID.ToString().Trim(), startDate.Date, endDate.Date, null);

                if (timeSchedule.Count > 0)
                {
                    for (int scheduleIndex = 0; scheduleIndex < timeSchedule.Count; scheduleIndex++)
                    {
                        int cycleDuration = 0;
                        int day = timeSchedule[scheduleIndex].StartCycleDay + 1;
                        int schemaID = timeSchedule[scheduleIndex].TimeSchemaID;

                        WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
                        if (schemas.ContainsKey(schemaID))
                        {
                            schema = schemas[schemaID];
                            cycleDuration = schema.CycleDuration;
                        }

                        DateTime i = timeSchedule[scheduleIndex].Date.Date;
                        if ((timeSchedule[scheduleIndex].Date.Year < month.Year) || ((timeSchedule[scheduleIndex].Date.Year == month.Year) && (timeSchedule[scheduleIndex].Date.Month < month.Month)))
                        {
                            i = startDate.Date;

                            TimeSpan ts = new TimeSpan((new DateTime(month.Year, month.Month, 1)).Date.Ticks - timeSchedule[scheduleIndex].Date.Date.Ticks);
                            day = ((timeSchedule[scheduleIndex].StartCycleDay + (int)ts.TotalDays) % cycleDuration) + 1;
                        }

                        //for(int i = ((EmployeesTimeSchedule) (timeSchedule[scheduleIndex])).Date.Day - 1;                        
                        for (; i < ((scheduleIndex + 1 < timeSchedule.Count) ? timeSchedule[scheduleIndex + 1].Date.Date : endDate.AddDays(1).Date); i = i.AddDays(1))
                        {
                            if (calendarDays.ContainsKey(i.Date))
                            {
                                calendarDays[i.Date].Schema = schema;
                                calendarDays[i.Date].StartDay = day - 1;
                                calendarDays[i.Date].setSchemaText(rm.GetString("Schema", culture) + ": " + schemaID.ToString().Trim());
                                calendarDays[i.Date].setCycleDayText(rm.GetString("SchemaDay", culture) + ": " + day.ToString().Trim());
                                day = (day == cycleDuration ? 1 : day + 1);
                            }
                        }

                        //int i = timeSchedule[scheduleIndex].Date.Day - 1;
                        //if (
                        //    (timeSchedule[scheduleIndex].Date.Year < month.Year)
                        //    ||
                        //    (
                        //        (timeSchedule[scheduleIndex].Date.Year == month.Year)
                        //        && (timeSchedule[scheduleIndex].Date.Month < month.Month)
                        //    )
                        //   )
                        //{
                        //    i = 0;

                        //    TimeSpan ts = new TimeSpan((new DateTime(month.Year, month.Month, 1)).Date.Ticks - timeSchedule[scheduleIndex].Date.Date.Ticks);
                        //    day = ((timeSchedule[scheduleIndex].StartCycleDay + (int)ts.TotalDays) % cycleDuration) + 1;
                        //}
                        //for (;
                        //    i < ((scheduleIndex + 1 < timeSchema.Count) ? timeSchedule[scheduleIndex + 1].Date.Day : calendarDays.Count); i++)
                        //{
                        //    calendarDays[i].Schema = schema;
                        //    calendarDays[i].StartDay = day - 1;
                        //    calendarDays[i].setSchemaText(rm.GetString("Schema", culture) + ": " + schemaID.ToString().Trim());
                        //    calendarDays[i].setCycleDayText(rm.GetString("SchemaDay", culture) + ": " + day.ToString().Trim());
                        //    day = (day == cycleDuration ? 1 : day + 1);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.populateTimeSchedule(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
        }

		private void setNotWorkingDays(DateTime month)
		{
            DateTime startDate = Common.Misc.getWeekBeggining(new DateTime(month.Year, month.Month, 1));
            DateTime endDate = Common.Misc.getWeekEnding(new DateTime(month.Year, month.Month, 1).AddMonths(1).AddDays(-1));

            List<HolidayTO> holidays = new Holiday().Search(startDate.Date, endDate.Date);
            List<HolidaysExtendedTO> holidaysExt = new HolidaysExtended().Search(startDate.Date, endDate.Date);
            List<DateTime> holDates = new List<DateTime>();

            foreach (HolidayTO hol in holidays)
            {
                if (!holDates.Contains(hol.HolidayDate.Date))
                    holDates.Add(hol.HolidayDate.Date);
            }

            foreach (HolidaysExtendedTO hol in holidaysExt)
            {
                DateTime holDate = hol.DateStart.Date;
                while (holDate.Date <= hol.DateEnd.Date)
                {
                    if (!holDates.Contains(holDate.Date))
                        holDates.Add(holDate.Date);

                    holDate = holDate.AddDays(1);
                }
            }

            foreach (DateTime day in calendarDays.Keys)
            {
                if (calendarDays[day].Date.Month != dtpMonth.Value.Month)
                    calendarDays[day].SetOtherMonthDay();

                if ((calendarDays[day].Date.DayOfWeek.Equals(DayOfWeek.Saturday)) ||
                    (calendarDays[day].Date.DayOfWeek.Equals(DayOfWeek.Sunday)) ||
                    (holDates.Contains(calendarDays[day].Date.Date)))
                {
                    if (calendarDays[day].Date.Month != dtpMonth.Value.Month)
                        calendarDays[day].SetNotWorkingDayOtherMonth();
                    else
                        calendarDays[day].SetNotWorkingDay();
                }
                if (Common.Misc.isLockedDate(calendarDays[day].Date))
                {
                    if (calendarDays[day].Date.Month != dtpMonth.Value.Month)
                        calendarDays[day].SetLockedDayOtherMonth();
                    else
                        calendarDays[day].SetLockedDay();
                }
            }
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
            foreach (DateTime day in calendarDays.Keys)
            {
                calendarDays[day].Schema = new WorkTimeSchemaTO();
                calendarDays[day].StartDay = -1;
                calendarDays[day].setSchemaText("");
                calendarDays[day].setCycleDayText("");
            }
		}

		// delete absences pairs for that month and update absences to unused
		/*private void deleteIOPUpdateEA()
		{
			try
			{
				ArrayList employees = new Employee().Search("", "", "", "", "", "", "", "", tbGroupID.Text.Trim(), "");

				foreach (Employee empl in employees)
				{
					DateTime date = dtpMonth.Value;
					DateTime start = new DateTime(date.Year, date.Month, 1);
					DateTime end = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1).AddDays(-1);
                    ArrayList nextSchedule = new EmployeesTimeSchedule().SearchEmployeesNextSchedule(empl.EmployeeID.ToString(), end);
                    if (nextSchedule.Count > 0)
                    {
                        end = ((EmployeesTimeSchedule)nextSchedule[0]).Date.AddDays(-1).Date;
                    }
                    else
                    {
                        end = new DateTime(0);
                    }

					ArrayList emplAbsences = new EmployeeAbsence().Search(empl.EmployeeID.ToString().Trim(), "", start,
						end, "", "");

					foreach (EmployeeAbsence abs in emplAbsences)
					{
						new EmployeeAbsence().UpdateEADeleteIOP(abs.RecID, abs.EmployeeID, abs.PassTypeID,
							abs.PassTypeID, abs.DateStart, abs.DateEnd, abs.DateStart, abs.DateEnd, (int) Constants.Used.No);
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.deleteIOPUpdateEA(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}*/

        /* 2008-03-14 */
        // delete absences pairs for that month and update absences to unused

        private void deleteIOPUpdateEA(int EmployeeID, DateTime end, IDbTransaction trans)
        {
            try
            {
                DateTime dateValue = dtpMonth.Value;
                DateTime start = new DateTime(dateValue.Year, dateValue.Month, 1);
                
                #region Reprocess Dates

                DateTime endDate = end;
                if (end == new DateTime())
                {
                    endDate = new IOPairProcessed().getMaxDateOfPair(EmployeeID.ToString(), trans);

                    if (endDate.Date == new DateTime())
                        endDate = DateTime.Now.Date;
                }
                //list od datetime for each employee
                Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();

                emplDateWholeDayList.Add(EmployeeID, datesList);
                if (datesList.Count > 0)
                    Common.Misc.ReprocessPairsAndRecalculateCounters(EmployeeID.ToString(), start, endDate, trans, emplDateWholeDayList, null, "");
  
                #endregion

                EmployeeAbsence ea = new EmployeeAbsence();
                ea.EmplAbsTO.EmployeeID = EmployeeID;
                ea.EmplAbsTO.DateStart = start;
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
                log.writeLog(DateTime.Now + " WTWorkingGroupTimeSchedules.deleteIOPUpdateEA(): " + ex.Message + "\n");
                throw new Exception(ex.Message);
            }
        }
	}
}
