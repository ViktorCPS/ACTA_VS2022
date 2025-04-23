using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Data;
using Common;
using TransferObjects;

using System.Resources;
using System.Globalization;
using Util;

namespace UI
{
	/// <summary>
	/// Summary description for WTEmployeesTimeSchedule.
	/// </summary>
	public class WTEmployeesTimeSchedule : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblEmployeeID;
		private System.Windows.Forms.TextBox tbEmployeeID;
		private System.Windows.Forms.Label lblFirstName;
		private System.Windows.Forms.TextBox tbFirstName;
		private System.Windows.Forms.Label lblLastName;
		private System.Windows.Forms.TextBox tbLastName;
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
        private System.Windows.Forms.Label lblTimeSchema;
        private System.Windows.Forms.ListView lvTimeSchemaDetails;
        private System.Windows.Forms.ComboBox cbTimeSchema;
        private System.Windows.Forms.TextBox tbSun;
        private System.Windows.Forms.Button btnClear;
        private Label lblMinDate;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private CultureInfo culture;
		ResourceManager rm;
		ApplUserTO logInUser;
		
		// Controller instance
		public NotificationController Controller;
		
		// Observer client instance
		public NotificationObserverClient observerClient;		
        
		DebugLog log;

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

        List<DateTime> datesChanged = new List<DateTime>();

        Dictionary<int, WorkTimeSchemaTO> schemas = new Dictionary<int, WorkTimeSchemaTO>();
        private CheckBox checkBoxAll;
        private CheckBox checkBoxContinueSchedule;
        EmployeeTO Empl = new EmployeeTO();

        //NATALIJA08112017
        int OldGroupID = -1;
        bool chbFirstChange = false;

		public WTEmployeesTimeSchedule(EmployeeTO empl)
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				IntitObserverClient();

				calendarDays = new Dictionary<DateTime,DayOfCalendar>();
				selectedDays = new List<DateTime>();

				this.CenterToScreen();
				logInUser = NotificationController.GetLogInUser();

				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
				rm = new ResourceManager("UI.Resource",typeof(WTEmployeesTimeSchedule).Assembly);
				setLanguage();
                
                this.Empl = empl;

				this.tbEmployeeID.Text = this.Empl.EmployeeID.ToString().Trim();
				this.tbFirstName.Text = this.Empl.FirstName.Trim();
				this.tbLastName.Text = this.Empl.LastName.Trim();
                
