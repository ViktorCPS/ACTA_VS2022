using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Resources;
using System.Globalization;
using System.Data;

using Common;
using Util;
using TransferObjects;

namespace UI
{
	/// <summary>
	/// Summary description for TimeSchemaPauseAdd.
	/// </summary>
	public class TimeSchemaPauseAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblShortBreakDuration;
		private System.Windows.Forms.TextBox tbShortBreakDuration;
		private System.Windows.Forms.Label lblStar7;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.GroupBox gbShortBreak;	
		private System.Windows.Forms.Label lblStar2;
		private System.Windows.Forms.TextBox tbLatestTime;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Label lblStar3;
		private System.Windows.Forms.TextBox tbPauseID;
		private System.Windows.Forms.Label lblEarliestTime;
		private System.Windows.Forms.Label lblStar5;
		private System.Windows.Forms.TextBox tbPauseDuration;
		private System.Windows.Forms.Label lblPauseID;
		private System.Windows.Forms.Label lblLatestTime;
		private System.Windows.Forms.Label lblStar6;
		private System.Windows.Forms.TextBox tbPauseOffset;
		private System.Windows.Forms.Label lblPauseOffset;
		private System.Windows.Forms.Label lblStar1;
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.Label lblPauseDuration;
		private System.Windows.Forms.Label lblStar4;
		private System.Windows.Forms.TextBox tbEarliestTime;
		private System.Windows.Forms.GroupBox gbPause;
		private System.Windows.Forms.Label lblMin7;
		private System.Windows.Forms.Label lblMin6;
		private System.Windows.Forms.Label lblMin5;
		private System.Windows.Forms.Label lblMin4;
		private System.Windows.Forms.Label lblMin3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		TimeSchemaPauseTO currentTimeSchemaPause = null;

		ApplUserTO logInUser;		
		ResourceManager rm;
		private CultureInfo culture;
		DebugLog log;

		public TimeSchemaPauseAdd()
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				currentTimeSchemaPause = new TimeSchemaPauseTO();
				logInUser = NotificationController.GetLogInUser();

				this.CenterToScreen();
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				rm = new ResourceManager("UI.Resource",typeof(TimeSchemaPauseAdd).Assembly);
				setLanguage();

				int pID = new TimeSchemaPause().FindMAXPauseID();
				pID++;
				tbPauseID.Text = pID.ToString();

