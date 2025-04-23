using System;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using TransferObjects;
using Common;
using Util;

namespace UI
{
	/// <summary>
	/// TimeAdvanceSettings enable user to view and change 
	/// defined work schedule in more details.
	/// </summary>
	public class WTSchemaAdvance : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblEAT;
		private System.Windows.Forms.Label lblST;
		private System.Windows.Forms.Label lblLAT;
		private System.Windows.Forms.Label lblELT;
		private System.Windows.Forms.Label lblET;
		private System.Windows.Forms.Label lblLLT;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Language
		private ResourceManager rm;
		private CultureInfo culture;
		private System.Windows.Forms.DateTimePicker dtEAT;
		private System.Windows.Forms.DateTimePicker dtST;
		private System.Windows.Forms.DateTimePicker dtLAT;
		private System.Windows.Forms.DateTimePicker dtELT;
		private System.Windows.Forms.DateTimePicker dtET;
		private System.Windows.Forms.DateTimePicker dtLLT;

		
		private DateTime _earliestArrived;
		private DateTime _startTime;
		private DateTime _latestArrivaed;
		private DateTime _earliestLeft;
		private DateTime _endTime;
		private DateTime _latestLeft;
		private int _autoClose;

		public bool Saved;

		// Debug log
		DebugLog log;
		private System.Windows.Forms.GroupBox gbAutoClose;
		private System.Windows.Forms.RadioButton rbWithout;
		private System.Windows.Forms.RadioButton rbStart;
		private System.Windows.Forms.RadioButton rbEnd;
		private System.Windows.Forms.RadioButton rbStartEnd;
		ApplUserTO logInUser;

		public DateTime EarliestArrived
		{
			get { return _earliestArrived; }
			set { _earliestArrived = value;}
		}

		public DateTime StartTime
		{
			get { return _startTime; }
			set { _startTime = value; }
		}

		public DateTime LatestArrived
		{
			get { return _latestArrivaed; }
			set { _latestArrivaed = value; }
		}

		public DateTime EarliestLeft
		{ 
			get { return _earliestLeft; }
			set { _earliestLeft = value; }
		}

		public DateTime EndTime
		{
			get { return _endTime; }
			set { _endTime = value; }
		}

		public DateTime LatestLeft
		{
			get { return _latestLeft; }
			set { _latestLeft = value; }
		}

		public int AutoClose
		{
			get { return _autoClose; }
			set { _autoClose = value; }
		}

		public WTSchemaAdvance(TimeSchema schema, int dayNum, int intervalNum)
		{
			InitializeComponent();
			this.CenterToScreen();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			//InitialiseObserverClient();

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();
			logInUser = NotificationController.GetLogInUser();

			rbWithout.Checked = true;

			// Set current schema
			//currentSchema = schema;
			//currentInterval = (TimeSchemaInterval) (((Hashtable) schema.Days[dayNum])[intervalNum]);
		}

