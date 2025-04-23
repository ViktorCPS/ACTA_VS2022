using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;

using System.Data;
using System.Data.SqlClient;
using Common;
using Util;
using TransferObjects;

using System.Resources;
using System.Globalization;

namespace UI
{
	/// <summary>
	/// Summary description for HolidaysAdd.
	/// </summary>
	public class HolidaysAdd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblDesc;
		private System.Windows.Forms.TextBox tbDesc;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		HolidayTO currentHoliday = null;
		ResourceManager rm;

		private CultureInfo culture;
		
		DebugLog log;
		private System.Windows.Forms.Label lblDate;
		private System.Windows.Forms.DateTimePicker dtpDate;

		ApplUserTO logInUser;

		public HolidaysAdd()
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentHoliday = new HolidayTO();
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(HolidaysAdd).Assembly);
			setLanguage();

			btnUpdate.Visible = false;
			dtpDate.Value = DateTime.Now;
		}

		public HolidaysAdd(HolidayTO holiday)
		{
			InitializeComponent();

			string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "Log.txt";
			log = new DebugLog(logFilePath);

			currentHoliday = new HolidayTO(holiday.Description, holiday.HolidayDate);
			logInUser = NotificationController.GetLogInUser();

			this.CenterToScreen();
			
			culture = CultureInfo.CreateSpecificCulture(NotificationController.GetLanguage());

			rm = new ResourceManager("UI.Resource",typeof(HolidaysAdd).Assembly);

			setLanguage();
			
			tbDesc.Text = holiday.Description.Trim();
			dtpDate.Value = holiday.HolidayDate;
			btnSave.Visible = false;
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
            this.lblDesc = new System.Windows.Forms.Label();
            this.tbDesc = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(8, 16);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(64, 23);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "Description";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDesc
            // 
            this.tbDesc.Location = new System.Drawing.Point(88, 16);
            this.tbDesc.MaxLength = 50;
            this.tbDesc.Name = "tbDesc";
            this.tbDesc.Size = new System.Drawing.Size(176, 20);
            this.tbDesc.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(16, 96);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(16, 96);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(72, 24);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(192, 96);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(8, 48);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(64, 23);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Date:";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "dd.MM.yyy";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(88, 48);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(176, 20);
            this.dtpDate.TabIndex = 3;
            // 
            // HolidaysAdd
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(288, 134);
            this.ControlBox = false;
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbDesc);
            this.Controls.Add(this.lblDesc);
            this.MaximumSize = new System.Drawing.Size(296, 168);
            this.MinimumSize = new System.Drawing.Size(296, 168);
            this.Name = "HolidaysAdd";
            this.ShowInTaskbar = false;
            this.KeyPreview = true;
            this.Text = "HolidaysAdd";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HolidaysAdd_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

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
				if (!currentHoliday.HolidayDate.Equals(new DateTime()))
				{
					this.Text = rm.GetString("updateHoliday", culture);
				}
				else
				{
					this.Text = rm.GetString("addHoliday", culture);
				}

				// button's text
				btnSave.Text = rm.GetString("btnSave", culture);
				btnUpdate.Text = rm.GetString("btnUpdate", culture);
				btnCancel.Text = rm.GetString("btnCancel", culture);

				// label's text
				lblDesc.Text = rm.GetString("lblDescription", culture);
				lblDate.Text = rm.GetString("Date", culture);
			}
			catch (Exception ex)
			{
				log.writeLog(DateTime.Now + " HolidaysAdd.setLanguage(): " + ex.Message + "\n");
				MessageBox.Show(ex.Message);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;

                HolidayTO hTO = new Holiday().Find(dtpDate.Value.Date);
                if (hTO.HolidayDate.Equals(new DateTime()))
                {
                    currentHoliday.Description = this.tbDesc.Text.Trim();
                    currentHoliday.HolidayDate = dtpDate.Value.Date;

                    int inserted = new Holiday().Save(currentHoliday.Description, currentHoliday.HolidayDate);
                    if (inserted > 0)
                    {
                        MessageBox.Show(rm.GetString("holidaySaved", culture));
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("holidayExists", culture));
                }
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now + " HolidaysAdd.btnSave_Click(): " + ex.Message + "\n");
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

				if (!currentHoliday.HolidayDate.Equals(dtpDate.Value.Date))
				{
					HolidayTO hTO = new Holiday().Find(dtpDate.Value.Date);
					if (!hTO.HolidayDate.Equals(new DateTime(0)))
					{
						MessageBox.Show(rm.GetString("holidayExists", culture));
						return;
					}
				}
				
				if (isUpdated = new Holiday().Update(this.tbDesc.Text.Trim(), currentHoliday.HolidayDate, dtpDate.Value.Date))
				{
					currentHoliday.Description = this.tbDesc.Text.Trim();
					currentHoliday.HolidayDate = dtpDate.Value;

					MessageBox.Show(rm.GetString("holidayUpdated", culture));
					this.Close();
				}
			}
			catch(Exception ex)
			{
                log.writeLog(DateTime.Now + " HolidaysAdd.btnUpdate_Click(): " + ex.Message + "\n");
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
                log.writeLog(DateTime.Now + " HolidaysAdd.btnCancel_Click(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
		}

        private void HolidaysAdd_KeyUp(object sender, KeyEventArgs e)
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
                log.writeLog(DateTime.Now + " HolidaysAdd.HolidaysAdd_KeyUp(): " + ex.Message + "\n");
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
	}
}
