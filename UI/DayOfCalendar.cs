using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Configuration;

using TransferObjects;
using Common;
using Util;


namespace UI
{
	/// <summary>
	/// Class DayOfCalendar - hold one Day of Calendar, hold Schema and day of Schema cycle for that day
	/// </summary>
	public class DayOfCalendar : Panel
	{
        const string otherMonthDay = "#E6E6C9";
        const string otherMonthNotWorkingDay = "#E6CDA3";
        const string notWorkingDay = "#FFE4B5";
        const string selectedDay = "#000080";
        const string otherMonthLockedDay = "#9CC2CF";
        const string lockedDay = "#ADD8E6";        

		private System.Windows.Forms.Label lblSchema;
		private System.Windows.Forms.Label lblDay;
		private System.Windows.Forms.Label lblCycleDay;
		
		DebugLog log;
        bool isLocked = false;

		// Controller instance
		public NotificationController Controller;
		
		// Observer client instance
		public NotificationObserverClient observerClient;

		private DateTime _date;
		private WorkTimeSchemaTO _schema;
        private string _routes;
		private int _startDay;

		public DateTime Date
		{
			get {return _date;}
			set {_date = value;}
		}

		public WorkTimeSchemaTO Schema
		{
			get {return _schema;}
			set {_schema = value;}
		}

        public string Routes
        {
            get { return _routes; }
            set { _routes = value; }
        }

		public int StartDay
		{
			get {return _startDay;}
			set {_startDay = value;}
		}

		public DayOfCalendar(int x, int y, int schemaID, string schemaText, string dayText, int cycleDay, string cycleDayText)
		{
			InitializeComponent();
			IntitObserverClient();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			this.Location = new Point(x, y);
			
			if (schemaID >= 0)
			{
				this.setSchemaText(schemaText);
			}
			this.lblDay.Text = dayText;
			if (cycleDay >= 0)
			{
				this.setCycleDayText(cycleDayText);
			}

			this.lblSchema.Location = new Point(1, 1);
			this.lblDay.Location = new Point(1, 20);
			this.lblCycleDay.Location = new Point(30, 20);
		}

        public DayOfCalendar(int x, int y, string routes, string schemaText, string dayText)
        {
            InitializeComponent();
            IntitObserverClient();

            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
            log = new DebugLog(logFilePath);

            this.Location = new Point(x, y);

            this.Routes = routes;
            if (routes.Length > 0)
            {
                this.setSchemaText(schemaText);
            }
            this.lblDay.Text = dayText;
            this.setCycleDayText("");
            
            this.lblSchema.Location = new Point(1, 1);
            this.lblDay.Location = new Point(1, 20);
            this.lblCycleDay.Location = new Point(30, 20);
        }

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblCycleDay = new System.Windows.Forms.Label();
			this.lblDay = new System.Windows.Forms.Label();
			this.lblSchema = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblCycleDay
			// 
			this.lblCycleDay.BackColor = System.Drawing.SystemColors.Info;
			this.lblCycleDay.Location = new System.Drawing.Point(17, 17);
			this.lblCycleDay.Name = "lblCycleDay";
			this.lblCycleDay.Size = new System.Drawing.Size(54, 19);
			this.lblCycleDay.TabIndex = 2;
			this.lblCycleDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblCycleDay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblCycleDay_MouseDown);
			// 
			// lblDay
			// 
			this.lblDay.BackColor = System.Drawing.SystemColors.Info;
			this.lblDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblDay.ForeColor = System.Drawing.Color.Navy;
			this.lblDay.Location = new System.Drawing.Point(125, 17);
			this.lblDay.Name = "lblDay";
			this.lblDay.Size = new System.Drawing.Size(29, 19);
			this.lblDay.TabIndex = 1;
			this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblDay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblDay_MouseDown);
			// 
			// lblSchema
			// 
			this.lblSchema.BackColor = System.Drawing.SystemColors.Info;
			this.lblSchema.Location = new System.Drawing.Point(207, 17);
			this.lblSchema.Name = "lblSchema";
			this.lblSchema.Size = new System.Drawing.Size(83, 19);
			this.lblSchema.TabIndex = 0;
			this.lblSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblSchema.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblSchema_MouseDown);
			// 
			// DayOfCalendar
			// 
			this.BackColor = System.Drawing.SystemColors.Info;
			this.Controls.Add(this.lblSchema);
			this.Controls.Add(this.lblDay);
			this.Controls.Add(this.lblCycleDay);
			this.Location = new System.Drawing.Point(8, 8);
			this.Size = new System.Drawing.Size(85, 40);
			this.TabIndex = 1;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DayOfCalendar_MouseDown);
			this.ResumeLayout(false);

		}
		#endregion

		public void IntitObserverClient()
		{
			Controller = NotificationController.GetInstance();
			observerClient = new NotificationObserverClient(this.ToString());
		}

		public void setSchemaText(string text)
		{
			this.lblSchema.Text = text;
		}

		public void setDayText(string text)
		{
			this.lblDay.Text = text;
		}

		public void setCycleDayText(string text)
		{
			this.lblCycleDay.Text = text;
		}

		/// <summary>
		/// Select Day of calendar
		/// </summary>
		public void SelectDay()
		{
			this.BackColor = ColorTranslator.FromHtml(selectedDay);
		}

		/// <summary>
		/// Unselect Day of calendar
		/// </summary>
		public void UnselectDay()
		{
            this.BackColor = System.Drawing.SystemColors.Info;
		}

		/// <summary>
		/// If day of calendar is not working day, set different color
		/// </summary>
        public void SetNotWorkingDay()
        {
            SetDayColor(ColorTranslator.FromHtml(notWorkingDay));            
        }

        /// <summary>
        /// If day of calendar is not working day, set different color
        /// </summary>
        public void SetNotWorkingDayOtherMonth()
        {
            SetDayColor(ColorTranslator.FromHtml(otherMonthNotWorkingDay));            
        }

        /// <summary>
        /// If day of calendar is from previous/next month, set different color
        /// </summary>
        public void SetOtherMonthDay()
        {
            SetDayColor(ColorTranslator.FromHtml(otherMonthDay));
        }

        public void SetLockedDay()
        {
            SetDayColor(ColorTranslator.FromHtml(lockedDay));
            isLocked = true;
        }

        public void SetLockedDayOtherMonth()
        {            
            SetDayColor(ColorTranslator.FromHtml(otherMonthLockedDay));           
            isLocked = true;
        }

        public void SetDayColor(Color color)
        {
            this.lblSchema.BackColor = color;
            this.lblDay.BackColor = color;
            this.lblCycleDay.BackColor = color;
        }

		/// <summary>
		/// On click on day, day of calendar is selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void onMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (!isLocked)
            {
                this.SelectDay();
                Controller.DaySelected(this.Date);
            }
            else
            {
                MessageBox.Show(Common.Misc.dataLockedMessage(this.Date));
            }
            
		}
		private void DayOfCalendar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{            
			onMouseDown(sender, e);
		}

		private void lblDay_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{           
			onMouseDown(sender, e);
		}

		private void lblCycleDay_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{           
			onMouseDown(sender, e);
		}

		private void lblSchema_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{           
			onMouseDown(sender, e);
		}
	}
}