				btnUpdate.Visible = false;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPauseAdd.TimeSchemaPauseAdd(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		public TimeSchemaPauseAdd(TimeSchemaPauseTO timeSchemaPause)
		{
			try
			{
				InitializeComponent();

				string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
				log = new DebugLog(logFilePath);

				currentTimeSchemaPause = new TimeSchemaPauseTO(timeSchemaPause.PauseID, timeSchemaPause.Description, timeSchemaPause.PauseDuration,
					timeSchemaPause.EarliestUseTime, timeSchemaPause.LatestUseTime, timeSchemaPause.PauseOffset, timeSchemaPause.ShortBreakDuration);
				logInUser = NotificationController.GetLogInUser();

				this.CenterToScreen();
				culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

				rm = new ResourceManager("UI.Resource",typeof(TimeSchemaPauseAdd).Assembly);
				setLanguage();

				tbPauseID.Text            = timeSchemaPause.PauseID.ToString();
				tbDesc.Text               = timeSchemaPause.Description.Trim();
				tbPauseDuration.Text      = timeSchemaPause.PauseDuration.ToString();
				tbEarliestTime.Text       = timeSchemaPause.EarliestUseTime.ToString();
				tbLatestTime.Text         = timeSchemaPause.LatestUseTime.ToString();
				tbPauseOffset.Text        = timeSchemaPause.PauseOffset.ToString();
				tbShortBreakDuration.Text = timeSchemaPause.ShortBreakDuration.ToString();

				btnSave.Visible = false;
				tbPauseID.Enabled = false;
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPauseAdd.TimeSchemaPauseAdd(): " + ex.Message + "\n");
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
            this.lblStar7 = new System.Windows.Forms.Label();
            this.tbShortBreakDuration = new System.Windows.Forms.TextBox();
            this.lblShortBreakDuration = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gbShortBreak = new System.Windows.Forms.GroupBox();
            this.lblMin7 = new System.Windows.Forms.Label();
            this.lblMin6 = new System.Windows.Forms.Label();
            this.lblMin5 = new System.Windows.Forms.Label();
            this.lblMin4 = new System.Windows.Forms.Label();
            this.lblMin3 = new System.Windows.Forms.Label();
            this.lblStar2 = new System.Windows.Forms.Label();
            this.tbLatestTime = new System.Windows.Forms.TextBox();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.lblStar3 = new System.Windows.Forms.Label();
            this.tbPauseID = new System.Windows.Forms.TextBox();
            this.lblEarliestTime = new System.Windows.Forms.Label();
            this.lblStar5 = new System.Windows.Forms.Label();
            this.tbPauseDuration = new System.Windows.Forms.TextBox();
            this.lblPauseID = new System.Windows.Forms.Label();
            this.lblLatestTime = new System.Windows.Forms.Label();
            this.lblStar6 = new System.Windows.Forms.Label();
            this.tbPauseOffset = new System.Windows.Forms.TextBox();
            this.lblPauseOffset = new System.Windows.Forms.Label();
            this.lblStar1 = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblPauseDuration = new System.Windows.Forms.Label();
            this.lblStar4 = new System.Windows.Forms.Label();
            this.tbEarliestTime = new System.Windows.Forms.TextBox();
            this.gbPause = new System.Windows.Forms.GroupBox();
            this.gbShortBreak.SuspendLayout();
            this.gbPause.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStar7
            // 
            this.lblStar7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar7.ForeColor = System.Drawing.Color.Red;
            this.lblStar7.Location = new System.Drawing.Point(272, 24);
            this.lblStar7.Name = "lblStar7";
            this.lblStar7.Size = new System.Drawing.Size(16, 16);
            this.lblStar7.TabIndex = 21;
            this.lblStar7.Text = "*";
            // 
            // tbShortBreakDuration
            // 
            this.tbShortBreakDuration.Location = new System.Drawing.Point(176, 24);
            this.tbShortBreakDuration.MaxLength = 2;
            this.tbShortBreakDuration.Name = "tbShortBreakDuration";
            this.tbShortBreakDuration.Size = new System.Drawing.Size(50, 20);
            this.tbShortBreakDuration.TabIndex = 20;
            this.tbShortBreakDuration.Text = "0";
            this.tbShortBreakDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblShortBreakDuration
            // 
            this.lblShortBreakDuration.Location = new System.Drawing.Point(16, 24);
            this.lblShortBreakDuration.Name = "lblShortBreakDuration";
            this.lblShortBreakDuration.Size = new System.Drawing.Size(150, 23);
            this.lblShortBreakDuration.TabIndex = 19;
            this.lblShortBreakDuration.Text = "Short break duration:";
            this.lblShortBreakDuration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(16, 320);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 22;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(448, 320);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 320);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbShortBreak
            // 
            this.gbShortBreak.Controls.Add(this.lblMin7);
            this.gbShortBreak.Controls.Add(this.lblShortBreakDuration);
            this.gbShortBreak.Controls.Add(this.tbShortBreakDuration);
            this.gbShortBreak.Controls.Add(this.lblStar7);
            this.gbShortBreak.Location = new System.Drawing.Point(16, 240);
            this.gbShortBreak.Name = "gbShortBreak";
            this.gbShortBreak.Size = new System.Drawing.Size(510, 56);
            this.gbShortBreak.TabIndex = 19;
            this.gbShortBreak.TabStop = false;
            this.gbShortBreak.Text = "Short break";
            // 
            // lblMin7
            // 
            this.lblMin7.Location = new System.Drawing.Point(232, 24);
            this.lblMin7.Name = "lblMin7";
            this.lblMin7.Size = new System.Drawing.Size(30, 23);
            this.lblMin7.TabIndex = 25;
            this.lblMin7.Text = "min";
            this.lblMin7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMin6
            // 
            this.lblMin6.Location = new System.Drawing.Point(232, 184);
            this.lblMin6.Name = "lblMin6";
            this.lblMin6.Size = new System.Drawing.Size(30, 23);
            this.lblMin6.TabIndex = 28;
            this.lblMin6.Text = "min";
            this.lblMin6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMin5
            // 
            this.lblMin5.Location = new System.Drawing.Point(232, 152);
            this.lblMin5.Name = "lblMin5";
            this.lblMin5.Size = new System.Drawing.Size(30, 23);
            this.lblMin5.TabIndex = 27;
            this.lblMin5.Text = "min";
            this.lblMin5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMin4
            // 
            this.lblMin4.Location = new System.Drawing.Point(232, 120);
            this.lblMin4.Name = "lblMin4";
            this.lblMin4.Size = new System.Drawing.Size(30, 23);
            this.lblMin4.TabIndex = 26;
            this.lblMin4.Text = "min";
            this.lblMin4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMin3
            // 
            this.lblMin3.Location = new System.Drawing.Point(232, 88);
            this.lblMin3.Name = "lblMin3";
            this.lblMin3.Size = new System.Drawing.Size(30, 23);
            this.lblMin3.TabIndex = 25;
            this.lblMin3.Text = "min";
            this.lblMin3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStar2
            // 
            this.lblStar2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar2.ForeColor = System.Drawing.Color.Red;
            this.lblStar2.Location = new System.Drawing.Point(482, 56);
            this.lblStar2.Name = "lblStar2";
            this.lblStar2.Size = new System.Drawing.Size(16, 16);
            this.lblStar2.TabIndex = 6;
            this.lblStar2.Text = "*";
            // 
            // tbLatestTime
            // 
            this.tbLatestTime.Location = new System.Drawing.Point(176, 152);
            this.tbLatestTime.MaxLength = 3;
            this.tbLatestTime.Name = "tbLatestTime";
            this.tbLatestTime.Size = new System.Drawing.Size(50, 20);
            this.tbLatestTime.TabIndex = 14;
            this.tbLatestTime.Text = "60";
            this.tbLatestTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(176, 56);
            this.tbDesc.MaxLength = 300;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(300, 20);
            this.tbDesc.TabIndex = 5;
            // 
            // lblStar3
            // 
            this.lblStar3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar3.ForeColor = System.Drawing.Color.Red;
            this.lblStar3.Location = new System.Drawing.Point(272, 88);
            this.lblStar3.Name = "lblStar3";
            this.lblStar3.Size = new System.Drawing.Size(16, 16);
            this.lblStar3.TabIndex = 9;
            this.lblStar3.Text = "*";
            // 
            // tbPauseID
            // 
            this.tbPauseID.Location = new System.Drawing.Point(176, 24);
            this.tbPauseID.MaxLength = 10;
            this.tbPauseID.Name = "tbPauseID";
            this.tbPauseID.Size = new System.Drawing.Size(50, 20);
            this.tbPauseID.TabIndex = 2;
            this.tbPauseID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblEarliestTime
            // 
            this.lblEarliestTime.Location = new System.Drawing.Point(16, 120);
            this.lblEarliestTime.Name = "lblEarliestTime";
            this.lblEarliestTime.Size = new System.Drawing.Size(150, 23);
            this.lblEarliestTime.TabIndex = 10;
            this.lblEarliestTime.Text = "Earliest use time:";
            this.lblEarliestTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStar5
            // 
            this.lblStar5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar5.ForeColor = System.Drawing.Color.Red;
            this.lblStar5.Location = new System.Drawing.Point(272, 152);
            this.lblStar5.Name = "lblStar5";
            this.lblStar5.Size = new System.Drawing.Size(16, 16);
            this.lblStar5.TabIndex = 15;
            this.lblStar5.Text = "*";
            // 
            // tbPauseDuration
            // 
            this.tbPauseDuration.Location = new System.Drawing.Point(176, 88);
            this.tbPauseDuration.MaxLength = 3;
            this.tbPauseDuration.Name = "tbPauseDuration";
            this.tbPauseDuration.Size = new System.Drawing.Size(50, 20);
            this.tbPauseDuration.TabIndex = 8;
            this.tbPauseDuration.Text = "30";
            this.tbPauseDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPauseID
            // 
            this.lblPauseID.Location = new System.Drawing.Point(16, 24);
            this.lblPauseID.Name = "lblPauseID";
            this.lblPauseID.Size = new System.Drawing.Size(150, 23);
            this.lblPauseID.TabIndex = 1;
            this.lblPauseID.Text = "Pause ID:";
            this.lblPauseID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLatestTime
            // 
            this.lblLatestTime.Location = new System.Drawing.Point(16, 152);
            this.lblLatestTime.Name = "lblLatestTime";
            this.lblLatestTime.Size = new System.Drawing.Size(150, 23);
            this.lblLatestTime.TabIndex = 13;
            this.lblLatestTime.Text = "Latest use time:";
            this.lblLatestTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStar6
            // 
            this.lblStar6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar6.ForeColor = System.Drawing.Color.Red;
            this.lblStar6.Location = new System.Drawing.Point(272, 184);
            this.lblStar6.Name = "lblStar6";
            this.lblStar6.Size = new System.Drawing.Size(16, 16);
            this.lblStar6.TabIndex = 18;
            this.lblStar6.Text = "*";
            // 
            // tbPauseOffset
            // 
            this.tbPauseOffset.Location = new System.Drawing.Point(176, 184);
            this.tbPauseOffset.MaxLength = 2;
            this.tbPauseOffset.Name = "tbPauseOffset";
            this.tbPauseOffset.Size = new System.Drawing.Size(50, 20);
            this.tbPauseOffset.TabIndex = 17;
            this.tbPauseOffset.Text = "5";
            this.tbPauseOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblPauseOffset
            // 
            this.lblPauseOffset.Location = new System.Drawing.Point(16, 184);
            this.lblPauseOffset.Name = "lblPauseOffset";
            this.lblPauseOffset.Size = new System.Drawing.Size(150, 23);
            this.lblPauseOffset.TabIndex = 16;
            this.lblPauseOffset.Text = "Pause offset:";
            this.lblPauseOffset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStar1
            // 
            this.lblStar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar1.ForeColor = System.Drawing.Color.Red;
            this.lblStar1.Location = new System.Drawing.Point(232, 24);
            this.lblStar1.Name = "lblStar1";
            this.lblStar1.Size = new System.Drawing.Size(16, 16);
            this.lblStar1.TabIndex = 3;
            this.lblStar1.Text = "*";
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(16, 56);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(150, 23);
            this.lblDesc.TabIndex = 4;
            this.lblDesc.Text = "Description:";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPauseDuration
            // 
            this.lblPauseDuration.Location = new System.Drawing.Point(16, 88);
            this.lblPauseDuration.Name = "lblPauseDuration";
            this.lblPauseDuration.Size = new System.Drawing.Size(150, 23);
            this.lblPauseDuration.TabIndex = 7;
            this.lblPauseDuration.Text = "Pause duration:";
            this.lblPauseDuration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStar4
            // 
            this.lblStar4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStar4.ForeColor = System.Drawing.Color.Red;
            this.lblStar4.Location = new System.Drawing.Point(272, 120);
            this.lblStar4.Name = "lblStar4";
            this.lblStar4.Size = new System.Drawing.Size(16, 16);
            this.lblStar4.TabIndex = 12;
            this.lblStar4.Text = "*";
            // 
            // tbEarliestTime
            // 
            this.tbEarliestTime.Location = new System.Drawing.Point(176, 120);
            this.tbEarliestTime.MaxLength = 3;
            this.tbEarliestTime.Name = "tbEarliestTime";
            this.tbEarliestTime.Size = new System.Drawing.Size(50, 20);
            this.tbEarliestTime.TabIndex = 11;
            this.tbEarliestTime.Text = "60";
            this.tbEarliestTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // gbPause
            // 
            this.gbPause.Controls.Add(this.lblMin6);
            this.gbPause.Controls.Add(this.lblMin5);
            this.gbPause.Controls.Add(this.lblMin4);
            this.gbPause.Controls.Add(this.lblMin3);
            this.gbPause.Controls.Add(this.lblStar2);
            this.gbPause.Controls.Add(this.tbLatestTime);
            this.gbPause.Controls.Add(this.tbDesc);
            this.gbPause.Controls.Add(this.lblStar3);
            this.gbPause.Controls.Add(this.tbPauseID);
            this.gbPause.Controls.Add(this.lblEarliestTime);
            this.gbPause.Controls.Add(this.lblStar5);
            this.gbPause.Controls.Add(this.tbPauseDuration);
            this.gbPause.Controls.Add(this.lblPauseID);
            this.gbPause.Controls.Add(this.lblLatestTime);
            this.gbPause.Controls.Add(this.lblStar6);
            this.gbPause.Controls.Add(this.tbPauseOffset);
            this.gbPause.Controls.Add(this.lblPauseOffset);
            this.gbPause.Controls.Add(this.lblStar1);
            this.gbPause.Controls.Add(this.lblDesc);
            this.gbPause.Controls.Add(this.lblPauseDuration);
            this.gbPause.Controls.Add(this.lblStar4);
            this.gbPause.Controls.Add(this.tbEarliestTime);
            this.gbPause.Location = new System.Drawing.Point(16, 8);
            this.gbPause.Name = "gbPause";
            this.gbPause.Size = new System.Drawing.Size(510, 216);
            this.gbPause.TabIndex = 0;
            this.gbPause.TabStop = false;
            this.gbPause.Text = "Pause";
            // 
            // TimeSchemaPauseAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(542, 346);
            this.ControlBox = false;
            this.Controls.Add(this.gbShortBreak);
            this.Controls.Add(this.gbPause);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(550, 380);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(550, 380);
            this.Name = "TimeSchemaPauseAdd";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "Add pause";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TimeSchemaPauseAdd_KeyUp);
            this.gbShortBreak.ResumeLayout(false);
            this.gbShortBreak.PerformLayout();
            this.gbPause.ResumeLayout(false);
            this.gbPause.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Set proper language
		/// </summary>
		private void setLanguage()
		{
			try
			{
				// Form name
				if (!currentTimeSchemaPause.Description.Equals(""))
				{
					this.Text = rm.GetString("updateTimeSchemaPause", culture);
				}
				else
				{
					this.Text = rm.GetString("addTimeSchemaPause", culture);
				}

				// group box text
				gbPause.Text = rm.GetString("gbPause", culture);
				gbShortBreak.Text = rm.GetString("gbShortBreak", culture);
				
				// button's text
				btnSave.Text   = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblPauseID.Text            = rm.GetString("lblPauseID", culture);
				lblDesc.Text               = rm.GetString("lblDescription", culture);
				lblPauseDuration.Text      = rm.GetString("lblPauseDuration", culture);
				lblEarliestTime.Text       = rm.GetString("lblEarliestTime", culture);
				lblLatestTime.Text         = rm.GetString("lblLatestTime", culture);
				lblPauseOffset.Text        = rm.GetString("lblOffset", culture);
				lblShortBreakDuration.Text = rm.GetString("lblShortBreakDuration", culture);
				lblMin3.Text               = rm.GetString("lblMin", culture);
				lblMin4.Text               = rm.GetString("lblMin", culture);
				lblMin5.Text               = rm.GetString("lblMin", culture);
				lblMin6.Text               = rm.GetString("lblMin", culture);
				lblMin7.Text               = rm.GetString("lblMin", culture);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPauseAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (validate())
                {
                    List<TimeSchemaPauseTO> timeSchemaPauses = new TimeSchemaPause().Search(tbDesc.Text.Trim());

                    if (timeSchemaPauses.Count == 0)
                    {
                        int inserted = new TimeSchemaPause().Save(Int32.Parse(tbPauseID.Text.Trim()), tbDesc.Text.Trim(),
                            Int32.Parse(tbPauseDuration.Text.Trim()), Int32.Parse(tbEarliestTime.Text.Trim()),
                            Int32.Parse(tbLatestTime.Text.Trim()), Int32.Parse(tbPauseOffset.Text.Trim()),
                            Int32.Parse(tbShortBreakDuration.Text.Trim()));

                        if (inserted > 0)
                        {
                            currentTimeSchemaPause.PauseID = Int32.Parse(tbPauseID.Text.Trim());
                            currentTimeSchemaPause.Description = tbDesc.Text.Trim();
                            currentTimeSchemaPause.PauseDuration = Int32.Parse(tbPauseDuration.Text.Trim());
                            currentTimeSchemaPause.EarliestUseTime = Int32.Parse(tbEarliestTime.Text.Trim());
                            currentTimeSchemaPause.LatestUseTime = Int32.Parse(tbLatestTime.Text.Trim());
                            currentTimeSchemaPause.PauseOffset = Int32.Parse(tbPauseOffset.Text.Trim());
                            currentTimeSchemaPause.ShortBreakDuration = Int32.Parse(tbShortBreakDuration.Text.Trim());

                            MessageBox.Show(rm.GetString("timeSchemaPauseSaved", culture));
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show(rm.GetString("timeSchemaPauseNotSaved", culture));
                        }
                    }
                    else
                    {
                        MessageBox.Show(rm.GetString("timeSchemaPauseExists", culture));
                    }
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " TimeSchemaPauseAdd.btnSave_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally {
                this.Cursor = Cursors.Arrow;
            }
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			try
            {
                this.Cursor = Cursors.WaitCursor;	
				bool isUpdated = false;

				if (validate())
				{
					if (!currentTimeSchemaPause.Description.Equals(tbDesc.Text.Trim()))
					{
                        List<TimeSchemaPauseTO> timeSchemaPauses = new TimeSchemaPause().Search(tbDesc.Text.Trim());
						if (timeSchemaPauses.Count != 0)
						{	
							MessageBox.Show(rm.GetString("timeSchemaPauseExists", culture));
							return;
						}
					}

                    if (isUpdated = new TimeSchemaPause().Update(Int32.Parse(tbPauseID.Text.Trim()), tbDesc.Text.Trim(),
							Int32.Parse(tbPauseDuration.Text.Trim()), Int32.Parse(tbEarliestTime.Text.Trim()), 
							Int32.Parse(tbLatestTime.Text.Trim()), Int32.Parse(tbPauseOffset.Text.Trim()),
							Int32.Parse(tbShortBreakDuration.Text.Trim())))
					{		
						currentTimeSchemaPause.PauseID = Int32.Parse(tbPauseID.Text.Trim());
						currentTimeSchemaPause.Description = tbDesc.Text.Trim();
						currentTimeSchemaPause.PauseDuration = Int32.Parse(tbPauseDuration.Text.Trim());
						currentTimeSchemaPause.EarliestUseTime = Int32.Parse(tbEarliestTime.Text.Trim());
						currentTimeSchemaPause.LatestUseTime = Int32.Parse(tbLatestTime.Text.Trim());
						currentTimeSchemaPause.PauseOffset = Int32.Parse(tbPauseOffset.Text.Trim());
						currentTimeSchemaPause.ShortBreakDuration = Int32.Parse(tbShortBreakDuration.Text.Trim());

						MessageBox.Show(rm.GetString("timeSchemaPauseUpdated", culture));
						this.Close();
					}
					else
					{
						MessageBox.Show(rm.GetString("timeSchemaPauseNotUpdated", culture));								
					}
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPauseAdd.btnUpdate_Click(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
            finally
            {
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
                log.writeLog(DateTime.Now + " TimeSchemaPauseAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

		private bool validate()
		{
			try
			{
				if (tbPauseID.Text.Equals(""))
				{
					MessageBox.Show(rm.GetString("addTSPausePIDNotNull", culture));
					return false;
				}
				try
				{
					if (Int32.Parse(tbPauseID.Text.Trim()) < 0)
					{
						MessageBox.Show(rm.GetString("addTSPausePIDNotNumber", culture));
						return false;
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("addTSPausePIDNotNumber", culture));
					return false;
				}

				if (tbDesc.Text.Trim().Equals(""))
				{
					MessageBox.Show(rm.GetString("addTSPauseDescEmpty", culture));
					return false;
				}

				if (tbPauseDuration.Text.Equals(""))
				{
					MessageBox.Show(rm.GetString("addTSPausePDurationNotNull", culture));
					return false;
				}
				try
				{
					if (Int32.Parse(tbPauseDuration.Text.Trim()) < 0)
					{
						MessageBox.Show(rm.GetString("addTSPausePDurationNotNumber", culture));
						return false;
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("addTSPausePDurationNotNumber", culture));
					return false;
				}

				if (tbEarliestTime.Text.Equals(""))
				{
					MessageBox.Show(rm.GetString("addTSPauseETimeNotNull", culture));
					return false;
				}
				try
				{
					if (Int32.Parse(tbEarliestTime.Text.Trim()) < 0)
					{
						MessageBox.Show(rm.GetString("addTSPauseETimeNotNumber", culture));
						return false;
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("addTSPauseETimeNotNumber", culture));
					return false;
				}

				if (tbLatestTime.Text.Equals(""))
				{
					MessageBox.Show(rm.GetString("addTSPauseLTimeNotNull", culture));
					return false;
				}
				try
				{
					if (Int32.Parse(tbLatestTime.Text.Trim()) < 0)
					{
						MessageBox.Show(rm.GetString("addTSPauseLTimeNotNumber", culture));
						return false;
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("addTSPauseLTimeNotNumber", culture));
					return false;
				}

				if (tbPauseOffset.Text.Equals(""))
				{
					MessageBox.Show(rm.GetString("addTSPauseOffsetNotNull", culture));
					return false;
				}
				try
				{
					if (Int32.Parse(tbPauseOffset.Text.Trim()) < 0)
					{
						MessageBox.Show(rm.GetString("addTSPauseOffsetNotNumber", culture));
						return false;
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("addTSPauseOffsetNotNumber", culture));
					return false;
				}

				if (tbShortBreakDuration.Text.Equals(""))
				{
					MessageBox.Show(rm.GetString("addTSPauseSBDurationNotNull", culture));
					return false;
				}
				try
				{
					if (Int32.Parse(tbShortBreakDuration.Text.Trim()) < 0)
					{
						MessageBox.Show(rm.GetString("addTSPauseSBDurationNotNumber", culture));
						return false;
					}
				}
				catch
				{
					MessageBox.Show(rm.GetString("addTSPauseSBDurationNotNumber", culture));
					return false;
				}

				if ((Int32.Parse(tbPauseDuration.Text.Trim()) == 0)
					&& (Int32.Parse(tbShortBreakDuration.Text.Trim()) == 0))
				{
					MessageBox.Show(rm.GetString("addTSPausePauseSBDurationBothZero", culture));
					return false;
				}

				if ((Int32.Parse(tbPauseDuration.Text.Trim()) > 0)
					&& (Int32.Parse(tbShortBreakDuration.Text.Trim()) >= Int32.Parse(tbPauseDuration.Text.Trim())))
				{
					MessageBox.Show(rm.GetString("addTSPauseSBDurationGreaterPause", culture));
					return false;
				}
			}
			catch(Exception ex)
			{
				log.writeLog(DateTime.Now + " TimeSchemaPauseAdd.validate(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}

			return true;
		}

        private void TimeSchemaPauseAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " TimeSchemaPauseAdd.TimeSchemaPauseAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
