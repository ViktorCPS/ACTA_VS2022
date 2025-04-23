using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using Common;
using TransferObjects;
using Util;

using System.Resources;
using System.Globalization;

namespace UI
{
	/// <summary>
	/// Summary description for ExitPermissionsWorkingDayBeginning.
	/// </summary>
	public class ExitPermissionsWorkingDayBeginning : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblWorkDayBeginning;
		private System.Windows.Forms.Label lblFirstIN;
		private System.Windows.Forms.Label lblPermissionPair;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private CultureInfo culture;
		ResourceManager rm;

		DebugLog log;

		ApplUser currentUser = null;

		int employeeID;
		DateTime date;
		DateTime start = new DateTime(0);
        private DateTimePicker dtpExitPermStartTime;
		DateTime firstIN = new DateTime(0);

        private DateTime earliestArrivalTime = new DateTime();
        private Label lblEarliest;
        private Label lblLatest;
        private DateTime latestArrivalTime = new DateTime();

		public ExitPermissionsWorkingDayBeginning(int emplID, DateTime date)
		{
			InitializeComponent();
	
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentUser = new ApplUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource", typeof(ApplUsers).Assembly);
			setLanguage();
			this.employeeID = emplID;
			this.date = date;
            dtpExitPermStartTime.Visible = false;
            lblEarliest.Visible = false;
            lblLatest.Visible = false;
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
            this.lblWorkDayBeginning = new System.Windows.Forms.Label();
            this.lblFirstIN = new System.Windows.Forms.Label();
            this.lblPermissionPair = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dtpExitPermStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblEarliest = new System.Windows.Forms.Label();
            this.lblLatest = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblWorkDayBeginning
            // 
            this.lblWorkDayBeginning.Location = new System.Drawing.Point(16, 36);
            this.lblWorkDayBeginning.Name = "lblWorkDayBeginning";
            this.lblWorkDayBeginning.Size = new System.Drawing.Size(304, 23);
            this.lblWorkDayBeginning.TabIndex = 0;
            this.lblWorkDayBeginning.Text = "Working day beginning:";
            this.lblWorkDayBeginning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFirstIN
            // 
            this.lblFirstIN.Location = new System.Drawing.Point(16, 108);
            this.lblFirstIN.Name = "lblFirstIN";
            this.lblFirstIN.Size = new System.Drawing.Size(304, 23);
            this.lblFirstIN.TabIndex = 1;
            this.lblFirstIN.Text = "First IN:";
            this.lblFirstIN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPermissionPair
            // 
            this.lblPermissionPair.Location = new System.Drawing.Point(16, 143);
            this.lblPermissionPair.Name = "lblPermissionPair";
            this.lblPermissionPair.Size = new System.Drawing.Size(304, 24);
            this.lblPermissionPair.TabIndex = 2;
            this.lblPermissionPair.Text = "Exit permission will cover period:";
            this.lblPermissionPair.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(12, 182);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(241, 182);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dtpExitPermStartTime
            // 
            this.dtpExitPermStartTime.CustomFormat = "HH:mm";
            this.dtpExitPermStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExitPermStartTime.Location = new System.Drawing.Point(145, 36);
            this.dtpExitPermStartTime.Name = "dtpExitPermStartTime";
            this.dtpExitPermStartTime.ShowUpDown = true;
            this.dtpExitPermStartTime.Size = new System.Drawing.Size(61, 20);
            this.dtpExitPermStartTime.TabIndex = 5;
            this.dtpExitPermStartTime.ValueChanged += new System.EventHandler(this.dtpExitPermStartTime_ValueChanged);
            // 
            // lblEarliest
            // 
            this.lblEarliest.Location = new System.Drawing.Point(16, 0);
            this.lblEarliest.Name = "lblEarliest";
            this.lblEarliest.Size = new System.Drawing.Size(304, 23);
            this.lblEarliest.TabIndex = 6;
            this.lblEarliest.Text = "Earliest arive:";
            this.lblEarliest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLatest
            // 
            this.lblLatest.Location = new System.Drawing.Point(16, 74);
            this.lblLatest.Name = "lblLatest";
            this.lblLatest.Size = new System.Drawing.Size(304, 23);
            this.lblLatest.TabIndex = 7;
            this.lblLatest.Text = "Latest arive:";
            this.lblLatest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ExitPermissionsWorkingDayBeginning
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(328, 217);
            this.ControlBox = false;
            this.Controls.Add(this.lblLatest);
            this.Controls.Add(this.lblEarliest);
            this.Controls.Add(this.dtpExitPermStartTime);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPermissionPair);
            this.Controls.Add(this.lblFirstIN);
            this.Controls.Add(this.lblWorkDayBeginning);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExitPermissionsWorkingDayBeginning";
            this.ShowInTaskbar = false;
            this.Text = "ExitPermissionsWorkingDayBeginning";
            this.Load += new System.EventHandler(this.ExitPermissionsWorkingDayBeginning_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void setLanguage()
		{
			try
			{
				// Form name
				this.Text = rm.GetString("ExitPermWorkDayBeginningForm", culture);

				// button's text
				btnOK.Text = rm.GetString("btnOK", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblWorkDayBeginning.Text = rm.GetString("lblWorkDayBeginning", culture);
				lblFirstIN.Text = rm.GetString("lblFirstIN", culture);
				lblPermissionPair.Text = rm.GetString("lblPermissionPair", culture);
                lblLatest.Text = rm.GetString("lblLatest", culture);
                lblEarliest.Text = rm.GetString("lblEarliest", culture);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsWorkingDayBeginning.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
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
                log.writeLog(DateTime.Now + " ExitPermissionsWorkingDayBeginning.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void ExitPermissionsWorkingDayBeginning_Load(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;
				Employee empl = new Employee();
				empl.EmplTO.EmployeeID = employeeID;
				ArrayList schemas = empl.findTimeSchema(date);
				start = new DateTime(1, 1, 1, 23, 59, 59);

				// find beginning of working day for that date
				if (schemas.Count > 1)
				{
					WorkTimeSchemaTO schema = (WorkTimeSchemaTO)schemas[0];
					int dayNum = (int) schemas[1];

					Dictionary<int, WorkTimeIntervalTO> intervals = schema.Days[dayNum];

					foreach (int intNum in intervals.Keys)
					{
                        WorkTimeIntervalTO interval = intervals[intNum];
                        if (schema.Type.Trim() != Constants.schemaTypeFlexi)
                        {                           
                            if (!intervals[intNum].StartTime.TimeOfDay.Equals(new DateTime().TimeOfDay)
                                && intervals[intNum].StartTime.TimeOfDay < start.TimeOfDay)
                            {
                                start = intervals[intNum].StartTime;
                            }
                        }

                        // if flexi time schema set start time for the permission to the Latest Arrived time of the schema <--> Darko 4.12.2007.
                        // if flexi let costumer choose start time for permission <--> Natasa 23.01.2009.
                        else
                        {
                            List<int> emplList = new List<int>();
                            emplList.Add(empl.EmplTO.EmployeeID);
                            List<IOPairTO> ioPairList = new IOPair().SearchForEmployees(date, date, emplList, -1);
                            foreach (IOPairTO iopair in ioPairList)
                            {
                                if (!interval.StartTime.TimeOfDay.Equals(new DateTime(0).TimeOfDay) && !interval.EndTime.TimeOfDay.Equals(new DateTime(0).TimeOfDay)
                                    && iopair.EndTime.TimeOfDay > interval.EarliestLeft.TimeOfDay && iopair.EndTime.TimeOfDay < interval.LatestLeft.TimeOfDay)
                                {
                                    start =new DateTime(iopair.EndTime.Ticks - (interval.EndTime.Ticks - interval.StartTime.Ticks));
                                }
                                if (iopair.EndTime.TimeOfDay > interval.LatestLeft.TimeOfDay && iopair.StartTime.TimeOfDay < interval.LatestLeft.TimeOfDay)
                                {
                                    if (!intervals[intNum].LatestArrivaed.TimeOfDay.Equals(new DateTime().TimeOfDay)
                                && intervals[intNum].LatestArrivaed.TimeOfDay < start.TimeOfDay)
                                    {
                                        start = intervals[intNum].LatestArrivaed;
                                    }
                                }
                            }
                            if (!intervals[intNum].StartTime.TimeOfDay.Equals(new DateTime().TimeOfDay)
                                && intervals[intNum].StartTime.TimeOfDay < start.TimeOfDay)
                            {
                                earliestArrivalTime  = intervals[intNum].EarliestArrived;
                                latestArrivalTime = intervals[intNum].LatestArrivaed;
                            }
                            if (start.Equals(new DateTime(1, 1, 1, 23, 59, 59)))
                            {
                                if (!intervals[intNum].StartTime.TimeOfDay.Equals(new DateTime().TimeOfDay)
                                && intervals[intNum].StartTime.TimeOfDay < start.TimeOfDay)
                                {
                                    start = intervals[intNum].StartTime;
                                }
                            }
                            if (!start.Equals(new DateTime(1, 1, 1, 23, 59, 59)))
                            {
                                dtpExitPermStartTime.Value = start;
                                dtpExitPermStartTime.Visible = true;
                                lblEarliest.Text += " " + earliestArrivalTime.ToString("HH:mm");
                                lblLatest.Text += " " + latestArrivalTime.ToString("HH:mm");
                                lblEarliest.Visible = true;
                                lblLatest.Visible = true;
                            }                           
                        }
					}

					if (!start.Equals(new DateTime(1, 1, 1, 23, 59, 59)))
					{
						start = new DateTime(date.Year, date.Month, date.Day, start.Hour, start.Minute, start.Second);
						lblWorkDayBeginning.Text += " " + start.ToString("HH:mm");
					}
				}

				// find first pass for that day and show it if it is IN and is after beginning of working day
                DateTime timeFrom = new DateTime(1900, 1, 1, 0, 0, 0);
                DateTime timeTo = new DateTime(1900, 1, 1, 23, 59, 0);
                Pass pass = new Pass();
                pass.PssTO.EmployeeID = empl.EmplTO.EmployeeID;
                List<PassTO> passes = pass.SearchInterval(date, date, "", timeFrom, timeTo);

				PassTO passTO = new PassTO();
				passTO.EventTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

                bool passOnStartNotExist = true;
                foreach (PassTO p in passes)
                {
                    if ((p.IsWrkHrsCount == 1) && p.Direction.Equals(Constants.DirectionIn) && p.EventTime.TimeOfDay <= start.TimeOfDay)
                    {
                        passOnStartNotExist = false;
                        break;
                    }
                }

                if (passOnStartNotExist)
                {
                    foreach (PassTO p in passes)
                    {
                        if ((p.IsWrkHrsCount == 1) && p.Direction.Equals(Constants.DirectionIn) && p.EventTime.TimeOfDay > start.TimeOfDay
                            && p.EventTime.TimeOfDay < passTO.EventTime.TimeOfDay)
                        {
                            passTO = p;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("passOnStartAlreadyExists", culture));
                    this.Close();
                }

				if (passTO.PassID != -1)
				{
					firstIN = passTO.EventTime;
					lblFirstIN.Text += " " + firstIN.ToString("HH:mm");
				}

				if (!start.Equals(new DateTime(1, 1, 1, 23, 59, 59)) && passTO.PassID != -1)
				{
                    lblPermissionPair.Text = rm.GetString("lblPermissionPair", culture) + " " + start.ToString("HH:mm") + "-" + firstIN.ToString("HH:mm");
				}

			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsWorkingDayBeginning.ExitPermissionsWorkingDayBeginning_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
                this.Close();
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;

				if (this.Owner is ExitPermissionsAdd)
				{
                    if (dtpExitPermStartTime.Visible)
                    {
                        ((ExitPermissionsAdd)this.Owner).setStartTime(new DateTime(start.Year,start.Month,start.Day, dtpExitPermStartTime.Value.Hour,dtpExitPermStartTime.Value.Minute,0));
                    }
					else if(!start.Equals(new DateTime (1, 1, 1, 23, 59, 59)))
					{
						((ExitPermissionsAdd) this.Owner).setStartTime(start);
					}

					if(!firstIN.Equals(new DateTime (0)))
					{
						((ExitPermissionsAdd) this.Owner).setFirstIN(firstIN);
					}
				}

				this.Close();
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " ExitPermissionsWorkingDayBeginning.btnOK_Click() " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void dtpExitPermStartTime_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!earliestArrivalTime.Equals(new DateTime()) && !latestArrivalTime.Equals(new DateTime()) && !firstIN.Equals(new DateTime()))
                {
                    if (dtpExitPermStartTime.Value.TimeOfDay > latestArrivalTime.TimeOfDay)
                        dtpExitPermStartTime.Value = latestArrivalTime;
                    if (dtpExitPermStartTime.Value.TimeOfDay < earliestArrivalTime.TimeOfDay)
                        dtpExitPermStartTime.Value = earliestArrivalTime;
                    if (dtpExitPermStartTime.Value.TimeOfDay > firstIN.TimeOfDay)
                        dtpExitPermStartTime.Value = firstIN;
                    start = new DateTime(date.Year, date.Month, date.Day, dtpExitPermStartTime.Value.Hour, dtpExitPermStartTime.Value.Minute, 0);
                    lblPermissionPair.Text = rm.GetString("lblPermissionPair", culture) + " " + start.ToString("HH:mm") + "-" + firstIN.ToString("HH:mm");
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " ExitPermissionsWorkingDayBeginning.dtpExitPermStartTime_ValueChanged() " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
