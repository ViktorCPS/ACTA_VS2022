using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Provide user to define woking time period in schema
	/// </summary>
	public class WTSchemaPeriod : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblDay;
		private System.Windows.Forms.Label lblStart;
		private System.Windows.Forms.Label lblEnd;
		private System.Windows.Forms.DateTimePicker dtStart;
		private System.Windows.Forms.DateTimePicker dtEnd;
		private System.Windows.Forms.NumericUpDown tolerance;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnDetails;
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Label lblTolernce;
		private System.Windows.Forms.TextBox tbDay;
		private System.Windows.Forms.TextBox tbInterval;
		private System.Windows.Forms.Label lblInterval;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Language
		private ResourceManager rm;
		private CultureInfo culture;

		private WorkTimeSchemaTO currentSchema;
        private WorkTimeIntervalTO currentInterval;

		// Observer 
		private NotificationController Controller;
		private System.Windows.Forms.CheckBox cbUniformTolerance;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;

		// Debug log
		DebugLog log;
		private System.Windows.Forms.GroupBox gbPause;
		private System.Windows.Forms.ComboBox cbPause;
		private System.Windows.Forms.Label lblPause;
		ApplUserTO logInUser;
        private TextBox tbDescription;
        private Label lblDescritpion;

		string pauseID = "";

		// Add 
		public WTSchemaPeriod(WorkTimeSchemaTO schema, string pauseID)
		{
			InitializeComponent();
			this.CenterToScreen();

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			// Init Observer
			InitialiseObserverClient();

			// Language
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();

			currentSchema = schema;
			currentInterval = new WorkTimeIntervalTO();
			logInUser = NotificationController.GetLogInUser();

			this.pauseID = pauseID;
			
			// Enable fields
			tbDay.Enabled = true;
			tbInterval.Enabled = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
		}

		// Update
		public WTSchemaPeriod(WorkTimeSchemaTO schema, int dayNum, int intervalNum, string pauseID)
		{
			InitializeComponent();
			this.CenterToScreen();

			// Init Observer
			InitialiseObserverClient();

			// Language
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());
			rm = new ResourceManager("UI.Resource",typeof(Locations).Assembly);
			setLanguage();

			currentSchema = schema;
            currentInterval = new WorkTimeIntervalTO();
            if (schema.Days.ContainsKey(dayNum) && schema.Days[dayNum].ContainsKey(intervalNum))
                currentInterval = schema.Days[dayNum][intervalNum];
			logInUser = NotificationController.GetLogInUser();

			this.pauseID = pauseID;

			// Enable fields
			tbDay.Enabled = false;
			tbInterval.Enabled = false;
			btnSave.Visible = false;
			btnUpdate.Visible = true;

			// Debug
			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbUniformTolerance = new System.Windows.Forms.CheckBox();
            this.lblInterval = new System.Windows.Forms.Label();
            this.tbInterval = new System.Windows.Forms.TextBox();
            this.tbDay = new System.Windows.Forms.TextBox();
            this.lblTolernce = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.tolerance = new System.Windows.Forms.NumericUpDown();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblDay = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDetails = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.gbPause = new System.Windows.Forms.GroupBox();
            this.cbPause = new System.Windows.Forms.ComboBox();
            this.lblPause = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescritpion = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tolerance)).BeginInit();
            this.gbPause.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbUniformTolerance);
            this.groupBox1.Controls.Add(this.lblInterval);
            this.groupBox1.Controls.Add(this.tbInterval);
            this.groupBox1.Controls.Add(this.tbDay);
            this.groupBox1.Controls.Add(this.lblTolernce);
            this.groupBox1.Controls.Add(this.lblMin);
            this.groupBox1.Controls.Add(this.tolerance);
            this.groupBox1.Controls.Add(this.dtEnd);
            this.groupBox1.Controls.Add(this.dtStart);
            this.groupBox1.Controls.Add(this.lblEnd);
            this.groupBox1.Controls.Add(this.lblStart);
            this.groupBox1.Controls.Add(this.lblDay);
            this.groupBox1.Location = new System.Drawing.Point(8, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 272);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Period";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(152, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "*";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(152, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "*";
            // 
            // cbUniformTolerance
            // 
            this.cbUniformTolerance.Checked = true;
            this.cbUniformTolerance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUniformTolerance.Location = new System.Drawing.Point(16, 192);
            this.cbUniformTolerance.Name = "cbUniformTolerance";
            this.cbUniformTolerance.Size = new System.Drawing.Size(168, 24);
            this.cbUniformTolerance.TabIndex = 10;
            this.cbUniformTolerance.Text = "Uniform tolerance";
            this.cbUniformTolerance.CheckedChanged += new System.EventHandler(this.cbUniformTolerance_CheckedChanged);
            // 
            // lblInterval
            // 
            this.lblInterval.Location = new System.Drawing.Point(32, 56);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(48, 23);
            this.lblInterval.TabIndex = 3;
            this.lblInterval.Text = "Interval:";
            this.lblInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbInterval
            // 
            this.tbInterval.Location = new System.Drawing.Point(88, 57);
            this.tbInterval.Name = "tbInterval";
            this.tbInterval.Size = new System.Drawing.Size(64, 20);
            this.tbInterval.TabIndex = 4;
            // 
            // tbDay
            // 
            this.tbDay.Location = new System.Drawing.Point(88, 24);
            this.tbDay.Name = "tbDay";
            this.tbDay.Size = new System.Drawing.Size(64, 20);
            this.tbDay.TabIndex = 1;
            // 
            // lblTolernce
            // 
            this.lblTolernce.Location = new System.Drawing.Point(8, 232);
            this.lblTolernce.Name = "lblTolernce";
            this.lblTolernce.Size = new System.Drawing.Size(72, 23);
            this.lblTolernce.TabIndex = 11;
            this.lblTolernce.Text = "Tolerance:";
            this.lblTolernce.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMin
            // 
            this.lblMin.Location = new System.Drawing.Point(136, 232);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(40, 23);
            this.lblMin.TabIndex = 13;
            this.lblMin.Text = "min";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tolerance
            // 
            this.tolerance.Location = new System.Drawing.Point(88, 232);
            this.tolerance.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.tolerance.Name = "tolerance";
            this.tolerance.Size = new System.Drawing.Size(48, 20);
            this.tolerance.TabIndex = 12;
            // 
            // dtEnd
            // 
            this.dtEnd.CustomFormat = "HH:mm";
            this.dtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEnd.Location = new System.Drawing.Point(88, 152);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.ShowUpDown = true;
            this.dtEnd.Size = new System.Drawing.Size(64, 20);
            this.dtEnd.TabIndex = 9;
            // 
            // dtStart
            // 
            this.dtStart.CustomFormat = "HH:mm";
            this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStart.Location = new System.Drawing.Point(88, 120);
            this.dtStart.Name = "dtStart";
            this.dtStart.ShowUpDown = true;
            this.dtStart.Size = new System.Drawing.Size(64, 20);
            this.dtStart.TabIndex = 7;
            // 
            // lblEnd
            // 
            this.lblEnd.Location = new System.Drawing.Point(16, 152);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(64, 23);
            this.lblEnd.TabIndex = 8;
            this.lblEnd.Text = "End Time:";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStart
            // 
            this.lblStart.Location = new System.Drawing.Point(16, 120);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(64, 23);
            this.lblStart.TabIndex = 6;
            this.lblStart.Text = "Start Time:";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDay
            // 
            this.lblDay.Location = new System.Drawing.Point(48, 23);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(32, 23);
            this.lblDay.TabIndex = 0;
            this.lblDay.Text = "Day:";
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(56, 475);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(152, 475);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDetails
            // 
            this.btnDetails.Location = new System.Drawing.Point(80, 419);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(128, 23);
            this.btnDetails.TabIndex = 4;
            this.btnDetails.Text = "Details >>";
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(56, 475);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // gbPause
            // 
            this.gbPause.Controls.Add(this.cbPause);
            this.gbPause.Controls.Add(this.lblPause);
            this.gbPause.Location = new System.Drawing.Point(8, 296);
            this.gbPause.Name = "gbPause";
            this.gbPause.Size = new System.Drawing.Size(264, 64);
            this.gbPause.TabIndex = 1;
            this.gbPause.TabStop = false;
            this.gbPause.Text = "Pause";
            // 
            // cbPause
            // 
            this.cbPause.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPause.Location = new System.Drawing.Point(72, 24);
            this.cbPause.Name = "cbPause";
            this.cbPause.Size = new System.Drawing.Size(180, 21);
            this.cbPause.TabIndex = 1;
            // 
            // lblPause
            // 
            this.lblPause.Location = new System.Drawing.Point(16, 24);
            this.lblPause.Name = "lblPause";
            this.lblPause.Size = new System.Drawing.Size(48, 23);
            this.lblPause.TabIndex = 0;
            this.lblPause.Text = "Pause:";
            this.lblPause.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(96, 378);
            this.tbDescription.MaxLength = 10;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(64, 20);
            this.tbDescription.TabIndex = 3;
            // 
            // lblDescritpion
            // 
            this.lblDescritpion.Location = new System.Drawing.Point(19, 377);
            this.lblDescritpion.Name = "lblDescritpion";
            this.lblDescritpion.Size = new System.Drawing.Size(69, 23);
            this.lblDescritpion.TabIndex = 2;
            this.lblDescritpion.Text = "Description:";
            this.lblDescritpion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WTSchemaPeriod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(282, 508);
            this.ControlBox = false;
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblDescritpion);
            this.Controls.Add(this.gbPause);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDetails);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "WTSchemaPeriod";
            this.ShowInTaskbar = false;
            this.Text = "Schema Time Period";
            this.Load += new System.EventHandler(this.WTSchemaPeriod_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.WTSchemaPeriod_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tolerance)).EndInit();
            this.gbPause.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void setLanguage()
		{
			try
			{
				this.Text = rm.GetString("WTSchPeriod", culture);

				this.groupBox1.Text = rm.GetString("WTSchPeriod", culture);
				this.btnCancel.Text = rm.GetString("btnCancel", culture);
				this.btnSave.Text = rm.GetString("btnSave", culture);
				this.btnDetails.Text = rm.GetString("btnDetails", culture);
				this.lblDay.Text = rm.GetString("lblDay", culture);
				this.lblEnd.Text = rm.GetString("lblEndTime", culture);
				this.lblStart.Text = rm.GetString("lblStartTime", culture);
				this.lblTolernce.Text = rm.GetString("lblTolernce", culture);
				this.lblMin.Text = rm.GetString("lblMin", culture);
                this.lblDescritpion.Text = rm.GetString("lblDescritpion", culture);
				this.cbUniformTolerance.Text = rm.GetString("cbUniformTolerance", culture);
				this.btnUpdate.Text = rm.GetString("btnUpdate", culture);
				gbPause.Text = rm.GetString("gbPause", culture);
				lblPause.Text = rm.GetString("lblPause", culture);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaPeriod.setLanguage(): " + ex.Message + "\n");
				throw ex;
			}
		}

		private void WTSchemaPeriod_Load(object sender, System.EventArgs e)
		{
			int maxDayNum = currentSchema.Days.Count;
            Dictionary<int, WorkTimeIntervalTO> lastDay = new Dictionary<int,WorkTimeIntervalTO>();
			int maxInt = 0;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                populatePauseCombo();

                // Fill form with default values (mode Add)
                if (currentInterval != null)
                {
                    // Add
                    if ((currentInterval.DayNum < 0) && (currentInterval.IntervalNum < 0))
                    {
                        if ((maxDayNum == 0) || (!currentSchema.Days.ContainsKey(maxDayNum - 1)))
                        {
                            maxDayNum = 1;
                            maxInt = 1;
                        }
                        else
                        {
                            lastDay = currentSchema.Days[maxDayNum - 1];
                            maxInt = lastDay.Count + 1;
                        }

                        tbDay.Text = maxDayNum.ToString();
                        tbInterval.Text = maxInt.ToString();

                        dtStart.Value = new DateTime(1900, 1, 1, 0, 0, 0);
                        dtEnd.Value = new DateTime(1900, 1, 1, 0, 0, 0);
                    }
                    // Update
                    else
                    {
                        tbDay.Text = (currentInterval.DayNum + 1).ToString();
                        tbInterval.Text = (currentInterval.IntervalNum + 1).ToString();

                        dtStart.Value = currentInterval.StartTime;
                        dtEnd.Value = currentInterval.EndTime;
                    }

                    tbDescription.Text = currentInterval.Description;
                    setTolerance();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaPeriod.WTSchemaPeriod_Load(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void populatePauseCombo()
		{
			try
			{
				List<TimeSchemaPauseTO> pauseArray = new List<TimeSchemaPauseTO>();

				pauseArray = new TimeSchemaPause().Search("");
				pauseArray.Insert(0, new TimeSchemaPauseTO(-1, rm.GetString("all", culture), -1, -1, -1, -1, -1));

				cbPause.DataSource    = pauseArray;
				cbPause.DisplayMember = "Description";
				cbPause.ValueMember   = "PauseID";

				cbPause.SelectedValue = Int32.Parse(pauseID);
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaAdd.populatePauseCombo(): " + ex.Message + "\n");
				throw ex;
			}
		}

		private void btnDetails_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DateTime startTimeTemp = dtStart.Value;
                DateTime endTimeTemp = dtEnd.Value;

                DateTime earliestArrivedTemp = new DateTime();
                DateTime latestArrivaedTemp = new DateTime();
                DateTime earliestLeftTemp = new DateTime();
                DateTime latestLeftTemp = new DateTime();
                int autoClose = currentInterval.AutoClose;

                if (tolerance.Enabled)
                {
                    earliestArrivedTemp = startTimeTemp.AddMinutes(Convert.ToDouble(tolerance.Value) * -1);
                    latestArrivaedTemp = startTimeTemp.AddMinutes(Convert.ToDouble(tolerance.Value));
                    earliestLeftTemp = endTimeTemp.AddMinutes(Convert.ToDouble(tolerance.Value) * -1);
                    latestLeftTemp = endTimeTemp.AddMinutes(Convert.ToDouble(tolerance.Value));
                }
                else
                {
                    earliestArrivedTemp = currentInterval.EarliestArrived;
                    if (earliestArrivedTemp.Equals(new DateTime()))
                        earliestArrivedTemp = new DateTime(1900, 1, 1);

                    latestArrivaedTemp = currentInterval.LatestArrivaed;
                    if (latestArrivaedTemp.Equals(new DateTime()))
                        latestArrivaedTemp = new DateTime(1900, 1, 1);

                    earliestLeftTemp = currentInterval.EarliestLeft;
                    if (earliestLeftTemp.Equals(new DateTime()))
                        earliestLeftTemp = new DateTime(1900, 1, 1);

                    latestLeftTemp = currentInterval.LatestLeft;
                    if (latestLeftTemp.Equals(new DateTime()))
                        latestLeftTemp = new DateTime(1900, 1, 1);
                }

                WTSchemaAdvance advance = new WTSchemaAdvance(earliestArrivedTemp, startTimeTemp,
                    latestArrivaedTemp, earliestLeftTemp, endTimeTemp, latestLeftTemp, autoClose);
                advance.ShowDialog(this);

                if (advance.Saved)
                {
                    currentInterval.EarliestArrived = advance.EarliestArrived;
                    currentInterval.StartTime = advance.StartTime;
                    currentInterval.LatestArrivaed = advance.LatestArrived;
                    currentInterval.EarliestLeft = advance.EarliestLeft;
                    currentInterval.EndTime = advance.EndTime;
                    currentInterval.LatestLeft = advance.LatestLeft;
                    currentInterval.AutoClose = advance.AutoClose;

                    // Fill Text Boxes
                    dtStart.Value = currentInterval.StartTime;
                    dtEnd.Value = currentInterval.EndTime;

                    setTolerance();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaPeriod.btnDetails_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
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
                log.writeLog(DateTime.Now + " WTSchemaPeriod.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{

            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.validate())
                {
                    currentInterval.DayNum = Int32.Parse(tbDay.Text) - 1;
                    currentInterval.IntervalNum = Int32.Parse(tbInterval.Text) - 1;
                    currentInterval.TimeSchemaID = currentSchema.TimeSchemaID;

                    currentInterval.StartTime = dtStart.Value;
                    currentInterval.EndTime = dtEnd.Value;
                    currentInterval.Description = tbDescription.Text.ToString().Trim();

                    if (cbPause.SelectedIndex > 0)
                        currentInterval.PauseID = Int32.Parse(cbPause.SelectedValue.ToString());
                    else
                        currentInterval.PauseID = -1;

                    if (tolerance.Enabled)
                    {
                        currentInterval.EarliestArrived = currentInterval.StartTime.AddMinutes(Convert.ToDouble(tolerance.Value) * -1);
                        currentInterval.LatestArrivaed = currentInterval.StartTime.AddMinutes(Convert.ToDouble(tolerance.Value));
                        currentInterval.EarliestLeft = currentInterval.EndTime.AddMinutes(Convert.ToDouble(tolerance.Value) * -1);
                        currentInterval.LatestLeft = currentInterval.EndTime.AddMinutes(Convert.ToDouble(tolerance.Value));

                        if (!validateSchema(currentInterval))
                        {
                            return;
                        }

                    }

                    Dictionary<int, WorkTimeIntervalTO> Intervals = new Dictionary<int, WorkTimeIntervalTO>();

                    if (!currentSchema.Days.ContainsKey(currentInterval.DayNum))
                    {
                        Intervals = new Dictionary<int, WorkTimeIntervalTO>();
                        Intervals.Add(currentInterval.IntervalNum, currentInterval);
                        currentSchema.Days.Add(currentInterval.DayNum, Intervals);
                    }
                    else
                    {
                        Intervals = currentSchema.Days[currentInterval.DayNum];
                        Intervals.Add(currentInterval.IntervalNum, currentInterval);
                    }

                    setTolerance();
                    MessageBox.Show(rm.GetString("schemaIntSaved", culture));
                    Controller.TimeShemaChanged(currentSchema);
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaPeriod.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		public void InitialiseObserverClient()
		{
			Controller = NotificationController.GetInstance();
		}

		private void cbUniformTolerance_CheckedChanged(object sender, System.EventArgs e)
		{
			if (cbUniformTolerance.Checked)
			{
				tolerance.Enabled = true;
			}
			else
			{
				tolerance.Enabled = false;
			}
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                // Start / End time
                if (dtStart.Value > dtEnd.Value)
                {
                    MessageBox.Show(rm.GetString("msgTimeNotCorrect", culture));
                    return;
                }
                currentInterval.StartTime = dtStart.Value;
                //currentInterval.TimeSchemaID = currentSchema.TimeSchemaID;

                if (cbPause.SelectedIndex > 0)
                    currentInterval.PauseID = Int32.Parse(cbPause.SelectedValue.ToString());
                else
                    currentInterval.PauseID = -1;
                currentInterval.Description = tbDescription.Text.ToString().Trim();
                if (tolerance.Enabled)
                {
                    currentInterval.EarliestArrived = currentInterval.StartTime.AddMinutes(Convert.ToDouble(tolerance.Value) * -1);
                    currentInterval.LatestArrivaed = currentInterval.StartTime.AddMinutes(Convert.ToDouble(tolerance.Value));

                    currentInterval.EndTime = dtEnd.Value;
                    currentInterval.EarliestLeft = currentInterval.EndTime.AddMinutes(Convert.ToDouble(tolerance.Value) * -1);
                    currentInterval.LatestLeft = currentInterval.EndTime.AddMinutes(Convert.ToDouble(tolerance.Value));
                }

                // Update Day array in to currentSchema
                //((TimeSchemaDay) (currentSchema.Days[currentInterval.DayNum])).Intervals[currentInterval.IntervalNum] = currentInterval.Clone();

                if (validateSchema(currentInterval))
                {
                    MessageBox.Show(rm.GetString("schemaIntChanged", culture));
                    Controller.TimeShemaChanged(currentSchema);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " WTSchemaPeriod.btnUpdate_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private bool validate()
		{
			int dayNum;
			int intNum;

			try
			{
				// Day Number
				if (!tbDay.Text.Trim().Equals(""))
				{
					
					try
					{
						if ((dayNum = Int32.Parse(tbDay.Text.Trim()) - 1) < 0)
						{
							MessageBox.Show(rm.GetString("msgNotValidDayNum", culture));
							return false;
						}
					}
					//catch(Exception ex)
					catch
					{
						MessageBox.Show(rm.GetString("msgNotValidDayNum", culture));
						return false;
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("msgEnterDayNum", culture));
					return false;
				}

				// Interval number
				if (!tbInterval.Text.Trim().Equals(""))
				{
					try
					{
						if ((intNum = Int32.Parse(tbInterval.Text) - 1) < 0)
						{
							MessageBox.Show(rm.GetString("msgNotValidIntNum", culture));
							return false;
						}

						if (currentSchema.Days.ContainsKey(dayNum))
						{
							if (currentSchema.Days[dayNum].ContainsKey(intNum))
							{
								MessageBox.Show(rm.GetString("msgInterExists", culture));
								return false;
							}
						}
						
					}
					//catch(Exception ex)
					catch
					{
						MessageBox.Show(rm.GetString("msgNotValidIntNum", culture));
						return false;
					}
				}
				else
				{
					MessageBox.Show(rm.GetString("msgEnterInterNum", culture));
					return false;
				}
				
				// Start / End time
				if (dtStart.Value > dtEnd.Value)
				{
					MessageBox.Show(rm.GetString("msgTimeNotCorrect", culture));
					return false;
				}

				// Start Time
				

				// Tolerance
				if (tolerance.Value < 0)
				{
					MessageBox.Show(rm.GetString("msgTolNotCorrect", culture));
					return false;
				}
				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaPeriod.validate(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return true;
		}

        public bool validateSchema(WorkTimeIntervalTO currentInterval)
		{
			bool isCorrect = false;

			try
			{
				if ((currentInterval.EarliestArrived > currentInterval.StartTime) || 
					(currentInterval.StartTime > currentInterval.LatestArrivaed) ||
					(currentInterval.EarliestLeft > currentInterval.EndTime) ||
					(currentInterval.EndTime > currentInterval.LatestLeft) ||
					(currentInterval.StartTime > currentInterval.EndTime))
				{
					MessageBox.Show(rm.GetString("msgTimeNotCorrect", culture));
					isCorrect = false;
				}
				else
				{
					isCorrect = true;
				}
				
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaPeriod.validate(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}

			return isCorrect;
		}
			 

		private void setTolerance()
		{
			try
			{
				TimeSpan ts0  = new TimeSpan();
				ts0 = currentInterval.StartTime.Subtract(currentInterval.EarliestArrived);

				int tm0;
				if (ts0.TotalMinutes < 0)
				{
					tm0 = 60 + Convert.ToInt32(ts0.TotalMinutes);
				}
				else
				{
					tm0 = Convert.ToInt32(ts0.TotalMinutes);
				}

				TimeSpan ts1 = new TimeSpan();
				ts1 = currentInterval.LatestArrivaed.Subtract(currentInterval.StartTime);
				int tm1;
				if (ts1.TotalMinutes < 0)
				{
					tm1 = 60 + Convert.ToInt32(ts1.TotalMinutes);
				}
				else
				{
					tm1 = Convert.ToInt32(ts1.TotalMinutes);
				}

				TimeSpan ts2 = new TimeSpan();
				ts2 = currentInterval.EndTime.Subtract(currentInterval.EarliestLeft);
				int tm2;
				if (ts2.TotalMinutes < 0)
				{
					tm2 = 60 + Convert.ToInt32(ts2.TotalMinutes);
				}
				else
				{
					tm2 = Convert.ToInt32(ts2.TotalMinutes);
				}
				
				TimeSpan ts3 = new TimeSpan();
				ts3 = currentInterval.LatestLeft.Subtract(currentInterval.EndTime);
				int tm3;
				if (ts3.TotalMinutes < 0)
				{
					tm3 = 60 + Convert.ToInt32(ts3.TotalMinutes);
				}
				else
				{
					tm3 = Convert.ToInt32(ts3.TotalMinutes);
				}
				
				if ((tm0 == tm1) && (tm0 == tm2) && (tm0 == tm3) && (tm1 == tm2) && (tm1 == tm3) && (tm2 == tm3)
					&& (tm0 >= 0) && (tm1 >= 0) && (tm2 >= 0) && (tm3 >= 0))  
				{
					cbUniformTolerance.Checked = true;
					tolerance.Value = Convert.ToInt32(ts0.TotalMinutes.ToString());
				}
				else
				{
					cbUniformTolerance.Checked = false;
					tolerance.Enabled = false;
					tolerance.Value = 0;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " WTSchemaPeriod.setTolerance(): " + ex.Message + "\n");
				throw new Exception(ex.Message);
			}
		}

        private void WTSchemaPeriod_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " WTSchemaPeriod.WTSchemaPeriod_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