		public WTSchemaAdvance(DateTime eaTime, DateTime sTime, DateTime laTime, DateTime elTime, 
						DateTime eTime, DateTime llTime, int autoClose)
		{
			InitializeComponent();
			this.CenterToScreen();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			//InitialiseObserverClient();

			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();
			logInUser = NotificationController.GetLogInUser();

			EarliestArrived = eaTime;
			StartTime = sTime;
			LatestArrived = laTime;
			EarliestLeft = elTime;
			EndTime = eTime;
			LatestLeft = llTime;

			switch (autoClose)
			{
				case (int) Constants.AutoClose.WithoutClose:
					rbWithout.Checked = true;
					break;
				case (int) Constants.AutoClose.StartClose:
					rbStart.Checked = true;
					break;
				case (int) Constants.AutoClose.EndClose:
					rbEnd.Checked = true;
					break;
				case (int) Constants.AutoClose.StartEndClose:
					rbStartEnd.Checked = true;
					break;
				default:
					rbWithout.Checked = true;
					break;
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
            this.dtEAT = new System.Windows.Forms.DateTimePicker();
            this.lblEAT = new System.Windows.Forms.Label();
            this.dtST = new System.Windows.Forms.DateTimePicker();
            this.lblST = new System.Windows.Forms.Label();
            this.dtLAT = new System.Windows.Forms.DateTimePicker();
            this.lblLAT = new System.Windows.Forms.Label();
            this.dtELT = new System.Windows.Forms.DateTimePicker();
            this.lblELT = new System.Windows.Forms.Label();
            this.dtET = new System.Windows.Forms.DateTimePicker();
            this.lblET = new System.Windows.Forms.Label();
            this.dtLLT = new System.Windows.Forms.DateTimePicker();
            this.lblLLT = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbAutoClose = new System.Windows.Forms.GroupBox();
            this.rbStartEnd = new System.Windows.Forms.RadioButton();
            this.rbEnd = new System.Windows.Forms.RadioButton();
            this.rbStart = new System.Windows.Forms.RadioButton();
            this.rbWithout = new System.Windows.Forms.RadioButton();
            this.gbAutoClose.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtEAT
            // 
            this.dtEAT.CustomFormat = "HH:mm";
            this.dtEAT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEAT.Location = new System.Drawing.Point(168, 32);
            this.dtEAT.Name = "dtEAT";
            this.dtEAT.ShowUpDown = true;
            this.dtEAT.Size = new System.Drawing.Size(64, 20);
            this.dtEAT.TabIndex = 1;
            // 
            // lblEAT
            // 
            this.lblEAT.Location = new System.Drawing.Point(8, 32);
            this.lblEAT.Name = "lblEAT";
            this.lblEAT.Size = new System.Drawing.Size(144, 23);
            this.lblEAT.TabIndex = 0;
            this.lblEAT.Text = "Earliest Attendance Time:";
            this.lblEAT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtST
            // 
            this.dtST.CustomFormat = "HH:mm";
            this.dtST.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtST.Location = new System.Drawing.Point(168, 64);
            this.dtST.Name = "dtST";
            this.dtST.ShowUpDown = true;
            this.dtST.Size = new System.Drawing.Size(64, 20);
            this.dtST.TabIndex = 3;
            // 
            // lblST
            // 
            this.lblST.Location = new System.Drawing.Point(88, 64);
            this.lblST.Name = "lblST";
            this.lblST.Size = new System.Drawing.Size(64, 23);
            this.lblST.TabIndex = 2;
            this.lblST.Text = "Start Time:";
            this.lblST.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtLAT
            // 
            this.dtLAT.CustomFormat = "HH:mm";
            this.dtLAT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtLAT.Location = new System.Drawing.Point(168, 96);
            this.dtLAT.Name = "dtLAT";
            this.dtLAT.ShowUpDown = true;
            this.dtLAT.Size = new System.Drawing.Size(64, 20);
            this.dtLAT.TabIndex = 5;
            // 
            // lblLAT
            // 
            this.lblLAT.Location = new System.Drawing.Point(16, 96);
            this.lblLAT.Name = "lblLAT";
            this.lblLAT.Size = new System.Drawing.Size(136, 23);
            this.lblLAT.TabIndex = 4;
            this.lblLAT.Text = "Latest Attendance Time:";
            this.lblLAT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtELT
            // 
            this.dtELT.CustomFormat = "HH:mm";
            this.dtELT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtELT.Location = new System.Drawing.Point(168, 128);
            this.dtELT.Name = "dtELT";
            this.dtELT.ShowUpDown = true;
            this.dtELT.Size = new System.Drawing.Size(64, 20);
            this.dtELT.TabIndex = 7;
            // 
            // lblELT
            // 
            this.lblELT.Location = new System.Drawing.Point(40, 128);
            this.lblELT.Name = "lblELT";
            this.lblELT.Size = new System.Drawing.Size(112, 23);
            this.lblELT.TabIndex = 6;
            this.lblELT.Text = "Earliest Leaft Time:";
            this.lblELT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtET
            // 
            this.dtET.CustomFormat = "HH:mm";
            this.dtET.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtET.Location = new System.Drawing.Point(168, 160);
            this.dtET.Name = "dtET";
            this.dtET.ShowUpDown = true;
            this.dtET.Size = new System.Drawing.Size(64, 20);
            this.dtET.TabIndex = 9;
            // 
            // lblET
            // 
            this.lblET.Location = new System.Drawing.Point(88, 160);
            this.lblET.Name = "lblET";
            this.lblET.Size = new System.Drawing.Size(64, 23);
            this.lblET.TabIndex = 8;
            this.lblET.Text = "End Time:";
            this.lblET.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtLLT
            // 
            this.dtLLT.CustomFormat = "HH:mm";
            this.dtLLT.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtLLT.Location = new System.Drawing.Point(168, 192);
            this.dtLLT.Name = "dtLLT";
            this.dtLLT.ShowUpDown = true;
            this.dtLLT.Size = new System.Drawing.Size(64, 20);
            this.dtLLT.TabIndex = 11;
            // 
            // lblLLT
            // 
            this.lblLLT.Location = new System.Drawing.Point(40, 192);
            this.lblLLT.Name = "lblLLT";
            this.lblLLT.Size = new System.Drawing.Size(112, 23);
            this.lblLLT.TabIndex = 10;
            this.lblLLT.Text = "Latest Left Time:";
            this.lblLLT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(168, 400);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(16, 400);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbAutoClose
            // 
            this.gbAutoClose.Controls.Add(this.rbStartEnd);
            this.gbAutoClose.Controls.Add(this.rbEnd);
            this.gbAutoClose.Controls.Add(this.rbStart);
            this.gbAutoClose.Controls.Add(this.rbWithout);
            this.gbAutoClose.Location = new System.Drawing.Point(16, 224);
            this.gbAutoClose.Name = "gbAutoClose";
            this.gbAutoClose.Size = new System.Drawing.Size(224, 160);
            this.gbAutoClose.TabIndex = 12;
            this.gbAutoClose.TabStop = false;
            this.gbAutoClose.Text = "Auto Close";
            // 
            // rbStartEnd
            // 
            this.rbStartEnd.Location = new System.Drawing.Point(8, 120);
            this.rbStartEnd.Name = "rbStartEnd";
            this.rbStartEnd.Size = new System.Drawing.Size(208, 24);
            this.rbStartEnd.TabIndex = 3;
            this.rbStartEnd.Text = "Start and/or End Close";
            // 
            // rbEnd
            // 
            this.rbEnd.Location = new System.Drawing.Point(8, 88);
            this.rbEnd.Name = "rbEnd";
            this.rbEnd.Size = new System.Drawing.Size(208, 24);
            this.rbEnd.TabIndex = 2;
            this.rbEnd.Text = "End Close";
            // 
            // rbStart
            // 
            this.rbStart.Location = new System.Drawing.Point(8, 56);
            this.rbStart.Name = "rbStart";
            this.rbStart.Size = new System.Drawing.Size(200, 24);
            this.rbStart.TabIndex = 1;
            this.rbStart.Text = "Start Close";
            // 
            // rbWithout
            // 
            this.rbWithout.Location = new System.Drawing.Point(8, 24);
            this.rbWithout.Name = "rbWithout";
            this.rbWithout.Size = new System.Drawing.Size(200, 24);
            this.rbWithout.TabIndex = 0;
            this.rbWithout.Text = "Without Close";
            // 
            // WTSchemaAdvance
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(256, 430);
            this.ControlBox = false;
            this.Controls.Add(this.gbAutoClose);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dtLLT);
            this.Controls.Add(this.lblLLT);
            this.Controls.Add(this.dtET);
            this.Controls.Add(this.lblET);
            this.Controls.Add(this.dtELT);
            this.Controls.Add(this.lblELT);
            this.Controls.Add(this.dtLAT);
            this.Controls.Add(this.lblLAT);
            this.Controls.Add(this.dtST);
            this.Controls.Add(this.lblST);
            this.Controls.Add(this.dtEAT);
            this.Controls.Add(this.lblEAT);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(264, 464);
            this.MinimumSize = new System.Drawing.Size(264, 464);
            this.Name = "WTSchemaAdvance";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Time Period Advance Settings";
            this.Load += new System.EventHandler(this.WTSchemaAdvance_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WTSchemaAdvance_KeyUp);
            this.gbAutoClose.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void setLanguage()
		{
            try
            {
                lblEAT.Text = rm.GetString("lblEAT", culture);
                lblST.Text = rm.GetString("lblST", culture);
                lblLAT.Text = rm.GetString("lblLAT", culture);
                lblELT.Text = rm.GetString("lblELT", culture);
                lblET.Text = rm.GetString("lblET", culture);
                lblLLT.Text = rm.GetString("lblLLT", culture);

                this.Text = rm.GetString("WTAdvanceTitle", culture);
                btnCancel.Text = rm.GetString("btnCancel", culture);
                btnOK.Text = rm.GetString("btnOK", culture);

                gbAutoClose.Text = rm.GetString("gbAutoClose", culture);
                rbWithout.Text = rm.GetString("rbWithout", culture);
                rbStart.Text = rm.GetString("rbStart", culture);
                rbEnd.Text = rm.GetString("rbEnd", culture);
                rbStartEnd.Text = rm.GetString("rbStartEnd", culture);
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdvance.setLanguage(): " + ex.Message + "\n");
                throw ex;
            }
		}

		private void WTSchemaAdvance_Load(object sender, System.EventArgs e)
		{
            try
            {
                dtEAT.Value = EarliestArrived;
                dtST.Value = StartTime;
                dtLAT.Value = LatestArrived;
                dtELT.Value = EarliestLeft;
                dtET.Value = EndTime;
                dtLLT.Value = LatestLeft;            
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdvance.WTSchemaAdvance_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Saved = false;
			this.Close();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
            try
            {
                Saved = true;

                StartTime = dtST.Value;
                EndTime = dtET.Value;
                EarliestArrived = dtEAT.Value;
                LatestArrived = dtLAT.Value;
                EarliestLeft = dtELT.Value;
                LatestLeft = dtLLT.Value;

                if (rbStartEnd.Checked)
                {
                    AutoClose = (int)Constants.AutoClose.StartEndClose;
                }
                else if (rbEnd.Checked)
                {
                    AutoClose = (int)Constants.AutoClose.EndClose;
                }
                else if (rbStart.Checked)
                {
                    AutoClose = (int)Constants.AutoClose.StartClose;
                }
                else
                {
                    AutoClose = (int)Constants.AutoClose.WithoutClose;
                }

                this.Close();
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaAdvance.btnOK_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
		}

        private void WTSchemaAdvance_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " WTSchemaAdvance.WTSchemaAdvance_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