                lblMinDate.Visible = false;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

		}

        //natalija08112017
        public WTEmployeesTimeSchedule(EmployeeTO empl, int oldGroupID)
        {
            try
            {
                InitializeComponent();

                string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
                log = new DebugLog(logFilePath);

                IntitObserverClient();

                calendarDays = new Dictionary<DateTime, DayOfCalendar>();
                selectedDays = new List<DateTime>();

                this.CenterToScreen();
                logInUser = NotificationController.GetLogInUser();

                culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
                rm = new ResourceManager("UI.Resource", typeof(WTEmployeesTimeSchedule).Assembly);
                setLanguage();

                this.Empl = empl;

                this.tbEmployeeID.Text = this.Empl.EmployeeID.ToString().Trim();
                this.tbFirstName.Text = this.Empl.FirstName.Trim();
                this.tbLastName.Text = this.Empl.LastName.Trim();

                lblMinDate.Visible = false;

                this.OldGroupID = oldGroupID;//natalija
                this.chbFirstChange = true;

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule(): " + ex.Message + "\n");
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
            this.lblEmployeeID = new System.Windows.Forms.Label();
            this.tbEmployeeID = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.tbLastName = new System.Windows.Forms.TextBox();
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
            this.lblMinDate = new System.Windows.Forms.Label();
            this.checkBoxAll = new System.Windows.Forms.CheckBox();
            this.checkBoxContinueSchedule = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEmployeeID
            // 
            this.lblEmployeeID.Location = new System.Drawing.Point(8, 64);
            this.lblEmployeeID.Name = "lblEmployeeID";
            this.lblEmployeeID.Size = new System.Drawing.Size(85, 23);
            this.lblEmployeeID.TabIndex = 0;
            this.lblEmployeeID.Text = "Employee ID:";
            this.lblEmployeeID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbEmployeeID
            // 
            this.tbEmployeeID.Enabled = false;
            this.tbEmployeeID.Location = new System.Drawing.Point(104, 64);
            this.tbEmployeeID.Name = "tbEmployeeID";
            this.tbEmployeeID.ReadOnly = true;
            this.tbEmployeeID.Size = new System.Drawing.Size(152, 20);
            this.tbEmployeeID.TabIndex = 1;
            // 
            // lblFirstName
            // 
            this.lblFirstName.Location = new System.Drawing.Point(8, 96);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(85, 23);
            this.lblFirstName.TabIndex = 2;
            this.lblFirstName.Text = "First Name:";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFirstName
            // 
            this.tbFirstName.Enabled = false;
            this.tbFirstName.Location = new System.Drawing.Point(104, 96);
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.ReadOnly = true;
            this.tbFirstName.Size = new System.Drawing.Size(152, 20);
            this.tbFirstName.TabIndex = 3;
            // 
            // lblLastName
            // 
            this.lblLastName.Location = new System.Drawing.Point(8, 128);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(85, 23);
            this.lblLastName.TabIndex = 4;
            this.lblLastName.Text = "Last Name:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbLastName
            // 
            this.tbLastName.Enabled = false;
            this.tbLastName.Location = new System.Drawing.Point(104, 128);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.ReadOnly = true;
            this.tbLastName.Size = new System.Drawing.Size(152, 20);
            this.tbLastName.TabIndex = 5;
            // 
            // lblMonth
            // 
            this.lblMonth.Location = new System.Drawing.Point(8, 240);
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
            this.dtpMonth.Location = new System.Drawing.Point(96, 240);
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
            this.btnSave.Location = new System.Drawing.Point(223, 586);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(610, 586);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
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
            this.btnAssign.Size = new System.Drawing.Size(75, 23);
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
            this.lvTimeSchemaDetails.UseCompatibleStateImageBehavior = false;
            this.lvTimeSchemaDetails.View = System.Windows.Forms.View.Details;
            this.lvTimeSchemaDetails.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvTimeSchemaDetails_ColumnClick);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(304, 586);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblMinDate
            // 
            this.lblMinDate.ForeColor = System.Drawing.Color.Red;
            this.lblMinDate.Location = new System.Drawing.Point(37, 618);
            this.lblMinDate.Name = "lblMinDate";
            this.lblMinDate.Size = new System.Drawing.Size(643, 15);
            this.lblMinDate.TabIndex = 16;
            this.lblMinDate.Text = "*";
            this.lblMinDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Location = new System.Drawing.Point(600, 240);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(85, 17);
            this.checkBoxAll.TabIndex = 17;
            this.checkBoxAll.Text = "checkBoxAll";
            this.checkBoxAll.UseVisualStyleBackColor = true;
            // 
            // checkBoxContinueSchedule
            // 
            this.checkBoxContinueSchedule.AutoSize = true;
            this.checkBoxContinueSchedule.Location = new System.Drawing.Point(40, 592);
            this.checkBoxContinueSchedule.Name = "checkBoxContinueSchedule";
            this.checkBoxContinueSchedule.Size = new System.Drawing.Size(161, 17);
            this.checkBoxContinueSchedule.TabIndex = 18;
            this.checkBoxContinueSchedule.Text = "checkBoxContinueSchedule";
            this.checkBoxContinueSchedule.UseVisualStyleBackColor = true;
            // 
            // WTEmployeesTimeSchedule
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(704, 627);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxContinueSchedule);
            this.Controls.Add(this.checkBoxAll);
            this.Controls.Add(this.lblMinDate);
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
            this.Controls.Add(this.tbLastName);
            this.Controls.Add(this.tbFirstName);
            this.Controls.Add(this.tbEmployeeID);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.lblEmployeeID);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(720, 665);
            this.MinimumSize = new System.Drawing.Size(720, 665);
            this.Name = "WTEmployeesTimeSchedule";
            this.ShowInTaskbar = false;
            this.Text = "WTEmployeesTimeSchedule";
            this.Load += new System.EventHandler(this.WTEmployeesTimeSchedule_Load);
            this.Closed += new System.EventHandler(this.WTEmployeesTimeSchedule_Closed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WTEmployeesTimeSchedule_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WTEmployeesTimeSchedule_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
					case WTEmployeesTimeSchedule.DayNum:
					case WTEmployeesTimeSchedule.IntervalNum:
					case WTEmployeesTimeSchedule.Tolerance:
					{
						if ((SortColumn == 0) && (Int32.Parse(sub1.Text.Trim()) == Int32.Parse(sub2.Text.Trim())))
						{
							return CaseInsensitiveComparer.Default.Compare(Int32.Parse(item1.SubItems[1].Text.Trim()),Int32.Parse(item2.SubItems[1].Text.Trim()));
						}
						else
						{
							return CaseInsensitiveComparer.Default.Compare(Int32.Parse(sub1.Text.Trim()),Int32.Parse(sub2.Text.Trim()));
						}
					}
					case WTEmployeesTimeSchedule.StartTime:
					case WTEmployeesTimeSchedule.EndTime:
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

                
                //checkBox text
                checkBoxAll.Text = rm.GetString("checkBoxAll", culture);//natalija08112017
                checkBoxContinueSchedule.Text = rm.GetString("checkBoxContinueSchedule", culture); //natalija08112017

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);
				btnAssign.Text = rm.GetString("btnAssign", culture);
				btnClear.Text = rm.GetString("btnClear", culture);

				// label's text
				lblEmployeeID.Text = rm.GetString("lblEployeeID", culture);
				lblFirstName.Text = rm.GetString("lblFirstName", culture);
				lblLastName.Text = rm.GetString("lblLastName", culture);
				lblMonth.Text = rm.GetString("lblMonth", culture);
				lblTimeSchema.Text = rm.GetString("lblSchema", culture);
                lblMinDate.Text = "* " + rm.GetString("minEmplSchMonth", culture);

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
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
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
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.populateTimeSchedule(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
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

				populateTimeSchedule(Empl.EmployeeID, dtpMonth.Value);
				setNotWorkingDays(dtpMonth.Value);
			
				this.panel1.Invalidate();
				this.Invalidate();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.setCalendar(): " + ex.Message + "\n");
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
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.getDayOfWeek(): " + ex.Message + "\n");
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
                datesChanged = new List<DateTime>();
                this.Cursor = Cursors.WaitCursor;
                setCalendar(this.dtpMonth.Value);

                DateTime lastMonth = DateTime.Now.Date.AddMonths(-1);
                DateTime firstLastMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                if (dtpMonth.Value.Date < firstLastMonth.Date)
                {
                    btnSave.Enabled = false;
                    lblMinDate.Text = "* " + rm.GetString("minEmplSchMonth", culture);
                    lblMinDate.Visible = true;
                }
                else
                {
                    // get dictionary of all rules, key is company and value are rules by employee type id
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> emplRules = new Common.Rule().SearchWUEmplTypeDictionary();
                    Dictionary<int, WorkingUnitTO> wUnits =  new WorkingUnit().getWUDictionary();
                    
                    int cutOffDate = -1;
                    int emplCompany = Common.Misc.getRootWorkingUnit(Empl.WorkingUnitID, wUnits);

                    if (emplRules.ContainsKey(emplCompany) && emplRules[emplCompany].ContainsKey(Empl.EmployeeTypeID) && emplRules[emplCompany][Empl.EmployeeTypeID].ContainsKey(Constants.RuleHRSSCCutOffDate))
                        cutOffDate = emplRules[emplCompany][Empl.EmployeeTypeID][Constants.RuleHRSSCCutOffDate].RuleValue;

                    if (cutOffDate != -1 && Common.Misc.countWorkingDays(DateTime.Now.Date, null) > cutOffDate && this.dtpMonth.Value.Date < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0))
                    {
                        btnSave.Enabled = false;
                        lblMinDate.Text = "* " + rm.GetString("cutOffDayPessed", culture);
                        lblMinDate.Visible = true;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        lblMinDate.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.dtpMonth_ValueChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
                log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.cbTimeSchema_SelectedIndexChanged(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
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
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.populateListView(): " + ex.Message + "\n");
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
                        //for (int i = (int)selectedDays[0]; i <= (int)selectedDays[selectedDays.Count - 1]; i++)
                        //{
                        //    datesChanged.Add(calendarDays[i - 1].Date);
                        //    TimeSchema sch = new TimeSchema();
                        //    sch.TimeSchemaTO.TimeSchemaID = (int)cbTimeSchema.SelectedValue;
                        //    calendarDays[i - 1].Schema = sch.Search()[0];
                        //    calendarDays[i - 1].StartDay = day - 1;
                        //    calendarDays[i - 1].setSchemaText(rm.GetString("Schema", culture) + ": " + calendarDays[i - 1].Schema.TimeSchemaID.ToString());
                        //    calendarDays[i - 1].setCycleDayText(rm.GetString("SchemaDay", culture) + ": " + (calendarDays[i - 1].StartDay + 1).ToString());
                        //    day = (day == calendarDays[i - 1].Schema.CycleDuration ? 1 : day + 1);
                        //    calendarDays[i - 1].Invalidate();
                        //}

                        //natalija07112017 
                        DateTime startDate = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);

                        for (int i = 0; i < selectedDays.Count; i++)
                        {
                            if (calendarDays.ContainsKey(selectedDays[i].Date))
                            {
                                datesChanged.Add(calendarDays[selectedDays[i].Date].Date);
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
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.btnAssign_Click(): " + ex.Message + "\n");
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
					if(!shiftStatus)
					{
						selectedDay = new DateTime();

                        // selected month is shown and days of next month that belongs to last week of selected month
                        // max and min selected date should have initial values that are less of first day of selected month, and greater then end of last week of selected month
                        minSelected = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1, 0, 0, 0).AddMonths(2);
						maxSelected = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1, 0, 0, 0).AddMonths(-1);
						UnselectDays(e.daySelected);
					}

					selectedDay = e.daySelected;
					if(selectedDay.Date < minSelected.Date)
					{
						minSelected = selectedDay.Date;
					}
					if(selectedDay.Date > maxSelected.Date)
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
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.DaySelected(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.WTEmployeesTimeSchedule_KeyUp(): " + ex.Message + "\n");
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

                DateTime date = DateTime.Today;
                this.dtpMonth.Value = new DateTime(date.Year, date.Month, 1);

                populateTimeSchema();
                setCalendar(this.dtpMonth.Value);

                //NATALIJA08112017
                if (OldGroupID != Empl.WorkingGroupID)
                {
                    EmployeeTimeScheduleTO emplTimeSchedule = new EmployeeTimeScheduleTO();
                    EmployeeGroupsTimeScheduleTO currentTS = new EmployeeGroupsTimeScheduleTO();
                    //EmployeesTimeSchedule emplTimeSchedule1 = new EmployeesTimeSchedule();
                    currentTS = new EmployeeGroupsTimeSchedule().Find(-1, Empl.WorkingGroupID, null);
                    emplTimeSchedule = new EmployeesTimeSchedule().Find(Empl.EmployeeID, DateTime.MinValue);
                    EmployeeTO employee = new EmployeeTO();
                    employee = new Employee().Find(Empl.EmployeeID.ToString());

                    int timeSchID = -1;
                    if (!currentTS.Equals(null))
                    {
                        timeSchID = currentTS.TimeSchemaID;
                    }

                    int wtsId = -1;
                    WorkTimeSchemaTO workTimeSchema = new WorkTimeSchemaTO();
                    foreach (int id in schemas.Keys)
                    {
                        workTimeSchema = schemas[id];
                        int timeSchemaId = workTimeSchema.TimeSchemaID;

                        if (timeSchemaId == timeSchID)
                        {
                            wtsId = id;
                            break;
                        }
                    }

                    int cbId = -1;
                    List<WorkTimeSchemaTO> tsArray = new List<WorkTimeSchemaTO>();

                    for (int i = 0; i < cbTimeSchema.Items.Count; ++i)
                    {
                        workTimeSchema = (WorkTimeSchemaTO)cbTimeSchema.Items[i];
                        if (workTimeSchema.TimeSchemaID == wtsId)
                        {
                            cbId = i;
                        }
                    }
                    cbTimeSchema.SelectedIndex = cbId;
                    if (chbFirstChange)
                    {
                        checkBoxContinueSchedule.Checked = true;
                    }
                }
                    //N
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.WTEmployeesTimeSchedule_Load(): " + ex.Message + "\n");
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
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.lvTimeSchemaDetails_ColumnClick(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        //  2017-10-02 Natalija
        private void checkBoxAll_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBoxAll.Checked)
                SelectDeselectAll(true);
            else
                SelectDeselectAll(false);
        }

        //  2017-10-02 Natalija
        private void SelectDeselectAll(bool bSelected)
        {
            if (bSelected)
            {
                foreach (DateTime date in this.calendarDays.Keys)
                {
                    selectedDays.Add(date);
                    calendarDays[date].SelectDay();
                }
            }
            else
                foreach (DateTime date in calendarDays.Keys)
                {
                    selectedDays.Remove(date);
                    calendarDays[date].UnselectDay();
                }
        }

        //  2017-10-02 Natalija
        private void btnSave_Click(object sender, EventArgs e)
        {
            IOPairProcessed pairProcessed = new IOPairProcessed();

            EmployeesTimeSchedule emplTimeSchedule = new EmployeesTimeSchedule();//N
            this.Cursor = Cursors.WaitCursor;

            EmployeeGroupsTimeSchedule emplGrpTimeSchedule = new EmployeeGroupsTimeSchedule();
            Employee employee = new Employee();
            List<EmployeeTimeScheduleTO> emplSchedulesList = new List<EmployeeTimeScheduleTO>();
                                            
            try
            {
                if (selectedDays.Count > 0)
                {
                    DateTime minDate = new DateTime();
                    DateTime maxDate = new DateTime();
                    foreach (DateTime date in calendarDays.Keys)//in selectedDays
                    {
                        if (minDate.Equals(new DateTime()))
                            minDate = date.Date;
                        else if (date.Date < minDate.Date)
                            minDate = date.Date;
                        if (date.Date > maxDate.Date)
                            maxDate = date.Date;
                    }

                    Dictionary<int, EmployeeTO> emplDict = new Employee().SearchDictionary(Empl.EmployeeID.ToString().Trim());
                    Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(Empl.EmployeeID.ToString().Trim());
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Common.Rule().SearchWUEmplTypeDictionary();

                    bool trans = emplTimeSchedule.BeginTransaction();
                    bool isSaved = true;
                    if (true)//if (Trans)
                    {
                        if (isSaved)
                        {
                            //DateTime startDate = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
                            //DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));

                            DateTime startDate = minDate;
                            DateTime endDate = maxDate;

                            bool statuscb = checkBoxContinueSchedule.Checked;

                            //get last time schedule for employee
                            List<EmployeeTimeScheduleTO> oldEmployeeTimeSchedule = emplTimeSchedule.SearchEmployeesSchedules(Empl.EmployeeID.ToString(), startDate.Date, endDate.Date, emplTimeSchedule.GetTransaction());

                            // delete time schedule for selected days
                            isSaved = isSaved && emplTimeSchedule.DeleteFromToSchedule(Empl.EmployeeID, startDate.Date, endDate.Date, "", false, statuscb);

                            //30.11.2017 Miodrag / Sredio proveru prvog dana u mesecu. 
                            WorkTimeSchemaTO actualTimeSchema = null;
                            List<WorkTimeSchemaTO> timeSchemaZ = new List<WorkTimeSchemaTO>();
                            TimeSchema s = new TimeSchema();
                            timeSchemaZ = s.Search(emplTimeSchedule.GetTransaction());
                            foreach (WorkTimeSchemaTO currentTimeSchema in timeSchemaZ)
                            {
                                if (currentTimeSchema.TimeSchemaID == oldEmployeeTimeSchedule[0].TimeSchemaID)
                                {
                                    actualTimeSchema = currentTimeSchema;
                                    break;
                                }
                            }
                            TimeSpan ts1 = new TimeSpan(startDate.Date.Ticks - oldEmployeeTimeSchedule[0].Date.Date.Ticks);
                            int dayNum1 = (oldEmployeeTimeSchedule[0].StartCycleDay + (int)ts1.TotalDays) % actualTimeSchema.CycleDuration;

                            //19.09.2017 Miodrag / Postojala je mogucnost da je dayNum negativan.
                            if (++dayNum1 < 0) //dayNum se povecava za jedan jer se poredi sa cycle dayom koji za razliku od njega krece od 1.
                            {
                                dayNum1 = actualTimeSchema.CycleDuration + dayNum1;
                            }
                            //mm
                            int prevSchemaID = oldEmployeeTimeSchedule[0].TimeSchemaID;
                            int expectedStartDay = dayNum1 - 1; // oldEmployeeTimeSchedule[0].StartCycleDay;

                            //int prevSchemaID = -1;
                            //int expectedStartDay = -1;

                            EmployeeTimeScheduleTO timeSceduleTO = new EmployeeTimeScheduleTO();

                            if (isSaved)
                            {
                                foreach (DateTime calDate in calendarDays.Keys)
                                {
                                    //if (selectedDays.Contains(calDate))
                                    //{
                                        // if schema is different, or if schema is the same the same but cycle day is not the expected one
                                        // save new record                                
                                        if ((calendarDays[calDate].Schema.TimeSchemaID != prevSchemaID) ||
                                                ((calendarDays[calDate].Schema.TimeSchemaID == prevSchemaID) && (calendarDays[calDate].StartDay != expectedStartDay)))
                                        {
                                            isSaved = (emplTimeSchedule.Save(Empl.EmployeeID, calDate, calendarDays[calDate].Schema.TimeSchemaID, calendarDays[calDate].StartDay, "", false) > 0 ? true : false) && isSaved;
                                            

                                            timeSceduleTO = new EmployeeTimeScheduleTO(Empl.EmployeeID, calDate, calendarDays[calDate].Schema.TimeSchemaID, calendarDays[calDate].StartDay);

                                            emplSchedulesList.Add(timeSceduleTO);

                                            if (!isSaved)
                                            {
                                                break;
                                            }

                                            prevSchemaID = calendarDays[calDate].Schema.TimeSchemaID;
                                        }

                                        expectedStartDay = (calendarDays[calDate].StartDay == calendarDays[calDate].Schema.CycleDuration - 1 ? 0 : calendarDays[calDate].StartDay + 1);

                                   // }
                                }
                            }

                            //NATALIJA 20.20.2017
                            // assign employee's working group time schedule for next month

                            if (statuscb == true)                            
                            {
                                if (isSaved)
                                {
                                    EmployeeGroupsTimeScheduleTO currentTS = new EmployeeGroupsTimeScheduleTO();
                                    currentTS = new EmployeeGroupsTimeSchedule().Find(calendarDays[maxDate].Schema.TimeSchemaID, -1, emplTimeSchedule.GetTransaction());
                                    WorkingGroupTO workingGroup = new WorkingGroupTO();
                                    workingGroup = new WorkingGroup().Find(currentTS.EmployeeGroupID, emplTimeSchedule.GetTransaction());
                                    //Empl.ModifiedBy = CommonWeb.Misc.getLoginUser(logInUser);
                                    isSaved = isSaved && employee.Update(Empl, workingGroup.EmployeeGroupID, emplTimeSchedule.GetTransaction());
                                }
                            }

                            if (isSaved)
                            {
                                //??? delete absences pairs for that month and update absences to unused
                                isSaved = deleteIOPUpdateEA(emplTimeSchedule.GetTransaction()) && isSaved;

                                List<DateTime> datesList = datesChanged;

                                //list od datetime for each employee
                                Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                                emplDateWholeDayList.Add(Empl.EmployeeID, datesList);

                                //  procesirani parovi koji jesu celodnevna odsustva
                                Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>> manualCreated = new Dictionary<int, Dictionary<DateTime, List<IOPairProcessedTO>>>();
                                manualCreated = pairProcessed.getManualCreatedPairsWholeDayAbsence(emplDateWholeDayList, emplTimeSchedule.GetTransaction());
                                
                                List<WorkTimeSchemaTO> timeSchemas = new List<WorkTimeSchemaTO>();

                                WorkTimeSchemaTO wtsTO = new WorkTimeSchemaTO(timeSceduleTO.TimeSchemaID);

                                timeSchemas = new TimeSchema().SearchTimeSchedule(wtsTO, emplTimeSchedule.GetTransaction());

                                Dictionary<DateTime, List<IOPairProcessedTO>> lista = new Dictionary<DateTime, List<IOPairProcessedTO>>();
                                List<IOPairProcessedTO> listaProcessed = new List<IOPairProcessedTO>();
                                //IOPairProcessedTO processed = new IOPairProcessedTO();

                                foreach (int emplID in manualCreated.Keys)
                                {
                                    lista = manualCreated[emplID];

                                    foreach (DateTime dt in lista.Keys)
                                    {
                                        listaProcessed = lista[dt];
                                        foreach (IOPairProcessedTO processed in listaProcessed)
                                        {
                                            bool is2DayShift = false;
                                            bool is2DaysShiftPrevious = false;
                                            WorkTimeIntervalTO firstIntervalNextDay = new WorkTimeIntervalTO();
                                            Dictionary<int, WorkTimeIntervalTO> workTimeInterval = Common.Misc.getDayTimeSchemaIntervals(emplSchedulesList, dt, ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);
                                            Dictionary<int, WorkTimeIntervalTO> workTimeIntervalNextDay = Common.Misc.getDayTimeSchemaIntervals(emplSchedulesList, dt.AddDays(1) , ref is2DayShift, ref is2DaysShiftPrevious, ref firstIntervalNextDay, timeSchemas);

                                            isSaved = isSaved && pairProcessed.UpdateManualCreatedProcessedPairs(processed, workTimeInterval,workTimeIntervalNextDay, is2DayShift, emplTimeSchedule.GetTransaction());
                                        }
                                    }
                                }
                            }

                            if (isSaved)
                            {
                                IOPair ioPair = new IOPair();
                                if ((startDate.Year < DateTime.Now.Year) ||
                                    ((startDate.Year == DateTime.Now.Year) && (startDate.Month <= DateTime.Now.Month)))
                                {
                                    if (endDate.Date > DateTime.Now.Date)
                                        endDate = DateTime.Now.Date;

                                    ioPair.recalculatePause(Empl.EmployeeID.ToString().Trim(), startDate.Date, endDate.Date, emplTimeSchedule.GetTransaction());
                                }
                            }
                        }

                        if (isSaved)
                        {
                            // validate new employee schedule
                            bool validFundHrs = true;
                            DateTime invalidDate = Common.Misc.isValidTimeSchedule(emplDict, ascoDict, rulesDict, Empl.EmployeeID.ToString().Trim(), minDate.Date,
                               maxDate.Date, emplTimeSchedule.GetTransaction(), null, false, ref validFundHrs, true);
                            if (invalidDate.Equals(new DateTime()))
                            {
                                emplTimeSchedule.CommitTransaction();
                                MessageBox.Show(rm.GetString("emplScheduleSaved", culture));
                                this.Close();
                            }
                            else
                            {
                                emplTimeSchedule.RollbackTransaction();
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
                            emplTimeSchedule.RollbackTransaction();
                            MessageBox.Show(rm.GetString("emplScheduleNotSaved", culture));
                        }

                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("assignDay", culture));

                    }
                }
            }
            catch (Exception ex)
            {
                if (emplTimeSchedule.GetTransaction() != null)
                {
                    emplTimeSchedule.RollbackTransaction();
                }
                log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
        /*
		private void btnSave_Click(object sender, System.EventArgs e)
		{
            EmployeesTimeSchedule emplTimeSchedule = new EmployeesTimeSchedule();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                // every day must have schema assigned
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
                    Dictionary<int, EmployeeTO> emplDict = new Employee().SearchDictionary(Empl.EmployeeID.ToString().Trim());
                    Dictionary<int, EmployeeAsco4TO> ascoDict = new EmployeeAsco4().SearchDictionary(Empl.EmployeeID.ToString().Trim());
                    Dictionary<int, Dictionary<int, Dictionary<string, RuleTO>>> rulesDict = new Common.Rule().SearchWUEmplTypeDictionary();

                    bool trans = emplTimeSchedule.BeginTransaction();
                    bool isSaved = true;

                    if (trans)
                    {
                        // delete time schedule for selected month
                        //isSaved = emplTimeSchedule.DeleteMonthSchedule(Int32.Parse(this.tbEmployeeID.Text), dtpMonth.Value, false);
                        DateTime startDate = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
                        DateTime endDate = Common.Misc.getWeekEnding(startDate.AddMonths(1).AddDays(-1));
                        isSaved = isSaved && emplTimeSchedule.DeleteFromToSchedule(Empl.EmployeeID, startDate.Date, endDate.Date, "", false);

                        int prevSchemaID = -1;
                        int expectedStartDay = -1;

                        if (isSaved)
                        {
                            foreach (DateTime calDate in calendarDays.Keys)
                            {
                                // if schema is different, or if schema is the same the same but cycle day is not the expected one
                                // save new record                                
                                if ((calendarDays[calDate].Schema.TimeSchemaID != prevSchemaID) ||
                                        ((calendarDays[calDate].Schema.TimeSchemaID == prevSchemaID) && (calendarDays[calDate].StartDay != expectedStartDay)))
                                    {
                                        isSaved = (emplTimeSchedule.Save(Empl.EmployeeID, calendarDays[calDate].Date, calendarDays[calDate].Schema.TimeSchemaID, calendarDays[calDate].StartDay, "", false) > 0 ? true : false) && isSaved;

                                        if (!isSaved)
                                        {
                                            break;
                                        }

                                        prevSchemaID = calendarDays[calDate].Schema.TimeSchemaID;
                                    }

                                expectedStartDay = (calendarDays[calDate].StartDay == calendarDays[calDate].Schema.CycleDuration - 1 ? 0 : calendarDays[calDate].StartDay + 1);
                            }
                        }

                        if (isSaved)
                        {
                            // assign employee's working group time schedule for next month
                            //DateTime month = dtpMonth.Value;
                            //DateTime firstNextMonth = new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1);
                            DateTime firstNextDate = endDate.AddDays(1).Date;
                            List<EmployeeTimeScheduleTO> nextSchedule = emplTimeSchedule.SearchEmployeesNextSchedule(Empl.EmployeeID.ToString().Trim(), endDate.Date, emplTimeSchedule.GetTransaction());
                            bool insertGroupSchedule = false;

                            if (nextSchedule.Count > 0)
                            {
                                DateTime nextScheduleDate = nextSchedule[0].Date.Date;
                                if (nextScheduleDate.Date > firstNextDate.Date)
                                    insertGroupSchedule = true;
                            }
                            else
                            {
                                insertGroupSchedule = true;
                            }

                            if (insertGroupSchedule)
                            {
                                if (Empl.WorkingGroupID != -1)
                                {
                                    List<EmployeeGroupsTimeScheduleTO> timeSchedule = new EmployeeGroupsTimeSchedule().SearchMonthSchedule(Empl.WorkingGroupID, firstNextDate.Date, emplTimeSchedule.GetTransaction());

                                    int timeScheduleIndex = -1;
                                    for (int scheduleIndex = 0; scheduleIndex < timeSchedule.Count; scheduleIndex++)
                                    {
                                        if (firstNextDate.Date >= timeSchedule[scheduleIndex].Date)
                                        {
                                            timeScheduleIndex = scheduleIndex;
                                        }
                                    }
                                    if (timeScheduleIndex >= 0)
                                    {
                                        EmployeeGroupsTimeScheduleTO egts = timeSchedule[timeScheduleIndex];
                                        int startDay = egts.StartCycleDay;
                                        int schemaID = egts.TimeSchemaID;

                                        WorkTimeSchemaTO actualTimeSchema = null;

                                        if (schemas.ContainsKey(schemaID))
                                            actualTimeSchema = schemas[schemaID];

                                        if (actualTimeSchema != null)
                                        {
                                            int cycleDuration = actualTimeSchema.CycleDuration;

                                            TimeSpan ts = new TimeSpan(firstNextDate.Date.Ticks - egts.Date.Date.Ticks);
                                            int dayNum = (startDay + (int)ts.TotalDays) % cycleDuration;

                                            int insert = emplTimeSchedule.Save(Empl.EmployeeID, firstNextDate.Date, schemaID, dayNum, "", false);

                                            isSaved = (insert > 0 ? true : false) && isSaved;

                                            List<EmployeeTimeScheduleTO> nextMonthSchedule = emplTimeSchedule.SearchEmployeesNextSchedule(Empl.EmployeeID.ToString(), firstNextDate.Date, emplTimeSchedule.GetTransaction());
                                            if (nextMonthSchedule.Count > 0)
                                            {
                                                List<DateTime> additionalDateList = new List<DateTime>();
                                                for (DateTime currDay = firstNextDate.Date; currDay.Date < nextMonthSchedule[0].Date.Date; currDay = currDay.Date.AddDays(1))
                                                {
                                                    additionalDateList.Add(currDay.Date);
                                                }

                                                if (additionalDateList.Count > 0)
                                                {
                                                    Dictionary<int, List<DateTime>>  emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                                                    emplDateWholeDayList.Add(Empl.EmployeeID, additionalDateList);

                                                    isSaved = isSaved && Common.Misc.ReprocessPairsAndRecalculateCounters(Empl.EmployeeID.ToString(), firstNextDate.Date.Date, nextMonthSchedule[0].Date.Date.AddDays(-1), emplTimeSchedule.GetTransaction(), emplDateWholeDayList, null, "");
                                                }
                                            }
                                        }
                                    } //if (timeScheduleIndex >= 0)
                                } //if (employeeTO.WorkingGroupID != -1)
                            } //if (insertGroupSchedule)

                            if (isSaved)
                            {
                                // delete absences pairs for that month and update absences to unused
                                isSaved = deleteIOPUpdateEA(emplTimeSchedule.GetTransaction()) && isSaved;

                                if (isSaved)
                                {
                                    IOPair ioPair = new IOPair();
                                    if ((startDate.Year < DateTime.Now.Year) ||
                                        ((startDate.Year == DateTime.Now.Year) && (startDate.Month <= DateTime.Now.Month)))
                                    {
                                        /*2008-03-14
                                        // * From now one, take the last existing time schedule, don't expect that every month has 
                                        // * time schedule
                                        //DateTime toDate = (new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1)).AddDays(-1);
                                       //ArrayList nextSchedule = emplTimeSchedule.SearchEmployeesNextSchedule(tbEmployeeID.Text, toDate);

                                        //if (nextSchedule.Count > 0)
                                        //{
                                        //    toDate = ((EmployeesTimeSchedule)nextSchedule[0]).Date.AddDays(-1).Date;
                                        //    if (toDate.Date > DateTime.Now.Date)
                                                toDate = DateTime.Now.Date;
                                        //}
                                       // else
                                       // {
                                        //    toDate = DateTime.Now.Date;
                                       // }
                                        if (endDate.Date > DateTime.Now.Date)
                                            endDate = DateTime.Now.Date;

                                        //ioPair.recalculatePause(tbEmployeeID.Text, new DateTime(month.Year, month.Month, 1), (new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, 1)).AddDays(-1));
                                        ioPair.recalculatePause(Empl.EmployeeID.ToString().Trim(), startDate.Date, endDate.Date, emplTimeSchedule.GetTransaction());
                                    }
                                }
                            }
                        }

                        if (isSaved)
                        {
                            // validate new employee schedule
                            bool validFundHrs = true;
                            DateTime invalidDate = Common.Misc.isValidTimeSchedule(emplDict, ascoDict, rulesDict, Empl.EmployeeID.ToString().Trim(), minDate.Date, 
                                maxDate.Date, emplTimeSchedule.GetTransaction(), null, false, ref validFundHrs, true);
                            if (invalidDate.Equals(new DateTime()))
                            {
                                emplTimeSchedule.CommitTransaction();
                                MessageBox.Show(rm.GetString("emplScheduleSaved", culture));
                                this.Close();
                            }
                            else
                            {
                                emplTimeSchedule.RollbackTransaction();
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
                            emplTimeSchedule.RollbackTransaction();
                            MessageBox.Show(rm.GetString("emplScheduleNotSaved", culture));                            
                        }
                    } // if(trans)
                } // if(assignStatus)
            }
            catch (Exception ex)
            {
                if (emplTimeSchedule.GetTransaction() != null)
                {
                    emplTimeSchedule.RollbackTransaction();
                }
                log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally 
            { 
                this.Cursor = Cursors.Arrow; 
            }
		}
*/
		private void populateTimeSchedule(int employeeID, DateTime month)
		{
			try
			{
                DateTime startDate = new DateTime(month.Year, month.Month, 1);
                DateTime endDate = Common.Misc.getWeekEnding(new DateTime(month.Year, month.Month, 1).AddMonths(1).AddDays(-1));

                //List<EmployeeTimeScheduleTO> timeSchedule = new EmployeesTimeSchedule().SearchMonthSchedule(employeeID, month);
                Dictionary<int, List<EmployeeTimeScheduleTO>> timeScheduleByEmpl = new EmployeesTimeSchedule().SearchEmployeesSchedulesExactDate(employeeID.ToString().Trim(), startDate.Date, endDate.Date, null);

                List<EmployeeTimeScheduleTO> timeSchedule = new List<EmployeeTimeScheduleTO>();

                if (timeScheduleByEmpl.ContainsKey(employeeID))
                    timeSchedule = timeScheduleByEmpl[employeeID];
                
				if (timeSchedule.Count > 0)
				{	
					for (int scheduleIndex = 0; scheduleIndex < timeSchedule.Count; scheduleIndex++)                    
					{
						int cycleDuration = 0;
						int day = timeSchedule[scheduleIndex].StartCycleDay + 1;
						int schemaID = timeSchedule[scheduleIndex].TimeSchemaID;                        
						
						WorkTimeSchemaTO schema = new WorkTimeSchemaTO();
						if(schemas.ContainsKey(schemaID))
						{
							schema = schemas[schemaID];
							cycleDuration = schema.CycleDuration;
						}

                        /* 2008-03-14
                         * From now one, take the last existing time schedule, don't expect that every month has 
                         * time schedule*/
                        //int i = timeSchedule[scheduleIndex].Date.Day - 1;
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
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.populateTimeSchedule(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void setNotWorkingDays(DateTime month)
		{
            try
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
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.setNotWorkingDays(): " + ex.Message + "\n");
                throw ex;
            }
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			foreach(DateTime day in calendarDays.Keys)
			{
                calendarDays[day].Schema = new WorkTimeSchemaTO();
                calendarDays[day].StartDay = -1;
                calendarDays[day].setSchemaText("");
                calendarDays[day].setCycleDayText("");
			}
		}

		// delete absences pairs for that month and update absences to unused
		private bool deleteIOPUpdateEA(IDbTransaction trans)
		{
            bool isDeleted = true;
			try
			{
				int employeeID = Empl.EmployeeID;

                DateTime dateValue = dtpMonth.Value;
                DateTime start = new DateTime(dateValue.Year, dateValue.Month, 1);
                //DateTime end = new DateTime(dateValue.AddMonths(1).Year, dateValue.AddMonths(1).Month, 1).AddDays(-1);
                DateTime end = Common.Misc.getWeekEnding(new DateTime(dateValue.AddMonths(1).Year, dateValue.AddMonths(1).Month, 1).AddDays(-1));

                #region Reprocess Dates
                List<DateTime> datesList = datesChanged;

                //list od datetime for each employee
                Dictionary<int, List<DateTime>> emplDateWholeDayList = new Dictionary<int, List<DateTime>>();
                emplDateWholeDayList.Add(employeeID, datesList);
                if (datesList.Count > 0)
                    Common.Misc.ReprocessPairsAndRecalculateCounters(employeeID.ToString(), start, end, trans, emplDateWholeDayList, null, "");
             
                #endregion

                /* 2008-03-14
                 * From now one, take the last existing time schedule, don't expect that every month has 
                 * time schedule*/
                /*ArrayList nextSchedule = new EmployeesTimeSchedule().SearchEmployeesNextSchedule(tbEmployeeID.Text, end);
                if (nextSchedule.Count > 0)
                {
                    end = ((EmployeesTimeSchedule)nextSchedule[0]).Date.AddDays(-1).Date;
                }
                else
                {
                    end = new DateTime(0);
                }*/

                EmployeeAbsence ea = new EmployeeAbsence();
                ea.EmplAbsTO.EmployeeID = employeeID;
                ea.EmplAbsTO.DateStart = start;
                ea.EmplAbsTO.DateEnd = end;
				List<EmployeeAbsenceTO> emplAbsences = ea.Search("", trans);

				foreach (EmployeeAbsenceTO abs in emplAbsences)
				{
					isDeleted = new EmployeeAbsence().UpdateEADeleteIOP(abs.RecID, abs.EmployeeID, abs.PassTypeID,
						abs.PassTypeID, abs.DateStart, abs.DateEnd, abs.DateStart, abs.DateEnd, (int) Constants.Used.No, trans) && isDeleted;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTEmployeesTimeSchedule.deleteIOPUpdateEA(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

            return isDeleted;
		}

        //private string manualPairCounters(IOPairProcessedTO pairToConfirm, Dictionary<int, int> emplCounters, Dictionary<int, EmployeeCounterValueTO> emplOldCounters, DateTime histCreated, IDbTransaction trans)
        //{
        //    try
        //    {
        //        IOPairProcessed pair = new IOPairProcessed();
        //        bool saved = true;
        //        string error = "";

        //        pair.SetTransaction(trans);

        //        try
        //        {
        //            IOPairsProcessedHist hist = new IOPairsProcessedHist();
        //            EmployeeCounterValue counter = new EmployeeCounterValue();
        //            EmployeeCounterValueHist counterHist = new EmployeeCounterValueHist();

        //            pair.IOPairProcessedTO = pairToConfirm;
        //            IOPairsProcessedHistTO histTO = new IOPairsProcessedHistTO(pairToConfirm);
        //            hist.IOPairProcessedHistTO = histTO;
        //            hist.IOPairProcessedHistTO.Alert = Constants.alertStatus.ToString();
        //            hist.IOPairProcessedHistTO.ModifiedTime = histCreated;
        //            hist.SetTransaction(pair.GetTransaction());
        //            hist.Save(false);
        //            saved = saved && (pair.Delete(pairToConfirm.RecID.ToString(), false));

        //            if (saved)
        //            {
        //                // update counters, updated counters insert to hist table
        //                counterHist.SetTransaction(pair.GetTransaction());
        //                counter.SetTransaction(pair.GetTransaction());
        //                // update counters and move old counter values to hist table if updated
        //                foreach (int type in emplCounters.Keys)
        //                {
        //                    if (emplOldCounters.ContainsKey(type) && emplOldCounters[type].Value != emplCounters[type])
        //                    {
        //                        // move to hist table
        //                        counterHist.ValueTO = new EmployeeCounterValueHistTO(emplOldCounters[type]);
        //                        saved = saved && (counterHist.Save(false) >= 0);

        //                        if (!saved)
        //                            break;

        //                        counter.ValueTO = new EmployeeCounterValueTO();
        //                        counter.ValueTO.EmplCounterTypeID = type;
        //                        counter.ValueTO.EmplID = pairToConfirm.EmployeeID;
        //                        counter.ValueTO.Value = emplCounters[type];

        //                        saved = saved && counter.Update(false);

        //                        if (!saved)
        //                            break;
        //                    }
        //                }
        //            }


        //            if (!saved)
        //            {
        //                if (pair.GetTransaction() != null)
        //                    pair.RollbackTransaction();
        //                log.writeLog(DateTime.Now +
        //        this.ToString() + ".manualPairCounters() : Processing manual pairs counters faild for Employee_ID = " + pairToConfirm.EmployeeID + "; Date = " + pairToConfirm.IOPairDate.ToString("dd.MM.yyyy") + "; Pass_type_ID = " + pairToConfirm.PassTypeID.ToString());
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (pair.GetTransaction() != null)
        //                pair.RollbackTransaction();
        //            log.writeLog(DateTime.Now + " Exception in: " +
        //         this.ToString() + ".manualPairCounters() - Message: " + ex.Message + "; StackTrace: " + ex.StackTrace);
        //        }
        //        //}
        //        //else
        //        //    log.writeLog(DateTime.Now +
        //        //    this.ToString() + ".manualPairCounters() : Processing manual pairs counters faild for Employee_ID = " + pairToConfirm.EmployeeID + "; Date = " + pairToConfirm.IOPairDate.ToString("dd.MM.yyyy") + "; Pass_type_ID = " + pairToConfirm.PassTypeID.ToString() + "; Begin transaction faild!");


        //        return error;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
	}
}
